using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    public class BlobmanAIV2 : MonoBehaviour
    {
        [Header("Check Objects")]
        public Transform nextGroundCheck;
        public Transform nextWallCheck;
        public Transform nextCeilingCheck;
        public LayerMask enemyLayer;

        private Vector2 moveDir;

        [Header("Spawn Phase Settings")]
        public float spawnWait;

        private float spawnElapsed;

        [Header("Patrol Phase Settings")]
        public float patrolSpeed;
        public float viewRange;

        [Header("Pursue & Attack Phase Settings")]
        public bool enablePursue = true;
        public EnemyAttackType attackType;
        public GameObject attackRoot;
        public GameObject attackPrefab; //used depending on attack type e.g. projectile
        public float pursueSpeed;
        public float attackRange;
        public float attackDuration;
        public float timeTillDamage;
        public bool isBoss = false;

        private GameObject attackTarget;
        private float attackElapsed;
        private bool damageDealt;

        [Header("Hurt and Death")]
        public float hurtDuration;
        public float deathDuration;

        private float hurtElapsed;
        private float deathElapsed;

        private float checkRadius = .15f;

        private EnemyPhase phase;

        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private PlatformerCharacter2D pCharacter;

        private void Awake()
        {
            moveDir = Vector2.right;

            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            pCharacter = GetComponent<PlatformerCharacter2D>();
        }

        private void OnEnable()
        {
            SetPhase(EnemyPhase.Spawn);
        }

        private void Update()
        {
            if(!spriteRenderer.flipX)
            {
                nextGroundCheck.localPosition = new Vector3(Mathf.Abs(nextGroundCheck.localPosition.x), nextGroundCheck.localPosition.y, nextGroundCheck.localPosition.z);
                nextWallCheck.localPosition = new Vector3(Mathf.Abs(nextWallCheck.localPosition.x), nextWallCheck.localPosition.y, nextWallCheck.localPosition.z);
                nextCeilingCheck.localPosition = new Vector3(Mathf.Abs(nextCeilingCheck.localPosition.x), nextCeilingCheck.localPosition.y, nextCeilingCheck.localPosition.z);
            }
            else
            {
                nextGroundCheck.localPosition = new Vector3(-Mathf.Abs(nextGroundCheck.localPosition.x), nextGroundCheck.localPosition.y, nextGroundCheck.localPosition.z);
                nextWallCheck.localPosition = new Vector3(-Mathf.Abs(nextWallCheck.localPosition.x), nextWallCheck.localPosition.y, nextWallCheck.localPosition.z);
                nextCeilingCheck.localPosition = new Vector3(-Mathf.Abs(nextCeilingCheck.localPosition.x), nextCeilingCheck.localPosition.y, nextCeilingCheck.localPosition.z);
            }
            if (hurtElapsed >= 0)
                hurtElapsed += Time.deltaTime;

            switch (phase)
            {
                case EnemyPhase.Spawn:
                    {
                        if (animator.speed > 0)
                        {
                            spawnElapsed += Time.deltaTime;
                            if (spawnElapsed >= spawnWait)
                            {
                                attackTarget = GameObject.FindGameObjectWithTag("Player");
                                SetPhase(EnemyPhase.Pursue);
                            }
                        }
                        else if (withinCamera() && animator.speed <= 0)
                        {
                            animator.speed = 1;
                            AudioManager.instance.Play("MonsterGrowl");
                        }
                    }
                    break;
                case EnemyPhase.Patrol:
                    {
                        if (Physics2D.OverlapCircle(nextGroundCheck.position, checkRadius, pCharacter.GetGroundLayer()) == null ||
                            Physics2D.OverlapCircle(nextWallCheck.position, checkRadius, pCharacter.GetGroundLayer()) != null)
                            moveDir = -moveDir;
                        MoveTo((Vector2)transform.position + moveDir, patrolSpeed, false);

                        Vector2 lookDir = (spriteRenderer.flipX) ? Vector2.left : Vector2.right;
                        RaycastHit2D viewScan = Physics2D.Raycast(transform.position, lookDir, viewRange, LayerMask.GetMask("Player"));
                        RaycastHit2D viewObstructScan = Physics2D.Raycast(transform.position, lookDir, viewRange, LayerMask.GetMask("Ground"));
                        bool unObstructedView = viewScan.collider != null && (viewObstructScan.collider == null || (viewObstructScan.collider != null && viewScan.distance < viewObstructScan.distance));
                        if (unObstructedView && viewScan.collider.gameObject.activeSelf)
                        {
                            attackTarget = viewScan.collider.gameObject;
                            SetPhase(EnemyPhase.Pursue);
                        }
                    }
                    break;
                case EnemyPhase.Pursue:
                    {
                        if (attackTarget == null || !attackTarget.gameObject.activeSelf)
                        {
                            SetPhase(EnemyPhase.Patrol);
                            return;
                        }

                        if (isBoss || (transform.position - attackTarget.transform.position).sqrMagnitude <= Mathf.Pow(attackRange, 2f)
                            && Mathf.Abs(transform.position.y - attackTarget.transform.position.y) <= 0.5f)
                        {
                            SetPhase(EnemyPhase.Attack);
                            return;
                        }

                        MoveTo(attackTarget.transform.position + new Vector3((0.5f * ((spriteRenderer.flipX) ? -1 : 1)), 0f, 0f), (enablePursue) ? pursueSpeed : 0, true);
                    }
                    break;
                case EnemyPhase.Attack:
                    {
                        if (attackTarget == null || !attackTarget.gameObject.activeSelf)
                        {
                            SetPhase(EnemyPhase.Patrol);
                            return;
                        }

                        MoveTo(attackTarget.transform.position, 0f, false);

                        attackElapsed += Time.deltaTime;
                        if (attackType != EnemyAttackType.Projectile && (transform.position - attackTarget.transform.position).sqrMagnitude > Mathf.Pow(attackRange, 2f) && !damageDealt)
                        {
                            SetPhase(EnemyPhase.Pursue);
                        }
                        if (attackElapsed >= timeTillDamage && !damageDealt)
                        {
                            switch (attackType)
                            {
                                case EnemyAttackType.Melee:
                                    attackTarget.GetComponent<HealthScript>().ProcessHit(GetComponent<Collider2D>(), gameObject.tag);
                                    AudioManager.instance.Play("MonsterSwing");
                                    break;
                                case EnemyAttackType.Projectile:
                                    GameObject bullet = Instantiate(attackPrefab);
                                    bullet.GetComponent<BulletScript>().Activate(gameObject, attackRoot.transform.position, new Vector2(((spriteRenderer.flipX) ? -1 : 1), 0f));
                                    if(isBoss)
                                    {
                                        float yMax = transform.position.y + spriteRenderer.bounds.extents.y - 0.5f;
                                        float yMin = transform.position.y - spriteRenderer.bounds.extents.y + 1.2f;
                                        bullet.transform.position = new Vector3(bullet.transform.position.x, Random.Range(yMin, yMax), bullet.transform.position.z);
                                    }
                                    AudioManager.instance.Play("MonsterSwing");
                                    break;
                            }
                            damageDealt = true;
                        }
                        if (attackElapsed >= attackDuration)
                            SetPhase(EnemyPhase.Pursue);
                    }
                    break;
                case EnemyPhase.Dead:
                    deathElapsed += Time.deltaTime;
                    if (deathElapsed >= deathDuration)
                        Destroy(gameObject);
                    break;
            }
        }

        private void MoveTo(Vector2 targetPos, float speed, bool canJump)
        {
            if (hurtElapsed >= 0 && hurtElapsed < hurtDuration)
                return;
            else if(hurtElapsed >= hurtDuration)
            {
                hurtElapsed = -5f;
            }

            pCharacter.SetMaxSpeed(speed);

            Vector2 lookDir = (spriteRenderer.flipX) ? Vector2.left : Vector2.right;

            bool gapInFloor = Physics2D.OverlapCircle(nextGroundCheck.position, checkRadius, pCharacter.GetGroundLayer()) == null;
            if (gapInFloor)
            {
                gapInFloor = Physics2D.Raycast(nextGroundCheck.position, Vector2.down, 1f, pCharacter.GetGroundLayer()).collider == null;
            }

            float move = 0;
            if (Physics2D.Raycast(nextWallCheck.position, lookDir, 0.05f, enemyLayer))
                move = -moveDir.x;
            else if (Mathf.Abs(transform.position.y - targetPos.y) >= 1f)
            {
                if (gapInFloor && transform.position.y - targetPos.y <= 0.5f)
                    move = -moveDir.x;
                else if (Mathf.Abs(transform.position.x - targetPos.x) <= 4f)
                    move = moveDir.x;
                else if (transform.position.x > targetPos.x)
                    move = -1f;
                else if (transform.position.x < targetPos.x)
                    move = 1f;
            }
            else if (transform.position.x > targetPos.x)
                move = -1f;
            else if (transform.position.x < targetPos.x)
                move = 1f;

            bool jump = false;
            if(canJump)
            {
                //bool wallInFront = Physics2D.OverlapCircle(nextWallCheck.position, checkRadius, pCharacter.GetGroundLayer()) != null;
                bool wallInFront = Physics2D.Raycast(nextWallCheck.position, lookDir, 0.25f, pCharacter.GetGroundLayer()).collider != null;
                jump = (gapInFloor || wallInFront);// && Physics2D.OverlapCircle(nextCeilingCheck.position, checkRadius, pCharacter.GetGroundLayer()) == null;
            }

            moveDir.x = move;
            pCharacter.Move(move, false, jump);
        }

        private bool withinCamera()
        {
            Vector2 spriteExtents = spriteRenderer.bounds.extents;
            Vector2 screenExtents = new Vector2();
            screenExtents.y = Camera.main.orthographicSize;
            screenExtents.x = screenExtents.y / Screen.height * Screen.width;

            Vector2 posDiff = Camera.main.transform.position - transform.position;
            if (Mathf.Abs(posDiff.x) <= screenExtents.x + spriteExtents.x && Mathf.Abs(posDiff.y) <= screenExtents.y + spriteExtents.y)
                return true;
            return false;
        }

        public void SetPhase(EnemyPhase newPhase)
        {
            switch (newPhase)
            {
                case EnemyPhase.Spawn:
                    //animator.Play("EnemyEntry");
                    animator.speed = 0;
                    spawnElapsed = 0f;
                    break;
                case EnemyPhase.Patrol:

                    break;
                case EnemyPhase.Pursue:

                    break;
                case EnemyPhase.Attack:
                    animator.SetTrigger("Attack");
                    attackElapsed = 0;
                    damageDealt = false;
                    break;
                case EnemyPhase.Dead:
                    AudioManager.instance.Play("MonsterGrowl");
                    animator.SetTrigger("Death");
                    deathElapsed = 0;
                    break;
            }
            phase = newPhase;
        }

        public void AlertEnemy()
        {
            if (phase == EnemyPhase.Patrol)
            {
                GameObject hitObj = GetComponent<HealthScript>().GetLatestHitObject();
                if (hitObj.tag == "Player")
                    attackTarget = hitObj;
                else if(hitObj.tag == "PlayerBullet")
                    attackTarget = GetComponent<HealthScript>().GetLatestHitObject().GetComponent<BulletScript>().GetSource();
                SetPhase(EnemyPhase.Pursue);
            }
        }

        public void KillEnemy()
        {
            SetPhase(EnemyPhase.Dead);
        }

        public void HurtEnemy()
        {
            hurtElapsed = 0;
        }
    }
}

public enum EnemyPhase
{
    Spawn,
    Patrol,
    Pursue,
    Attack,
    Dead
}

public enum EnemyAttackType
{
    Melee,
    Projectile
}
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

        private Vector2 moveDir;

        [Header("Spawn Phase Settings")]
        public EnemySpawnType spawnType;
        public float spawnWait;

        private float spawnElapsed;

        [Header("Patrol Phase Settings")]
        public float patrolSpeed;
        public float viewRange;

        [Header("Pursue & Attack Phase Settings")]
        public EnemyAttackType attackType;
        public GameObject attackRoot;
        public GameObject attackPrefab; //used depending on attack type e.g. projectile
        public float pursueSpeed;
        public float attackRange;
        public float attackDuration;
        public float timeTillDamage;

        private GameObject attackTarget;
        private float attackElapsed;
        private bool damageDealt;

        private float checkRadius = .2f;

        private EnemyPhase phase;

        private Animator animator;
        private PlatformerCharacter2D pCharacter;

        private void Awake()
        {
            moveDir = Vector2.right;

            animator = GetComponent<Animator>();
            pCharacter = GetComponent<PlatformerCharacter2D>();
        }

        private void OnEnable()
        {
            SetPhase(EnemyPhase.Spawn);
        }

        private void Update()
        {
            switch (phase)
            {
                case EnemyPhase.Spawn:
                    {
                        spawnElapsed += Time.deltaTime;
                        if (spawnElapsed >= spawnWait)
                            SetPhase(EnemyPhase.Patrol);
                    }
                    break;
                case EnemyPhase.Patrol:
                    {
                        if (Physics2D.OverlapCircle(nextGroundCheck.position, checkRadius, pCharacter.GetGroundLayer()) == null ||
                            Physics2D.OverlapCircle(nextWallCheck.position, checkRadius, pCharacter.GetGroundLayer()) != null)
                            moveDir = -moveDir;
                        MoveTo((Vector2)transform.position + moveDir, patrolSpeed, false);

                        RaycastHit2D viewScan = Physics2D.Raycast(transform.position, transform.localScale.x * Vector2.right, viewRange, LayerMask.GetMask("Player"));
                        if (viewScan.collider != null && viewScan.collider.tag == "Player" && viewScan.collider.gameObject.activeSelf)
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

                        MoveTo(attackTarget.transform.position, pursueSpeed, true);

                        if ((transform.position - attackTarget.transform.position).sqrMagnitude <= Mathf.Pow(attackRange, 2f))
                            SetPhase(EnemyPhase.Attack);
                    }
                    break;
                case EnemyPhase.Attack:
                    {
                        attackElapsed += Time.deltaTime;
                        if (attackElapsed >= timeTillDamage && !damageDealt)
                        {
                            switch (attackType)
                            {
                                case EnemyAttackType.Melee:
                                    attackTarget.GetComponent<HealthScript>().ProcessHit(GetComponent<Collider2D>(), gameObject.tag);
                                    break;
                                case EnemyAttackType.Projectile:
                                    GameObject bullet = Instantiate(attackPrefab);
                                    bullet.GetComponent<BlobmanBulletScript>().Activate(attackRoot.transform.position, new Vector2(transform.localScale.x, 0f));
                                    break;
                            }
                            damageDealt = true;
                        }
                        if (attackElapsed >= attackDuration)
                            SetPhase(EnemyPhase.Pursue);
                    }
                    break;
            }
        }

        private void MoveTo(Vector2 targetPos, float speed, bool canJump)
        {
            pCharacter.SetMaxSpeed(speed);

            float move = 0;
            if (transform.position.x > targetPos.x)
                move = -1f;
            else if (transform.position.x < targetPos.x)
                move = 1f;

            bool jump = false;
            if(canJump)
            {
                bool gapInFloor = Physics2D.OverlapCircle(nextGroundCheck.position, checkRadius, pCharacter.GetGroundLayer()) == null;
                if (gapInFloor)
                {
                    gapInFloor = Physics2D.Raycast(nextGroundCheck.position, Vector2.down, 10f, pCharacter.GetGroundLayer()).collider == null;
                    Debug.Log(Physics2D.Raycast(nextGroundCheck.position, Vector2.down, 10f, pCharacter.GetGroundLayer()).collider);
                }
                bool wallInFront = Physics2D.OverlapCircle(nextWallCheck.position, checkRadius, pCharacter.GetGroundLayer()) != null;
                jump = (gapInFloor || wallInFront);// && Physics2D.OverlapCircle(nextCeilingCheck.position, checkRadius, pCharacter.GetGroundLayer()) == null;
            }

            moveDir.x = move;
            pCharacter.Move(move, false, jump);
        }

        public void SetPhase(EnemyPhase newPhase)
        {
            switch (newPhase)
            {
                case EnemyPhase.Spawn:
                    animator.Play("EnemyEntry");
                    spawnElapsed = 0f;
                    break;
                case EnemyPhase.Patrol:

                    break;
                case EnemyPhase.Pursue:

                    break;
                case EnemyPhase.Attack:
                    animator.Play("EnemyAttack");
                    attackElapsed = 0;
                    damageDealt = false;
                    break;
            }
            phase = newPhase;
        }
    }
}

public enum EnemyPhase
{
    Spawn,
    Patrol,
    Pursue,
    Attack
}

public enum EnemyAttackType
{
    Melee,
    Projectile
}

public enum EnemySpawnType
{
    Random,
    SpawnOnSight,
    SpawnPatrol
}
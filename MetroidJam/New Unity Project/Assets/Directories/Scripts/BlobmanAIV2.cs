using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobmanAIV2 : MonoBehaviour
{
    [Header("Spawn Phase Settings")]
    public EnemySpawnType spawnType;
    public float spawnWait;

    private float spawnElapsed;

    [Header("Patrol Phase Settings")]
    public List<Transform> patrolPoints;
    public float patrolSpeed;
    public float viewRange;

    private int patrolIndex;

    [Header("Pursue & Attack Phase Settings")]
    public EnemyAttackType attackType;
    public GameObject attackPrefab; //used depending on attack type e.g. projectile
    public float pursueSpeed;
    public float attackRange;
    public float attackDuration;

    private GameObject attackTarget;
    private float attackElapsed;

    private EnemyPhase phase;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
                    MoveTo(patrolPoints[patrolIndex].position, patrolSpeed);
                    if((transform.position - patrolPoints[patrolIndex].position).sqrMagnitude <= 0.01f)
                    {
                        patrolIndex++;
                        if (patrolIndex >= patrolPoints.Count)
                            patrolIndex = 0;
                    }

                    RaycastHit2D viewScan = Physics2D.Raycast(transform.position, transform.localScale.x * Vector2.right, viewRange, LayerMask.GetMask("Player"));
                    if (viewScan.collider != null && viewScan.collider.tag == "Player")
                    {
                        attackTarget = viewScan.collider.gameObject;
                        SetPhase(EnemyPhase.Pursue);
                    }
                }
                break;
            case EnemyPhase.Pursue:
                {
                    MoveTo(attackTarget.transform.position, pursueSpeed);

                    if ((transform.position - attackTarget.transform.position).sqrMagnitude <= Mathf.Pow(attackRange, 2f))
                        SetPhase(EnemyPhase.Attack);
                }
                break;
            case EnemyPhase.Attack:
                {
                    attackElapsed += Time.deltaTime;
                    if (attackElapsed >= attackDuration)
                        SetPhase(EnemyPhase.Pursue);
                }
                break;
        }
    }

    private void MoveTo(Vector2 targetPos, float speed)
    {
        if (transform.position.x > targetPos.x)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (transform.position.x < targetPos.x)
            transform.localScale = new Vector3(1f, 1f, 1f);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    public void SetPhase(EnemyPhase newPhase)
    {
        switch(newPhase)
        {
            case EnemyPhase.Spawn:
                animator.Play("EnemyEntry");
                spawnElapsed = 0f;
                break;
            case EnemyPhase.Patrol:
                patrolIndex = 0;
                break;
            case EnemyPhase.Pursue:

                break;
            case EnemyPhase.Attack:
                switch(attackType)
                {
                    case EnemyAttackType.Melee:
                        animator.Play("EnemyAttack");
                        attackTarget.GetComponent<HealthScript>().ProcessHit(GetComponent<Collider2D>(), gameObject.tag);
                        attackElapsed = 0;
                        break;
                }
                break;
        }
        phase = newPhase;
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
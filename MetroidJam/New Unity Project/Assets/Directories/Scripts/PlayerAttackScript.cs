using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PlayerAttackScript : MonoBehaviour
{
    [Header("Ranged (Pistol)")]
    public Transform bulletSpawnRoot;
    public GameObject bulletPrefab;
    public float pistolCooldown;

    private float pistolElapsed = -5;

    [Header("Ranged (Shotgun)")]
    public float shotgunRange;
    public float shotgunCooldown;

    private float shotgunElapsed = -5;

    [Header("Melee")]
    public float meleeRange;
    public float timeTillMeleeDamage;
    public float meleeDuration;
    public LayerMask enemyLayer;

    private float meleeElapsed = -5;
    private bool damageDealt = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Platformer2DUserControl platformer2DUserControl;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformer2DUserControl = GetComponent<Platformer2DUserControl>();
    }

    private void Update()
    {
        PerformPistolShoot();
        PerformShotgunShoot();
        PerformMelee();
    }

    private void PerformPistolShoot()
    {
        if (pistolElapsed >= 0)
        {
            pistolElapsed += Time.deltaTime;
            if (pistolElapsed >= pistolCooldown)
                pistolElapsed = -5;
        }

        if (Input.GetMouseButton(0) && pistolElapsed < 0)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.GetComponent<BulletScript>().Activate(gameObject, bulletSpawnRoot.position, new Vector2(((spriteRenderer.flipX) ? -1 : 1), 0f));
            pistolElapsed = 0;
        }
    }

    private void PerformShotgunShoot()
    {
        if (shotgunElapsed >= 0)
        {
            shotgunElapsed += Time.deltaTime;
            if (shotgunElapsed >= shotgunCooldown)
                shotgunElapsed = -5;
        }

        if (Input.GetMouseButton(1) && shotgunElapsed < 0)
        {
            Vector2 lookDir = new Vector2(((spriteRenderer.flipX) ? -1 : 1), 0f);
            RaycastHit2D[] hitList = Physics2D.RaycastAll(transform.position, lookDir, shotgunRange, enemyLayer);
            for(int i = 0; i < hitList.Length; i++)
            {
                hitList[i].collider.GetComponent<HealthScript>().ProcessHit(GetComponent<Collider2D>(), "PlayerShotgun");
            }
            shotgunElapsed = 0;
        }
    }

    private void PerformMelee()
    {
        if (Input.GetKey(KeyCode.F) && meleeElapsed < 0)
        {
            //platformer2DUserControl.enabled = false;
            animator.SetFloat("Speed", 0);
            meleeElapsed = 0;
            damageDealt = false;
        }
        if (meleeElapsed >= 0)
        {
            meleeElapsed += Time.deltaTime;
            if (meleeElapsed >= timeTillMeleeDamage && !damageDealt)
            {
                Vector2 faceDir = (spriteRenderer.flipX) ? Vector2.left : Vector2.right;
                RaycastHit2D[] hitList = Physics2D.RaycastAll(transform.position, faceDir, meleeRange, enemyLayer);
                for(int i = 0; i < hitList.Length; i++)
                {
                    HealthScript healthScript = hitList[i].collider.GetComponent<HealthScript>();
                    healthScript.ProcessHit(GetComponent<Collider2D>(), "PlayerMelee");
                }
                damageDealt = true;
            }
            if (meleeElapsed >= meleeDuration)
            {
                meleeElapsed = -5;
                //platformer2DUserControl.enabled = true;
            }
        }
    }
}
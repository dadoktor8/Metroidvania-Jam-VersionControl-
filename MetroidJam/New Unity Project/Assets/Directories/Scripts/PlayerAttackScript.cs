using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PlayerAttackScript : MonoBehaviour
{
    [Header("Ranged")]
    public Transform bulletSpawnRoot;
    public GameObject bulletPrefab;
    public float shootCooldown;

    private float shootElapsed = -5;

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
        PerformShoot();
        PerformMelee();
    }

    private void PerformShoot()
    {
        if (shootElapsed >= 0)
        {
            shootElapsed += Time.deltaTime;
            if (shootElapsed >= shootCooldown)
                shootElapsed = -5;
        }

        if (Input.GetMouseButton(0) && shootElapsed < 0)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.GetComponent<BulletScript>().Activate(gameObject, bulletSpawnRoot.position, new Vector2(((spriteRenderer.flipX) ? -1 : 1), 0f));
            shootElapsed = 0;
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
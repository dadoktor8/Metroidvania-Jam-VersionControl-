using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 
using UnityStandardAssets._2D;

public class PlayerAttackScript : MonoBehaviour
{




    [Header("Ranged (Pistol)")]
    public bool enablePistol;
    public Transform bulletSpawnRoot;
    public GameObject bulletPrefab;
    public float pistolCooldown;

    private float pistolElapsed = -5;

    [Header("Ranged (Shotgun)")]
    public bool enableShotgun;
    public float shotgunRange;
    public float shotgunCooldown;
    public ParticleSystem shotgunParticles;

    private float shotgunElapsed = -5;

    [Header("Melee")]
    public bool enableMelee;
    public float meleeRange;
    public float timeTillMeleeDamage;
    public float meleeDuration;
    public LayerMask enemyLayer;

    private float meleeElapsed = -5;
    private bool damageDealt = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Platformer2DUserControl platformer2DUserControl;
	public AudioManager sound; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformer2DUserControl = GetComponent<Platformer2DUserControl>();
		sound = FindObjectOfType<AudioManager> (); 
    }



    private void Update()
    {
        if(enablePistol)
			FindObjectOfType<AudioManager> ().Play ("pistolShoot"); 
            PerformPistolShoot();
        if(enableShotgun)
			FindObjectOfType<AudioManager> ().Play ("shotgunShoot"); 
            PerformShotgunShoot();
        if(enableMelee)
            PerformMelee();
    }

    public void PerformPistolShoot()
    {
        animator.SetBool("pistolUse", Input.GetMouseButton(0));


        if (pistolElapsed >= 0)
        {
            pistolElapsed += Time.deltaTime;
            if (pistolElapsed >= pistolCooldown)
                pistolElapsed = -5;
        }

        if (Input.GetMouseButton(0) && pistolElapsed < 0)
        {
			
            GameObject bullet = Instantiate(bulletPrefab);
            float spawnXPos = Mathf.Abs(bulletSpawnRoot.localPosition.x) * ((spriteRenderer.flipX) ? -1 : 1);
            bulletSpawnRoot.localPosition = new Vector3(spawnXPos, bulletSpawnRoot.localPosition.y, bulletSpawnRoot.localPosition.z);
            bullet.GetComponent<BulletScript>().Activate(gameObject, bulletSpawnRoot.position, new Vector2(((spriteRenderer.flipX) ? -1 : 1), 0f));
            pistolElapsed = 0;

        }
    }

	public void PerformShotgunShoot()
    {
        animator.SetBool("shotgunUse", Input.GetMouseButton(1));


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
            if (hitList.Length <= 0)
                GetComponent<HealthScript>().ProcessHit(GetComponent<Collider2D>(), "PlayerShotgun");
            else
                GetComponent<HealthScript>().ProcessHit(10 * hitList.Length);
            shotgunElapsed = 0;

            float spawnXPos = Mathf.Abs(shotgunParticles.transform.localPosition.x) * ((spriteRenderer.flipX) ? -1 : 1);
            shotgunParticles.transform.localPosition = new Vector3(spawnXPos, shotgunParticles.transform.localPosition.y, shotgunParticles.transform.localPosition.z);
            shotgunParticles.transform.localScale = new Vector3(1f, 1f, ((spriteRenderer.flipX) ? -1 : 1));
            shotgunParticles.Play();


        }
    }

    private void PerformMelee()
    {
        if (Input.GetKey(KeyCode.F) && meleeElapsed < 0)
        {
            platformer2DUserControl.enabled = false;
            animator.SetFloat("Speed", 0);
            meleeElapsed = 0;
            damageDealt = false;
            animator.SetTrigger("meleeUse");
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
                platformer2DUserControl.enabled = true;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    [Header("Ranged")]
    public Transform bulletSpawnRoot;
    public GameObject bulletPrefab;
    public float shootCooldown;

    private float shootElapsed = -5;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(shootElapsed >= 0)
        {
            shootElapsed += Time.deltaTime;
            if (shootElapsed >= shootCooldown)
                shootElapsed = -5;
        }

        if(Input.GetMouseButton(0) && shootElapsed < 0)
        {
            PerformShoot();
            shootElapsed = 0;
        }
    }

    private void PerformShoot()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.GetComponent<BulletScript>().Activate(bulletSpawnRoot.position, new Vector2(((spriteRenderer.flipX) ? -1 : 1), 0f));
    }
}
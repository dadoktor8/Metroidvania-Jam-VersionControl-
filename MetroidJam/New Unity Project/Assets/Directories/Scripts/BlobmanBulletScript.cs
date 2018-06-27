using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobmanBulletScript : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 moveDir;

    public void Activate(Vector3 pos, Vector2 dir)
    {
        transform.position = pos;
        transform.localScale = Vector3.one;
        if (dir == Vector2.left)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        moveDir = dir;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            Destroy(gameObject);
    }
}
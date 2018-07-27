using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 moveDir;

    private GameObject source;

    public void Activate(GameObject aSource, Vector3 pos, Vector2 dir)
    {
        source = aSource;
        transform.position = pos;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * dir.x, transform.localScale.y, transform.localScale.z);
        moveDir = dir;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    public GameObject GetSource()
    {
        return source;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            Destroy(gameObject);
    }
}
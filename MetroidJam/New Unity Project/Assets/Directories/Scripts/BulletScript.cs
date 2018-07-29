using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 moveDir;

    private GameObject source;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

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
        if (outsideCamera())
            Destroy(gameObject);
    }

    public GameObject GetSource()
    {
        return source;
    }

    private bool outsideCamera()
    {
        Vector2 spriteExtents = spriteRenderer.bounds.extents;
        Vector2 screenExtents = new Vector2();
        screenExtents.y = Camera.main.orthographicSize;
        screenExtents.x = screenExtents.y / Screen.height * Screen.width;

        Vector2 posDiff = Camera.main.transform.position - transform.position;
        if (Mathf.Abs(posDiff.x) > screenExtents.x + spriteExtents.x)
            return true;
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            Destroy(gameObject);
    }
}
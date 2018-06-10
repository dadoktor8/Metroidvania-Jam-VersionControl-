using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthScript : MonoBehaviour
{
    public float totalHealth;
    public float currHealth;

    public List<DamageType> damageTypeList;

    private Collider2D latestDamageObj;

    private void Awake()
    {
        ResetHealth();
    }

    public void ResetHealth()
    {
        currHealth = totalHealth;
    }

    public void TakeDamage(Collider2D sourceObj, string sourceTag)
    {
        for (int i = 0; i < damageTypeList.Count; i++)
        {
            if (sourceTag == damageTypeList[i].tag)
            {
                latestDamageObj = sourceObj;
                TakeDamage(damageTypeList[i]);
                return;
            }
        }
    }

    public void TakeDamage(DamageType damageType)
    {
        currHealth -= damageType.damage;
        damageType.onDamaged.Invoke();
        if (currHealth <= 0)
            damageType.onDeath.Invoke();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        for(int i = 0; i < damageTypeList.Count; i++)
        {
            if(damageTypeList[i].isTrigger && collision.tag == damageTypeList[i].tag)
            {
                latestDamageObj = collision;
                TakeDamage(damageTypeList[i]);
                break;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < damageTypeList.Count; i++)
        {
            if (damageTypeList[i].isCollider && collision.gameObject.tag == damageTypeList[i].tag)
            {
                latestDamageObj = collision.collider;
                TakeDamage(damageTypeList[i]);
                break;
            }
        }
    }

    #region Common Effects Functions

    public void FlashGameObject(GameObject flashObj)
    {
        StartCoroutine(FlashRoutine(flashObj));
    }

    private IEnumerator FlashRoutine(GameObject flashObj)
    {
        flashObj.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        flashObj.SetActive(false);
    }

    public void KnockBack(float force)
    {
        GetComponent<PhysicsObject>().AddForce((transform.position - latestDamageObj.transform.position).normalized * force);
    }

    #endregion Common Effects Functions
}

[System.Serializable]
public class DamageType
{
    public string tag;
    public bool isTrigger;
    public bool isCollider;
    public float damage;
    public UnityEvent onDamaged;
    public UnityEvent onDeath;
}
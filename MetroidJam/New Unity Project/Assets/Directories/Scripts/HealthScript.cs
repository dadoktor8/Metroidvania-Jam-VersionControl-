using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HealthScript : MonoBehaviour
{
    public bool interactable = true;

    [Header("Display Variables")]
    public Image healthBar; 

    [Header("Health Settings")]
    public float totalHealth;
    private float currHealth;

    public List<HitType> hitTypeList;

    private Collider2D latestHitObj;

    private void Awake()
    {
        ResetHealth();
    }

    public void ResetHealth()
    {
        currHealth = totalHealth;
        UpdateHealthDisplay();
    }

    public void ProcessHit(Collider2D sourceObj, string sourceTag)
    {
        for (int i = 0; i < hitTypeList.Count; i++)
        {
            if (sourceTag == hitTypeList[i].tag)
            {
                latestHitObj = sourceObj;
                ProcessHit(hitTypeList[i]);
                return;
            }
        }
    }

    public void ProcessHit(HitType hitType)
    {
        ProcessHit(hitType.hitValue);
        hitType.onHit.Invoke();
        if (currHealth <= 0)
            hitType.onDeath.Invoke();
    }

    public void ProcessHit(float hitValue)
    {
        currHealth += hitValue;
        currHealth = Mathf.Clamp(currHealth, 0f, totalHealth);
        UpdateHealthDisplay();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!interactable)
            return;

        for (int i = 0; i < hitTypeList.Count; i++)
        {
            if (collision.tag == hitTypeList[i].tag && hitTypeList[i].isTrigger)
            {
                latestHitObj = collision;
                ProcessHit(hitTypeList[i]);
                return;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!interactable)
            return;

        for (int i = 0; i < hitTypeList.Count; i++)
        {
            if (collision.gameObject.tag == hitTypeList[i].tag && hitTypeList[i].isCollider)
            {
                latestHitObj = collision.collider;
                ProcessHit(hitTypeList[i]);
                return;
            }
        }
    }

    public void UpdateHealthDisplay()
    {
        if(healthBar != null)
            healthBar.fillAmount = Mathf.Clamp01(currHealth / totalHealth);
    }

    #region Common Effects Functions

    public GameObject GetLatestHitObject()
    {
        return latestHitObj.gameObject;
    }

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
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            Vector2 forceVector = new Vector2();
            forceVector.x = (transform.position - latestHitObj.transform.position).normalized.x;
            forceVector.y = 0.35f;
            rb.AddForce(forceVector * force);
        }
    }

    public void RemoveHitObject(bool isDestroy)
    {
        if (isDestroy)
            Destroy(latestHitObj.gameObject);
        else
            latestHitObj.gameObject.SetActive(false);
        latestHitObj = null;
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public void PlaySfx(string sfxName)
    {
        AudioManager.instance.Play(sfxName);
    }

    public void ReloadScene(float delay)
    {
        if (delay > 0)
            Invoke("ReloadScene", delay);
        else
            ReloadScene();
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion Common Effects Functions
}

[System.Serializable]
public class HitType
{
    public string tag;
    public bool isTrigger;
    public bool isCollider;
    public float hitValue;
    public UnityEvent onHit;
    public UnityEvent onDeath;
}
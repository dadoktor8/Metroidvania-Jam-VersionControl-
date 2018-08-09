using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropScript : MonoBehaviour
{
    public DropType type;
    public GameObject prefab;

    [Range(0f, 1f)]
    public float dropChance = 1f;

    private void OnDisable()
    {
        if (type != DropType.OnDisable)
            return;
        DropItem();
    }

    private void OnDestroy()
    {
        if (Application.isPlaying || type != DropType.OnDestroy)
            return;
        DropItem();
    }

    private void DropItem()
    {
        if ((float)Random.Range(0, 101) / 100f > dropChance)
            return;
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}

public enum DropType
{
    OnDestroy,
    OnDisable
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public List<string> tagFilter;

    private Collider2D triggerObj;

    public Collider2D TriggerObj
    {
        get
        {
            return triggerObj;
        }
    }

    public bool IsTriggered
    {
        get
        {
            return triggerObj != null;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(tagFilter.Count == 0 || tagFilter.Contains(collision.tag))
            triggerObj = collision;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(triggerObj == collision)
            triggerObj = null;
    }
}
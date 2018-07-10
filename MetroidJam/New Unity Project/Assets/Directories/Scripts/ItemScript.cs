using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public ItemData itemData;
    public AnimationCurve audioVolumeCurve;

    private AudioSource audioSource;
    private TriggerScript pickUpTrigger;

    private float enterDistToCenter = 0; //used to calculate volume

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        pickUpTrigger = transform.Find("item_sprite").GetComponent<TriggerScript>();
    }

    
    private void Update()
    {
        CheckForPickUp();
    }

    private void CheckForPickUp()
    {
        if(pickUpTrigger.IsTriggered && pickUpTrigger.TriggerObj.tag == "Player" && Input.GetKeyDown(KeyCode.X))
        {
            pickUpTrigger.TriggerObj.GetComponent<InventoryScript>().AddToInventory(itemData);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            audioSource.volume = 0;
            enterDistToCenter = (pickUpTrigger.transform.position - collision.transform.position).magnitude;
            audioSource.Play();
        }
    }

    //assuming collider2d is always circle collider
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            float distToCenter = (pickUpTrigger.transform.position - collision.transform.position).magnitude;
            audioSource.volume = audioVolumeCurve.Evaluate(1f - (distToCenter / enterDistToCenter));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            audioSource.Stop();
        }
    }
}
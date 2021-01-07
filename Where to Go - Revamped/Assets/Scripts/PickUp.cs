using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    //public ParticleSystem effect;
    //public bool pickedUp = false;
    public new BoxCollider2D collider2D;
    public new string tag;
    private void Start()
    {
        collider2D = gameObject.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
    }

    public void GetPickedUp()
    {
        StartCoroutine(PickupRoutine(gameObject.transform.GetChild(1).gameObject, 5f));        
    }

    IEnumerator PickupRoutine(GameObject obj,float time)
    {
        tag = obj.tag;
        obj.SetActive(false);
        collider2D.enabled = false;
        yield return new WaitForSeconds(time);
        collider2D.enabled = true;
        obj.SetActive(true);      
    }

    private void Update()
    {
        if (GameManager.instance.player.GetComponent<BoxCollider2D>().IsTouching(collider2D) && collider2D.enabled)
        {
            GameManager.instance.player.GetComponent<playerMovement>().currentStance = tag == "frost" ? playerMovement.Stances.Frost : playerMovement.Stances.Fire;
            GetPickedUp();
        }        
    }
}

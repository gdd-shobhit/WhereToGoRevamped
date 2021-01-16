using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    //public ParticleSystem effect;
    //public bool pickedUp = false;
    public new Collider collider;
    public new string tag;
    private void Start()
    {
        tag = gameObject.transform.GetChild(0).gameObject.tag;
        collider = gameObject.GetComponent<Collider>();
    }

    public void GetPickedUp()
    {
        StartCoroutine(PickupRoutine(gameObject.transform.GetChild(0).gameObject, 5f));        
    }

    IEnumerator PickupRoutine(GameObject obj , float time)
    {
        obj.SetActive(false);
        collider.enabled = false;
        yield return new WaitForSeconds(time);
        collider.enabled = true;
        obj.SetActive(true);      
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            GameManager.instance.player.GetComponent<playerMovement>().currentStance = tag == "frost" ? playerMovement.Stances.Frost : playerMovement.Stances.Fire;
            GetPickedUp();
        }
    }
}

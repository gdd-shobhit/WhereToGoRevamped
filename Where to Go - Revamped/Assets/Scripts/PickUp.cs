using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    //public ParticleSystem effect;
    //public bool pickedUp = false;
    public void GetPickedUp()
    {
        gameObject.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(PickupRoutine(gameObject.transform.GetChild(1).gameObject, 5f));
        gameObject.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    IEnumerator PickupRoutine(GameObject obj,float time)
    {
        Debug.Log("just before");
        obj.SetActive(false);
        yield return new WaitForSeconds(time);
        obj.SetActive(true);
        Debug.Log("just after");
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            if (gameObject.name == "Frost")
            {
                Debug.Log("" + gameObject.name);
                collision.gameObject.GetComponent<playerMovement>().currentStance = playerMovement.Stances.Frost;
            }
            else
            {
                collision.gameObject.GetComponent<playerMovement>().currentStance = playerMovement.Stances.Fire;
                Debug.Log("" + gameObject.name);
            }

            GetPickedUp();
        }  
    }
}

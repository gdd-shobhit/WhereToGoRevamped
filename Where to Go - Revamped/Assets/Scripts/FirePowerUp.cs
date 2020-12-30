using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePowerUp : PickUp
{
    private void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (gameObject.tag)
        {
            case "Fire":GameManager.instance.currentStance = GameManager.Stances.Fire;
                break;
            case "Frost":
                GameManager.instance.currentStance = GameManager.Stances.Frost;
                break;
            default:
                break;
        }
        GetPickedUp(this.gameObject);
    }

}

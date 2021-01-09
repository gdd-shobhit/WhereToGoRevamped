using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sawRotator : MonoBehaviour
{
    public GameObject player;
    public Collider boxCollider;
    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.Rotate(Vector3.forward, -10f * GameManager.instance.timeMultiplier);
    }
    private void OnTriggerEnter(Collider collision)
    {                
           collision.gameObject.tag = "dead";       
    }
}

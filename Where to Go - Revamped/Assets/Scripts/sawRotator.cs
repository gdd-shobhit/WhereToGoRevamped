﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sawRotator : MonoBehaviour
{
    public GameObject player;
    public BoxCollider2D boxCollider;
    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.Rotate(Vector3.forward, -10f * GameManager.instance.timeMultiplier);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {                
           collision.gameObject.tag = "dead";       
    }
}

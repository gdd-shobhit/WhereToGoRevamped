using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float lifetime;
    private float destructTimer;

    // Update is called once per frame
    void FixedUpdate()
    {
        destructTimer += Time.deltaTime;
        if (destructTimer >= lifetime) Destroy(gameObject);
    }
}

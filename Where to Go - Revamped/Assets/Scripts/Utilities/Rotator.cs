using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    void FixedUpdate()
    {
        gameObject.transform.Rotate(Vector3.forward, -10f);
    }
}

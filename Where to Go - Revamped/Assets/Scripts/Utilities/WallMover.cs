using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMover : MonoBehaviour
{
    private Rigidbody rb;
    public float wallSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(new Vector3(0 ,0 , -wallSpeed), ForceMode.Acceleration);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 100f);
        if(gameObject.transform.position.z < -250f)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 500);
        }
    }
}

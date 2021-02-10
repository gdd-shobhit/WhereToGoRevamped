using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HomeObjects : MonoBehaviour
{
    
    public Transform targetTransform;

    private Rigidbody rb;

    public ParticleSystem ps;
    public float speed = 5f;
    public bool isRight = true;
    public float timer = 0;
    public float rotateSpeed =200f;
    // Start is called before the first frame update
    
    void Start()
    {
        //targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        targetTransform = GameObject.Find("Player").GetComponent<Transform>();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (targetTransform != null)
        {
            Vector3 direction = targetTransform.position - rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            //rb.angularVelocity = new Vector3(0,0,-rotateAmount * rotateSpeed * GameManager.instance.timeMultiplier);
            rb.rotation = Quaternion.FromToRotation(transform.position.normalized, direction);          
            rb.velocity = transform.up * speed * GameManager.instance.timeMultiplier;

        }
        if (timer > 10)
        {
            gameObject.tag = "dead";
            timer = 0;
        }
        DeathMangager();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "alive")
        {
            collision.gameObject.GetComponent<playerMovement>().life--;
            if(collision.gameObject.GetComponent<playerMovement>().life <= 0)
            collision.gameObject.tag = "dead";
        }
        gameObject.tag = "dead";
    }
    void DeathMangager()
    {
        if (gameObject.tag == "dead")
        {
            //Vector3 bloodVector = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1);
            ///Instantiate(ps, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}

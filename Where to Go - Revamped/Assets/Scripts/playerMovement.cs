﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public GameObject grapplePrefab;
    private GameObject grapple;
    private LineRenderer grappleLine;
    public Material m_Line;
    private float GRAPPLE_DISTANCE = 4.0f;
    private Vector3 grappleDestination;
    private Vector3 futureDirection;

    public Rigidbody2D rb;
    public float speed;
    public int jumps=2;
    public int extraJumpValue;
    private float movementHorizontal=0f;
    private float movementVertical=0f;
    private bool isGrappling = false;
    private bool grappleHit = false;
    public bool facingRight = true;
    public bool isGrounded;
    public bool isWalled;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public Transform feetPos;
    public float feetRadius;
    public float jumpForce=12f;

    public Vector2 finalForce;
    private float clampVel;
    public ParticleSystem skull;
    public ParticleSystem deathBlood;
    public bool fireStance;
    public bool frostStance;
    //public int localExtraJumps;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grappleLine = gameObject.AddComponent<LineRenderer>();
        clampVel = 9f;
        finalForce = Vector2.zero;
        futureDirection = Vector3.zero;
        grappleLine.material = m_Line;
        grappleLine.startWidth = 0.20f;
        grappleLine.enabled = false;
        grapple = Instantiate(grapplePrefab, transform.position, Quaternion.identity);
        grapple.SetActive(false);
        jumps = extraJumpValue+1;
        fireStance = false;
        frostStance = false;
    }

    void Update()
    {
        // making the player always visible because it sometimes go off the z axis and isnt visible
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if (!isGrappling)
        {
            InitializeGrapple();
            Movement();  
           
        }
        else
        {
            ShootGrapple();
        }
        FireStance();
        FrostStance();
        DeathMangager();

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void Movement()
    {
        finalForce = Vector2.zero;
        movementHorizontal = Input.GetAxis("Horizontal");
        movementVertical = Input.GetAxis("Vertical");
        futureDirection = new Vector3(movementHorizontal, movementVertical, 0);
        finalForce += new Vector2(movementHorizontal * speed * Time.deltaTime , 0) ;
                 
        jumps = (IsGrounded() || IsWalled()) ? extraJumpValue+1: jumps;

        if (Input.GetKeyDown(KeyCode.W) && jumps>0)
        {
            rb.AddForce(Vector2.up * jumpForce * Time.deltaTime , ForceMode2D.Impulse);       
            jumps--;
        }

        if (movementHorizontal > 0 && facingRight == false) Flip();
        else if (movementHorizontal < 0 && facingRight == true) Flip();

        // Applying Force
        rb.AddForce(finalForce, ForceMode2D.Force);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, clampVel);
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(feetPos.position, Vector2.down);
        if (hit)
        {
            isGrounded = hit.rigidbody.gameObject.layer==8 && hit.distance < 0.02f;
        }
        return isGrounded;
    }

    bool IsWalled()
    {
        isWalled = Physics2D.OverlapCircle(feetPos.position, feetRadius, whatIsWall);
        return isWalled;
    }

    void DeathMangager()
    {
        if (gameObject.tag == "dead")
        {
            Vector3 bloodVector = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1);
            Instantiate(deathBlood, bloodVector, Quaternion.identity);
            gameObject.SetActive(false);
            new WaitForSeconds(2);
            gameObject.transform.position = new Vector3(0, 4, 1);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            gameObject.SetActive(true);
           
        }

        if (gameObject.transform.position.y < -15f)
        {
            gameObject.transform.position = new Vector3(0, 4, 1);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void FireStance()
    {
        if (fireStance)
        {
            // checking for fire stance
            if (Input.GetKeyDown(KeyCode.F))
            {
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
                StartCoroutine(FirePower(3, gameObject.transform.GetChild(2).gameObject));
            }              
        }   
    }

    void FrostStance()
    {
        if (frostStance)
        {

        }
    }

    IEnumerator FirePower(float time, GameObject fireObject)
    {       
        yield return new WaitForSeconds(time);
        fireObject.SetActive(false);
        fireStance = false;      
    }

    //Initialize the grapple
    void InitializeGrapple()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
            grappleDestination = mousePos - this.transform.position;
            grappleDestination.Normalize();
            grappleDestination = (grappleDestination * GRAPPLE_DISTANCE) + transform.position;
            grapple.transform.position = transform.position;
            grapple.SetActive(true);
            isGrappling = true;
            rb.gravityScale = 0.2f;
            rb.velocity *= 0.2f;
            grappleLine.enabled = true;
            grappleLine.SetPosition(0, transform.position);
            grappleLine.SetPosition(1, transform.position);
        }
    }

    //Called while grappling
    void ShootGrapple()
    {
        if (!grappleHit)
        {
            grapple.transform.position = Vector3.Lerp(grapple.transform.position, grappleDestination, 0.06f);
            grappleLine.SetPosition(1, grapple.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(grapple.transform.position, grappleDestination, 0.05f);
            if (hit)
                if (hit.rigidbody.gameObject.tag != "alive")
                    grappleHit = true;
            if (Vector2.Distance(grappleDestination, grapple.transform.position) < 0.2f)
                EndGrapple();
          
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, grappleDestination, 0.06f);
            grappleLine.SetPosition(0, transform.position);
            if (Vector2.Distance(grapple.transform.position, transform.position) < 0.2f)
                EndGrapple();
        }
    }

    private void EndGrapple()
    {
        isGrappling = false;
        grappleHit = false;
        grappleLine.enabled = false;
        grapple.SetActive(false);
        rb.gravityScale = 1;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    public enum Stances
    {
        Fire,
        Frost,
        Normal
    }

    public Material m_Line;
    public Material m_Fire;
    public Material m_Frost;
    public Material m_player;
    private Vector3 futureDirection;

    public Stances currentStance = Stances.Normal;

    public Rigidbody rb;
    public Vector3 centerOfMassActual;
    public float speed;
    public int jumps = 2;
    public int extraJumpValue;
    private float movementHorizontal = 0f;
    private float movementVertical = 0f;
    public bool facingRight = true;
    public bool isGrounded = false;
    public bool isWalled;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public Transform feetPos;
    public float feetRadius;
    public float jumpForce = 12f;

    public Vector3 finalForce;
    private float clampVel;
    public ParticleSystem skull;
    public ParticleSystem deathBlood;

    //Grapple Data
    [Header("Grapple")]
    [SerializeField] float GRAPPLE_DISTANCE = 4.0f;
    [SerializeField] float GRAPPLE_COOLDOWN;
    [SerializeField] GameObject grapplePrefab;
    [SerializeField] GameObject grappleIndicatorPrefab;
    private GameObject grapple;
    private LineRenderer grappleLine;
    private Vector3 grappleDestination;
    private bool isGrappling = false;
    private float grappleTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grappleLine = gameObject.AddComponent<LineRenderer>();
        clampVel = 9f;
        finalForce = Vector2.zero;
        centerOfMassActual = Vector3.zero;
        rb.centerOfMass = centerOfMassActual;
        m_player = gameObject.GetComponent<MeshRenderer>().material;
        grappleLine.material = m_Line;
        grappleLine.startWidth = 0.20f;
        grappleLine.enabled = false;
        grapple = Instantiate(grapplePrefab, transform.position, Quaternion.identity);
        grapple.SetActive(false);
        jumps = extraJumpValue + 1;
    }

    void Update()
    {
        rb.angularVelocity = Vector3.zero;
        // making the player always visible because it sometimes go off the z axis and isnt visible
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if (!isGrappling)
        {
            if (Input.GetMouseButtonDown(0) && grappleTimer == 0) InitializeGrapple();
            Movement();
        }

        // Checkers
        ManageStances();

        if (gameObject.activeSelf || gameObject.tag!="test")
        {
            DeathMangager();
        }
    }

    private void ManageStances()
    {
        if (currentStance == Stances.Fire)
        {
            StartCoroutine(FireStance());
            currentStance = Stances.Normal;
        }
        else if (currentStance == Stances.Frost)
        {
            StartCoroutine(FrostStance());
            currentStance = Stances.Normal;
        }
        else if (currentStance == Stances.Normal)
        {
            NormalStances();
        }

    }
    IEnumerator FireStance()
    {
        gameObject.tag = "immortal";
        gameObject.GetComponent<MeshRenderer>().material = m_Fire;
        yield return new WaitForSeconds(3);
        gameObject.tag = gameObject.tag != "test" ? "alive" : gameObject.tag;
        gameObject.GetComponent<MeshRenderer>().material = m_player;
        currentStance = Stances.Normal;
    }

    IEnumerator FrostStance()
    {
        GameManager.instance.timeMultiplier *= 0.25f;
        gameObject.GetComponent<MeshRenderer>().material = m_Frost;
        yield return new WaitForSeconds(3);
        gameObject.GetComponent<MeshRenderer>().material = m_player;
        GameManager.instance.timeMultiplier = 1.0f;
        currentStance = Stances.Normal;
    }

    private void NormalStances()
    {

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
        finalForce = new Vector3(movementHorizontal * speed * Time.deltaTime, 0);

        jumps = (IsGrounded() || IsWalled()) ? extraJumpValue + 1 : jumps;

        if (Input.GetKeyDown(KeyCode.W) && jumps > 0)
        {
            rb.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
            //transform.position += Vector3.up * jumpForce * Time.deltaTime;
            jumps--;
        }

        if (movementHorizontal > 0 && facingRight == false) Flip();
        else if (movementHorizontal < 0 && facingRight == true) Flip();

        // Applying Force
        rb.AddForceAtPosition(finalForce, gameObject.transform.position,ForceMode.Force);
        // rb.AddForce(finalForce, ForceMode.Force);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, clampVel);

        // for kinetic object
        // transform.position += new Vector3(movementHorizontal, 0, 0);

    }

    bool IsGrounded()
    {
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(feetPos.position, Vector3.down, out hitInfo);       

        if ( hit && hitInfo.rigidbody != null)
        {
            isGrounded = hitInfo.rigidbody.gameObject.layer == 8 && hitInfo.distance < 0.02f;
        }
        return isGrounded;
    }

    bool IsWalled()
    {
        Collider[] collidersHit = Physics.OverlapSphere(feetPos.position, feetRadius);
        //Collider[] collidersHit = Physics.OverlapBox(feetPos.position,)
        foreach (Collider collider in collidersHit)
        {
            isWalled = collider.gameObject.layer == 9;
        }
        
        return isWalled;
    }

    void DeathMangager()
    {
        //if (gameObject.tag == "dead")
        //{
        //    Vector3 bloodVector = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1);
        //    Instantiate(deathBlood, bloodVector, Quaternion.identity);
        //    gameObject.SetActive(false);
        //    new WaitForSeconds(2);
        //    gameObject.transform.position = new Vector3(0, 4, 1);
        //    gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        //    gameObject.SetActive(true);

        //}
        if(gameObject.tag=="dead")
        Respawn();

        if (gameObject.transform.position.y < -15f)
        {
            gameObject.transform.position = new Vector3(0, 4, 0);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void Respawn()
    {
            gameObject.SetActive(true);
            gameObject.transform.position = new Vector3(0, 3, 0);
            gameObject.tag = "alive";
    }

    //Initialize the grapple
    void InitializeGrapple()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        grappleDestination = mousePos - transform.position;
        grappleDestination = new Vector3(grappleDestination.x, grappleDestination.y, transform.position.z);
        grappleDestination.Normalize();
        grappleDestination = (grappleDestination * GRAPPLE_DISTANCE) + transform.position;
        //Debug.Log(Mathf.Atan2(grappleDestination.y - transform.position.y, grappleDestination.x - transform.position.x) * Mathf.Rad2Deg);
        grapple.transform.position = transform.position;
        grapple.SetActive(true);
        isGrappling = true;
        rb.gravityScale = 0.2f;
        rb.velocity *= 0.2f;
        grappleLine.enabled = true;
        grappleLine.SetPosition(0, transform.position);
        grappleLine.SetPosition(1, transform.position);
        StartCoroutine("ShootGrapple");
    }

    //Called while grappling
    IEnumerator ShootGrapple()
    {
        bool grappleHit = false;
        while (Vector2.Distance(grappleDestination, grapple.transform.position) > 0.2f)
        {
            grapple.transform.position = Vector3.Lerp(grapple.transform.position, grappleDestination, 0.06f);
            grappleLine.SetPosition(1, grapple.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(grapple.transform.position, grappleDestination, 0.05f);
            if (hit)
            {
                if (hit.rigidbody.gameObject.tag != "alive")
                {
                    StartCoroutine("ReturnGrapple");
                    grappleHit = true;
                    break;
                }
            }
            yield return null;
        }
        if (!grappleHit) EndGrapple();
    }

    IEnumerator ReturnGrapple()
    {
        while (Vector2.Distance(grapple.transform.position, transform.position) > 0.2f)
        {
            transform.position = Vector3.Lerp(transform.position, grappleDestination, 0.06f);
            grappleLine.SetPosition(0, transform.position);
            yield return null;
        }
        grappleTimer += 0.01f;
        StartCoroutine("BeginGrappleTimer");
        EndGrapple();
    }

    IEnumerator BeginGrappleTimer()
    {
        Color initColor = m_player.color;
        Color currentColor = new Color(initColor.r / 100.0f, initColor.g / 100.0f, initColor.b / 100.0f);
        float r, g, b;
        while (grappleTimer < GRAPPLE_COOLDOWN)
        {
            grappleTimer += Time.deltaTime;
            r = Mathf.Lerp(currentColor.r, initColor.r, Time.deltaTime);
            g = Mathf.Lerp(currentColor.g, initColor.g, Time.deltaTime);
            b = Mathf.Lerp(currentColor.b, initColor.b, Time.deltaTime);
            currentColor = new Color(r,g,b);
            m_player.SetColor("_EmissionColor", currentColor);
            yield return null;
        }
        grappleTimer = 0;
        m_player.SetColor("_EmissionColor", initColor);
        Instantiate(grappleIndicatorPrefab, transform);
    }

    private void EndGrapple()
    {
        isGrappling = false;
        grappleLine.enabled = false;
        grapple.SetActive(false);
    }
}

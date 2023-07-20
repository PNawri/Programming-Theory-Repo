using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpPower;
    [SerializeField] float gravityMod;
    [SerializeField] GameObject sword;
    [SerializeField] float turnSpeed;
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera secondCamera;
    [SerializeField] GameObject polySurface1;
    
    public bool defUp;
    static public Rigidbody playerRb;
    
    float horizontalInput;
    float verticalInput;
    float defMove = 1;
    float attackDelay = 0.4f;
    float nextAttackTime = 0;
    bool onGround;
    Animator animator;
    Collider swordCollider;
    SkinnedMeshRenderer playerMesh;
    Vector3 lastPos;
    Vector2 turn;
    Vector3 zoom;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityMod;
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        swordCollider = sword.GetComponent<Collider>();
        playerMesh = polySurface1.GetComponent<SkinnedMeshRenderer>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        lastPos = transform.position;

        Movement();

        //check player position to actived moving animation
        if (transform.position != lastPos)
        {
            animator.SetBool("isMoving", true);
        }

        else
        {
            animator.SetBool("isMoving", false);
        }

        //limit attack per sec
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + attackDelay;
            }
        }

        //def up
        if (Input.GetMouseButtonDown(1) && onGround)
        {
            Defend(true);
        }

        //def down
        if (Input.GetMouseButtonUp(1))
        {
            Defend(false);
        }

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && onGround && !defUp)
        {
            playerRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            onGround = false;
        }

        //change camera
        zoom.z = Input.GetAxis("Mouse ScrollWheel");

        if (zoom.z > 0) //first person view
        {
            mainCamera.enabled = false;
            secondCamera.enabled = true;
            playerMesh.enabled = false;
        }

        if (zoom.z < 0) //third person view
        {
            mainCamera.enabled = true;
            secondCamera.enabled = false;
            playerMesh.enabled = true;
        }
    }
    

    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        transform.Translate(movementDirection * speed * Time.deltaTime * defMove);

        turn.x += Input.GetAxis("Mouse X") * turnSpeed;
        transform.localRotation = Quaternion.Euler(0, turn.x, 0);
                
    }

    void Attack()
    {
        animator.SetTrigger("attack_trig");
        swordCollider.enabled = true;
    }

    public void AfterAttack() //animationevent
    {
        swordCollider.enabled = false;
    }

    void Defend(bool def)
    {
        animator.SetBool("isDef", def);

        if (def)
        {
            defUp = true;
            defMove = 0.5f;
        }
        else
        {
            defUp = false;
            defMove = 1;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }
}

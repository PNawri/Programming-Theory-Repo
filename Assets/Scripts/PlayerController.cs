using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;    
    private Animator motion;
    private Rigidbody playerRb;
    private bool canMove;
    private float defDis = 1;
    private bool onGround;
        
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float gravityMod;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityMod;
        playerRb = GetComponent<Rigidbody>();
        motion = GetComponent<Animator>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {        
        if (canMove)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up * horizontalInput * turnSpeed * Time.deltaTime * defDis);

            verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector3.forward * verticalInput * speed * Time.deltaTime * defDis);
        }
       
        if(Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if(Input.GetMouseButtonDown(0) && onGround)
        {
            Attack();
            canMove = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Defend(true);
            defDis = 0.5f;
        }

        if(Input.GetMouseButtonUp(1))
        {
            Defend(false);
            defDis = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            playerRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            onGround = false;
        }

        if (verticalInput != 0)
        {
            motion.SetBool("isMoving", true);
        }

        else
        {
            motion.SetBool("isMoving", false);            
        }
    }

    void Attack()
    {
        motion.SetTrigger("attack_trig");
        
    }

    void Defend(bool x)
    {
        motion.SetBool("isDef", x);
    }

    public void AfterAttack() //animationevent
    {
        canMove = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }

        
    }
}

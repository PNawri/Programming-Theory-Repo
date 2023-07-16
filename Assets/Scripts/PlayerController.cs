using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float turnSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float gravityMod;
    [SerializeField] float pushForce;
    [SerializeField] int maxHP;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] GameObject sword;
    [SerializeField] AudioClip getHit;
    [SerializeField] AudioClip playerDieSound;
    
    public int maxAttack;
    public int minAttack;

    float horizontalInput;
    float verticalInput;    
    float defMove = 1;
    float attackDelay = 0.4f;
    float nextAttackTime = 0;
    int defCoef = 1;
    int currentHP;
    bool canMove;
    bool onGround;
    bool defUp;
    bool playerDie;
    Animator animator;
    Rigidbody playerRb;
    Collider swordCollider;
    Vector3 lastPos;
    AudioSource playerAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityMod;
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        canMove = true;
        nameText.text = GameManager.Instance.playerName;
        swordCollider = sword.GetComponent<Collider>();
        currentHP = maxHP;
        playerAudio = GetComponent<AudioSource>();
        playerDie = false;
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = "HP: " + currentHP;
        lastPos = transform.position;

        if (canMove)
        {
            Movement();
        }
       
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + attackDelay;
            }                      
        }
        
        if(Input.GetMouseButtonDown(1) && onGround)
        {
            Defend(true);            
        }

        if(Input.GetMouseButtonUp(1))
        {
            Defend(false);
        }

        if(Input.GetKeyDown(KeyCode.Space) && onGround && !defUp)
        {
            playerRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            onGround = false;
        }

        if(currentHP <= 0 && !playerDie)
        {
            PlayerDie();
        }        

        if(transform.position != lastPos)
        {
            animator.SetBool("isMoving", true);
        }

        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * horizontalInput * turnSpeed * Time.deltaTime * defMove);

        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * verticalInput * speed * Time.deltaTime * defMove);
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

        if(def)
        {
            defCoef = 3;
            pushForce *= 0.5f;
            defUp = true;
            defMove = 0.5f;
        }
        else
        {
            defCoef = 1;
            pushForce *= 1;
            defUp = false;
            defMove = 1;
        }            
    }

    void OnCollisionEnter(Collision collision)
    {   
        if(collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }     
        
        if(collision.gameObject.CompareTag("Monster"))
        {
            var monsterDie = collision.gameObject.GetComponent<DamageManager>().die;

            if(!monsterDie)
            {
                if(!defUp)
                {
                    animator.SetTrigger("getHit");
                }

                var atk = collision.gameObject.GetComponent<DamageManager>().attack;
                var monsterTransform = collision.gameObject.GetComponent<Transform>();
                Vector3 pushDirection = (transform.position - monsterTransform.position).normalized;
                playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
                currentHP -= atk / defCoef;
                playerAudio.PlayOneShot(getHit, 1f);
                Debug.Log("Player HP:" + currentHP);
            }
        }
    }
    
    void PlayerDie()
    {
        var monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach(GameObject target in monsters)
        {
            GameObject.Destroy(target);
        }

        playerDie = true;
        animator.SetTrigger("playerDie");
        playerAudio.PlayOneShot(playerDieSound, 1f);
        StartCoroutine(DelayGameOver(2));
    }

    IEnumerator DelayGameOver(int sec)
    {
        yield return new WaitForSeconds(sec);
        GameOver();
    }

    void GameOver()
    {
        SceneManager.LoadScene(2);
    }  

}

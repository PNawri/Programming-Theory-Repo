using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [SerializeField] int maxHP;
    [SerializeField] float pushForce;

    public int attack;
    public bool die;
    public int percentHP;

    int currentHP;
    Animator animator;
        
    void Start()
    {
        die = false;
        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentHP <= 0 && !die)
        {
            Die();
        }

        percentHP = (currentHP / maxHP) * 100;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword") && !die)
        {
            var playerMaxAttack = GameObject.Find("Player").GetComponent<PlayerController>().maxAttack;
            var playerMinAttack = GameObject.Find("Player").GetComponent<PlayerController>().minAttack;
            currentHP -= Random.Range(playerMinAttack, playerMaxAttack);
            var monsterRb = GetComponent<Rigidbody>();
            var playerTransform = GameObject.Find("Player").GetComponent<Transform>();
            Vector3 pushDirection = (transform.position - playerTransform.position).normalized;
            animator.SetTrigger("GetHit");
            monsterRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            Debug.Log(gameObject.name + " HP " + currentHP);
        }
    }

    public void Die()
    {
        die = true;
        var monsters = GetComponent<Monsters>().enabled = false;  
        animator.SetTrigger("isDeath");
        StartCoroutine(DelayBeforDestroy(3));        
    }

    IEnumerator DelayBeforDestroy(int sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(gameObject);
    }

}

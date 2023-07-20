using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersHealth : MonoBehaviour
{
    [SerializeField] int maxHP;
    [SerializeField] float pushBack;

    public int attack;
    public bool die = false;
    //public int percentHP;

    int currentHP;
    Animator animator;
        
    void Start()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentHP <= 0 && !die)
        {
            Die();
        }

        //percentHP = (currentHP / maxHP) * 100;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword") && !die)
        {
            var playerMaxAttack = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>().maxAttack;
            var playerMinAttack = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>().minAttack;
            int amount = Random.Range(playerMinAttack, playerMaxAttack);
            var monsterRb = GetComponent<Rigidbody>();
            var playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
            Vector3 pushDirection = (transform.position - playerTransform.position).normalized;
            monsterRb.AddForce(pushDirection * pushBack, ForceMode.Impulse);
            GetHit(amount);
        }
    }

    void GetHit(int amount)
    {
        currentHP -= amount;
        animator.SetTrigger("GetHit");
        Debug.Log(gameObject.name + " HP " + currentHP);
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

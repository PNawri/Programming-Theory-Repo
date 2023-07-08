using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsters : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected int maxHP;
    
    protected int currentHP;
    protected int maxDamage = 10;
    protected int minDamage = 8;
    protected Transform player;
    protected Rigidbody monsterRb;

    // Start is called before the first frame update
    protected void Start()
    {
        monsterRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        currentHP = maxHP;
    }

    // Update is called once per frame
    protected void Update()
    {
        Move();

        if (currentHP < 1)
        {
            Die();
        }
    }

    protected virtual void Move()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        monsterRb.AddForce(lookDirection * speed);

        transform.LookAt(player);
    }

    protected void OnTriggerEnter(Collider other)
    {
        other.CompareTag("Sword");
        currentHP -= Random.Range(minDamage, maxDamage);
        Debug.Log( gameObject.name + " HP: " + currentHP);
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}

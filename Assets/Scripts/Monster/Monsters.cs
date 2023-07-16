using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsters : MonoBehaviour
{
    [SerializeField] protected float speed;

    protected Transform player;
    protected Rigidbody monsterRb;

    // Start is called before the first frame update
    void Start()
    {
        monsterRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    
    protected virtual void Move()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        monsterRb.AddForce(lookDirection * speed * Time.deltaTime);

        transform.LookAt(player);
                
    }    
}

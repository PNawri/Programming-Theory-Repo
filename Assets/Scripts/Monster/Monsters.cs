using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsters : MonoBehaviour
{
    [SerializeField] protected float speed;

    protected Transform player;
    protected Rigidbody monsterRb;

    // Start is called before the first frame update
    protected void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        monsterRb = GetComponent<Rigidbody>();
    }

    
    protected virtual void Move()
    {
        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squid : Monsters
{
    [SerializeField] GameObject bomb;
    [SerializeField] GameObject bombSpawner;
    
    float bombRate = 2;
    float attackRange = 6;
    float bombForce = 10;
    bool bombReady = true;

    private void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        if (dist > attackRange)
        {
            Move();            
        }

        if (dist <= attackRange && bombReady)
        {
            transform.LookAt(player);
            Bomb();
        }
    }

    void Bomb()
    {
        var insta = Instantiate(bomb, bombSpawner.transform.position, bomb.transform.rotation).GetComponent<Rigidbody>();
        insta.AddForce((player.transform.position - transform.position).normalized * bombForce, ForceMode.Impulse);
        insta.AddForce(Vector3.up * (bombForce / 3), ForceMode.Impulse);
        bombReady = false;
        StartCoroutine(BombDelay(bombRate));
    }

    IEnumerator BombDelay(float sec)
    {
        yield return new WaitForSeconds(sec);
        bombReady = true;        
    }
}

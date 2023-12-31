using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparrow : Monsters
{
    [SerializeField] float fly;
    
    private void Update()
    {
        Move();
    }

    protected override void Move() //POLYMORPHISM
    {
        base.Move();

        monsterRb.AddForce(Vector3.up * fly * Time.deltaTime);
        
        if(transform.position.y > 1)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Monsters // INHERITANCE
{
    Animator animator;
    [SerializeField] ParticleSystem flame;

    void Update()
    {
        Move();
    }

   /* void Fly()
    {
        canFly = false;
        animator = GetComponent<Animator>();
        animator.SetTrigger("Fly");
    }*/

    public void FlyFlame() //animationevent
    {
        flame.Play();
    }

}

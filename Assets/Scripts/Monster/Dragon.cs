using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Monsters // INHERITANCE
{
    Animator animator;
    bool canFly = true;
    public ParticleSystem flame;

    void Update()
    {
        Move();

        var dragonHP = GetComponent<DamageManager>().percentHP;

        if (dragonHP <= 50 && canFly)
        {
            Fly();
        }
    }

    void Fly()
    {
        canFly = false;
        animator = GetComponent<Animator>();
        animator.SetTrigger("Fly");
    }

    public void FlyFlame() //animationevent
    {
        flame.Play();
    }

}

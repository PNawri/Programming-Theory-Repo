using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] ParticleSystem boom;

    public int attack = 10;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyBomb(5));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Instantiate(boom, transform.position, transform.rotation).Play();
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyBomb(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(gameObject);
    }
}

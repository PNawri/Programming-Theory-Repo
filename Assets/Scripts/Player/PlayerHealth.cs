using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float pushBack;
    [SerializeField] int maxHP;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] AudioClip playerDieAudio;
    [SerializeField] AudioClip getHit;

    public int maxAttack;
    public int minAttack;
    public bool playerDie = false;

    Animator animator;
    AudioSource playerAudio;
    PlayerController playerController;
    int currentHP;

    void Start()
    {
        nameText.text = GameManager.Instance.playerName;
        currentHP = maxHP;
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        hpText.text = "HP: " + currentHP;

        if (currentHP <= 0 && !playerDie)
        {
            PlayerDie();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            var monsterDie = collision.gameObject.GetComponent<MonstersHealth>().die;

            if (!monsterDie)
            {
                if (!playerController.defUp)
                {
                    animator.SetTrigger("getHit");
                }

                var atk = collision.gameObject.GetComponent<MonstersHealth>().attack;
                var monsterTransform = collision.gameObject.GetComponent<Transform>();
                Vector3 pushDirection = (transform.position - monsterTransform.position).normalized;
                PlayerController.playerRb.AddForce(pushDirection * pushBack, ForceMode.Impulse);
                GetHit(atk);
            }
        }

        if (collision.gameObject.CompareTag("Bomb"))
        {
                if (!playerController.defUp)
                {
                    animator.SetTrigger("getHit");
                }

                var atk = collision.gameObject.GetComponent<Bomb>().attack;
                var bombTransform = collision.gameObject.GetComponent<Transform>();
                Vector3 pushDirection = (transform.position - bombTransform.position).normalized;
                PlayerController.playerRb.AddForce(pushDirection * pushBack, ForceMode.Impulse);
                GetHit(atk);            
        }
    }

    void GetHit(int amount)
    {
        currentHP -= amount;
        playerAudio.PlayOneShot(getHit, 1f);
        Debug.Log("Player HP:" + currentHP);
    }

    void PlayerDie()
    {
        playerController.enabled = false;
        playerDie = true;

        var monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject target in monsters)
        {
            GameObject.Destroy(target);
        }
                        
        animator.SetTrigger("playerDie");
        playerAudio.PlayOneShot(playerDieAudio, 1f);
        StartCoroutine(DelayGameOver(2));
        Cursor.lockState = CursorLockMode.Confined;
    }

    IEnumerator DelayGameOver(int sec)
    {
        yield return new WaitForSeconds(sec);
        GameOver();
    }

    void GameOver()
    {
        SceneManager.LoadScene(2);
    }
}

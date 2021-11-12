using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{

    public bool isActive;

    //This objects animator
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        ActivateTrap();
    }

    public void ActivateTrap()
    {
        if (isActive)
        {
            ActivateDamageTrap();
        }
    }

    private void ActivateDamageTrap()
    {
        if (!anim.GetBool("Active"))
        {
            anim.SetBool("Active", true);
        } else if (gameObject.GetComponent<Rigidbody2D>().gravityScale <= 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("Freed"))
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 2;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            anim.SetBool("isOnGround", true);
            gameObject.GetComponent<AudioSource>().Play();
            isActive = false;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
}

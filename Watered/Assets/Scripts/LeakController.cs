using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeakController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            StartCoroutine("TouchTheGround");
        }
    }

    IEnumerator TouchTheGround()
    {
        gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
}

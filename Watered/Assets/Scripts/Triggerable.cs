using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerable : MonoBehaviour
{

    public GameObject attachedElement;
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateElement()
    {
        if (isActive)
        {
            attachedElement.GetComponent<Traps>().isActive = true;
            gameObject.GetComponent<Animator>().SetBool("activated", true);
            gameObject.GetComponent<AudioSource>().Play();
            isActive = false;
        }
    }
}

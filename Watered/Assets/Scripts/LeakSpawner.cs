using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeakSpawner : MonoBehaviour
{

    public GameObject leak;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CreateLeak", 1f, 3f);
    }

    void CreateLeak()
    {
        Instantiate(leak, transform.position, Quaternion.identity);
    }
}

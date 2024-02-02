using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShellInstantiation : MonoBehaviour
{
    public GameObject prefab;

    public float testVar;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject newObject = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

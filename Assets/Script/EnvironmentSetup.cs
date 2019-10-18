using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSetup : MonoBehaviour
{
    void Awake()
    {
        Vector3 vSize = transform.Find("Terrain").gameObject.GetComponent<Renderer>().bounds.size;
        Vector3 vCurrentPosition = transform.position;
        transform.position = new Vector3(vCurrentPosition.x + vSize.x/2, vCurrentPosition.y, vCurrentPosition.z + vSize.z/2);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

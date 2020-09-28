using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour {
    
    public Transform sphereTransform;
    // Start is called before the first frame update
    void Start()
    {
        sphereTransform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3(0, 180 *Time.deltaTime, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform myCamera;

    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(transform.position.x, myCamera.position.y, myCamera.position.z));
    }
}

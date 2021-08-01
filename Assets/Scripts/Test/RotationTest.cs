using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    public GameObject testCube;   

    // Update is called once per frame
    void Update()
    {
        Vector3 eulerRotation = new Vector3(0, 0, transform.eulerAngles.z);
        testCube.transform.rotation = Quaternion.Euler(eulerRotation);
    }
}

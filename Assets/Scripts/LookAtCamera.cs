using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to make sure all text is always facing toward the user
public class LookAtCamera : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}

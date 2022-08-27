using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script used to centre an object between four points
public class AreaPointText : MonoBehaviour
{
    public GameObject pos1;
    public GameObject pos2;
    public GameObject pos3;
    public GameObject pos4;


    // Update is called once per frame
    void Update()
    {
        transform.position = (pos1.transform.position + pos2.transform.position + pos3.transform.position + pos4.transform.position) / 4f;
    }
}

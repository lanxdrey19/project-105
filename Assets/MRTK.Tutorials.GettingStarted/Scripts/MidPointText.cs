using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidPointText : MonoBehaviour
{
    // used to maintain position between two objects

    public GameObject pos1;
    public GameObject pos2;

    // Update is called once per frame
    void Update()
    {
        transform.position = (pos1.transform.position + pos2.transform.position) / 2f;
    }
}

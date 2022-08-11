using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class SummonToFinger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Summon(Vector3 pos)
    {
        transform.position = pos;
    }
}

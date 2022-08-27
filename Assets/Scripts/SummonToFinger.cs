using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

// Script to summon object to a specified Vector3 position
public class SummonToFinger : MonoBehaviour
{
    public void Summon(Vector3 pos)
    {
        transform.position = pos;
    }
}

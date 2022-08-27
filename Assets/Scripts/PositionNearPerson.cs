using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;

public class PositionNearPerson : MonoBehaviour
{
    private float distFace = 0.2f;
    private float distOffSet = 0.1f;

    // position the objects in front of the user camera with specified offset
    public void PositionInFront(string offset)
    {
        Vector3 position = CameraCache.Main.transform.position + CameraCache.Main.transform.forward * distFace;        

        switch (offset)
        {
            case "left":
                position -= CameraCache.Main.transform.right * distOffSet;
                break;
            case "right":
                position += CameraCache.Main.transform.right * distOffSet;
                break;
            case "up":
                position += CameraCache.Main.transform.up * distOffSet;
                break;
            case "down":
                position -= CameraCache.Main.transform.up * distOffSet;
                break;
            default:
                break;
        }

        transform.position = position;

    }
}

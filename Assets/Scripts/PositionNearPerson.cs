using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;

public class PositionNearPerson : MonoBehaviour
{
    public void PositionInFront(GameObject gameObject)
    {
        Vector3 position = CameraCache.Main.transform.position + CameraCache.Main.transform.forward * 0.2f;
        position = new Vector3(position.x, CameraCache.Main.transform.position.y, position.z);
        gameObject.transform.position = position;
    
    }
    public void PositionSlightLeft(GameObject gameObject)
    {
        Vector3 position = CameraCache.Main.transform.position + CameraCache.Main.transform.forward * 0.2f;
        position = new Vector3(position.x, CameraCache.Main.transform.position.y, position.z);
        gameObject.transform.position = position;
        gameObject.transform.Translate(-0.1f, 0f, 0f);
    }

    public void PositionSlightRight(GameObject gameObject)
    {

        Vector3 position = CameraCache.Main.transform.position + CameraCache.Main.transform.forward * 0.2f;
        position = new Vector3(position.x, CameraCache.Main.transform.position.y, position.z);
        gameObject.transform.position = position;
        gameObject.transform.Translate(0.1f, 0f, 0f);
    }

    public void PositionSlightLeftFar(GameObject gameObject)
    {

        Vector3 position = CameraCache.Main.transform.position + CameraCache.Main.transform.forward * 0.2f;
        position = new Vector3(position.x, CameraCache.Main.transform.position.y, position.z);
        gameObject.transform.position = position;
        gameObject.transform.Translate(-0.1f, -0.1f, 0.1f);

    }

    public void PositionSlightRightFar(GameObject gameObject)
    {
        Vector3 position = CameraCache.Main.transform.position + CameraCache.Main.transform.forward * 0.2f;
        position = new Vector3(position.x, CameraCache.Main.transform.position.y, position.z);
        gameObject.transform.position = position;
        gameObject.transform.Translate(0.1f, -0.1f, 0.1f);

    }


}

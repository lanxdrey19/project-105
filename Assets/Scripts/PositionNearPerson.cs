using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionNearPerson : MonoBehaviour
{
    public void PositionSlightLeft(GameObject gameObject)
    {
        
        Vector3 position = Camera.main.transform.position;
        gameObject.transform.position = position;
        gameObject.transform.Translate(-0.1f, 0f, 0f);
    }

    public void PositionSlightRight(GameObject gameObject)
    {

        Vector3 position = Camera.main.transform.position;
        gameObject.transform.position = position;
        gameObject.transform.Translate(0.1f, 0f, 0f);
    }

    public void PositionSlightLeftFar(GameObject gameObject)
    {

        Vector3 position = Camera.main.transform.position;
        gameObject.transform.position = position;
        gameObject.transform.Translate(-0.15f, 0.1f, 0.1f);
    }

    public void PositionSlightRightFar(GameObject gameObject)
    {

        Vector3 position = Camera.main.transform.position;
        gameObject.transform.position = position;
        gameObject.transform.Translate(0.15f, 0.1f, 0.1f);
    }


}

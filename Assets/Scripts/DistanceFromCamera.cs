using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class DistanceFromCamera : MonoBehaviour
{
    public GameObject mainCamera;
    public TextMeshPro distanceText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(mainCamera.transform.position, transform.position);
        distanceText.SetText(dist.ToString("0.00") + "m");
    }
}

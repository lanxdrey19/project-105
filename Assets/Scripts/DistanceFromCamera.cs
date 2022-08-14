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
        float distX = Mathf.Sqrt(Mathf.Pow(transform.position.x - mainCamera.transform.position.x, 2) + Mathf.Pow(transform.position.z - mainCamera.transform.position.z, 2));
        float distY = Mathf.Abs(transform.position.y - mainCamera.transform.position.y);

        float dist = Vector3.Distance(mainCamera.transform.position, transform.position);
        distanceText.SetText("Horizontal: " + distX.ToString("0.00") + "m\n" + "Vertical: " + distY.ToString("0.00") + "m\n" + "Total: " + dist.ToString("0.00") + "m");
    }
}

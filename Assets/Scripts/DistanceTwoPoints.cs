using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceTwoPoints : MonoBehaviour
{
    public GameObject Point1;
    public GameObject Point2;
    public float DistanceBetweenPoints;
    public TMP_Text DistanceText;
    public LineRenderer LineBetweenPoints;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DistanceBetweenPoints = Vector3.Distance(Point1.transform.position, Point2.transform.position);
        DistanceText.text = "Distance: " + DistanceBetweenPoints.ToString("0.00") + "m";
        LineBetweenPoints.SetPosition(0, Point1.transform.position);
        LineBetweenPoints.SetPosition(1, Point2.transform.position);
    }
}

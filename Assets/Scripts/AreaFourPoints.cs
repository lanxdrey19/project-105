using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AreaFourPoints : MonoBehaviour
{
    public GameObject Point1;
    public GameObject Point2;
    public GameObject Point3;
    public GameObject Point4;
    public LineRenderer LineBetweenPoints;
    public float AreaBetweenPoints;
    public TMP_Text AreaText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Vector3[] positions = new Vector3[5];
        positions[0] = Point1.transform.position;
        positions[1] = Point2.transform.position;
        positions[2] = Point3.transform.position;
        positions[3] = Point4.transform.position;
        positions[4] = Point1.transform.position;
        LineBetweenPoints.positionCount = positions.Length;
        LineBetweenPoints.SetPositions(positions);

        float AreaBetweenPoints = 0;
        for (int i = positions.Length - 1, j = 0; j < positions.Length; i = j++)
        {
            AreaBetweenPoints += Vector3.Cross(positions[j], positions[i]).magnitude;
        }
        AreaBetweenPoints *= 0.5f;
        AreaText.text = "Area: " + AreaBetweenPoints.ToString("0.00" + "m\xB2");

    }
}

using UnityEngine;

public class PanSimulation : MonoBehaviour
{

    public void MoveToRight(GameObject gameObject)
    {
        gameObject.transform.Translate(0f, 0f, 1f);
    }

    public void MoveToLeft(GameObject gameObject)
    {
        gameObject.transform.Translate(0f, 0f, -1f);
    }

}

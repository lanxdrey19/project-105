using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowFire : MonoBehaviour
{
    public float timer = 0f;
    public float growTime = 10f;
    public float maxSize = 200f;

    public bool isMaxSize = false;

    // Start is called before the first frame update
    void Start()
    {

        if (isMaxSize == false)
        {
            StartCoroutine(Grow());
        }
    }

    private IEnumerator Grow()
    {
        Vector3 startScale = transform.localScale;
        Vector3 maxScale = new Vector3(maxSize, maxSize, maxSize);



        do
        {
            bool positive = timer > 0;

            if (positive == true)
            {
                transform.localScale = Vector3.Lerp(startScale, maxScale, timer / growTime);
            }

            timer += Time.deltaTime;
            yield return null;
        }
        while (timer < growTime);

        isMaxSize = true;

    }
}


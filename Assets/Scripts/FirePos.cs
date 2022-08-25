using UnityEngine;

using Microsoft.MixedReality.Toolkit.Utilities;

public class FirePos : MonoBehaviour
{
    public float spawnDistance = 2;
    // Summons fire to behind the player
    public void Summon()
    {
        transform.position = CameraCache.Main.transform.position - CameraCache.Main.transform.forward * spawnDistance;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
    

}

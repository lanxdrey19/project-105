using UnityEngine;

using Microsoft.MixedReality.Toolkit.Utilities;

public class FirePos : MonoBehaviour
{
    public float spawnDistance = 2;
    // Summons fire to behind the player
    public void Summon()
    {
        transform.position = CameraCache.Main.transform.position - CameraCache.Main.transform.forward * spawnDistance;
        //sets elevation to be in line with users view to not clip into ground
        transform.position = new Vector3(transform.position.x, CameraCache.Main.transform.position.y, transform.position.z);
    }
    

}

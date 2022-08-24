using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeToGestureMode()
    {
        SceneManager.LoadScene(1);
    }

    public void ChangeToButtonMode()
    {
        SceneManager.LoadScene(0);
    }
}

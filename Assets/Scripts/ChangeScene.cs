using UnityEngine;
using UnityEngine.SceneManagement;

// Script to change between the button and gesture scenes
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

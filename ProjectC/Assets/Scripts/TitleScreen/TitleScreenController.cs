using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleScreenController : MonoBehaviour
{
    public string firstSceneName;

    void Start()
    {
        //Should be used to load audiomanager.
    }

    public void NewGameButtonClick()
    {
        SceneManager.LoadScene(firstSceneName);
    }
    public void SecondButtonClick()
    {
        // Empty pending addition of saving - continue button
    }
    public void ThirdButtonClick()
    {
        // Empty pending addition of "exiting game" - exit game button, only if this is built for desktop.
    }
}

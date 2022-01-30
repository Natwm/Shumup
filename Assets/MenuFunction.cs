using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunction : MonoBehaviour
{
    // Start is called before the first frame update
    
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LaunchGame()
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(1);
        float loadProgress = loadingOperation.progress;
        if (loadingOperation.isDone)
        {
            // Loading is finished !
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour
{

    public void GoToScene ( int sceneIndex ) {
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitApplication () {
        Application.Quit();
    }

}
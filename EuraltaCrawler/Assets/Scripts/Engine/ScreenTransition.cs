using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour
{
    public int targetScene;

    void midTransition(){
        // change scene
        SceneManager.LoadScene(targetScene);
    }

    void finishTransition(){
        // finish
        GameManager.paused = false;
        Destroy(gameObject);
    }
}

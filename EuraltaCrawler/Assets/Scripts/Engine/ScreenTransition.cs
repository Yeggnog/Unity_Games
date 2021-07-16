using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour
{
    public int targetScene;

    /*void Update(){
        RectTransform rectTrans = GetComponent<RectTransform>();
        Debug.Log("Currently have width "+rectTrans.sizeDelta.x+" and x coord "+rectTrans.position.x+", totaling to "+(rectTrans.sizeDelta.x + rectTrans.position.x));
    }*/

    void midTransition(){
        // change scene
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(targetScene);
    }

    void finishTransition(){
        // finish
        //Destroy(transform.parent);
        //Debug.Log("Unpaused after transition");
        GameManager.paused = false;
        Destroy(gameObject);
    }
}

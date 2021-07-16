using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Menu : MonoBehaviour
{
    public GameObject trans_prefab;

    public void start(){
        // start game
        GameObject transition = Instantiate(trans_prefab, Vector3.zero, Quaternion.identity);
        transition.GetComponent<TransitionManager>().targetScene = 5;
        transition.GetComponent<TransitionManager>().startTransition();
    }
    
    public void resume(){
        // resumes game
        GameManager.paused = false;
        gameObject.SetActive(false);
    }
    
    public void restart(){
        // restarts in current floor
        GameObject transition = Instantiate(trans_prefab, Vector3.zero, Quaternion.identity);
        transition.GetComponent<TransitionManager>().targetScene = SceneManager.GetActiveScene().buildIndex;
        transition.GetComponent<TransitionManager>().startTransition();
    }

    public void quit(){
        // quits to title
        GameObject transition = Instantiate(trans_prefab, Vector3.zero, Quaternion.identity);
        transition.GetComponent<TransitionManager>().targetScene = 0;
        transition.GetComponent<TransitionManager>().startTransition();
    }

    public void fullQuit(){
        // quits game
        Application.Quit();
    }
}

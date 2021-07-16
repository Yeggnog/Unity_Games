using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public GameObject effect;
    public int targetScene;

    void Start(){
        DontDestroyOnLoad(gameObject);
    }
    
    public void startTransition(){
        // start animation
        GameManager.paused = true;
        DontDestroyOnLoad(effect);
        effect.transform.SetParent(transform,false); 
        effect.GetComponent<ScreenTransition>().targetScene = targetScene;
        Animator anim = effect.GetComponent<Animator>();
        anim.Play("Screentransition");
    }
}

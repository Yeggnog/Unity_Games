using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    // vars
    public GameObject orangeText;
    public GameObject blueText;
    Vector3 initPos = Vector3.zero;

    void Start(){
        initPos = blueText.transform.position;
    }

    void Update(){
        // Apply screenshake
        orangeText.transform.position = initPos + (ScreenManip.getOffset(0) * 200f);
        blueText.transform.position = initPos + (ScreenManip.getOffset(1) * 200f);
    }

    // Write a score value to both displays
    public void WriteScore(int score){
        orangeText.GetComponent<Text>().text = score.ToString();
        blueText.GetComponent<Text>().text = score.ToString();
    }
}

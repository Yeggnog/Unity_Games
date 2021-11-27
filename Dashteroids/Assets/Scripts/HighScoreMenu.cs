using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreMenu : MonoBehaviour
{
    public GameObject[] OrangeEntries;
    public GameObject[] BlueEntries;
    int[] scores = new int[5];
    static int currentScore = -1;
    float angle = 0f;
    int rest = 0;
    bool updateFlag = false;
    Vector3 offset = Vector3.zero;
    Vector3 scoresOrigin;
    Vector3 scoreDivision;
    
    // Initialize them all to 100 to start
    void Start(){
        scoresOrigin = new Vector3(-12f, 66f, 0f);
        scoreDivision = new Vector3(0f, -38f, 0f);
        for(var i=0; i<5; i++){
            //scores[i] = 100;
            OrangeEntries[i].GetComponent<Text>().text = "000000";
            BlueEntries[i].GetComponent<Text>().text = "000000";
        }
    }

    // Do the swirly thing on the current score
    void Update(){
        if(updateFlag){
            for(var l=0; l<5; l++){
                string line = stringPad(scores[l]);
                OrangeEntries[l].GetComponent<Text>().text = line;
                BlueEntries[l].GetComponent<Text>().text = line;
                OrangeEntries[currentScore].transform.position = transform.position + (scoresOrigin + (l * scoreDivision));
                BlueEntries[currentScore].transform.position = transform.position + (scoresOrigin + (l * scoreDivision));
            }
            updateFlag = false;
        }
        if(currentScore >= 0){
            if(rest > 0){
                rest -= 1;
            }else{
                if(angle < (2f * Mathf.PI)){
                    angle += 0.02f;
                }else{
                    angle = 0f;
                }
                offset = (Vector3.right * Mathf.Cos(angle)) + (Vector3.up * Mathf.Sin(angle));
                float mag = 10 * Mathf.Sin(angle * 2.5f);
                if(Mathf.Round(mag) == 0f){
                    int randomDraw = (int)Mathf.Round(Random.Range(1f, 10f));
                    if(randomDraw < 3){
                        rest = 48;
                    }
                }
                OrangeEntries[currentScore].transform.position = transform.position + (scoresOrigin + (currentScore * scoreDivision)) - (offset * mag);
                BlueEntries[currentScore].transform.position = transform.position + (scoresOrigin + (currentScore * scoreDivision)) + (offset * mag);
            }
        }
    }

    public void logScore(int score){
        // determine place in hierarchy
        int index = 5;
        for(var i=4; i>=0; i--){
            int value = scores[i];
            if(value < score){
                index = i;
            }
        }
        if(index < 5){
            // update
            for(var j=4; j>index; j--){
                scores[j] = scores[j-1];
            }
            scores[index] = score;
            currentScore = index;
            updateFlag = true;
        }else{
            currentScore = -1;
        }
    }

    string stringPad(int value){
        string insert = value.ToString();
        int sizeDiff = 6 - (insert.Length);
        if(sizeDiff > 0){
            for(var k=0; k<sizeDiff; k++){
                insert = ("0" + insert);
            }
        }
        return insert;
    }
}

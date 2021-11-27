using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    // Vars
    public GameObject titleOrange;
    public GameObject titleBlue;
    public Sprite[] titleSprites;
    float angle = 0f;
    int rest = 0;
    Vector3 offset = Vector3.zero;

    void Update(){
        // animate title sway
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
            titleOrange.transform.position = transform.position - (offset * mag);
            titleBlue.transform.position = transform.position + (offset * mag);
            // swap on random chance
            int randoDraw = (int)Mathf.Round(Random.Range(1f, 15f));
            if(randoDraw < 2){
                int newSprite = (int)Mathf.Round(Random.Range(0f,2f));
                titleOrange.GetComponent<Image>().sprite = titleSprites[newSprite];
                titleBlue.GetComponent<Image>().sprite = titleSprites[newSprite];
            }
        }
    }
}

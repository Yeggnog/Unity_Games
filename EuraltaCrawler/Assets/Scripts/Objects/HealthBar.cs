using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image bar;
    public Slider sld;
    public float value;
    public float maxValue;
    float barwidth;
    
    // Start is called before the first frame update
    void Start(){
        // get width for ref
        barwidth = bar.GetComponent<RectTransform>().sizeDelta.x;
        sld.maxValue = maxValue;
        sld.value = value;
    }

    // Update is called once per frame
    //void Update(){
        // clamp
        //value = System.Math.Max(0, value);
        //value = System.Math.Min(maxValue, value);
        // update
        //sld.value = value;
        //bar.GetComponent<RectTransform>().sizeDelta = new Vector2((value/maxValue)*barwidth, bar.GetComponent<RectTransform>().sizeDelta.y);
    //}

    public void UpdateValue(float val){
        value = val;
        // clamp
        value = System.Math.Max(0, value);
        value = System.Math.Min(maxValue, value);
        // update
        sld.value = value;
    }
}

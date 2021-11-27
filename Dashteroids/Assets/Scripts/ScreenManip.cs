using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManip : MonoBehaviour
{
    // vars
    static int shakeTimer = 0;
    int lerpTimer = 0;
    static float shakeMag = 0.3f;
    Vector3 shakeOffset = Vector3.zero;
    static Vector3 pos = Vector3.zero;

    void Start(){
        pos = transform.position;
    }

    void Update(){
        if(shakeTimer > 0){
            // shake the position
            shakeOffset = (Vector3.right * Random.Range(-shakeMag, shakeMag)) + (Vector3.up * Random.Range(-shakeMag, shakeMag));
            lerpTimer = 3;
            shakeTimer -= 1;
        }else if(shakeOffset != Vector3.zero){
            // reset
            shakeOffset = Vector3.zero;
            lerpTimer = 3;
        }
        // move to the offset
        if(lerpTimer > 0){
            //transform.position = Vector3.Lerp(transform.position, shakeOffset, 1-( ((float)lerpTimer) / 3f ));
            pos = shakeOffset;
            transform.position = pos;
            //Debug.Log("lerped position to "+transform.position);
            lerpTimer -= 1;
        }
    }

    public static void screenShake(float mag, int duration){
        // shake the screen
        shakeTimer += duration;
        shakeMag = mag;
    }

    public static Vector3 getOffset(int type){
        if(type == 0){
            return pos;
        }else{
            return -pos;
        }
    }
}

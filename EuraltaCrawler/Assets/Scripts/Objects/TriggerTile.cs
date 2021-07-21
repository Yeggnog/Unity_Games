using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTile : MonoBehaviour
{
    bool triggered = false;
    
    // Update is called once per frame
    void Update(){
        if(!GameManager.paused){
            RaycastHit hit;
            bool chk = Physics.Raycast(transform.position + (new Vector3(0f, -4f, 0f)), Vector3.up, out hit, 8f);
            if(chk && !triggered){
                if(hit.collider.tag == "Player"){
                    // perform action
                    triggered = true;
                    TileAction();
                }
            }
            if(!chk && triggered){
                triggered = false;
                PostAction();
            }
        }
    }

    protected virtual void TileAction(){
        // does nothing for now
    }

    protected virtual void PostAction(){
        // does nothing for now
    }
}

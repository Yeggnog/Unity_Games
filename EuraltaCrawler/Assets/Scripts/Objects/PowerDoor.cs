using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDoor : PowerObject
{
    public GameObject door;
    public float moveAmount = 16f;
    public float moveSpeed = 1f;
    float doorOffset = 0f;
    bool moving = false;
    Vector3 doorPos = Vector3.zero;
    public Vector3 orientation = new Vector3(0f, 0f, -1f);
    bool open = false;
    
    protected override void ObjectAction(){
        // open door
        if(!open){
            Debug.Log("Opening door");
            doorPos -= (orientation * moveAmount);
            doorOffset = moveAmount;
            open = true;
        }else{
            Debug.Log("Closing door");
            doorPos += (orientation * moveAmount);
            doorOffset = -moveAmount;
            open = false;
        }
    }

    protected override void PostAction(){
        // close door
        Debug.Log("Closing door");
        doorPos += (orientation * moveAmount);
        doorOffset = -moveAmount;
    }

    void Start(){
        // set up
        doorPos = door.transform.position;
    }

    void Update(){
        if(doorOffset != 0f){
            if(!moving){
                // pause game
                GameManager.paused = true;
                moving = true;
            }
            // update door's position
            door.transform.position = doorPos + (orientation * doorOffset);
        }

        if(doorOffset >= moveSpeed*0.1f){
            doorOffset -= moveSpeed*0.1f;
        }else if(doorOffset <= -moveSpeed*0.1f){
            doorOffset += moveSpeed*0.1f;
        }else{
            if(moving){
                // unpause game
                GameManager.paused = false;
                moving = false;
            }
            // arrived at position
            doorOffset = 0;
        }
    }
}

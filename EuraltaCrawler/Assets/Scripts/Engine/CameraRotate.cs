using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float camera_speed = 40f;
    public GameObject manager;

    void Update(){
        // move to the position of the character taking a turn
        if(manager != null){
            TurnManager turnMana = manager.GetComponent<TurnManager>();
            if(turnMana.GetFocusedUnit() != null){
                transform.position = Vector3.MoveTowards(transform.position, turnMana.GetFocusedUnit().transform.position, camera_speed*Time.deltaTime);
            }
        }
    }
    
    public void RotateLeft(){
        // rotates left 90 degrees
        transform.Rotate(Vector3.up, 90, Space.Self);
    }

    public void RotateRight(){
        // rotates right 90 degrees
        transform.Rotate(Vector3.up, -90, Space.Self);
    }
}

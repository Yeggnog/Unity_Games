using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Move_Old : MonoBehaviour
{
    // vars
    //Vector3 forward = Vector3.zero;
    //Vector3 currentDir = Vector3.zero;
    //Vector3 nextPos;
    Vector3 destination;
    float speed = 3f;
    bool moving = false;
    Queue<int> moves = new Queue<int>();
    
    void Start(){
        // init
        destination = transform.position;
    }

    void Update(){
        // general move
        if(Vector3.Distance(destination, transform.position) <= 0.00001f){
            // at destination
            moving = false;
            if(moves.Count > 0){
                int nextMove = moves.Dequeue();
                SingleMove(nextMove);
            }
        }else{
            // move
            transform.position = Vector3.MoveTowards(transform.position, destination, speed*Time.deltaTime);
        }
        // debug controls
        if(!moving){
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
                moves.Enqueue(0);
            }

            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
                moves.Enqueue(1);
            }

            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
                moves.Enqueue(2);
            }

            if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
                moves.Enqueue(3);
            }
        }
    }

    void SingleMove(int dir){
        moving = true;
        switch(dir){
            // up
            case 0:
            destination = transform.position + Vector3.zero;
            break;

            // left
            case 1:
            destination = transform.position + new Vector3(0, 90, 0);
            break;

            // down
            case 2:
            destination = transform.position + new Vector3(0, 180, 0);
            break;

            // right
            case 3:
            destination = transform.position + new Vector3(0, 270, 0);
            break;
        }
    }

    bool IsValid(){
        // checks for if the current move hits a wall
        Ray ry = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.forward);
        return false;
    }
}

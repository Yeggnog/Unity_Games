using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControl : MonoBehaviour
{
    public float wheel_torque = 1.2f; // 3 is about the limit to what is manageable
    public GameObject shot;
    public GameObject barrel_pos;
    //Rigidbody rb;
    Collider coll;
    
    void Start(){
        //rb = gameObject.GetComponent<Rigidbody>();
        coll = gameObject.GetComponent<Collider>();
    }

    void Update(){
        // get keyboard input
        float turn_mag = 0;
        float move_mag = 0;

        if(Input.GetKey("a")){
            // left
            //print("left");
            turn_mag -= 1.0f;
        }
        if(Input.GetKey("d")){
            // right
            //print("right");
            turn_mag += 1.0f;
        }
        if(Input.GetKey("w")){
            // forward
            //print("forward");
            //rb.AddForce((transform.forward * wheel_torque));
            move_mag += 1.0f;
        }
        if(Input.GetKey("s")){
            // backward
            //print("backward");
            //rb.AddForce((transform.forward * wheel_torque * -1.0f));
            move_mag -= 1.0f;
        }

        // move
        RaycastHit hit;
        int best = -1;
        for(int i=0; i<4; i++){
            // quarter-step approach
            float move_offset = 4f * (i+1) * ((wheel_torque * Time.deltaTime * move_mag) / 4f);
            if(!Physics.SphereCast(transform.position, 0.5f, transform.forward, out hit, move_offset)){
                best = i;
                break;
            }
        }
        if(best != -1){
            float move_offset = 4f * (best+1) * ((wheel_torque * Time.deltaTime * move_mag) / 4f);
            transform.position = (transform.position + (transform.forward * move_offset));
        }
        if(best < 3 && Mathf.Abs(move_mag) > 0f){
            // tests for buttery smooth collisions
            float move_offset = 4f * ((wheel_torque * Time.deltaTime * move_mag) / 4f);
            bool[] tests = {false, false, false, false};
            Vector3[] angles = new Vector3[4];

            for(int i=-2; i<3; i++){
                if(i != 0){
                    // get the associated orientation
                    Vector3 new_dir = (Quaternion.AngleAxis((22.5f*i), Vector3.up)) * (transform.forward);
                    int ind = i;
                    if(i > 0){
                        ind -= 1;
                    }
                    ind += 2;
                    tests[ind] = !Physics.SphereCast(transform.position, 0.5f, new_dir, out hit, move_offset);
                    angles[ind] = new_dir;
                }
            }
            // go to the best working test position
            int[] check_order = {1, 2, 3, 0};
            for(int i=0; i<4; i++){
                int ind = check_order[i];
                if(tests[ind]){
                    transform.position = (transform.position + (angles[ind] * move_offset));
                    break;
                }
            }
        }

        // rotate
        transform.Rotate(0f, (1.45f * wheel_torque * turn_mag), 0f);

        // fire
        if(Input.GetKeyDown(KeyCode.Space)){
            // fire a shot
            GameObject inst = Instantiate(shot, barrel_pos.transform.position, transform.rotation);
        }
    }
}

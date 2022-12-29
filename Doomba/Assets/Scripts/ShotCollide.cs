using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotCollide : MonoBehaviour
{
    public float move_speed = 1.2f;
    int despawn_timer = 360;
    
    // Start is called before the first frame update
    //void Start(){
        
    //}

    // Update is called once per frame
    void Update(){
        // despawn if we've moved for too long
        if(despawn_timer > 0){
            despawn_timer -= 1;
        }else{
            Destroy(gameObject);
        }

        // move until a collision
        transform.position = (transform.position + (transform.forward * move_speed * Time.deltaTime));

        // deal damage if the collision is something
        RaycastHit hit;
        if(Physics.BoxCast(transform.position, new Vector3(0.05f, 0.05f, 0.105f), transform.forward, out hit, transform.rotation, (move_speed * Time.deltaTime))){
            GameObject g_obj = hit.collider.gameObject;
            print("hit game object "+g_obj);
            Destroy(gameObject);
        }
    }
}

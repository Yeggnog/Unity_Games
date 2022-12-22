using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotCollide : MonoBehaviour
{
    public float move_speed = 0.1f;
    
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update(){
        // move until a collision
        transform.position = (transform.position + (transform.forward * move_speed));

        // deal damage if the collision is something
        RaycastHit hit;
        if(Physics.BoxCast(transform.position, new Vector3(0.05f, 0.05f, 0.105f), transform.forward, out hit, transform.rotation, 0)){
            GameObject g_obj = hit.collider.gameObject;
            print("hit game object "+g_obj);
            Destroy(gameObject);
        }
    }
}

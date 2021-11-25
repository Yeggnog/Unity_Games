using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // movement variables
    bool dashing = false;
    Vector2 pos = new Vector2(0f, 0f);
    Vector2 trailPos;
    Vector2 size = new Vector2(3f, 3f);
    float dashSpeed = 0.04f;
    Vector2 target;
    // other vars
    LayerMask mask;
    Vector3 zUp = new Vector3(0f, 0f, 1f);
    public GameObject orangeChild;
    public GameObject blueChild;
    
    // Setup
    void Start(){
        pos = transform.position;
        trailPos = pos;
        mask = LayerMask.GetMask("Asteroids");
        size = GetComponent<BoxCollider2D>().size;
        target = pos;
    }

    // input, movement, collisions
    void Update(){
        trailPos = pos;
        pos = transform.position;
        // input
        if(Input.GetMouseButtonDown(0) && !dashing){
            // left clicked
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(distance(clickPos, pos) > 0.6f){
                // clamp cursor position
                Vector2 pointToClick = (clickPos - pos);
                pointToClick.Normalize();
                pointToClick *= 0.6f;
                clickPos = (pos + pointToClick);
            }
            if(Mathf.Abs(clickPos.x) <= 0.7f && Mathf.Abs(clickPos.y) <= 0.7f){
                target = clickPos;
                //Debug.Log("Got target position "+target);
                dashing = true;
            }
        }

        // movement
        if(dashing){
            if(distance(target, pos) >= dashSpeed){
                // move towards target
                Vector2 dashOffsets = target-pos;
                dashOffsets.Normalize();
                dashOffsets *= dashSpeed;
                transform.position += ( (Vector3.right * dashOffsets.x) + (Vector3.up * dashOffsets.y) );
                // move afterimage
                orangeChild.transform.position = Vector3.zero + (Vector3.right * trailPos.x) + (Vector3.up * trailPos.y);
            }else{
                // stop dashing
                transform.position = target;
                dashing = false;
                pos = target;
            }
        }else{
            // rotate to cursor position
            Vector3 lookPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = -Mathf.Atan2( (lookPoint.x-pos.x), (lookPoint.y-pos.y) ) * Mathf.Rad2Deg;
            //Debug.Log("Got angle "+angle);
            transform.localEulerAngles = (Vector3.zero + (zUp * angle));
            // move afterimage
            Vector2 pos2D = orangeChild.transform.position;
            if(pos2D != target){
                orangeChild.transform.position = target;
            }
            orangeChild.transform.position = transform.position + ScreenManip.getOffset(0);
            blueChild.transform.position = transform.position + ScreenManip.getOffset(1);
        }

        // collisions
        var collisions = Physics2D.BoxCastAll(pos, size, 0f, Vector2.zero, 0f, mask);
        if(collisions.Length > 0){
            // colliding with an asteroid
            if(dashing){
                // break all asteroids
                var list = "Breaking asteroids ";
                for(var i=0; i<collisions.Length; i++){
                    GameObject collided = collisions[i].transform.gameObject;
                    AsteroidControl astrCont = collided.GetComponent<AsteroidControl>();
                    if(astrCont.getAegis() == 0){
                        list += collisions[i].collider.name+", ";
                        astrCont.setAegis(9);
                        astrCont.breakApart();
                        ScreenManip.screenShake(0.03f, 6);
                    }
                }
                Debug.Log(list);
            }else{
                // die and start respawn sequence
                Debug.Log("Dying now");
            }
        }
    }

    // Move for dash, collisions
    //void FixedUpdate(){

    //}

    // get the distance between two positions
    float distance(Vector2 pos1, Vector2 pos2){
        return Mathf.Sqrt( Mathf.Pow(pos1.x - pos.x, 2f) + Mathf.Pow(pos1.y - pos2.y, 2f) );
    }
}

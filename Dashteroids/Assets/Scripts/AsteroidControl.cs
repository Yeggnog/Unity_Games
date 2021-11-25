using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidControl : MonoBehaviour
{
    // set variables
    float angleIncrement = 2f;
    int aegis = 0;
    int type = 0; // 0-3 => Large, 4-7 => small
    int mass = 4; // 4/3 => large, 2/1 => small
    float size = 3f;
    Vector3 velocity = Vector3.zero;
    // other vars
    LayerMask playerMask;
    LayerMask selfMask;
    public GameObject small_asteroid;
    public GameObject large_debris;
    public GameObject small_debris;
    
    // Set up
    void Start(){
        angleIncrement = Random.Range(-10f, 10f);
        size = GetComponent<CircleCollider2D>().radius;
        playerMask = LayerMask.GetMask("Player");
        selfMask = LayerMask.GetMask("Asteroids");
    }

    // Move, bounce
    void Update(){
        // Update aegis
        if(aegis > 0){
            aegis -= 1;
            Vector2 pos = transform.position;
            if(!Physics2D.CircleCast(pos, size, Vector2.zero, 0f, playerMask)){
                aegis = 0;
            }
        }

        // Move
        transform.position += velocity;

        // Bounce
        Vector2 posit = transform.position;
        RaycastHit2D[] others = Physics2D.CircleCastAll(posit, size, Vector2.zero, 0f, selfMask);
        if(others.Length > 0){
            // process collisions [ DEBUG THIS BULLSHIT ]
            for(var i=0; i<others.Length; i++){
                // make sure we're not colliding with ourselves
                if(others[i].collider.gameObject.GetInstanceID() != this.gameObject.GetInstanceID()){
                    AsteroidControl other = others[i].collider.gameObject.GetComponent<AsteroidControl>();
                    // get momentum/angular momentum of both involved
                    Vector3 Ptot = ( mass * velocity ) + ( other.getMass() * other.getVelocity() );
                    float Ltot = ( mass * Mathf.Pow(this.getRadius(), 2f) * angleIncrement ) + ( other.getMass() * Mathf.Pow(other.getRadius(), 2f) * other.getAngSpd() );
                    // set velocity/angular velocities accordingly
                    velocity = ( Ptot / other.getMass() );
                    angleIncrement = ( Ltot / ( other.getMass() * Mathf.Pow(getRadius(),2f) ) );
                    Vector3 otherVel = ( Ptot / mass );
                    float otherAngSpd = ( Ltot / ( mass * Mathf.Pow(other.getRadius(),2f) ) );
                    other.setPhys(otherVel, otherAngSpd);
                }
            }
        }

        // Out-of-bounds
        if(Mathf.Abs(transform.position.x) > 0.9 || Mathf.Abs(transform.position.y) > 0.9){
            Destroy(gameObject);
        }
    }

    // getters and setters
    public int getAegis(){ return aegis; }
    public void setAegis(int newVal){ aegis = newVal; }
    public float getMass(){ return mass; }
    public Vector3 getVelocity(){ return velocity; }
    public float getRadius(){
        if(type < 4){
            // large
            return 0.12f;
        }else{
            // small
            return 0.08f;
        }
    }
    public float getAngSpd(){ return angleIncrement; }
    public void setVars(int newType, float newAngleInc, int newAegis, Vector3 newVel){
        type = newType;
        angleIncrement = newAngleInc;
        aegis = newAegis;
        velocity = newVel;
    }
    public void setPhys(Vector3 newVel, float newAngSpd){
        velocity = newVel;
        angleIncrement = newAngSpd;
    }

    public void breakApart(){
        if(type < 4){
            // break into other asteroids
            int count = (int)Mathf.Round(Random.Range(2f, 4f));
            for(var i=0; i<count; i++){
                Vector3 offset = new Vector3( Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0f );
                GameObject debris = Instantiate(small_asteroid, transform.position+offset, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
                AsteroidControl astr = debris.GetComponent<AsteroidControl>();
                Vector3 vel = (transform.position - debris.transform.position);
                if(vel == Vector3.zero){
                    vel = Vector3.right;
                }
                vel.Normalize();
                vel *= GameControl.asteroidSpeed;
                astr.setVars(5, Random.Range(-40f, 40f), 9, vel);
            }
        }
        int particle = (int)Mathf.Round(Random.Range(0f, 2f));
        // break into particles
        if(particle == 0 || particle == 2){
            GameObject debris = Instantiate(large_debris, transform.position, Quaternion.Euler(90f, 0f, 0f));
        }
        if(particle == 1 || particle == 2){
            GameObject debris = Instantiate(small_debris, transform.position, Quaternion.Euler(90f, 0f, 0f));
        }
        Destroy(gameObject);
    }
}

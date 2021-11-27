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
    int pts = 40; // 40/30 => large, 20/10 => small
    int lifespan = 1600; // in case it never goes offscreen nor gets hit
    float size = 3f;
    Vector3 velocity = Vector3.zero;
    // other vars
    LayerMask playerMask;
    LayerMask selfMask;
    public GameObject smallAsteroid;
    public GameObject largeDebris;
    public GameObject smallDebris;
    public Sprite[] asteroidSprites;
    Vector3 zUp = new Vector3(0f, 0f, 1f);
    
    // Set up
    void Start(){
        size = GetComponent<CircleCollider2D>().radius;
        playerMask = LayerMask.GetMask("Player");
        selfMask = LayerMask.GetMask("Asteroids");
        // set mass/point value based on type
        if(type < 2){
            mass = 4;
        }else if(type < 4){
            mass = 3;
        }else if(type < 6){
            mass = 2;
        }else{
            mass = 1;
        }
        pts = mass * 10;
        GetComponent<SpriteRenderer>().sprite = asteroidSprites[type];
    }

    // Move, bounce
    void Update(){
        // Update lifespan
        if(lifespan > 0){
            lifespan -= 1;
        }else{
            Destroy(this.gameObject);
        }

        // Update aegis
        if(aegis > 0){
            aegis -= 1;
            Vector2 pos = transform.position;
            if(!Physics2D.CircleCast(pos, size, Vector2.zero, 0f, playerMask)){
                aegis = 0;
            }
        }

        // clamp velocity so it doesn't get out of hand
        if(velocity.magnitude > GameControl.asteroidSpeed){
            velocity.Normalize();
            velocity *= GameControl.asteroidSpeed;
        }

        // Move
        transform.position += velocity;
        transform.rotation *= Quaternion.AngleAxis(angleIncrement, zUp);

        // Bounce
        Vector2 posit = transform.position;
        RaycastHit2D[] others = Physics2D.CircleCastAll(posit, size, Vector2.zero, 0f, selfMask);
        if(others.Length > 0){
            // process collisions
            for(var i=0; i<others.Length; i++){
                // make sure we're not colliding with ourselves
                if(others[i].collider.gameObject.GetInstanceID() != this.gameObject.GetInstanceID()){
                    AsteroidControl other = others[i].collider.gameObject.GetComponent<AsteroidControl>();
                    // get momentum/angular momentum of both involved
                    float Mtot = (mass + other.getMass());
                    Vector3 Ptot = ( mass * velocity ) + ( other.getMass() * other.getVelocity() );
                    float Ltot = ( mass * Mathf.Pow(this.getRadius(), 2f) * angleIncrement ) + ( other.getMass() * Mathf.Pow(other.getRadius(), 2f) * other.getAngSpd() );
                    
                    // reverse parallel vel. components
                    Vector3 selfToOther = (other.transform.position - transform.position);
                    selfToOther.Normalize();
                    Vector3 selfResult = (-selfToOther) * Vector3.Dot(velocity, selfToOther);
                    Vector3 otherResult = (selfToOther) * Vector3.Dot(other.getVelocity(), -selfToOther);

                    // set velocity/angular velocities accordingly
                    velocity = GameControl.collisionDampen * (( Ptot / Mtot ) + (2 * selfResult));
                    angleIncrement = GameControl.collisionDampen * ( Ltot / ( other.getMass() * Mathf.Pow(getRadius(),2f) ) );
                    Vector3 otherVel = GameControl.collisionDampen * (( Ptot / Mtot ) + (2 * otherResult));
                    float otherAngSpd = GameControl.collisionDampen * ( Ltot / ( mass * Mathf.Pow(other.getRadius(),2f) ) );

                    // set for other
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
        GameControl.tallyPoints(pts);
        if(type < 4){
            // break into other asteroids
            int count = (int)Mathf.Round(Random.Range(2f, 4f));
            for(var i=0; i<count; i++){
                Vector3 offset = new Vector3( Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0f );
                GameObject debris = Instantiate(smallAsteroid, transform.position+offset, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
                AsteroidControl astr = debris.GetComponent<AsteroidControl>();
                Vector3 vel = (transform.position - debris.transform.position);
                if(vel == Vector3.zero){
                    vel = Vector3.right;
                }
                vel.Normalize();
                vel *= GameControl.asteroidSpeed;
                astr.setVars(5, Random.Range(-0.5f, 0.5f), 9, vel);
            }
        }
        int particle = (int)Mathf.Round(Random.Range(0f, 2f));
        // break into particles
        if(particle == 0 || particle == 2){
            Instantiate(largeDebris, transform.position, Quaternion.Euler(90f, 0f, 0f));
        }
        if(particle == 1 || particle == 2){
            Instantiate(smallDebris, transform.position, Quaternion.Euler(90f, 0f, 0f));
        }
        Destroy(gameObject);
    }
}

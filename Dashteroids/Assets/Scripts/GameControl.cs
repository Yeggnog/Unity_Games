using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    // vars
    public GameObject asteroidPrefab;
    public static float asteroidSpeed = 0.0025f;
    //public static int asteroidCount = 0;
    int creationDelay = 45;
    
    // Start is called before the first frame update
    /*void Start()
    {
        
    }*/

    // Update is called once per frame
    void Update(){
        // spawn asteroids
        if(creationDelay > 0){
            creationDelay -= 1;
        }else{
            spawnAsteroid();
            creationDelay = 135;
        }
    }

    void spawnAsteroid(){
        // place outside but along screen border
        int edge = ((int)Mathf.Round(Random.Range(0f, 3f)));
        Vector3 pos = Vector3.zero;
        if(edge == 0){
            // left edge
            pos += (Vector3.up * Random.Range(-(0.89f), 0.89f));
            pos += (Vector3.right * -(0.89f));
        }else if(edge == 1){
            // right edge
            pos += (Vector3.up * Random.Range(-0.89f, 0.89f));
            pos += (Vector3.right * 0.89f);
        }else if(edge == 2){
            // top edge
            pos += (Vector3.right * Random.Range(-0.89f, 0.89f));
            pos += (Vector3.up * 0.89f);
        }else{
            // bottom edge
            pos += (Vector3.right * Random.Range(-0.89f, 0.89f));
            pos += (Vector3.up * -0.89f);
        }
        GameObject asteroid = Instantiate(asteroidPrefab, pos, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        // point to within 0.3 of center
        Vector3 target = ((Vector3.right * Random.Range(-0.3f, 0.3f)) + (Vector3.up * Random.Range(-0.3f, 0.3f)));
        Vector3 vel = (target - pos);
        vel.Normalize();
        vel *= asteroidSpeed;
        asteroid.GetComponent<AsteroidControl>().setVars((int)Random.Range(0f, 7f), Random.Range(-40f, 40f), 0, vel);
    }
}

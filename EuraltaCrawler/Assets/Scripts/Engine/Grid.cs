using System.Collections;
using System.Collections.Generic;
//using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public GameObject arrowTile;
    public List<GameObject> arrowTiles;
    public List<GameObject> vertTiles;
    // arrow(U R D L), straight(U S), bend(D>R D>L U>L U>R)
    public Sprite[] arrowSprites;
    
    // Start is called before the first frame update
    void Start(){
        ResetPathArrow();
    }

    public void UpdatePathArrow(List<Tile> path){
        ResetPathArrow();
        Tile[] pth = path.ToArray();
        //Tile prev = null;
        for(int i=0; i<pth.Length; i++){
            Tile tile = pth[i];
            Transform tilePos = tile.GetComponent<Transform>();
            // instantiate
            GameObject tl = Instantiate(arrowTile, tilePos.position - new Vector3(4f, -0.1f, -4f), Quaternion.identity);
            tl.GetComponent<pathArrow>().vertical = true;
            arrowTiles.Add(tl);
        }
        for(int i=0; i<arrowTiles.Count; i++){
            Transform tilePos = arrowTiles[i].GetComponent<Transform>();
            // handle arrow sprites
            if(i+1 >= arrowTiles.Count){
                // last in path
                Tile prev;
                if(i > 0){
                    // get previous
                    prev = pth[i-1];
                }else{
                    // only path element
                    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                    if(players.Length > 0){
                        prev = players[0].GetComponent<Player_Move>().currentTile;
                    }else{
                        prev = null;
                    }
                }
                Transform prevPos = prev.GetComponent<Transform>();
                Vector3 prevToCurr = ((tilePos.position + new Vector3(4f, -0.1f, -4f)) - prevPos.position);
                prevToCurr.Normalize();
                Vector3 PTChoriz = new Vector3(prevToCurr.x, 0f, prevToCurr.z);
                if((tilePos.position.y-0.1f) != prevPos.position.y){
                    // add vertical intermediates
                    int vDist = (int)System.Math.Round((tilePos.position.y-0.1f)-prevPos.position.y);
                    int tileCount = (int)System.Math.Round((System.Math.Abs(vDist))/8f);
                    Vector3 offset = new Vector3(-5.5f*prevToCurr.z, 0f, -5.5f*prevToCurr.x);
                    for(int j=0; j<tileCount; j++){
                        float edgeNudge = 5.5f;
                        if(vDist/System.Math.Abs(vDist) < 0){
                            edgeNudge = 6f;
                        }
                        GameObject newTile = Instantiate(arrowTile, prevPos.position + offset + (PTChoriz*edgeNudge) + dir_offset_H(dir_index(PTChoriz, Vector3.zero, true)), Quaternion.identity);
                        if(vDist >= 0){
                            newTile.GetComponent<Transform>().position += (((j+1)*8.3f)*Vector3.up);
                        }else{
                            newTile.GetComponent<Transform>().position -= ((j*8.3f)*Vector3.up);
                        }
                        newTile.GetComponent<Transform>().forward = PTChoriz;
                        newTile.GetComponent<SpriteRenderer>().sprite = arrowSprites[5];
                        newTile.GetComponent<pathArrow>().vertical = true;
                        vertTiles.Add(newTile);
                    }
                }
                int ind = dir_index(prevToCurr, Vector3.zero, true);
                arrowTiles[i].GetComponent<SpriteRenderer>().sprite = arrowSprites[ind];
                arrowTiles[i].GetComponent<Transform>().rotation = new Quaternion(1f, 0f, 0f, 1f);
            }else{
                // middle of path
                Tile prev;
                if(i > 0){
                    // get previous
                    prev = pth[i-1];
                }else{
                    // only path element
                    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                    if(players.Length > 0){
                        prev = players[0].GetComponent<Player_Move>().currentTile;
                    }else{
                        prev = null;
                    }
                }
                Transform prevPos = prev.GetComponent<Transform>();
                Vector3 prevToCurr = ((tilePos.position + new Vector3(4f, -0.1f, -4f)) - prevPos.position);
                prevToCurr.Normalize();
                // get next
                Tile next = pth[i+1];
                Vector3 currToNext = (next.GetComponent<Transform>().position - (tilePos.position + new Vector3(4f, -0.1f, -4f)));
                currToNext.Normalize();
                Vector3 PTChoriz = new Vector3(prevToCurr.x, 0f, prevToCurr.z);
                if((tilePos.position.y-0.1f) != prevPos.position.y){
                    // add vertical intermediates
                    int vDist = (int)System.Math.Round(tilePos.position.y-prevPos.position.y);
                    int tileCount = (int)System.Math.Round((System.Math.Abs(vDist))/8f);
                    Vector3 offset = new Vector3(-5.5f*prevToCurr.z, 0f, -5.5f*prevToCurr.x);
                    for(int j=0; j<tileCount; j++){
                        float edgeNudge = 5.5f;
                        if(vDist/System.Math.Abs(vDist) < 0){
                            edgeNudge = 6f;
                        }
                        GameObject newTile = Instantiate(arrowTile, prevPos.position + offset + (PTChoriz*edgeNudge) + dir_offset_H(dir_index(PTChoriz, Vector3.zero, true)), Quaternion.identity);
                        if(vDist >= 0){
                            newTile.GetComponent<Transform>().position += (((j+1)*8.3f)*Vector3.up);
                        }else{
                            newTile.GetComponent<Transform>().position -= ((j*8.3f)*Vector3.up);
                        }
                        newTile.GetComponent<Transform>().forward = PTChoriz;
                        newTile.GetComponent<SpriteRenderer>().sprite = arrowSprites[5];
                        newTile.GetComponent<pathArrow>().vertical = true;
                        vertTiles.Add(newTile);
                    }
                }
                int ind = dir_index(prevToCurr, currToNext, false);
                arrowTiles[i].GetComponent<SpriteRenderer>().sprite = arrowSprites[ind];
                arrowTiles[i].GetComponent<Transform>().forward = -Vector3.up;
            }
        }
    }

    public void ResetPathArrow(){
        // delete all objects
        foreach(GameObject at in arrowTiles){
            Destroy(at);
        }
        arrowTiles = new List<GameObject>();
        foreach(GameObject at in vertTiles){
            Destroy(at);
        }
        vertTiles = new List<GameObject>();
    }

    Vector3 dir_offset_H(int index){
        if(index == 0){
            // up
            return new Vector3(0f, 0f, 8f);
        }else if(index == 1){
            // right
            return new Vector3(0f, 0f, 0f);
        }else if(index == 2){
            // down
            return new Vector3(0f, 0f, -8f);
        }else{
            // left
            return Vector3.zero;
        }
    }

    int dir_index(Vector3 inDir, Vector3 outDir, bool end){
        // gets an index based on direction
            if(end){
                // last in path
                if(inDir.x > 0){
                    // up
                    return 0;
                }else if(inDir.x < 0){
                    // down
                    return 2;
                }else{
                    if(inDir.z > 0){
                        // left
                        return 3;
                    }else{
                        // right
                        return 1;
                    }
                }
            }else{
                // middle of path
                // get direction of input
                int in_ind = 0; // U R D L
                if(inDir.x > 0){
                    // up
                    in_ind = 0;
                }else if(inDir.x < 0){
                    // down
                    in_ind = 2;
                }else{
                    // neutral
                    if(inDir.z > 0){
                        // left
                        in_ind = 3;
                    }else{
                        // right
                        in_ind = 1;
                    }
                }
                // get direction of output
                int out_ind = 0;
                if(outDir.x > 0){
                    // up
                    out_ind = 0;
                }else if(outDir.x < 0){
                    // down
                    out_ind = 2;
                }else{
                    // neutral
                    if(outDir.z > 0){
                        // left
                        out_ind = 3;
                    }else{
                        // right
                        out_ind = 1;
                    }
                }
                // get final index
                if(in_ind == 0){
                    // up
                    if(out_ind == 0){
                        // up
                        return 4;
                    }else if(out_ind == 1){
                        // right
                        return 6;
                    }else if(out_ind == 2){
                        // down
                        return 4;
                    }else{
                        // left
                        return 7;
                    }
                }else if(in_ind == 1){
                    // right
                    if(out_ind == 0){
                        // up
                        return 8;
                    }else if(out_ind == 1){
                        // right
                        return 5;
                    }else if(out_ind == 2){
                        // down
                        return 7;
                    }else{
                        // left
                        return 4;
                    }
                }else if(in_ind == 2){
                    // down
                    if(out_ind == 0){
                        // up
                        return 4;
                    }else if(out_ind == 1){
                        // right
                        return 9;
                    }else if(out_ind == 2){
                        // down
                        return 4;
                    }else{
                        // left
                        return 8;
                    }
                }else{
                    // left
                    if(out_ind == 0){
                        // up
                        return 9;
                    }else if(out_ind == 1){
                        // right
                        return 4;
                    }else if(out_ind == 2){
                        // down
                        return 6;
                    }else{
                        // left
                        return 5;
                    }
                }
            }
    }
}

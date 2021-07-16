﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public List<Tile> adjacent = new List<Tile>();
    public Material[] tile_colors;

    // BFS stuff
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    // A* stuff
    public float f = 0; // total cost
    public float g = 0; // parent->current cost
    public float h = 0; // heuristic cost (tile->dest. cost)

    // Update is called once per frame
    void Update(){
        // recolor tile
        if(current){
            // current tile color
            // white?
            //GetComponent<Renderer>().material.color = new Color32(0, 222, 255, 102); //Color.white;
            GetComponent<Renderer>().material = tile_colors[3];
        }else if(target){
            // target tile color
            // greenish blue
            //GetComponent<Renderer>().material.color = new Color32(0, 255, 201, 102); //Color.green;
            GetComponent<Renderer>().material = tile_colors[1];
        }else if(selectable){
            // selectable tile color(in move range)
            // light blue
            //GetComponent<Renderer>().material.color = new Color32(0, 150, 255, 102); //Color.blue;
            GetComponent<Renderer>().material = tile_colors[2];
        }else{
            // default tile color
            // dark blue
            //GetComponent<Renderer>().material.color = new Color32(21, 41, 203, 102); //Color.grey;
            GetComponent<Renderer>().material = tile_colors[0];
        }
    }

    // reset vars
    public void Reset(){
        adjacent.Clear();
        current = false;
        target = false;
        selectable = false;
        visited = false;
        parent = null;
        distance = 0;
        f = 0;
        g = 0;
        h = 0;
    }

    // finds the four neighbors for a tile
    public void FindNeighbors(float jumpHeight, Tile target){
        Reset();
        CheckTile(Vector3.forward, jumpHeight, target);
        CheckTile(-Vector3.forward, jumpHeight, target);
        CheckTile(Vector3.right, jumpHeight, target);
        CheckTile(-Vector3.right, jumpHeight, target);
    }

    // checks for if a tile is accessible
    public void CheckTile(Vector3 direction, float jumpHeight, Tile target){
        Vector3 halfExt = new Vector3(4f, (8f+jumpHeight)/2f, 4f);
        Collider[] coll = Physics.OverlapBox(transform.position + 8*direction, halfExt);
        foreach(Collider item in coll){
            Tile tile = item.GetComponent<Tile>();
            if(tile != null && tile.walkable){
                //if(/*!Physics.Raycast(tile.transform.position + (new Vector3(0f, -4f, 0f)), Vector3.up, out hit, 8) ||*/ (tile == target)){
                    adjacent.Add(tile);
                //}
            }
        }
    }
}

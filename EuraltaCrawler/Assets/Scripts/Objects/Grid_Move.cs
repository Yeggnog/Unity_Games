using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Move : MonoBehaviour
{
    List<Tile> selectable = new List<Tile>();
    GameObject[] tiles;
    public Tile currentTile;
    Stack<Tile> path = new Stack<Tile>();
    protected List<Tile> custPath = new List<Tile>();
    public Animator anim;

    public bool turn = false;

    public bool moving = false;
    public int move = 5;
    public float jumpHeight = 2f;
    public float moveSpeed = 5f;
    public float jumpVelocity = 4.5f;

    // movement vars
    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();
    float yOffset = 4f;
    public Tile actualTargetTile; // tile we actually move to with A*

    // movement tweening
    bool falling = false;
    bool jumping = false;
    bool moveToEdge = false;
    Vector3 jumpTarget;

    // unit variables
    public int HP = 10;
    public int MP = 4;
    public GameObject health_bar_prefab;
    public GameObject healthBar;
    public enum unitStates { MoveMode, Moving, ActionMode, Actioning, MenuMode };
    public enum menuStates { Attack, Magic, Interact, Select };
    public unitStates state = unitStates.MoveMode;
    public menuStates actionState = menuStates.Select;
    public int spellIndex = -1;
    
    // setup
    protected void Init(){
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        GameObject bar_canvas = Instantiate(health_bar_prefab, transform.position + (7.5f * Vector3.up), Quaternion.identity);
        healthBar = bar_canvas.transform.Find("HealthBar").gameObject;
        bar_canvas.transform.SetParent(transform);
        healthBar.GetComponent<HealthBar>().value = HP;
        healthBar.GetComponent<HealthBar>().maxValue = HP;
        bar_canvas.GetComponent<Billboard>().camer = GameObject.FindGameObjectsWithTag("MainCamera")[0].transform;
    }

    public void GetCurrentTile(){
        currentTile = GetTargetTile(gameObject);
        //if(currentTile == null){
            //Debug.Log("["+this+"] got current tile ["+currentTile+"]");
        //}
        currentTile.current = true;
    }

    public Tile GetTargetTile(GameObject target){
        RaycastHit hit;
        Tile targ = null;

        // check for the current tile
        //Vector3 extents = GetComponent<Collider>().bounds.max;
        //Collider[] coll = Physics.OverlapBox(transform.position, extents);
        //foreach(Collider item in coll){
            //Tile tile = item.GetComponent<Tile>();
            //if(tile != null && targ == null){
                //targ = tile;
            //}
        //}
        // secondary check
        if(Physics.Raycast(target.transform.position, -Vector3.up, out hit, 8)){
            targ = hit.collider.GetComponent<Tile>();
        }
        return targ;
    }

    public void ComputeAdjacencyLists(float jumpHeight, Tile target){
        // can also find current readout of tiles here if the map changes size
        foreach(GameObject tile in tiles){
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(jumpHeight, target);
        }
    }

    // BFS for reachable / selectable tiles
    public void FindSelectableTiles(int dist){
        ComputeAdjacencyLists(jumpHeight, null);
        GetCurrentTile();

        Queue<Tile> proc = new Queue<Tile>();
        proc.Enqueue(currentTile);
        currentTile.visited = true;

        while(proc.Count > 0){
            Tile t = proc.Dequeue();
            selectable.Add(t);
            t.selectable = true;

            if(t.distance < dist){
                List<Tile> adj = t.adjacent;
                foreach(Tile neighbor in adj){
                    if(!neighbor.visited){
                        neighbor.visited = true;
                        neighbor.parent = t;
                        neighbor.distance = (t.distance + 1);
                        proc.Enqueue(neighbor);
                    }
                }
            }
        }
    }

    public void MoveToTile(Tile tile){
        path.Clear();
        moving = true;

        if(custPath.Count > 0){
            // custom pathfinding
            custPath[custPath.Count-1].target = true;
            for(int i=custPath.Count-1; i>=0; i--){
                path.Push(custPath[i]);
            }
            custPath.Clear();
        }else{
            // auto pathfinding
            tile.target = true;
            Tile next = tile;
            while(next != null){
                path.Push(next);
                next = next.parent;
            }
        }
    }

    public void Move(){
        if(path.Count > 0){
            //Debug.Log("Current tile is "+currentTile);
            state = unitStates.Moving;
            // move along the path
            Tile t = path.Peek();
            //Debug.Log("Got target tile "+t);
            Vector3 target = t.transform.position;
            target.y += yOffset + t.GetComponent<Collider>().bounds.extents.y;
            // tile visibility, only show tiles in path
            foreach(GameObject item in tiles){
                Tile tile = item.GetComponent<Tile>();
                if(!path.Contains(tile)){
                    tile.GetComponent<MeshRenderer>().enabled = false;
                }
            }

            if(Vector3.Distance(transform.position, target) >= 0.5f){
                // move
                if(transform.position.y != target.y){
                    // vertical movement
                    Jump(target);
                }else{
                    // horizontal movement
                    CalculateHeading(target);
                    SetHorizontalVelocity();
                    anim.Play("Walk");
                }
                // <add animation here>
                //transform.forward = velocity;
                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }else{
                // reached target position
                transform.position = target;
                currentTile = t;
                //Debug.Log("Updated current tile to "+currentTile);
                path.Pop();
                falling = false;
                jumping = false;
                moveToEdge = false;
            }
        }else{
            // end of the path
            RemoveSelectableTiles();
            moving = false;
            // tile visibility
            /*foreach(GameObject item in tiles){
                Tile tile = item.GetComponent<Tile>();
                tile.GetComponent<MeshRenderer>().enabled = true;
            }*/
            showGrid();
            // add combat turn here
            //Debug.Log("["+this+"] Calling for action phase");
            //StartAction();
            state = unitStates.MenuMode;
            actionState = menuStates.Select;
            anim.Play("Idle");
        }
    }

    protected void RemoveSelectableTiles(){
        /*if(currentTile != null){
            currentTile.current = false;
            currentTile = null;
        }*/
        foreach(Tile tile in selectable){
            tile.Reset();
        }
        selectable.Clear();
    }

    void CalculateHeading(Vector3 target){
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity(){
        velocity = heading * moveSpeed;
    }

    void Jump(Vector3 target){
        if(falling){
            // fall down
            //Debug.Log("<jump> Falling down");
            FallDown(target);
        }else if(jumping){
            // jump up
            JumpUp(target);
        }else if(moveToEdge){
            // move to the edge in prep to jump down
            //Debug.Log("<jump> Moving to edge");
            MoveToEdge();
        }else{
            //Debug.Log("<jump> Preparing to jump");
            PrepareJump(target);
        }
    }

    void PrepareJump(Vector3 target){
        // prevent tilting
        float targetY = target.y;
        target.y = transform.position.y;
        CalculateHeading(target);
        // determine jump state
        if(transform.position.y > targetY){
            //Debug.Log("<jump> Decided to jump down");
            // jump down (move to edge and fall down)
            falling = false;
            jumping = false;
            moveToEdge = true;
            jumpTarget = transform.position + ((target - transform.position) / 2.0f);
            //Debug.Log("<jump> Set jump target pos to "+jumpTarget+", current pos is "+transform.position);
            anim.Play("JumpDown");
        }else{
            // jump up (jump over edge and fall down)
            falling = false;
            jumping = true;
            moveToEdge = false;
            velocity = heading * (moveSpeed / 3f); // 3f is fungible
            float diff = targetY - transform.position.y;
            velocity.y = jumpVelocity * ((0.5f + diff) / 2f);
            anim.Play("JumpUp");
        }
    }

    void FallDown(Vector3 target){
        velocity += (Physics.gravity*9f) * Time.deltaTime;
        if(transform.position.y <= target.y){
            //Debug.Log("<jump> Landed from falling");
            // landed
            falling = false;
            Vector3 pos = transform.position;
            pos.y = target.y;
            transform.position = pos;
            velocity = new Vector3();
        }
    }

    void JumpUp(Vector3 target){
        velocity += (Physics.gravity*9f) * Time.deltaTime;
        if(transform.position.y > target.y){
            // jump successful
            jumping = false;
            falling = true;
        }
    }

    void MoveToEdge(){
        Vector3 targ_horiz = jumpTarget;
        targ_horiz.y = transform.position.y;
        if(Vector3.Distance(transform.position, targ_horiz) >= 1f){
            //Debug.Log("<jump> Moving, current pos is "+transform.position);
            SetHorizontalVelocity();
        }else{
            //Debug.Log("<jump> Reached the edge");
            // reached edge
            moveToEdge = false;
            falling = true;
            velocity /= 4f;
            velocity.y = 2.5f; // small hop
        }
    }

    // taking a turn in the overworld
    public void BeginTurn(){
        //
        turn = true;
    }

    public void EndTurn(){
        //
    }

    protected void FindPath(Tile target){
        ComputeAdjacencyLists(jumpHeight, target);
        GetCurrentTile();

        List<Tile> closedList = new List<Tile>();
        List<Tile> openList = new List<Tile>();

        // set up A* for first step
        openList.Add(currentTile);
        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;

        while(openList.Count > 0){
            Tile t = FindLowestF(openList);
            closedList.Add(t);
            if(t != target){
                // move ahead
                foreach(Tile tile in t.adjacent){
                    if(!closedList.Contains(tile)){
                        if(openList.Contains(tile)){
                            // look for a faster way
                            float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                            if(tempG < tile.g){
                                tile.parent = t;
                                tile.g = tempG;
                                tile.f = tile.h + tile.g;
                            }
                        }else{
                            // add to open list
                            tile.parent = t;
                            tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                            tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                            tile.f = tile.g + tile.h;
                            openList.Add(tile);
                        }
                    }
                }
            }else{
                // found target
                actualTargetTile = FindEndTile(t);
                actualTargetTile.target = true;
                MoveToTile(actualTargetTile);
                //return true;
            }
        }

        Debug.Log("path not found, skipping turn for now");
        //return false;
    }

    protected Tile FindLowestF(List<Tile> list){
        Tile lowest = list[0];
        foreach(Tile t in list){
            if(t.selectable){
            if(t.f < lowest.f){
                lowest = t;
            }
            }
        }
        list.Remove(lowest);
        return lowest;
    }

    public Tile FindEndTile(Tile t){
        Stack<Tile> tempPath = new Stack<Tile>();
        Tile next = t.parent;
        while(next != null){
            tempPath.Push(next);
            next = next.parent;
        }
        if(tempPath.Count <= move){
            // can move to the end tile
            return t.parent;
        }else{
            // end tile out of range
            Tile endTile = null;
            for(int i=0; i<=move; i++){
                endTile = tempPath.Pop();
            }
            return endTile;
        }
    }

    /*public void StartAction(){
        // do nothing for now
        Debug.Log("Doing nothing in default StartAction");
    }*/

    public void hideGrid(){
        foreach(GameObject item in tiles){
            Tile tile = item.GetComponent<Tile>();
            tile.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void showGrid(){
        foreach(GameObject item in tiles){
            Tile tile = item.GetComponent<Tile>();
            tile.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}

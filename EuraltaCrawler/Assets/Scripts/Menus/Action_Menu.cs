using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Menu : MonoBehaviour
{
    GameObject player;
    Player_Move ply;
    public GameObject magicList;
    public GameObject magicBar;
    
    // Start is called before the first frame update
    void Start(){
        GetPlayer();
    }

    // Update is called once per frame
    //void Update(){
        
    //}

    public void Attack_MenuOpt(){
        // Go into attack state
        if(player != null){
            Debug.Log("clicked attack button");
            if(ply.turn){
                Debug.Log("Going into attack mode");
                ply.state = Grid_Move.unitStates.ActionMode;
                ply.actionState = Grid_Move.menuStates.Attack;
                magicList.SetActive(false);
                magicBar.SetActive(false);
            }
        }else{
            Debug.Log("Player not found");
        }
    }

    public void Magic_MenuOpt(){
        // Go into magic state
        if(player != null){
            Debug.Log("clicked magic button");
            if(ply.turn){
                Debug.Log("Going into magic mode");
                ply.state = Grid_Move.unitStates.ActionMode;
                ply.actionState = Grid_Move.menuStates.Magic;
                magicList.SetActive(true);
                magicBar.SetActive(true);
            }
        }else{
            Debug.Log("Player not found");
        }
    }

    public void Open_MenuOpt(){
        // interact with objects
        if(player != null){
            Debug.Log("clicked open button");
            if(ply.turn){
                Debug.Log("Going into interact mode");
                ply.state = Grid_Move.unitStates.ActionMode;
                ply.actionState = Grid_Move.menuStates.Interact;
                magicList.SetActive(false);
                magicBar.SetActive(false);
            }
        }else{
            Debug.Log("Player not found");
        }
    }

    public void Wait_MenuOpt(){
        // wait and skip turn
        if(player != null){
            Debug.Log("clicked wait button");
            if(ply.turn){
                Debug.Log("Waiting");
                ply.state = Grid_Move.unitStates.MoveMode;
                ply.turn = false;
                magicList.SetActive(false);
                magicBar.SetActive(false);
                TurnManager.EndTurn();
            }
        }else{
            Debug.Log("Player not found");
        }
    }

    bool GetPlayer(){
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        if(player != null){
            ply = player.GetComponent<Player_Move>();
            return true;
        }else{
            return false;
        }
    }
}

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

    public void Attack_MenuOpt(){
        // Go into attack state
        if(player != null){
            if(ply.turn){
                ply.state = Grid_Move.unitStates.ActionMode;
                ply.actionState = Grid_Move.menuStates.Attack;
                magicList.SetActive(false);
                magicBar.SetActive(false);
            }
        }
    }

    public void Magic_MenuOpt(){
        // Go into magic state
        if(player != null){
            if(ply.turn){
                ply.state = Grid_Move.unitStates.ActionMode;
                ply.actionState = Grid_Move.menuStates.Magic;
                magicList.SetActive(true);
                magicBar.SetActive(true);
            }
        }
    }

    public void Open_MenuOpt(){
        // interact with objects
        if(player != null){
            if(ply.turn){
                ply.state = Grid_Move.unitStates.ActionMode;
                ply.actionState = Grid_Move.menuStates.Interact;
                magicList.SetActive(false);
                magicBar.SetActive(false);
            }
        }
    }

    public void Wait_MenuOpt(){
        // wait and skip turn
        if(player != null){
            if(ply.turn){
                ply.state = Grid_Move.unitStates.MoveMode;
                ply.turn = false;
                magicList.SetActive(false);
                magicBar.SetActive(false);
                TurnManager.EndTurn();
            }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic_Menu : MonoBehaviour
{
    GameObject player;
    Player_Move ply;
    public GameObject magicList;
    public GameObject magicBar;
    
    // Start is called before the first frame update
    void Start(){
        GetPlayer();
    }

    public void Smite_MenuOpt(){
        // Go into attack state
        if(player != null){
            if(ply.turn){
                ply.state = Grid_Move.unitStates.ActionMode;
                ply.actionState = Grid_Move.menuStates.Magic;
                ply.spellIndex = 0;
            }
        }
    }

    public void Heal_MenuOpt(){
        // Go into attack state
        if(player != null){
            if(ply.turn){
                ply.state = Grid_Move.unitStates.ActionMode;
                ply.actionState = Grid_Move.menuStates.Magic;
                ply.spellIndex = 1;
            }
        }
    }

    public void Back_MenuOpt(){
        // Go back to menu state
        if(player != null){
            if(ply.turn){
                ply.state = Grid_Move.unitStates.MenuMode;
                ply.actionState = Grid_Move.menuStates.Select;
                ply.spellIndex = -1;
                magicList.SetActive(false);
                magicBar.SetActive(false);
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

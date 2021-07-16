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
                Debug.Log("Selected smite spell");
                ply.state = Grid_Move.unitStates.ActionMode;
                ply.actionState = Grid_Move.menuStates.Magic;
                ply.spellIndex = 0;
            }
        }else{
            Debug.Log("Player not found");
        }
    }

    public void Heal_MenuOpt(){
        // Go into attack state
        if(player != null){
            if(ply.turn){
                Debug.Log("Selected heal spell");
                ply.state = Grid_Move.unitStates.ActionMode;
                ply.actionState = Grid_Move.menuStates.Magic;
                ply.spellIndex = 1;
            }
        }else{
            Debug.Log("Player not found");
        }
    }

    public void Back_MenuOpt(){
        // Go back to menu state
        if(player != null){
            if(ply.turn){
                Debug.Log("Going back to menu");
                ply.state = Grid_Move.unitStates.MenuMode;
                ply.actionState = Grid_Move.menuStates.Select;
                ply.spellIndex = -1;
                magicList.SetActive(false);
                magicBar.SetActive(false);
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

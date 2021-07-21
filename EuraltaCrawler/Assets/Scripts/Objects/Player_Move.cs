using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : Grid_Move
{
    public GameObject actionMenu;
    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject magicList;
    public GameObject magicBar;
    public Grid Grid;
    public GameObject sigil_prefab;
    public ParticleSystem attack_prefab;
    
    // Start is called before the first frame update
    void Start(){
        Init();
        magicBar.GetComponent<HealthBar>().value = MP;
        magicBar.GetComponent<HealthBar>().maxValue = MP;
        TurnManager.AddUnit(this);
    }

    // Update is called once per frame
    void Update(){
        // death check
        if(HP > 0){
        
        // pause input
        if(Input.GetKeyDown("escape")){
            if(!GameManager.paused){
                // pause game and bring up menu
                GameManager.paused = true;
                pauseMenu.SetActive(true);
            }else{
                // unpause
                GameManager.paused = false;
                pauseMenu.SetActive(false);
            }
        }

        if(!GameManager.paused){

        if(turn){
            if(!moving){
                if(state == unitStates.MenuMode || state == unitStates.ActionMode){
                    // starting the action phase
                    actionMenu.SetActive(true);
                    switch(actionState){
                        case menuStates.Attack:
                            FindSelectableTiles(1);
                        break;
                        case menuStates.Magic:
                            // magic rangefinding
                            if(spellIndex == 0){
                                // smite
                                FindSelectableTiles(4);
                            }else if(spellIndex == 1){
                                // heal
                                FindSelectableTiles(3);
                            }
                        break;
                        case menuStates.Interact:
                            FindSelectableTiles(1);
                        break;
                    }
                }else{
                    actionMenu.SetActive(false);
                    FindSelectableTiles(move);
                }
                // movement state
                CheckMouse();
            }else{
                Move();
            }
        }else{
            actionMenu.SetActive(false);
            magicList.SetActive(false);
            magicBar.SetActive(false);
        }

        }

        }else{
            // die
            GameManager.paused = true;
            // activate death menu
            deathMenu.SetActive(true);
        }
    }

    void CheckMouse(){
        // check for button press
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit)){
            // custom pathing for player characters
            if(hit.collider.tag == "Tile" && state == unitStates.MoveMode){
                Tile t = hit.collider.GetComponent<Tile>();
                RaycastHit chk;
                if(!Physics.Raycast(t.transform.position + (new Vector3(0f, -4f, 0f)), Vector3.up, out chk, 4.5f)){ // occupation check
                    // update path
                    if(custPath.Count > 0){
                        // check against path
                        Tile head = custPath[custPath.Count-1];
                        if(head.adjacent.Contains(t)){
                            if(custPath.Count > 1 && t == custPath[custPath.Count-2]){
                                // go back
                                custPath.Remove(head);
                                Grid.UpdatePathArrow(custPath);
                            }else if(t == currentTile){
                                // clear path
                                custPath.Clear();
                                Grid.ResetPathArrow();
                            }else if(t.selectable && t.walkable && custPath.Count < move && !custPath.Contains(t)){
                                // add to path
                                custPath.Add(t);
                                Grid.UpdatePathArrow(custPath);
                            }
                        }
                    }else if(currentTile.adjacent.Contains(t) && t.selectable && t.walkable && custPath.Count < move){
                        // start path
                        custPath.Add(t);
                        Grid.UpdatePathArrow(custPath);
                    }
                }
            }else if(hit.collider.tag == "Player" && state == unitStates.MoveMode){
                // clear path
                custPath.Clear();
                Grid.ResetPathArrow();
            }

            if(Input.GetMouseButtonDown(0)){
                // clicked on a tile
                if(hit.collider.tag == "Tile"){
                    Tile t = hit.collider.GetComponent<Tile>();
                    if(t.selectable && state == unitStates.MoveMode){
                        // set target
                        Grid.ResetPathArrow();
                        MoveToTile(t);
                    }
                }
                // clicked on an entity
                Grid_Move clicked = hit.collider.GetComponent<Grid_Move>();
                if(clicked == null){
                    // not a tile nor a unit
                    switch(state){
                        case unitStates.MoveMode:
                            // do nothing, go to menu mode
                            state = unitStates.MenuMode;
                        break;
                        case unitStates.ActionMode:
                            if(actionState == menuStates.Attack || actionState == menuStates.Select){
                                // return to menu mode
                                state = unitStates.MenuMode;
                                magicList.SetActive(false);
                                magicBar.SetActive(false);
                            }else if(actionState == menuStates.Interact){
                                if(hit.collider.tag == "HealPoint"){
                                    // restore MP
                                    hit.collider.GetComponent<HealPoint>().Heal(this);
                                    magicBar.GetComponent<HealthBar>().UpdateValue(MP);
                                    state = unitStates.MoveMode;
                                    turn = false;
                                    TurnManager.EndTurn();
                                }
                            }
                        break;
                    }
                }else{
                    // got a unit
                    if(hit.collider.tag == "Player"){
                        // clicked on the player
                        switch(clicked.state){
                            case unitStates.ActionMode:
                                // do action on self
                                switch(actionState){
                                    case menuStates.Magic:
                                        // cast spell
                                        switch(spellIndex){
                                            case 1:
                                                if(MP > 0){
                                                // heal
                                                actionMenu.SetActive(false);
                                                magicList.SetActive(false);
                                                magicBar.SetActive(false);
                                                hideGrid();
                                                GameObject newSigil = Instantiate(sigil_prefab, currentTile.GetComponent<Transform>().position, Quaternion.identity);
                                                SpellEffect sefc = newSigil.GetComponent<SpellEffect>();
                                                sefc.target = this;
                                                sefc.dmg = -4;
                                                sefc.ply = this;
                                                sefc.spell_index = spellIndex;
                                                spellIndex = -1;
                                                state = unitStates.Actioning;
                                                anim.Play("Cast");
                                                MP -= 1;
                                                magicBar.GetComponent<HealthBar>().UpdateValue(MP);
                                                sefc.SpellCast();
                                                healthBar.GetComponent<HealthBar>().UpdateValue(healthBar.GetComponent<HealthBar>().value + 4);
                                                }
                                            break;
                                        }
                                    break;
                                }
                            break;
                            case unitStates.MoveMode:
                                // do nothing, go into menu mode
                                clicked.state = unitStates.MenuMode;
                                actionMenu.SetActive(true);
                            break;
                        }
                    }else if(hit.collider.tag == "Enemy"){
                        Grid_Move egm = hit.collider.GetComponent<Grid_Move>();
                        egm.GetCurrentTile();
                    if(egm != null && egm.currentTile != null && egm.currentTile.selectable){
                        // clicked on an enemy
                        switch(state){
                            case unitStates.ActionMode:
                                // face enemy
                                Vector3 forw = (clicked.transform.position - transform.position);
                                forw.Normalize();
                                transform.forward = new Vector3(forw.x, 0f, forw.z);

                                // do action on enemy
                                actionMenu.SetActive(false);
                                magicList.SetActive(false);
                                magicBar.SetActive(false);
                                switch(actionState){
                                    case menuStates.Attack:
                                        // attack enemy
                                        hideGrid();
                                        ParticleSystem atk = Instantiate(attack_prefab, clicked.currentTile.GetComponent<Transform>().position, Quaternion.identity);
                                        AttackEffect atkfct = atk.GetComponent<AttackEffect>();
                                        atkfct.target = clicked;
                                        atkfct.dmg = 3;
                                        atkfct.ply = this;
                                        state = unitStates.Actioning;
                                        anim.Play("Slash");
                                        atkfct.delay = 30;
                                        clicked.healthBar.GetComponent<HealthBar>().UpdateValue(clicked.healthBar.GetComponent<HealthBar>().value - 3);
                                    break;
                                    case menuStates.Magic:
                                        // cast spell
                                        if(MP > 0){
                                        switch(spellIndex){
                                            case 0:
                                                // smite
                                                hideGrid();
                                                GameObject newSigl = Instantiate(sigil_prefab, clicked.currentTile.GetComponent<Transform>().position, Quaternion.identity);
                                                SpellEffect sef = newSigl.GetComponent<SpellEffect>();
                                                sef.target = clicked;
                                                sef.dmg = 6;
                                                sef.ply = this;
                                                sef.spell_index = spellIndex;
                                                state = unitStates.Actioning;
                                                anim.Play("Cast");
                                                MP -= 1;
                                                magicBar.GetComponent<HealthBar>().UpdateValue(MP);
                                                sef.SpellCast();
                                                clicked.healthBar.GetComponent<HealthBar>().UpdateValue(clicked.healthBar.GetComponent<HealthBar>().value - 6);
                                            break;
                                            case 1:
                                                // heal (does nothing unless undead?)
                                                hideGrid();
                                                GameObject newSgl = Instantiate(sigil_prefab, clicked.currentTile.GetComponent<Transform>().position, Quaternion.identity);
                                                SpellEffect sefct = newSgl.GetComponent<SpellEffect>();
                                                sefct.target = clicked;
                                                sefct.dmg = 4;
                                                sefct.ply = this;
                                                sefct.spell_index = spellIndex;
                                                state = unitStates.Actioning;
                                                anim.Play("Cast");
                                                MP -= 1;
                                                magicBar.GetComponent<HealthBar>().UpdateValue(MP);
                                                sefct.SpellCast();
                                                clicked.healthBar.GetComponent<HealthBar>().UpdateValue(clicked.healthBar.GetComponent<HealthBar>().value - 4);
                                            break;
                                        }
                                        }
                                    break;
                                }
                                spellIndex = -1;
                            break;
                            case unitStates.MoveMode:
                                // move to enemy
                                actionMenu.SetActive(false);
                                Tile t = hit.collider.GetComponent<Grid_Move>().currentTile;
                                Tile targ = FindEndTile(t);
                                if(targ != null){
                                    // move next to the enemy's position
                                    MoveToTile(targ);
                                }
                            break;
                        }
                    }
                    }
                }
            }
        }
    }
}

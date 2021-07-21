using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Move : Grid_Move
{
    GameObject target;
    public ParticleSystem attack_prefab;
    
    // Start is called before the first frame update
    void Start(){
        Init();
        TurnManager.AddUnit(this);
    }

    // Update is called once per frame
    void Update(){
        if(HP <= 0){
            // die
            TurnManager.RemoveUnit(this);
            turn = false;
            TurnManager.EndTurn();
            Destroy(gameObject);
        }else{
            if(turn){
                if(moving){
                    Move();
                }else{
                    // starting turn
                    switch(state){
                        case unitStates.MenuMode:
                            // move to move mode
                            state = unitStates.ActionMode;
                        break;
                        case unitStates.MoveMode:
                            // move towards target
                            FindNearestTarget();
                            if(target != null){
                                FindSelectableTiles(move);
                                CalculatePath();
                            }else{
                                // end turn
                                state = unitStates.MoveMode;
                                turn = false;
                                TurnManager.EndTurn();
                            }
                        break;
                        case unitStates.ActionMode:
                            // find target
                            FindNearestTarget();
                            FindSelectableTiles(1);
                            if(target != null && target.GetComponent<Grid_Move>().currentTile.selectable){
                                Player_Move trg = target.GetComponent<Player_Move>();
                                if(trg.currentTile.selectable){
                                    // attack player
                                    hideGrid();
                                    ParticleSystem atk = Instantiate(attack_prefab, trg.currentTile.GetComponent<Transform>().position, Quaternion.identity);
                                    AttackEffect atkfct = atk.GetComponent<AttackEffect>();
                                    atkfct.target = trg;
                                    atkfct.dmg = 3;
                                    atkfct.ply = this;
                                    state = unitStates.Actioning;
                                    anim.Play("Slash");
                                    atkfct.delay = 30;
                                    trg.healthBar.GetComponent<HealthBar>().UpdateValue(trg.healthBar.GetComponent<HealthBar>().value - 3);
                                }
                            }else{
                                state = unitStates.MoveMode;
                                turn = false;
                                TurnManager.EndTurn();
                            }
                        break;
                    }
                
                }
            }else{
                state = unitStates.MoveMode;
            }
        }
    }

    void CalculatePath(){
        Tile targetTile = GetTargetTile(target);
        bool test = FindPath(targetTile); // A* pathfinding
        if(!test){
            // no path available
            state = unitStates.MoveMode;
            turn = false;
            TurnManager.EndTurn();
        }
    }

    void FindNearestTarget(){
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearest = null;
        float distance = 56f; // within 7 tiles' distance (prev Mathf.Infinity)
        foreach(GameObject obj in targets){
            float d = Vector3.Distance(transform.position, obj.transform.position);
            if(d < distance){
                distance = d;
                nearest = obj;
            }
        }
        target = nearest;
    }
}

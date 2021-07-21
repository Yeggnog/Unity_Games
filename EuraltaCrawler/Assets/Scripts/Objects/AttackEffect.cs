using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public Grid_Move target;
    public int dmg;
    public int delay;
    bool damaged = false;
    public Grid_Move ply;
    ParticleSystem part;

    void Update(){
        if(delay > 0){
            delay -= 1;
        }else{
            if(!damaged){
                AttackDamage();
                delay = 10;
                damaged = true;
            }else{
                EndAttack();
            }
        }
    }

    public void AttackDamage(){
        // do damage
        target.HP -= dmg;
    }

    public void EndAttack(){
        // end player turn
        ply.showGrid();
        ply.state = Grid_Move.unitStates.MoveMode;
        ply.turn = false;
        TurnManager.EndTurn();
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public Grid_Move target;
    public int dmg;
    public int delay;
    bool damaged = false;
    //public Animator anim;
    //public GameObject self;
    public Grid_Move ply;
    //public ParticleSystem effect;
    ParticleSystem part;

    void Update(){
        if(delay > 0){
            delay -= 1;
        }else{
            if(!damaged){
                //AttackPart();
                AttackDamage();
                delay = 10;
                damaged = true;
            }else{
                EndAttack();
            }
        }
    }

    /*public void AttackPart(){
        // make particles
        //part = Instantiate(effect, transform.position, Quaternion.identity);
    }*/

    public void AttackDamage(){
        //Debug.Log("<anim> Finished sigil spin, damaging");
        // do damage
        target.HP -= dmg;
    }

    public void EndAttack(){
        // end player turn
        ply.showGrid();
        ply.state = Grid_Move.unitStates.MoveMode;
        ply.turn = false;
        TurnManager.EndTurn();
        //Destroy(part, 0.5f);
        //Destroy(self);
        Destroy(gameObject);
    }
}

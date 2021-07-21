using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffect : MonoBehaviour
{
    public Grid_Move target;
    public int dmg;
    public Animator anim;
    public Player_Move ply;
    public ParticleSystem[] effects;
    public int spell_index;
    ParticleSystem part;

    public void SpellCast(){
        anim.Play("SigilRotate");
    }

    public void SpellPart(){
        // make particles
        part = Instantiate(effects[spell_index], transform.position, Quaternion.identity);
    }

    public void SpellDamage(){
        // do damage
        target.HP -= dmg;
    }

    public void EndSpell(){
        // end player turn
        ply.showGrid();
        ply.state = Grid_Move.unitStates.MoveMode;
        ply.turn = false;
        TurnManager.EndTurn();
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffect : MonoBehaviour
{
    public Grid_Move target;
    public int dmg;
    public Animator anim;
    //public GameObject self;
    public Player_Move ply;
    public ParticleSystem[] effects;
    public int spell_index;
    ParticleSystem part;
    
    // Start is called before the first frame update
    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/

    public void SpellCast(){
        //
        //anim = GetComponent<Animator>();
        Debug.Log("<anim> Starting sigil spin");
        anim.Play("SigilRotate");
    }

    public void SpellPart(){
        // make particles
        part = Instantiate(effects[spell_index], transform.position, Quaternion.identity);
    }

    public void SpellDamage(){
        Debug.Log("<anim> Finished sigil spin, damaging");
        // do damage
        target.HP -= dmg;
    }

    public void EndSpell(){
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

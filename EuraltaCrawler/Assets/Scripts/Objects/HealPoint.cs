using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPoint : MonoBehaviour
{
    public int MP = 6;
    public ParticleSystem heal_prefab;

    public void Heal(Grid_Move target){
        if(MP > 0){
            int diff = (4 - target.MP);
            if(MP > diff){
                // keep extra
                target.MP += diff;
                MP -= diff;
            }else{
                // heal with what we have
                target.MP += MP;
                MP = 0;
            }
            Instantiate(heal_prefab, transform.position, Quaternion.identity);
        }
    }
}

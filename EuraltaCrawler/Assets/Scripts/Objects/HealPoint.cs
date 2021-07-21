using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPoint : MonoBehaviour
{
    public int MP = 6;
    public ParticleSystem heal_prefab;
    public GameObject lightObj;

    public void Heal(Grid_Move target){
        int diff = (8 - target.MP);
        if(MP > 0 && diff > 0){
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
            if(MP <= 0){
                lightObj.GetComponent<HealLight>().Change();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitch : TriggerTile
{
    public GameObject next;
    bool powered = false;
    //public GameObject lightsrc;
    
    protected override void TileAction() {
        Debug.Log("Stepped on power switch");
        PowerLine pwr = next.GetComponent<PowerLine>();
        pwr.PowerUpdate(!powered, pwr.prev);
        //lightsrc.SetActive(true);
        powered = !powered;
    }

    protected override void PostAction(){
        //Debug.Log("Stepped off power switch");
        //PowerLine pwr = next.GetComponent<PowerLine>();
        //pwr.PowerUpdate(false, pwr.prev);
    }
}

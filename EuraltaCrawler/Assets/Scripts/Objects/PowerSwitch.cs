using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitch : TriggerTile
{
    public GameObject next;
    bool powered = false;
    
    protected override void TileAction() {
        Debug.Log("Stepped on power switch");
        PowerLine pwr = next.GetComponent<PowerLine>();
        pwr.PowerUpdate(!powered, pwr.prev);
        powered = !powered;
    }

    protected override void PostAction(){
        //
    }
}

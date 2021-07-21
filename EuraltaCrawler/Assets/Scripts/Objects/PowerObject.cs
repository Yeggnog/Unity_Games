using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerObject : PowerLine
{
    public override void PowerUpdate(bool state, GameObject caller){
        // change power state
        powered = state;
        ObjectAction();
    }

    protected virtual void ObjectAction(){
        // does nothing for now
    }

    protected virtual void PostAction(){
        // does nothing for now
    }
}

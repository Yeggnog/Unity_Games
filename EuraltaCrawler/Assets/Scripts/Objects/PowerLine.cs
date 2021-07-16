using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerLine : MonoBehaviour
{
    public GameObject prev = null;
    public GameObject next = null;
    //public GameObject lightsrc;
    public Material unlit;
    public Material lit;
    protected bool powered = false;

    public virtual void PowerUpdate(bool state, GameObject caller){
        // change power state
        powered = state;
        if(caller == prev && next != null){
            Debug.Log("Passed power state "+state+" to "+next);
            next.GetComponent<PowerLine>().PowerUpdate(state, gameObject);
        }else if(caller == next && prev != null){
            Debug.Log("Passed power state "+state+" to "+prev);
            prev.GetComponent<PowerLine>().PowerUpdate(state, gameObject);
        }
        //lightsrc.SetActive(powered);
        if(powered){
            gameObject.GetComponent<MeshRenderer>().material = lit;
        }else{
            gameObject.GetComponent<MeshRenderer>().material = unlit;
        }
    }
}

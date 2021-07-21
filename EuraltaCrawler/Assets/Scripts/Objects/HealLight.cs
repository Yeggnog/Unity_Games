using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealLight : MonoBehaviour
{
    public Material unlit;
    
    public void Change(){
        gameObject.GetComponent<MeshRenderer>().material = unlit;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadZone : TriggerTile
{
    public int targetScene;
    public GameObject trans_prefab;
    
    protected override void TileAction() {
        Debug.Log("Entered load zone bound for scene "+targetScene);
        GameObject transition = Instantiate(trans_prefab, Vector3.zero, Quaternion.identity);
        transition.GetComponent<TransitionManager>().targetScene = targetScene;
        transition.GetComponent<TransitionManager>().startTransition();
    }
}

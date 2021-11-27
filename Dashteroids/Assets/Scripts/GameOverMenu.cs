using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    // Vars
    public GameObject menuOrange;
    public GameObject menuBlue;

    void Update(){
        // Apply screenshake
        menuOrange.transform.position = transform.position + (ScreenManip.getOffset(0) * 300f);
        menuBlue.transform.position = transform.position + (ScreenManip.getOffset(1) * 300f);
    }
}

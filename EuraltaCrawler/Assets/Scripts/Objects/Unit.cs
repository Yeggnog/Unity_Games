using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float HP = 3;
    public enum unitStates { MoveMode, Moving, ActionMode, Actioning, MenuMode };
    public unitStates state = unitStates.MoveMode;
}

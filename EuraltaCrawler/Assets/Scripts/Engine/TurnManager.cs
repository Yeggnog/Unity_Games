using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    static Dictionary<string, List<Grid_Move>> units = new Dictionary<string, List<Grid_Move>>(); // string is for team affil. tag
    static Queue<string> teamKeys = new Queue<string>();
    static Queue<Grid_Move> unitQueue = new Queue<Grid_Move>();
    public static  Grid_Move focused_unit;
    
    // Start is called before the first frame update
    void Start(){
        focused_unit = null;
        InitTeamTurnQueue();
    }

    // Update is called once per frame
    void Update(){
        if(unitQueue.Count == 0){
            // init unit queue
            InitTeamTurnQueue();
        }
    }

    static void InitTeamTurnQueue(){
        // get team list for this team
        List<Grid_Move> teamList = units[teamKeys.Peek()];
        foreach(Grid_Move unit in teamList){
            unitQueue.Enqueue(unit);
        }
        // start turn
        StartTurn();
    }

    public static void StartTurn(){
        if(unitQueue.Count > 0){
            // take turn for first unit
            Grid_Move unit = unitQueue.Peek();
            focused_unit = unit;
            unit.BeginTurn();
        }
    }

    public static void EndTurn(){
        // end a unit's turn
        Grid_Move unit = unitQueue.Dequeue();
        unit.EndTurn();
        if(unitQueue.Count > 0){
            // start for next unit
            StartTurn();
        }else{
            // move to next team
            string team = teamKeys.Dequeue();
            teamKeys.Enqueue(team);
            InitTeamTurnQueue();
        }
    }

    public static void AddUnit(Grid_Move unit){
        // add newly created units to the dictionary
        List<Grid_Move> list;
        if(!units.ContainsKey(unit.tag)){
            // unit's not in dictionary
            list = new List<Grid_Move>();
            units[unit.tag] = list;
            if(!teamKeys.Contains(unit.tag)){
                // tag not in team registry either
                teamKeys.Enqueue(unit.tag);
            }
        }else{
            // fetch from dictionary
            list = units[unit.tag];
        }
        list.Add(unit);
    }

    public static void RemoveUnit(Grid_Move unit){
        // remove from the dictionary
        if(units.ContainsKey(unit.tag)){
            // get a count of units with the same tag
            int count = 1;
            List<Grid_Move> list = units[unit.tag];
            count = list.Count;
            // remove from unit dictionary
            units[unit.tag].Remove(unit);
            if(count == 1){
                // last member of team, remove team tag
                if(teamKeys.Contains(unit.tag)){
                    // go through stack and remove refs to team tag
                    Queue<string> transfer = new Queue<string>();
                    while(teamKeys.Count > 0){
                        string teamTag = teamKeys.Dequeue();
                        if(teamTag != unit.tag){
                            transfer.Enqueue(teamTag);
                        }
                    }
                    while(transfer.Count > 0){
                        string item = transfer.Dequeue();
                        teamKeys.Enqueue(item);
                    }
                }
            }
        }
    }

    public Grid_Move GetFocusedUnit(){
        return focused_unit;
    }

    void OnDestroy(){
        units = new Dictionary<string, List<Grid_Move>>();
        teamKeys = new Queue<string>();
        unitQueue = new Queue<Grid_Move>();
        focused_unit = null;
    }

    static void PrintUnits(){
        foreach(string key in teamKeys){
            string group = "group "+key+" - [";
            List<Grid_Move> lst = units[key];
            foreach(Grid_Move unit in lst){
                // print
                group += "<"+unit.ToString()+">, ";
            }
            group += "]";
            Debug.Log(group);
        }
    }
}

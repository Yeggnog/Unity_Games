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
        //Debug.Log("[started battle system]");
        //PrintUnits();
        focused_unit = null;
        InitTeamTurnQueue();
        //PrintUnits();
    }

    // Update is called once per frame
    void Update(){
        if(unitQueue.Count == 0){
            // init unit queue
            //Debug.Log("[starting init]");
            InitTeamTurnQueue();
            //Debug.Log("[initialized team turn queue to "+units[teamKeys.Peek()]+"]");
        }//else{
            //Debug.Log("[unit queue is still "+unitQueue+"]");
            /*string str = "";
            List<Grid_Move> tmp = units[teamKeys.Peek()];
            foreach(Grid_Move temp in tmp){
                str += temp.ToString()+" --|-- ";
            }
            Debug.Log("[unit list is still "+str+"]");*/
        //}
        //Debug.Log("[post]");
    }

    static void InitTeamTurnQueue(){
        // get team list for this team
        //Debug.Log("[start of team "+teamKeys.Peek()+"'s turn]");
        List<Grid_Move> teamList = units[teamKeys.Peek()];
        foreach(Grid_Move unit in teamList){
            unitQueue.Enqueue(unit);
            //Debug.Log("[enqueued unit "+unit+"]");
        }
        // start turn
        StartTurn();
    }

    public static void StartTurn(){
        if(unitQueue.Count > 0){
            // take turn for first unit
            Grid_Move unit = unitQueue.Peek();
            //Debug.Log("[start of unit "+unit.name+"'s turn]");
            focused_unit = unit;
            unit.BeginTurn();
        }
    }

    public static void EndTurn(){
        // end a unit's turn
        Grid_Move unit = unitQueue.Dequeue();
        //Debug.Log("[dequeued unit "+unit+"]");
        unit.EndTurn();
        if(unitQueue.Count > 0){
            // start for next unit
            //Debug.Log("[end of unit's turn]");
            //Debug.Log("-----------------------------");
            StartTurn();
        }else{
            // move to next team
            //Debug.Log("[end of team's turn]");
            //Debug.Log("--------------------------------------------------------");
            string team = teamKeys.Dequeue();
            //Debug.Log("[dequeued team "+team+"]");
            teamKeys.Enqueue(team);
            //Debug.Log("[enqueued team "+team+"]");
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
        //Debug.Log("Current Units: (pre)");
        //PrintUnits();
        //Debug.Log("[trying to remove unit "+unit+"...]");
        // remove from the dictionary
        if(units.ContainsKey(unit.tag)){
            // get a count of units with the same tag
            int count = 1;
            List<Grid_Move> list = units[unit.tag];
            //Debug.Log("==================================== GOT HEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEERE");
            // remove from unit dictionary
            //Debug.Log("[removing unit "+unit+"...]");
            units[unit.tag].Remove(unit);
            //Debug.Log("[done]");
            //count += list.Count;
            count = System.Math.Max(count, list.Count);
            if(count == 1){
                //Debug.Log("[unit is the last member of team "+unit.tag+"]");
                // last member of team, remove team tag
                if(teamKeys.Contains(unit.tag)){
                    //Debug.Log("[removing tag "+unit.tag+"...]");
                    // go through stack and remove refs to team tag
                    Queue<string> transfer = new Queue<string>();
                    while(teamKeys.Count > 0){
                        string teamTag = teamKeys.Dequeue();
                        if(teamTag != unit.tag){
                            //Debug.Log("[re-enqueueing tag "+teamTag+"]");
                            transfer.Enqueue(teamTag);
                        }
                    }
                    while(transfer.Count > 0){
                        string item = transfer.Dequeue();
                        teamKeys.Enqueue(item);
                        //Debug.Log("[enqueued unit "+item+"]");
                    }
                }
            }
            // remove from current unit queue?
            //unitQueue.Dequeue();
            //Debug.Log("Current Units: (post)");
            //PrintUnits();
        }
    }

    public Grid_Move GetFocusedUnit(){
        //Debug.Log("getting focused unit "+focused_unit);
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

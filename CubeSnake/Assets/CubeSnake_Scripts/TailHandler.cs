using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TailHandler : MonoBehaviour
{

    private static List<Vector3> TheList;
    private const int MaxList = 100000;
    private static Dictionary<int, Tail> AllTails;

    // Start is called before the first frame update
    private void Awake()
    {
        AllTails = new Dictionary<int, Tail>();
        TheList = new List<Vector3>();
        Tail.BlockIndex = 0;
    }

    public static void AddTailSegment(Tail tail)
    {
        if(AllTails.ContainsKey(tail.tailIndex))
        {
            AllTails.Remove(tail.tailIndex);
        }


        AllTails.Add(tail.tailIndex, tail);
    }

    public static void RemoveTailSegment(Tail tail)
    {
        AllTails.Remove(tail.tailIndex);
    }

    public static void ObjectInTheWay(int ID)
    {
        int start = ID - 1;

        for (int i = 0; i < 3; i++)
        {
            if (AllTails.ContainsKey(start + i))
            {
                Tail tail = AllTails[start + i];
                tail.YourInTheWay();
            }
        }
    }


    public static void AddBlock(int input = 1)
    {
        for (int i = 0; i < GameHandler.newBodyCount * input; i++)
        {
            GameObject body = UnityEngine.Object.Instantiate(Tail.instance);
        }
    }

    public static void PushPos(Vector3 input)
    {
        if(TheList != null)
        {
            TheList.Insert(0, input);
        }

        if (TheList.Count > MaxList)
        {
            TheList.RemoveAt(MaxList - 1);
        }

    }

    public static Vector3 GetPos(int index)
    {
        if(TheList != null)
        {
            if (TheList.Count > index)
            {
                return TheList.ElementAt(index);
            }
        }
        
        return new Vector3();
    }
}

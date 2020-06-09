using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    private Transform transform;
    public static GameObject instance = null;
    
    private int listIndex;
    public int tailIndex;

    public static int deadBlocks;
    public static int BlockIndex;

    private Material transucent;
    private Material regular;

    private int ClearClock;

    private bool alive;
    private void Awake()
    {
        if(instance == null)
        {
            instance = gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "tail";
        BlockIndex++;
        tailIndex = BlockIndex;
        listIndex = (tailIndex - 1) * 10;

        TailHandler.AddTailSegment(this);

        Color color = Gradient.GetGradient(listIndex % 500, 500);
        regular = gameObject.GetComponent<MeshRenderer>().material;
        regular.color = color;
        transucent = new Material(Resources.Load<Material>("Snake_Materials/Translucent"));
        transucent.color = new Color(color.r, color.g, color.b, transucent.color.a);


        if (tailIndex < 5)
        {
            gameObject.tag = "closeToHead";
            gameObject.layer = 1;
        }
        else
        {
            gameObject.layer = 0;
            gameObject.name = ((int)(tailIndex)).ToString();
        }

        alive = true;

        transform = gameObject.GetComponent<Transform>();
        SnakeUI.UpdatePoints(Tail.BlockIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameHandler.IsGamePaused())
        {
            if (alive)
            {
                transform.position = TailHandler.GetPos(listIndex);

                if (deadBlocks < tailIndex && deadBlocks != -1)
                {
                    alive = false;
                    gameObject.GetComponent<MeshRenderer>().materials[0].color = new Color(.3f, .3f, .3f);
                    gameObject.name = "dead";
                    gameObject.tag = "dead";
                    TailHandler.RemoveTailSegment(this);
                }

                if (ClearClock > 0)
                {
                    ClearClock--;
                    if (ClearClock == 0)
                    {
                        gameObject.GetComponent<MeshRenderer>().material = regular;
                    }
                }
            }
        }
    }

    public void YourInTheWay()
    {
        ClearClock = 5;
        gameObject.GetComponent<MeshRenderer>().material = transucent;
    }

    public bool IsAlive()
    {
        return alive;
    }


}

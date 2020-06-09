using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstical : MonoBehaviour
{
    private static Obstical instance;
    private static List<Obstical> obsticals = new List<Obstical>();

    private Material transusent;
    private Material regular;
    private bool visable;

    private void Awake()
    {
        regular = gameObject.GetComponent<MeshRenderer>().material;
        Color color = regular.color;
        transusent = new Material(Resources.Load<Material>("Snake_Materials/Translucent"));
        transusent.color = new Color(color.r, color.g, color.b, transusent.color.a);

        obsticals.Add(this);
        visable = true;
    }

    private void Update()
    {
        if(GameHandler.ObjectBeingHit != null && visable)
        {
            if(GameHandler.ObjectBeingHit.name == gameObject.name)
            {
                visable = false;
                gameObject.GetComponent<MeshRenderer>().material = transusent;
            }
        }
        else if(!visable)
        {
            if (GameHandler.ObjectBeingHit != null)
            {
                if (GameHandler.ObjectBeingHit.name != gameObject.name)
                {
                    visable = true;
                    gameObject.GetComponent<MeshRenderer>().material = regular;
                }
            }
            else
            {
                visable = true;
                gameObject.GetComponent<MeshRenderer>().material = regular;
            }

        }
    }
}

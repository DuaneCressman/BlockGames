using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector2Int GamePos;

    public Transform trans;

    private float Scale;
    private const float Scale_Max = 0.9f;
    private const float Scale_Min = 0.0f;

    private bool Growing;
    private bool Dying;


    // Start is called before the first frame update
    void Start()
    {
        trans = gameObject.GetComponent<Transform>();

        Growing = true;
        Scale = Scale_Min;
        trans.localScale = new Vector3(Scale, Scale, Scale);
    }

    // Update is called once per frame
    void Update()
    {
        if (Growing)
        {
            Scale += 0.1f;

            if (Scale > Scale_Max)
            {
                Scale = Scale_Max;
                Growing = false;
            }

            trans.localScale = new Vector3(Scale, Scale, Scale);
        }

        if (Dying)
        {
            Scale -= 0.1f;

            if (Scale < Scale_Min)
            {
                Destroy(this.gameObject);
            }

            trans.localScale = new Vector3(Scale, Scale, Scale);
        }

    }


    public void KillNode()
    {
        Dying = true;
        Growing = false;
    }

    public Vector2Int GetGamePos()
    {
        return GamePos;
    }

    public void SetGamePos(Vector2Int inGamePos)
    {
        GamePos = inGamePos;
    }
}

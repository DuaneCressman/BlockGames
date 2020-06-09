using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerNode : MonoBehaviour
{
    public Vector2Int GamePos;
    private const float Scale_Max = 0.9f;
    public Transform trans;


    // Start is called before the first frame update
    void Start()
    {
        trans = gameObject.GetComponent<Transform>();
        trans.localScale = new Vector3(Scale_Max, Scale_Max, Scale_Max);
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

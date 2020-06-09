using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClass : MonoBehaviour
{
    private const float MovementSpeed = 0.1f;
    private Transform trans;


    // Start is called before the first frame update
    void Start()
    {
        trans = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            trans.localPosition += new Vector3(0, MovementSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            trans.localPosition += new Vector3(0, -MovementSpeed, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            trans.localPosition += new Vector3(-MovementSpeed, 0, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            trans.localPosition += new Vector3(MovementSpeed, 0, 0);
        }
    }
}

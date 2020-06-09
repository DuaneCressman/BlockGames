using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    private Transform trans;
    public int rotating;
    public int rotatingMax;
    //private Snek snek;

    private Vector3 RotationDone;
    LineRenderer lineRenderer;

    private static Snake snek;

    private void Awake()
    {
        lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        trans = gameObject.GetComponent<Transform>();
        rotating = 0;
        rotatingMax = 35;
    }

    public static void SetSnek(Snake inSnek)
    {
        snek = inSnek;
    }

    // Update is called once per frame
    void Update()
    { 

        if(!GameHandler.IsGamePaused())
        {
            trans.transform.LookAt(snek.transform.position);

            Vector3 Start = trans.position;
            Vector3 Drirection = trans.forward;



            bool HitAnotherBlock = false;

            do
            {
                HitAnotherBlock = false;
                Ray ray = new Ray(Start, Drirection);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit, Vector3.Distance(snek.transform.position, Start), 1))
                {
                    if (hit.collider.gameObject.CompareTag("tail"))
                    {
                        Tail tail = hit.transform.GetComponent<Tail>();
                        if (tail != null)
                        {
                            HitAnotherBlock = true;
                            Start = hit.point + Drirection;
                            TailHandler.ObjectInTheWay(tail.tailIndex);
                        }
                    }
                    else
                    {
                        GameHandler.ObjectBeingHit = hit.collider.gameObject;
                    }
                }
                else
                {
                    GameHandler.ObjectBeingHit = null;
                }
            }
            while (HitAnotherBlock);





            if (GameHandler.SnakeRotating == 1)
            {
                GameHandler.SnakeRotating = 0;
                GameHandler.CameraMoving = true;
                rotating = 1;
            }
            else if (GameHandler.SnakeRotating == 2)
            {
                GameHandler.SnakeRotating = 0;
                GameHandler.CameraMoving = true;
                rotating = -1;
            }



            if (rotating == 0)
            {
                trans.transform.position = snek.transform.position + (snek.Direction * -1 * 20) + Vector3.up * snek.transform.position.y * .3f;
            }
            else
            {
                float rotationAbs = Mathf.Abs(trans.transform.rotation.eulerAngles.y);

                if (Mathf.Abs(rotating) > 4)
                {
                    int setDefault = -1;

                    if (rotationAbs > 88 && rotationAbs < 92)
                    {
                        setDefault = 90;
                    }
                    else if (rotationAbs > 355 || rotationAbs < 2)
                    {
                        setDefault = 0;
                    }
                    else if (rotationAbs > 178 && rotationAbs < 182)
                    {
                        setDefault = 180;
                    }
                    else if (rotationAbs > 268 && rotationAbs < 272)
                    {
                        setDefault = 270;
                    }

                    if (setDefault != -1)
                    {
                        GameHandler.CameraMoving = false;

                        rotating = 0;
                    }
                }

                if (rotating > 0)
                {
                    rotating++;
                    trans.transform.Translate(Vector3.left * 1f);
                }
                else if (rotating < 0)
                {
                    rotating--;
                    trans.transform.Translate(Vector3.right * 1f);
                }
            }
        }
    }    
}

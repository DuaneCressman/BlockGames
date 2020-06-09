using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour
{
    enum em
    {
        right,
        left,
        up,
        down,
        not
    }

    public Vector3 Direction;

    private Transform trans;
    private float Speed;
    private float PrevSpeed;

    private float changingHeight;
    private float changingHeightMax;
    private float SpeedingUp;
    private float SlowingDown;

    private int chopClock;

    private GameObject headlight;


    // Start is called before the first frame update
    void Start()
    {

        headlight = GameObject.Find("HeadLight");
        headlight.transform.rotation = new Quaternion();
        headlight.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);

        trans = gameObject.GetComponent<Transform>();

        GameHandler.SetSnek(this);

        Direction = new Vector3(1, 0, 0);
        Speed = 0.1f;
        trans.position = (GameHandler.CubeSize + 1.5f) * Vector3.up;

        changingHeight = 0;
        changingHeightMax = (1 / Speed);

        gameObject.GetComponent<MeshRenderer>().material.color = Gradient.GetGradient(0 % 500, 500);

        chopClock = 0;

        Grow(5);

        Tail.deadBlocks = -1;

        MyCamera.SetSnek(this);

    }


    



    // Update is called once per frame
    void Update()
    {
        //Check if the game is paused
        if(!GameHandler.IsGamePaused())
        {
            //check if the snake should grow for debugging 
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Grow();
            }

            HandleSpeedChangingUpdate();

            //check if the camera is moving
            if (!GameHandler.CameraMoving)
            {
                //push the position of the head onto the list
                TailHandler.PushPos(trans.position);

                //check if the snake is moving vertically
                if(!HandleHeightChangingUpdate())
                {

                    //handle the input from the key board
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        //start moving up
                        trans.position += Vector3.up * Speed;
                        changingHeight = 1;
                    }
                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        //check if at the bottom of the stage
                        if (trans.position.y > (GameHandler.CubeSize + .5f))
                        {
                            //start moving down
                            trans.position -= Vector3.up * Speed;
                            changingHeight = -1;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.A))
                    {
                        //start moving left
                        Turn(em.left);
                        GameHandler.SnakeRotating = 2;
                    }
                    else if (Input.GetKeyDown(KeyCode.D))
                    {
                        //start moving right
                        Turn(em.right);
                        GameHandler.SnakeRotating = 1;
                    }

                    //check the game handler if the snake should grow
                    if (GameHandler.SnakeShouldGrow)
                    {
                        GameHandler.SnakeShouldGrow = false;
                        Grow();
                    }

                    //move the snake
                    trans.position += Direction * Speed;
                }

                HandleChopClock();

            }
        }
    }

    /* Method Name: Turn
        * Parameters: em direction - The direction to turn
        * Return: void
        * 
        * Purpose: This method can be used to change the direction that the snake is moving
        */
    private void Turn(em direction)
    {

        if (direction == em.left)
        {
            Direction = Rotate(Direction, Vector3.up * -1);
            headlight.transform.Rotate(0.0f, -90.0f, 0.0f, Space.Self);
        }
        else if (direction == em.right)
        {
            Direction = Rotate(Direction, Vector3.up);
            headlight.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
        }

    }

    /* Method Name: Rotate
        * Parameters: Vector3 input - The vector to be rotated
        *             Vector3 rotate - The rotation to be done
        * Return: Vector3 - The resultant vector
        * 
        * Purpose: This method will calculate where on a gradient the input would be.
        *          It allows 2 inputs so that you can enter a ratio instead of a set gradient value.
        */
    private Vector3 Rotate(Vector3 input, Vector3 rotate)
    {
        //The vector to be returned 
        Vector3 rVal = new Vector3();
        float theAngle = 90;

        if (rotate.x != 0)
        {
            //rotate in the x direction
            theAngle *= rotate.x;
            rVal = new Vector3(input.x, input.y * Mathf.Cos(theAngle * Mathf.Deg2Rad) - input.z * Mathf.Sin(theAngle * Mathf.Deg2Rad),
                                input.y * Mathf.Sin(theAngle * Mathf.Deg2Rad) + input.z * Mathf.Cos(theAngle * Mathf.Deg2Rad));
        }
        else if (rotate.y != 0)
        {
            //rotate in the y direction
            theAngle *= rotate.y;
            rVal = new Vector3((input.x * Mathf.Cos(theAngle * Mathf.Deg2Rad)) + (input.z * Mathf.Sin(theAngle * Mathf.Deg2Rad)), input.y, (-1 * input.x * Mathf.Sin(theAngle * Mathf.Deg2Rad)) + (input.z * Mathf.Cos(theAngle * Mathf.Deg2Rad)));
        }
        else if (rotate.z != 0)
        {
            //rotate in the z direction
            theAngle *= rotate.z;
            rVal = new Vector3(input.x * Mathf.Cos(theAngle * Mathf.Deg2Rad) - input.y * Mathf.Sin(theAngle * Mathf.Deg2Rad), input.x * Mathf.Sin(theAngle * Mathf.Deg2Rad) + input.y * Mathf.Cos(theAngle * Mathf.Deg2Rad), input.z);
        }

        return rVal;
    }

    private bool HandleHeightChangingUpdate()
    {
        //if the snake is moving up
        if (changingHeight > 0)
        {
            //check if moved the increment
            if (changingHeight >= changingHeightMax)
            {
                //check if the user still wants to move up
                if (Input.GetKey(KeyCode.W))
                {
                    changingHeight = 1;
                }
                else
                {
                    changingHeight = 0;
                }
            }
            else
            {
                //increment the counter
                changingHeight++;
            }

            trans.position += Vector3.up * Speed;

            if (changingHeight == 0)
            {
                trans.position = new Vector3(trans.position.x, (float)Math.Truncate(trans.position.y) + .5f, trans.position.z);
            }

            return true;
        }
        else if (changingHeight < 0)
        {
            if (Mathf.Abs(changingHeight) >= changingHeightMax)
            {
                if (Input.GetKey(KeyCode.S) && trans.position.y > (GameHandler.CubeSize + .5f))
                {
                    changingHeight = -1;
                }
                else
                {
                    changingHeight = 0;
                }
            }
            else
            {
                changingHeight--;
            }

            trans.position -= Vector3.up * Speed;

            if (changingHeight == 0)
            {
                trans.position = new Vector3(trans.position.x, (float)Math.Truncate(trans.position.y) + .5f, trans.position.z);
            }

            return true;
        }

        return false;
    }

    private void HandleSpeedChangingUpdate()
    {
        //check if the speed is changing
        if ((SpeedingUp == 0 && SlowingDown == 0) && Input.GetKeyDown(KeyCode.Space))
        {
            SpeedingUp = 1;
            PrevSpeed = Speed;
        }

        if (SpeedingUp != 0)
        {
            if (SpeedingUp < 8)
            {
                Speed += 0.01f;
                SpeedingUp++;
            }
            else
            {
                if (!Input.GetKey(KeyCode.Space))
                {
                    SlowingDown = 1;
                    SpeedingUp = 0;
                }
            }
        }
        else if (SlowingDown != 0)
        {
            Speed -= 0.01f;


            if (Mathf.Abs(Speed - PrevSpeed) < .1)
            {
                Speed = PrevSpeed;
                SlowingDown = 0;
            }


        }
    }

    private void HandleChopClock()
    {
        if (chopClock != 0)
        {
            chopClock--;

            if (chopClock == 0)
            {
                Tail.deadBlocks = -1;
            }
        }
    }

    public Snake GetSnek()
    {
        return this;
    }

    private void Grow(int input = 1)
    {
        TailHandler.AddBlock(input); 
    }

    private void OnTriggerEnter(Collider other)
    {
        bool Died = true;

        if(other.gameObject.CompareTag("food"))
        {
            TailHandler.AddBlock();
            Died = false;
        }
        else if(other.gameObject.CompareTag("closeToHead"))
        {
            Died = false;
        }
        else
        {
            Tail tail = other.gameObject.GetComponent<Tail>();
            if(tail != null)
            {
                if(tail.IsAlive())
                {
                    Tail.BlockIndex = tail.tailIndex;
                    Tail.deadBlocks = Tail.BlockIndex + 1;
                    SnakeUI.UpdatePoints(Tail.BlockIndex);
                    chopClock = 10;
                    Died = false;
                }
            }        
        }

        if(Died && chopClock == 0)
        {
            Tail.BlockIndex /= 2;
            Tail.deadBlocks = Tail.BlockIndex + 1;
            SnakeUI.UpdatePoints(Tail.BlockIndex);
            chopClock = 10;
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    private bool ScrollWheelPressed;
    private Vector3 LastMousePos;

    // Start is called before the first frame update
    void Start()
    {
        //counter = 0;
        LastMousePos = new Vector3(0, 0, 0);

        ScrollWheelPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            //GameHandler.AutoStep_Off();
            ScrollWheelPressed = true;
            LastMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            //GameHandler.AutoStep_On();
            ScrollWheelPressed = false;
            LastMousePos = Input.mousePosition;
        }

        if (ScrollWheelPressed)
        {
            if (LastMousePos == new Vector3(0, 0, 0))
            {
                LastMousePos = Input.mousePosition;
            }
            else
            {
                Vector2 delta = Input.mousePosition - LastMousePos;
                LastMousePos = Input.mousePosition;
                GOL_GameHandler.MoveGameBoard(delta * 0.01f);
            }
        }

        GOL_GameHandler.SetMousePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 20)));

        if (Input.GetMouseButtonDown(0))
        {
            GOL_GameHandler.PauseGame();
            GOL_GameHandler.CreateMarker();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            GOL_GameHandler.ResumeGame();
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            GOL_GameHandler.UpdateGameBoardScale(Input.mouseScrollDelta.y);
        }

    }
}

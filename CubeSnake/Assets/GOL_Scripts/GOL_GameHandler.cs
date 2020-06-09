using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOL_GameHandler : MonoBehaviour
{
    private static GameBoard gameBoard;

    private static bool HardPause;

    private static bool GameRunning;
    private static float GameSpeed;
    private const float DefaultGameSpeedChange = 0.05f;
    private const float FastestGameSpeed = 0.01f;

    private static Vector3 MousePosition;

    private static bool SelectionStarted;
    private static Vector2Int SelectionOne;


    public static void UpdateGameBoardScale(float input)
    {
        gameBoard.UpdateScale(input * 0.05f);
    }

    public static void SetMousePosition(Vector3 inPos)
    {
        MousePosition = inPos;
    }

    public static Vector3 GetMousePosition()
    {
        return MousePosition;
    }

    public static bool IsGameRunning()
    {
        if (!HardPause)
        {
            return GameRunning;
        }

        return false;
    }

    public static void HardPauseOn()
    {
        HardPause = true;
    }

    public static void HardPauseOff()
    {
        HardPause = false;
    }


    public static void ResumeGame()
    {
        GameRunning = true;
    }

    public static void PauseGame()
    {
        GameRunning = false;
    }

    public static void DoOneStep()
    {
        gameBoard.Step();
    }

    public static void SetGameBoard(GameBoard inGameBoard)
    {
        gameBoard = inGameBoard;
    }

    public static void MoveGameBoard(Vector3 movment)
    {
        if (gameBoard != null)
        {
            gameBoard.MoveGameBoard(movment);
        }
    }

    public static void CreateMarker()
    {
        if (gameBoard != null)
        {
            gameBoard.CreateMarker();
        }
    }

    public static void ResetBoard()
    {
        gameBoard.ResetBoard();
    }

    public static void StartSelection()
    {
        SelectionStarted = true;
        SelectionOne = gameBoard.GetMarkerPosition();
    }



    // Start is called before the first frame update
    void Start()
    {
        GameRunning = true;
        HardPause = false;
        GameSpeed = 1;
    }

    public static float GetGameSpeed()
    {
        return GameSpeed;
    }

    public static void IncreaseGameSpeed(float speed = DefaultGameSpeedChange)
    {
        for (int i = 0; i < 15; i++)
        {
            if (GameSpeed - speed < FastestGameSpeed)
            {
                speed /= 2;
            }
            else
            {
                break;
            }
        }



        GameSpeed -= speed;

        if (GameSpeed < FastestGameSpeed)
        {
            GameSpeed = FastestGameSpeed;
        }
    }

    public static void DecreaseGameSpeed(float speed = DefaultGameSpeedChange)
    {
        GameSpeed += speed;
    }
}

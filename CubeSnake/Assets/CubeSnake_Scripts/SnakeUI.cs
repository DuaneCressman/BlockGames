using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeUI : MonoBehaviour
{
    private static TextMeshProUGUI PointText;
    private static GameObject PauseMenu;


    private void Awake()
    {
        PointText = GetComponentInChildren<TextMeshProUGUI>();

        PauseMenu = GameObject.Find("PauseMenu");
        PauseMenu.SetActive(false);
    }

    public static void UpdatePoints(int inPoints)
    {
        if(PointText != null)
        {
            PointText.text = "Points: " + inPoints;
        }
    }

    public static void ShowPauseMenu()
    {
        PauseMenu.SetActive(true);
    }

    public static void HidePauseMenu()
    {
        PauseMenu.SetActive(false);
    }

    public void ContinuePressed()
    {
        HidePauseMenu();
        GameHandler.PauseMenuLowered();
    }

    public void MainMenuPressed()
    {
        SceneManager.LoadScene(0);
    }
}

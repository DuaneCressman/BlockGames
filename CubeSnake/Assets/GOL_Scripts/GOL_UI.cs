using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GOL_UI : MonoBehaviour
{
    private bool AutoTogglePressed;
    public GameObject PauseButton;
    private Button UpButton;
    


    private Color UnpressedColour;
    private Color PressedColour;

    private void Start()
    {
        AutoTogglePressed = false;
        UnpressedColour = new Color(0.8f, 0.8f, 0.8f);
        PressedColour = new Color(0.4f, 0.4f, 0.4f);

        PauseButton.GetComponent<Image>().color = UnpressedColour;
        AutoTogglePressed = false;
    }



    public void SpeedUpPressed()
    {
        GOL_GameHandler.IncreaseGameSpeed();
    }

    public void SlowDownPressed()
    {
        GOL_GameHandler.DecreaseGameSpeed();
    }

    public void ResetPressed()
    {
        GOL_GameHandler.ResetBoard();
    }

    public void PausePressed()
    {
        if (AutoTogglePressed)
        {
            PauseButton.GetComponent<Image>().color = UnpressedColour;
            AutoTogglePressed = false;
            GOL_GameHandler.HardPauseOff();
        }
        else
        {
            PauseButton.GetComponent<Image>().color = PressedColour;
            AutoTogglePressed = true;
            GOL_GameHandler.HardPauseOn();
        }
    }

    public void MainMenuPressed()
    {
        SceneManager.LoadScene(0);
    }
}

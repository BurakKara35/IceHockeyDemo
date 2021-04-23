using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int redScore = 0;
    private int blueScore = 0;

    private UIManager ui;

    private void Awake()
    {
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
    }

    public void ScoreForRed()
    {
        redScore++;
        ui.changeRedScore(redScore);
    }

    public void ScoreForBlue()
    {
        blueScore++;
        ui.changeBlueScore(blueScore);
    }
}

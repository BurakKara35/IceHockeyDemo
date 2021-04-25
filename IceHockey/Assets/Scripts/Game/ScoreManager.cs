using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int redScore = 0;
    private int blueScore = 0;

    private UIManager ui;
    private GameManager game;

    private void Awake()
    {
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        game = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void ScoreForRed()
    {
        redScore++;
        ui.changeRedScore(redScore);
        CheckForFinish(redScore, "COMPUTER");
    }

    public void ScoreForBlue()
    {
        blueScore++;
        ui.changeBlueScore(blueScore);
        CheckForFinish(blueScore, "PLAYER");
    }

    private void CheckForFinish(int score, string winner)
    {
        if (score == 5)
        {
            ui.SetWinner(winner, blueScore, redScore);
            redScore = 0;
            blueScore = 0;
            ui.InitializeScore();
            game.StopGameAfterFinish();
        }
    }
}

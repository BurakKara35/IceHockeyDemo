using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text redScore;
    [SerializeField] private Text blueScore;

    [SerializeField] private Text winnerText;
    [SerializeField] private Text scoreText;

    public void changeRedScore(int score)
    {
        redScore.text = "" + score;
    }

    public void changeBlueScore(int score)
    {
        blueScore.text = "" + score;
    }

    public void InitializeScore()
    {
        blueScore.text = "0";
        redScore.text = "0";
    }

    public void SetWinner(string winner, int blueScore, int redScore)
    {
        scoreText.text = blueScore + " - " + redScore;
        winnerText.text = winner + " WIN.";
    }
}

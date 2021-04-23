using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text redScore;
    [SerializeField] private Text blueScore;

    public void changeRedScore(int score)
    {
        redScore.text = "" + score;
        CheckFinalScore(score);
    }

    public void changeBlueScore(int score)
    {
        blueScore.text = "" + score;
        CheckFinalScore(score);
    }

    private void CheckFinalScore(int score)
    {
        if(score == 5)
        {
            Debug.Log("Finish");
        }
    }
}

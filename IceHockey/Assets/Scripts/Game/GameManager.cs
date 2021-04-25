using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject redPlayer;
    [SerializeField] private GameObject bluePlayer;
    [SerializeField] private GameObject ball;

    [Header("UI Objects")]
    [SerializeField] private GameObject gameLevelMenu;
    [SerializeField] private GameObject finishUI;

    public static float playerWidth = 1.8f;

    private int gameLevel;

    private void Awake()
    {
        StopGameFirst();
    }

    public void SelectedLevel(int level)
    {
        gameLevel = level - 1;
    }

    public void StartGame()
    {
        gameLevelMenu.SetActive(false);
        finishUI.SetActive(false);
        redPlayer.SetActive(true);
        redPlayer.GetComponent<AIPlayer>().GameLevel = gameLevel;
        bluePlayer.SetActive(true);
        ball.SetActive(true);
    }

    public void StopGameFirst()
    {
        StopGame();
        finishUI.SetActive(false);
    }

    public void StopGameAfterFinish()
    {
        StopGame();
        finishUI.SetActive(true);
    }

    private void StopGame()
    {
        redPlayer.SetActive(false);
        bluePlayer.SetActive(false);
        ball.SetActive(false);
        gameLevelMenu.SetActive(true);
    }
}
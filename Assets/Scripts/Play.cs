using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Play : MonoBehaviour
{
    [SerializeField] private GameObject playMenu;
    [SerializeField] private GameObject selectGridSize;
    [SerializeField] private GameObject selectGameMode;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject whosTurn;

    [SerializeField] private GameObject[] grids;

    public int gridSizeIndex;
    public int gameMode;

    TextMeshProUGUI gameOverText;
    GameController gameController;

    private void Start()
    {
        gameOverText = gameOver.GetComponentInChildren<TextMeshProUGUI>();
        gameController = GetComponent<GameController>();
    }

    public void SelectGridSize()
    {
        playMenu.SetActive(false);
        selectGridSize.SetActive(true);
    }

    public void SelectGameMode(int index)
    {
        selectGridSize.SetActive(false);
        selectGameMode.SetActive(true);
        gridSizeIndex = index;
    }

    public void StartGame(int turn)
    {
        grids[gridSizeIndex].SetActive(true);
        gameController.Init(gridSizeIndex + 3, turn, gameMode);
    }
    public void WhosTurn(int mode)
    {
        gameMode = mode;
        selectGameMode.SetActive(false);
        if (mode != 1)
        {
            StartGame(-1);
            return;
        }
        whosTurn.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver(string receivedText)
    {
        grids[gridSizeIndex].SetActive(false);
        gameOver.SetActive(true);
        if (receivedText == "Draw")
        {
            gameOverText.text = receivedText;
        }
        else
        {
            gameOverText.text = receivedText + " Wins!!!";
        }

    }

}

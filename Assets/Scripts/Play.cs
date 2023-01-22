using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour
{
    [SerializeField] private GameObject playMenu;
    [SerializeField] private GameObject selectGridSize;
    [SerializeField] private GameObject selectGameMode;
    [SerializeField] private GameObject[] grids;

    public int gridSizeIndex;
    public int gameMode;

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

    public void StartGame(int mode)
    {
        gameMode = mode;
        selectGameMode.SetActive(false);
        grids[gridSizeIndex].SetActive(true);
        GetComponent<GameController>().Init(gridSizeIndex + 3);
    }

    public void Quit()
    {
        Application.Quit();
    }

}

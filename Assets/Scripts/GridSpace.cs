using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridSpace : MonoBehaviour
{
    Button button;
    TextMeshProUGUI buttonText;
    public GameController gameController;

    private void Start()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetSpace()
    {
        buttonText.text = gameController.playerSide;
        button.interactable = false;
        gameController.ChangeTurn(Convert.ToInt32(name));
    }
}

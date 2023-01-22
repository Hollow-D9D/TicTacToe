using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    int matrixSize;
    public string playerSide = "X";
    int[,] matrix;
    int turnCount;

    public void Init(int size)
    {
        turnCount = 0;
        matrixSize = size;
        matrix = new int[size,size];
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                matrix[i,j] = 0;
    }

    private void Start()
    {
        Init(5);
    }

    private void fillMatrix(int index)
    {
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                index--;
                if (index == 0)
                {
                    matrix[i, j] = playerSide == "X" ? 1 : 2;
                    Debug.Log("" + i + " " + j + " " + playerSide);
                    return ;
                }
            }
        }
    }

    public bool checkWin()
    {
        return (checkHorizontal() || checkVertical() || checkDiagonal());
    }

    private bool checkDiagonal()
    {
        int end = matrixSize - 1;
        int i = 0;
        for (; i < end; i++)
        {
            if (matrix[i,i] == 0 || matrix[i, i] != matrix[i + 1, i + 1])
                break;
        }
        if (i == end)
            return true;
        for (i = 0; i < matrixSize - 1; i++)
        {
            if (matrix[i, end - i] == 0 || matrix[i, end - i] != matrix[i + 1, end - (i + 1)])
                return false;
        }
        return true;
    }

    private bool checkVertical()
    {
        int end = matrixSize - 1;
        for (int i = 0; i < matrixSize; i++)
        {
            int j;
            for (j = 0; j < end; j++)
            {
                if (matrix[j, i] == 0 || matrix[j, i] != matrix[j + 1, i])
                    break;
            }
            if (j == end)
                return true;
        }
        return false;
    }

    private bool checkHorizontal()
    {
        int end = matrixSize - 1;
        for (int i = 0; i < matrixSize; i++)
        {
            int j;
            for (j = 0; j < end; j++)
            {
                if (matrix[i, j] == 0 || matrix[i, j] != matrix[i, j + 1])
                    break;
            }
            if (j == end)
                return true;
        }
        return false;
    }

    public void ChangeTurn(int index)
    {
        fillMatrix(index);
        Debug.Log(checkWin());
        playerSide = playerSide == "X" ? "O" : "X";
        turnCount++;
        if (turnCount == matrixSize * matrixSize)
            Debug.Log("Draw");

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public string playerSide = "X";

    Play playRef;

    int matrixSize;
    int turnCount;
    int[,] matrix;

    bool isAI_X = true;
    bool isAI_O;

    private void Start()
    {
        playRef = GetComponent<Play>();
    }

    public void Init(int size)
    {
        turnCount = 0;
        matrixSize = size;
        matrix = new int[size,size];
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                matrix[i,j] = 0;

        if (isAI_X)
            StartCoroutine(makeMove(true, turnCount));
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

    public bool checkWin(int[,] matrix)
    {
        return (checkHorizontal(matrix) || checkVertical(matrix) || checkDiagonal(matrix));
    }

    private bool checkDiagonal(int[,] matrix)
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

    private bool checkVertical(int[,] matrix)
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

    private bool checkHorizontal(int[,] matrix)
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
        if(checkWin(matrix))
        {
            playRef.GameOver(playerSide);   
        }
        playerSide = playerSide == "X" ? "O" : "X";
        turnCount++;
        if (turnCount == matrixSize * matrixSize)
            playRef.GameOver("Draw");
        if (isAI_X && playerSide == "X")
            StartCoroutine(makeMove(true, turnCount));
        else if (isAI_O && playerSide == "O")
            StartCoroutine(makeMove(false, turnCount));
    }
    int Minimax(int[,] matrixGrid, bool maximizing, int turn)
    {
        //int result;
        //if (checkWin(matrixGrid))
        //{
        //    result = maximizing ? 1 : -1;
        //    return result;
        //}
        
           

        return 1;
    }

    private IEnumerator makeMove(bool maximizing, int turn)
    {
        
        int bestMove = 1;
        int bestScore = maximizing ? int.MinValue : int.MaxValue;
        int counter = 1;
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                if (matrix[i,j] == 0)
                {
                    matrix[i, j] = maximizing ? 1 : 2;
                    int score = Minimax(matrix, maximizing, turn);
                    matrix[i, j] = 0;
                    if ((maximizing && score > bestScore) || (!maximizing && score < bestScore))
                    {
                        bestMove = counter;
                        bestScore = score;
                    }
                }
                counter++;
            }

        }
        yield return (new WaitForSeconds(0.5f));
        GameObject.Find("" + bestMove).GetComponent<GridSpace>().SetSpace();
    }    
}

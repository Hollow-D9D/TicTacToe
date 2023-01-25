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

    bool isAI_X = false;
    bool isAI_O = false;

    private void Start()
    {
        playRef = GetComponent<Play>();
    }

    public void Init(int size, int playerTurn, int gameMode)
    {

        turnCount = 0;
        matrixSize = size;
        matrix = new int[size, size];
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                matrix[i, j] = 0;
        if (gameMode == 1)
        {
            if (playerTurn == 0)
                isAI_O = true;
            else
                isAI_X = true;
        }
        else if (gameMode == 2)
        {
            isAI_X = true;
            isAI_O = true;
        }
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
                    return;
                }
            }
        }
    }

    public bool checkWin(int[,] matrixGrid)
    {
        return (checkHorizontal(matrixGrid) || checkVertical(matrixGrid) || checkDiagonal(matrixGrid));
    }

    private bool checkDiagonal(int[,] matrixGrid)
    {
        int end = matrixSize - 1;
        int i = 0;
        for (; i < end; i++)
        {
            if (matrixGrid[i, i] == 0 || matrixGrid[i, i] != matrixGrid[i + 1, i + 1])
                break;
        }
        if (i == end)
            return true;
        for (i = 0; i < matrixSize - 1; i++)
        {
            if (matrixGrid[i, end - i] == 0 || matrixGrid[i, end - i] != matrixGrid[i + 1, end - (i + 1)])
                return false;
        }
        return true;
    }

    private bool checkVertical(int[,] matrixGrid)
    {
        int end = matrixSize - 1;
        for (int i = 0; i < matrixSize; i++)
        {
            int j;
            for (j = 0; j < end; j++)
            {
                if (matrixGrid[j, i] == 0 || matrixGrid[j, i] != matrixGrid[j + 1, i])
                    break;
            }
            if (j == end)
                return true;
        }
        return false;
    }

    private bool checkHorizontal(int[,] matrixGrid)
    {
        int end = matrixSize - 1;
        for (int i = 0; i < matrixSize; i++)
        {
            int j;
            for (j = 0; j < end; j++)
            {
                if (matrixGrid[i, j] == 0 || matrixGrid[i, j] != matrixGrid[i, j + 1])
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
        if (checkWin(matrix))
        {
            playRef.GameOver(playerSide);
            return;
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

    int Minimax(int[,] matrixGrid, bool maximizing, int turn, int alpha, int beta, int depth)
    {
        if (depth == 0)
            return 0;
        if (checkWin(matrixGrid))
            return maximizing ? -1 : 1;
        if (turn == matrixSize * matrixSize)
            return 0;

        int bestScore = maximizing ? -2 : 2;
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                if (matrixGrid[i, j] == 0)
                {
                    matrixGrid[i, j] = maximizing ? 1 : 2;
                    int score = Minimax(matrixGrid, !maximizing, turn + 1, alpha, beta, depth - 1);
                    matrixGrid[i, j] = 0;
                    if (maximizing)
                    {
                        bestScore = Math.Max(score, bestScore);
                        alpha = Math.Max(alpha, score);
                        if (beta <= alpha || bestScore == 1)
                            return bestScore;
                    }
                    else
                    {
                        bestScore = Math.Min(score, bestScore);
                        beta = Math.Min(beta, score);
                        if (beta <= alpha || bestScore == -1)
                            return bestScore;
                    }
                }
            }
        }
        return bestScore;
    }

    private IEnumerator makeMove(bool maximizing, int turn)
    {
        if (turn < 2)
        {
            yield return (new WaitForSeconds(1f));
            GameObject.Find("" + UnityEngine.Random.Range(1, matrixSize * matrixSize)).GetComponent<GridSpace>().SetSpace();
        }
        else
        {
            int depth = matrixSize == 5 ? 8 : 12;
            int bestMove = 1;
            int counter = 1;
            int bestScore = maximizing ? int.MinValue : int.MaxValue;
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        matrix[i, j] = maximizing ? 1 : 2;
                        int score = Minimax(matrix, !maximizing, turn + 1, int.MinValue, int.MaxValue, depth);
                        matrix[i, j] = 0;
                        if (maximizing && score > bestScore)
                        {
                            bestMove = counter;
                            bestScore = score;
                            if (bestScore == 1)
                                break;
                        }
                        if (!maximizing && score < bestScore)
                        {
                            bestMove = counter;
                            bestScore = score;
                            if (bestScore == -1)
                                break;
                        }

                    }
                    if (maximizing && 1 == bestScore)
                        break;
                    if (!maximizing && bestScore == -1)
                        break;
                    counter++;
                }
        }
            yield return (new WaitForSeconds(1f));
            GameObject.Find("" + bestMove).GetComponent<GridSpace>().SetSpace();
        }
    }
}
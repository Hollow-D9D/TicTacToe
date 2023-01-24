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
        matrix = new int[size,size];
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                matrix[i,j] = 0;
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
                    return ;
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
            if (matrixGrid[i,i] == 0 || matrixGrid[i, i] != matrixGrid[i + 1, i + 1])
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

    private int getDiagonal(int[,] matrixGrid)
    {
        int end = matrixSize - 1;
        int i = 0;
        for (; i < end; i++)
        {
            if (matrixGrid[i, i] == 0 || matrixGrid[i, i] != matrixGrid[i + 1, i + 1])
                break;
        }
        if (i == end)
            return matrixGrid[i,i];
        for (i = 0; i < matrixSize - 1; i++)
        {
            if (matrixGrid[i, end - i] == 0 || matrixGrid[i, end - i] != matrixGrid[i + 1, end - (i + 1)])
                return 0;
        }
        return matrixGrid[i, end - i];
    }

    private int getVertical(int[,] matrixGrid)
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
                return matrixGrid[j,i];
        }
        return 0;
    }

    private int getHorizontal(int[,] matrixGrid)
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
                return matrixGrid[i,j];
        }
        return 0;
    }

    int getWinner(int [,] matrixGrid)
    {
        int rtn = getDiagonal(matrixGrid);
        if (rtn != 0)
            return rtn;
        rtn = getVertical(matrixGrid);
        if (rtn != 0)
            return rtn;
        rtn = getHorizontal(matrixGrid);
        return rtn;
    }

    void printMatrix()
    {
        Debug.Log("\n\n\n");
        Debug.Log(matrix[0, 0] + " " + matrix[0, 1] + " " + matrix[0, 2]);
        Debug.Log(matrix[1, 0] + " " + matrix[1, 1] + " " + matrix[1, 2]);
        Debug.Log(matrix[2, 0] + " " + matrix[2, 1] + " " + matrix[2, 2]);
    }    

    public void ChangeTurn(int index)
    {
        //printMatrix();
        fillMatrix(index);
        if(checkWin(matrix))
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

    int Minimax(int[,] matrixGrid, bool maximizing, int turn, int alpha, int beta)
    {
        if (turn > 10)
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
                    int score = Minimax(matrixGrid, !maximizing, turn + 1, alpha, beta);
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
        
        int bestMove = 1;
        int counter = 1;
        int bestScore = maximizing ? int.MinValue : int.MaxValue;
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                if (matrix[i,j] == 0)
                {
                    matrix[i, j] = maximizing ? 1 : 2;
                    int score = Minimax(matrix, !maximizing, turn + 1, int.MinValue, int.MaxValue);
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
        yield return (new WaitForSeconds(.1f));
        GameObject.Find("" + bestMove).GetComponent<GridSpace>().SetSpace();
    }

    bool isFull(int[,] matrixGrid)
    {
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                if (matrixGrid[i, j] == 0)
                    return false;
            }
        }    

        return true;
    }

    int SimulatePlayout(Node node)
    {
        int[,] matrixGrid = (int[,])node.matrixGrid.Clone();
        int currentPlayer = node.player;
        System.Random rand = new System.Random();

        while (!checkWin(matrixGrid) && !isFull(matrixGrid))
        {
            List<Tuple<int, int>> emptyCells = new List<Tuple<int, int>>();
            for (int i = 0; i < matrixGrid.GetLength(0); i++)
            {
                for (int j = 0; j < matrixGrid.GetLength(1); j++)
                {
                    if (matrixGrid[i, j] == 0)
                    {
                        emptyCells.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            int randomIndex = rand.Next(emptyCells.Count);
            Tuple<int, int> move = emptyCells[randomIndex];
            matrixGrid[move.Item1, move.Item2] = currentPlayer;
            currentPlayer = currentPlayer == 1 ? 2 : 1;
        }
        int winner = getWinner(matrixGrid);
        if (winner == 0)
        {
            return 0; // draw
        }
        else
        {
            return winner;
        }
    }

    int MCTS(int[,] matrixGrid, int turn, int simulations)
    {
        Node root = new Node(matrixGrid);

        for (int i = 0; i < simulations; i++)
        {
            // Select the most promising node from the tree
            Node current = root;
            while (current.unexplored.Count == 0 && current.children.Count != 0)
            {
                current = current.SelectChild();
            }

            // Expand the tree by adding a new child node
            if (current.unexplored.Count > 0)
            {
                Tuple<int, int> move = current.unexplored[0];
                current.unexplored.RemoveAt(0);
                current = current.AddChild(move);
            }

            // Simulate random playouts from the current node
            int winner = SimulatePlayout(current);

            // Update the win count and visit count of all nodes in the path
            while (current != null)
            {
                current.visits++;
                if (current.player == winner)
                {
                    current.wins++;
                }
                current = current.parent;
            }
        }

        // Select the child node with the highest win rate
        Node bestChild = root.SelectChild();
        return bestChild.move.Item1 * matrixSize + bestChild.move.Item2 + 1;
    }
}

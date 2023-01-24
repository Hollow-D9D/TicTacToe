using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node
{
    public int[,] matrixGrid;
    public int player;
    public List<Node> children;
    public List<Tuple<int, int>> unexplored;
    public int wins;
    public int visits;
    public Tuple<int, int> move;
    public Node parent;

    public Node(int[,] matrixGrid, Tuple<int, int> move = null, Node parent = null)
    {
        this.matrixGrid = matrixGrid;
        this.player = DeterminePlayer();
        this.children = new List<Node>();
        this.unexplored = GetUnexplored();
        this.wins = 0;
        this.visits = 0;
        this.move = move;
        this.parent = parent;
    }

    public int DeterminePlayer()
    {
        int emptyCells = 0;
        for (int i = 0; i < matrixGrid.GetLength(0); i++)
        {
            for (int j = 0; j < matrixGrid.GetLength(1); j++)
            {
                if (matrixGrid[i, j] == 0)
                {
                    emptyCells++;
                }
            }
        }
        return emptyCells % 2 == 0 ? 1 : 2; // assuming player 1 is 'X' and player 2 is 'O'
    }

    public List<Tuple<int, int>> GetUnexplored()
    {
        var unexplored = new List<Tuple<int, int>>();
        for (int i = 0; i < matrixGrid.GetLength(0); i++)
        {
            for (int j = 0; j < matrixGrid.GetLength(1); j++)
            {
                if (matrixGrid[i, j] == 0)
                {
                    unexplored.Add(new Tuple<int, int>(i, j));
                }
            }
        }
        return unexplored;
    }

    public Node SelectChild()
    {
        Node bestChild = null;
        double bestScore = double.NegativeInfinity;
        double c = 1.4; // exploration parameter, you can adjust it to your liking

        foreach (var child in children)
        {
            double score = (double)child.wins / child.visits + c * Math.Sqrt(Math.Log(visits) / child.visits);
            if (score > bestScore)
            {
                bestChild = child;
                bestScore = score;
            }
        }

        return bestChild;
    }

    public Node AddChild(Tuple<int, int> move)
    {
        int[,] newMatrixGrid = (int[,])matrixGrid.Clone();
        newMatrixGrid[move.Item1, move.Item2] = player;
        var newNode = new Node(newMatrixGrid, move, this);
        children.Add(newNode);
        return newNode;
    }
}

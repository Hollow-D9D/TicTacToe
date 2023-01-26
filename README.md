<h1 align="center"> Tic Tac Toe</h1>
This README file is written as supplementary for the Tic Tac Toe game.

The game has 3 modes
1. Player vs Player
2. Player vs AI
3. AI vs AI (demo)

The game provides the player a choice between
1. 3x3 grid
2. 4x4 grid
3. 5x5 grid

Moreover the player has a choice	to pick a turn

The game is developed by the help of Unity game engine, applying Minimax algorithm.
As Minimax is considered to be a non-efficient algorithm, it was optimized using alpha-beta pruning and custom depth implementation.

Even the optimized version of Minimax exhibits high time complexity, thus a different algorithm, the Monte Carlo Tree Search (MCTS), was attempted to be applied.
Despite the attempts, it was showing lower results, so the decision was made to continue with Minimax algorithm.

There are several ways to improve current algorithm.
1. Application of dynamic programming for caching the possible outcomes of the game, depending on the current game state. In this case more appropriate hash-table data structure would be applied. The key would be the game state and the value would be the most optimal solution. This will be a tradeoff increasing  space complexity for decreasing time complexity.
2. Creation of a scriptable object in stage of development, which will store all the possible game states in JSON format. This object would be parsed during the game and assist in finding the best possible move.  


All the comments and suggestions are highly appreciated.

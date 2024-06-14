using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourEngine
{
    
    public struct MyBot
    {
        public int maxDepth = 0;

        public MyBot()
        {
        }

        public Move think(Board board)
        {
            List<Move> movesList = board.generateLegalMoves();
            this.maxDepth = 9 + board.moveNumber / 6 + (7 - movesList.Count) * (7 - movesList.Count);
            Move bestMove = movesList[0];
            Move firstMove = movesList[movesList.Count / 2];
            search(board, ref bestMove, 2, true, -int.MaxValue, int.MaxValue, new Move(-1), firstMove, 2);
            firstMove = bestMove;
            //best way of guessing the best first move.
            //Console.WriteLine("Num Legal Moves: " + movesList.Count);
            int eval = search(board, ref bestMove, this.maxDepth, true, -int.MaxValue, int.MaxValue, new Move(-1), firstMove, this.maxDepth);

            Console.WriteLine("Evaluation: " + eval);
            
            return bestMove;
        }
        public int evaluateBoard(Board board, bool maximizing)
        {
            int score = 0;
            for(int i = 0; i < 42; i++)
            {
                if (board.squares[i] == 0)
                {
                    continue;
                }
                if (maximizing && board.turn == board.squares[i] || !maximizing && board.squares[i] != board.turn)
                {
                    score += 6 - Math.Abs((i % 7) - 3);
                } else
                {
                    score -= 6 - Math.Abs((i % 7) - 3);
                }
            }
            return score;
        }
        public int evaluateLastMove(Move move)
        {
            return 6 - Math.Abs(move.moveValue % 7 - 3) + 6 - Math.Abs(move.moveValue / 7 - 3);
        }
        public int search(Board board, ref Move bestMove, int depth, bool maximizingPlayer, int alpha, int beta, Move lastMove, Move firstMove, int initialDepth)
        {
            int result = -1;
            if(lastMove.moveValue > -1)
            {
                result = board.checkGameStateFromMoveValue(lastMove.moveValue);
            }

            if (result > 0)
            {
                if(maximizingPlayer)
                {
                    return -10000 - depth;
                } else
                {
                    return 10000 - depth;
                }
                //bot prefers to drag out its opponents suffering
            }
            if(result == 0)
            {
                return 0;
            }
            if(depth == 0)
            {

                return evaluateBoard(board, maximizingPlayer);
                
            }
            if(maximizingPlayer)
            {
                List<Move> movesList;
                if(firstMove.moveValue != -1)
                {
                    movesList = board.generateLegalMovesFromMove(firstMove);
                } else
                {
                    movesList = board.generateLegalMoves();
                }

                int bestEval = -int.MaxValue;
                foreach (Move move in movesList)
                {
                    board.makeMove(move);
                    int currentMoveEval = search(board, ref bestMove, depth - 1, false, alpha, beta, move, new Move(-1), initialDepth); ;
                    board.unMakeMove(move);
                    if (currentMoveEval > bestEval)
                    {
                        if (depth == initialDepth)
                        {
                            //Console.WriteLine("BestMove: " + move.moveValue + " found at depth " + depth);
                            bestMove = move;
                        }
                        bestEval = currentMoveEval;
                        
                    }
                    alpha = Math.Max(alpha, currentMoveEval);
                    if(alpha >= beta)
                    {
                        break;
                    }
                }
                return bestEval;
            } else
            {
                List<Move> movesList = board.generateLegalMoves();
                int bestEval = int.MaxValue;
                foreach (Move move in movesList)
                {
                    board.makeMove(move);
                    int currentMoveEval = search(board, ref bestMove, depth - 1, true, alpha, beta, move, new Move(-1), initialDepth);
                    board.unMakeMove(move);
                    if (currentMoveEval < bestEval)
                    {
                        if (depth == initialDepth)
                        {
                            Console.WriteLine("BestMove: " + move.moveValue + " found at depth " + depth);
                            bestMove = move;
                        }
                        bestEval = currentMoveEval;
                    }
                    beta = Math.Min(beta, currentMoveEval);
                    if(alpha >= beta)
                    {
                        break;
                    }
                }
                return bestEval;
            }
            
        }
    }
}

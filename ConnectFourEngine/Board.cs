using Raylib_cs;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourEngine
{
    public struct Board
    {
        public List<int> squares;
        public int turn = 1;
        public int moveNumber = 1;
        public Board(List<int> squareValues)
        {
            this.squares = squareValues;
            //list of 42 different squares on the board. squares 0, 7, 14 etc. are on the first column.
        }

        public List<Move> generateLegalMoves()
        {
            List<Move> legalMovesList = new List<Move>();
            for (int i = 0; i < squares.Count; i++)
            {
                if (this.squares[i] == 0)
                {
                    if (i < 7)
                    {
                        legalMovesList.Add(new Move(i));
                    } else if (squares[i - 7] != 0)
                    {
                        legalMovesList.Add(new Move(i));
                    }
                }
            }
            return legalMovesList;
        }
        public List<Move> generateLegalMovesFromMove(Move move)
        {
            List<Move> legalMovesList = new List<Move>();
            legalMovesList.Add(move);
            for(int i = 0; i < squares.Count; i++)
            {
                if(i == move.moveValue)
                {
                    continue;
                }
                if(this.squares[i] == 0)
                {
                    if (i < 7)
                    {
                        legalMovesList.Add(new Move(i));
                    }
                    else if (squares[i - 7] != 0)
                    {
                        legalMovesList.Add(new Move(i));
                    }
                }
            }
            return legalMovesList;
        }
        public void makeMove(Move move)
        {
            squares[move.moveValue] = turn;
            turn = turn == 1 ? 2 : 1;
            moveNumber++;

        }
        public void unMakeMove(Move move)
        {
            squares[move.moveValue] = 0;
            turn = turn == 1 ? 2 : 1;
        }
        public bool isLegalMove(Move move)
        {
            if(move.moveValue < 0 || move.moveValue > 6)
            {
                return false;
            }
            if (this.squares[(move.moveValue % 7) + 35] == 0) {
                return true;
            }
            return false;
        }
        public bool IsMatching(int a, int b, int c, int d) 
        {
            return a == b && b == c && c == d;
        }
        public int checkGameState()
        {
            // -1: game is still going
            // 0: draw
            // 1: player 1 wins
            // 2: player 2 wins
            for(int i = 0; i < 39; i++)
            {
                int thisType = this.squares[i];
                if (thisType == 0)
                {
                    continue;
                }
                if(i % 7 <= 3)
                {
                    if(IsMatching(thisType, this.squares[i + 1], this.squares[i + 2], this.squares[i + 3]))
                    {
                        return thisType;
                    }
                    if (i + 21 < 42)
                    {
                        if (IsMatching(thisType, this.squares[i + 8], this.squares[i + 16], this.squares[i + 24]))
                        {
                            return thisType;
                        }
                    } else
                    {
                        if (IsMatching(thisType, this.squares[i - 6], this.squares[i - 12], this.squares[i - 18]))
                        {
                            return thisType;
                        }
                    }
                }
                if(i + 21 < 42)
                {
                    if(IsMatching(thisType, this.squares[i + 7], this.squares[i + 14], this.squares[i + 21]))
                    {
                        return thisType;
                    }
                }
            }
            if(this.squares[41] != 0 && this.squares[40] != 0 && this.squares[39] != 0 && this.squares[38] != 0 && this.squares[37] != 0 && this.squares[36] != 0 && this.squares[35] != 0)
            {
                return 0;
            }

            return -1;

            
        }
        public int checkLeftSquares(int square, int squareValue)
        {
            int numConnected = 0;
            int currentSquare = square;
            while(currentSquare % 7 > 0)
            {
                currentSquare--;
                if (this.squares[currentSquare] == squareValue)
                {
                    numConnected++;
                } else
                {
                    break;
                }
            }
            return numConnected;
        }
        public int checkRightSquares(int square, int squareValue)
        {
            int numConnected = 0;
            int currentSquare = square;
            while (currentSquare % 7 < 6)
            {
                currentSquare++;
                if (this.squares[currentSquare] == squareValue)
                {
                    numConnected++;
                }
                else
                {
                    break;
                }
            }
            return numConnected;
        }
        public int checkUpLeftSquares(int square, int squareValue)
        {
            int numConnected = 0;
            int currentSquare = square;
            while(currentSquare % 7 > 0 && currentSquare < 35)
            {
                currentSquare += 6;
                if (this.squares[currentSquare] == squareValue)
                {
                    numConnected++;
                } else
                {
                    break;
                }
            }
            return numConnected;
        }
        public int checkUpRightSquares(int square, int squareValue)
        {
            int numConnected = 0;
            int currentSquare = square;
            while (currentSquare % 7 < 6 && currentSquare < 35)
            {
                currentSquare += 8;
                if (this.squares[currentSquare] == squareValue)
                {
                    numConnected++;
                }
                else
                {
                    break;
                }
            }
            return numConnected;
        }
        public int checkDownLeftSquares(int square, int squareValue)
        {
            int numConnected = 0;
            int currentSquare = square;
            while (currentSquare % 7 > 0 && currentSquare > 6)
            {
                currentSquare -= 8;
                if (this.squares[currentSquare] == squareValue)
                {
                    numConnected++;
                }
                else
                {
                    break;
                }
            }
            return numConnected;
        }
        public int checkDownRightSquares(int square, int squareValue)
        {
            int numConnected = 0;
            int currentSquare = square;
            while (currentSquare % 7 < 6 && currentSquare > 6)
            {
                currentSquare -= 6;
                if (this.squares[currentSquare] == squareValue)
                {
                    numConnected++;
                }
                else
                {
                    break;
                }
            }
            return numConnected;
        }

        public int checkGameStateFromMoveValue(int moveValue)
        {
            int thisType = squares[moveValue];
            //check for vertical win
            if(moveValue >= 21)
            {
                if(IsMatching(thisType, squares[moveValue - 7], squares[moveValue - 14], squares[moveValue - 21]))
                {
                    return thisType;
                }
            }
            if((checkRightSquares(moveValue, thisType) + checkLeftSquares(moveValue, thisType)) > 2 || (checkUpLeftSquares(moveValue, thisType) + checkDownRightSquares(moveValue, thisType)) > 2 || (checkUpRightSquares(moveValue, thisType) + checkDownLeftSquares(moveValue, thisType)) > 2) {
                return thisType;
            }


            
            if(moveValue > 34 && this.squares[41] != 0 && this.squares[40] != 0 && this.squares[39] != 0 && this.squares[38] != 0 && this.squares[37] != 0 && this.squares[36] != 0 && this.squares[35] != 0)
            {
                return 0;
            }
            return -1;
        }
        public void draw()
        {
            for (int rank = 0; rank < 7; rank++)
            {
                for (int file = 0; file < 6; file++)
                {
                    if (this.squares[rank + (file * 7)] != 0)
                    {
                        Raylib_cs.Color fillColor = this.squares[rank + (file * 7)] == 1 ? Raylib_cs.Color.Red : Raylib_cs.Color.Yellow;
                        Raylib.DrawCircle(rank * 100 + 200, 800 - (file * 100 + 150), 45, fillColor);
                    }
                    Raylib.DrawRectangleLines(rank * 100 + 150, 800 - (file * 100 + 200), 100, 100, Raylib_cs.Color.Black);

                }
            }
        }
        public void reset()
        {
            this.squares.Clear();
            for(var i = 0; i < 42; i++)
            {
                this.squares.Add(0);
            }
            this.turn = 1;
            this.moveNumber = 1;

        }
    }
}

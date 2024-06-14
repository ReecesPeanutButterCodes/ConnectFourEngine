// See https://aka.ms/new-console-template for more information
using ConnectFourEngine;
using Raylib_cs;
using System.Diagnostics.Eventing.Reader;
using System.Numerics;
using System.Runtime.CompilerServices;


int currentScene = 1;
int gameResult = -1;
Raylib.InitWindow(1000, 800, "Connect Four Engine");
List<string> players = new List<string>();
MyBot bot = new MyBot();
List<int> emptyBoard = new List<int>();
for (int i = 0; i < 42; i++)
{
    emptyBoard.Add(0);
}
Board board = new Board(emptyBoard);
void newGame(string playerOne, string playerTwo)
{
    currentScene = 2;
    board.reset();
    players.Clear();
    players.Add(playerOne);
    players.Add(playerTwo);
    gameResult = -1;

}
int getFileFromMousePosition()
{
    decimal mouseX = (Raylib.GetMouseX() - 150) / 100;
    decimal mouseFile = Math.Floor(mouseX);
    if (mouseFile >= 0 && mouseFile < 7)
    {
        return (int)mouseFile;
    }
    return -1;
    //returns -1 if the mouse is not within one of the files.
}

bool mouseButtonReleased = true;

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();

    Raylib.ClearBackground(Raylib_cs.Color.White);
    switch (currentScene)
    {
        case 1:

            drawSceneMain();
            break;

        case 2:

            drawScenePlay();
            break;
        case 3:

            drawSceneGameOver(gameResult);
            break;

    }
    
    Raylib.EndDrawing();
}
void drawSceneMain()
{
    Raylib.DrawText("Connect Four!", 150, 200, 100, Raylib_cs.Color.Black);
    Raylib.DrawRectangleLines(300, 500, 400, 50, Raylib_cs.Color.Black);
    Raylib.DrawText("Play Against Bot", 325, 506, 40, Raylib_cs.Color.Black);
    Raylib.DrawRectangleLines(300, 600, 400, 50, Raylib_cs.Color.Black);
    Raylib.DrawText("Two Player Mode", 325, 606, 40, Raylib_cs.Color.Black);
    if (mouseButtonReleased && Raylib.IsMouseButtonDown(MouseButton.Left))
    {
        double mouseX = Raylib.GetMouseX();
        double mouseY = Raylib.GetMouseY();

        if (mouseX >= 300 && mouseX <= 700 && mouseY >= 500 && mouseY <= 550)
        {
            if(Raylib.GetRandomValue(0, 1) == 0)
            {
                newGame("human", "bot");
            } else
            {
                newGame("bot", "human");
            }
            

        }
        if (mouseX >= 300 && mouseX <= 700 && mouseY >= 600 && mouseY <= 650)
        {
            newGame("human", "human");
        }
        mouseButtonReleased = false;

    }

    if (!Raylib.IsMouseButtonDown(MouseButton.Left))
    {
        mouseButtonReleased = true;
    }
}
void drawScenePlay()
{
    string currentTurn = players[board.turn - 1];
    Move moveToPlay = new Move(0);
    //placeholder move to avoid an error
    bool moveMade = false;
    
    if (currentTurn == "bot")
    {

        moveToPlay = bot.think(board);
        moveMade = true;
    } else {
        Move mouseMoveFile = new Move(getFileFromMousePosition());
        //the file which the move will be placed. Board.isLegalMove only checks which file the move is on, so mouseMoveFile doesn't need to be converted into an actual move yet.
        if (Raylib.IsMouseButtonDown(MouseButton.Left) && board.isLegalMove(mouseMoveFile) && mouseButtonReleased)
        {
            mouseButtonReleased = false;
            int moveLocation = 0;
            for (int i = 0; i < 6; i++)
            {
                if (board.squares[i * 7 + mouseMoveFile.moveValue] == 0)
                {
                    moveLocation = i * 7 + mouseMoveFile.moveValue;
                    break;
                }
            }
            moveToPlay = new Move(moveLocation);
            moveMade = true;
        }
        if (!Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            mouseButtonReleased = true;
        }
    }
    if(moveMade)
    {
        board.makeMove(moveToPlay);
        gameResult = board.checkGameStateFromMoveValue(moveToPlay.moveValue);
       
    }
    if (players[board.turn - 1] == "bot")
    {
        Raylib.DrawText("Bot is thinking...", 385, 30, 40, Raylib_cs.Color.Brown);
    }
    else
    {
        Raylib.DrawText("Player Turn", 385, 30, 40, Raylib_cs.Color.Brown);
    }
    board.draw();


    if (gameResult != -1)
    {
        currentScene = 3;
    }
}


void drawSceneGameOver(int result)
{
    board.draw();
    switch (result)
    {
        case 0:
            Raylib.DrawText("Draw", 452, 30, 40, Raylib_cs.Color.Black);
            break;
        case 1:
            Raylib.DrawText("Red Wins", 415, 30, 40, Raylib_cs.Color.Black);
            break;
        case 2:
            Raylib.DrawText("Yellow Wins", 385, 30, 40, Raylib_cs.Color.Black);
            break;
    }
    Raylib.DrawText("Click anywhere to return to main menu", 110, 730, 40, Raylib_cs.Color.Black);
    if(mouseButtonReleased && Raylib.IsMouseButtonDown(MouseButton.Left))
    {
        currentScene = 1;
        mouseButtonReleased = false;
    }
    if(!Raylib.IsMouseButtonDown(MouseButton.Left))
    {
        mouseButtonReleased = true;
    }

}
Raylib.CloseWindow();
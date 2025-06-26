using System;
using System.Linq;

namespace ConnectFour
{
    // Enumeration for a possible game mode

    public enum GameMode
    {
        TwoPlayer,  // Played between Humans ie Human vs Human
        OnePlayer   // Human vs Computer
    }

    // ConnectFour Gameboard 

    public class Board
    {
        private char[,] _grid; // 2D array representing game board
        private const int _rows = 6; // Rows in the Gameboard
        private const int _cols = 7; // Columns in the Gameboard
        public const char EmptySlot = '.'; // Empty slots on the GameBoard

        // Gets the number of columns on the board.
        public int Cols => _cols;

        // Gets the number of rows on the board.

        public int Rows => _rows;

        public char GetSymbolAt(int row, int col)
        {
            if (row >= 0 && row < _rows && col >= 0 && col < _cols)
            {
                return _grid[row, col];
            }
            return EmptySlot; // Return empty 
        }

        // Empty Game grid

        public Board()
        {
            _grid = new char[_rows, _cols];
            ResetBoard(); // Initialize all slots as empty
        }

        public Board(Board other)
        {
            _grid = new char[_rows, _cols];
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    _grid[r, c] = other._grid[r, c]; // Copy each cell's content
                }
            }
        }

        // Resets the entire board

        public void ResetBoard()
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    _grid[r, c] = EmptySlot;
                }
            }
        }

        // Attempts to drop a piece into the specified column

        public bool DropPiece(int column, char symbol)
        {
            // Validate column input
            if (column < 0 || column >= _cols)
            {
                return false; // Invalid column
            }

            // Find the lowest empty row in the chosen column
            for (int r = _rows - 1; r >= 0; r--)
            {
                if (_grid[r, column] == EmptySlot)
                {
                    _grid[r, column] = symbol; // Place the piece
                    return true; // Piece successfully placed
                }
            }

            // If we reach here, the column is full
            return false;
        }

        // Checks if the last move made by a player resulted into a win whether is horizontally, vertically or diagonally 

        public bool CheckWin(char symbol)
        {
            // Check horizontal wins
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c <= _cols - 4; c++)
                {
                    if (_grid[r, c] == symbol &&
                        _grid[r, c + 1] == symbol &&
                        _grid[r, c + 2] == symbol &&
                        _grid[r, c + 3] == symbol)
                    {
                        return true;
                    }
                }
            }
            // Check vertical wins
            for (int c = 0; c < _cols; c++)
                    {
                        for (int r = 0; r <= _rows - 4; r++)
                        {
                            if (_grid[r, c] == symbol &&
                                    _grid[r + 1, c] == symbol &&
                                    _grid[r + 2, c] == symbol &&
                                    _grid[r + 3, c] == symbol)
                            {
                                return true;
                            }
                        }
                    }

                    // Check diagonal (top-left to bottom-right) wins
                    for (int r = 0; r <= _rows - 4; r++)
                    {
                        for (int c = 0; c <= _cols - 4; c++)
                        {
                            if (_grid[r, c] == symbol &&
                                _grid[r + 1, c + 1] == symbol &&
                                _grid[r + 2, c + 2] == symbol &&
                                _grid[r + 3, c + 3] == symbol)
                            {
                                return true;
                            }
                        }
                    }
                    // Check diagonal (bottom-left to top-right) wins
                    for (int r = 3; r < _rows; r++) // Start from row 3 (0-indexed) to have enough space upwards
                    {
                        for (int c = 0; c <= _cols - 4; c++)
                        {
                            if (_grid[r, c] == symbol &&
                                _grid[r - 1, c + 1] == symbol &&
                                _grid[r - 2, c + 2] == symbol &&
                                 _grid[r - 3, c + 3] == symbol)
                            {
                                return true;
                            }
                        }
                    }

                    return false; // No win found
                }

        // check if a specific column is completely full
        public bool IsColumnFull(int column)
        {
            // A column is full if the top-most slot (row 0) is not empty
            if (column < 0 || column >= _cols) return true; // Treat invalid columns as full
            return _grid[0, column] != EmptySlot;
        }


        // Checks if the entire board is full maybe a tie
        public bool IsBoardFull()
        {
            for (int c = 0; c < _cols; c++)
            {
                if (!IsColumnFull(c))
                {
                    return false; // Found at least one non-full column
                }
            }
            return true;
        }// All columns are full

        // Current state of the Game 

        public void DisplayBoard()
        {
            Console.WriteLine("\nConnect Four Board:");
            Console.WriteLine("--------------------");

            // Print column numbers for user reference
            Console.Write("|");
            for (int c = 0; c < _cols; c++)
            {
                Console.Write($" {c + 1} "); // Column numbers 1-7
            }
            Console.WriteLine("|");

            // Print the board grid with colors and background colors
            for (int r = 0; r < _rows; r++)
            {
                Console.Write("|");
                for (int c = 0; c < _cols; c++)
                {
                    char symbol = _grid[r, c];

                    // Set background color based on the symbol // Searched Online on how to do it
                    if (symbol == 'X')
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed; // Player X's piece background
                        Console.ForegroundColor = ConsoleColor.White; // Make symbol visible on red
                    }
                    else if (symbol == 'O')
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow; // Player O's piece background
                        Console.ForegroundColor = ConsoleColor.Black; // Make symbol visible on yellow
                    }
                    else
                    {
                        // Use a "board" background color for empty slots
                        Console.BackgroundColor = ConsoleColor.Green; // Simulate Green Connect Four board
                        Console.ForegroundColor = ConsoleColor.White; // Make empty slot symbol visible on blue
                    }

                    Console.Write($" {symbol} "); // Print the symbol with its background and foreground color
                    Console.ResetColor(); // Reset colors to default after printing each symbol
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("--------------------");
        }
    }

    // abstract class for all players in the game 
    // Common properties and methods for a player

    public abstract class Player
    {
        // Protected field to store the player's symbol (e.g. 'X' or 'O')
        protected char Symbol { get; private set; }

        public Player(char symbol)
        {
            Symbol = symbol;
        }

        // player symbol
        public char GetSymbol()
        {
            return Symbol;
        }

        // For Moves
        public abstract int MakeMove(Board board);
    }

    // Human Player

    public class HumanPlayer : Player
    {
        public HumanPlayer(char symbol) : base(symbol) { }

        public override int MakeMove(Board board)
        {
            int column = -1;
            bool isValidInput = false;

            // Loop until a valid, non-full column is chosen
            while (!isValidInput)
            {
                Console.Write($"Player {Symbol}, enter column (1-{board.Cols}): ");
                string input = Console.ReadLine();

                // Try to parse the input as an integer
                if (int.TryParse(input, out column))
                {
                    // Adjust to 0-indexed column
                    column--;

                    // Check if the column is within valid range and not full
                    if (column >= 0 && column < board.Cols)
                    {
                        if (!board.IsColumnFull(column))
                        {
                            isValidInput = true; // Valid input and column available
                        }
                        else
                        {
                            Console.WriteLine($"Column {column + 1} is full. Please choose another column.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid column number. Please enter a number between 1 and 7.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
            return column;
        }
    }

    // Computer Player : AI 

    public class ComputerPlayer : Player
    {
        private char _opponentSymbol;
        private Random _random;


        public ComputerPlayer(char symbol) : base(symbol)
        {
            // Determine the opponent's symbol based on the computer's symbol
            _opponentSymbol = (symbol == 'X') ? 'O' : 'X';
            _random = new Random();
        }

        // Determines the computer's next move using a basic AI strategy. 
        // It prioritizes winning and blocking the Human Player. 

        public override int MakeMove(Board board)
        {
            Console.WriteLine($"Computer ({Symbol}) is making a move...");

            // 1. Check for a winning move for self
            for (int col = 0; col < board.Cols; col++)
            {
                if (!board.IsColumnFull(col))
                {
                    Board tempBoard = new Board(board); // Create a deep copy to simulate
                    if (tempBoard.DropPiece(col, Symbol)) // Simulate dropping the piece
                    {
                        if (tempBoard.CheckWin(Symbol))
                        {
                            return col; // Found a winning move
                        }
                    }
                }
            }

            // Block opponent's winning move
            for (int col = 0; col < board.Cols; col++)
            {
                if (!board.IsColumnFull(col))
                {
                    Board tempBoard = new Board(board); // Create a copy
                    if (tempBoard.DropPiece(col, _opponentSymbol)) // Simulate opponent's move
                    {
                        if (tempBoard.CheckWin(_opponentSymbol))
                        {
                            return col; // Found a move to block opponent's win
                        }
                    }
                }
            }

            // Prioritize center columns or columns that lead to potential 3-in-a-row

            int[] strategicColumns = { 3, 2, 4, 1, 5, 0, 6 }; // Order of preference (center out)
            foreach (int col in strategicColumns)
            {
                if (!board.IsColumnFull(col))
                {
                    Board tempBoard = new Board(board);
                    if (tempBoard.DropPiece(col, Symbol))
                    {
                        // Simple check for creating 3-in-a-row (horizontal/vertical/diagonal)
                        if (CheckForPotentialThree(tempBoard, Symbol))
                        {
                            return col;
                        }
                    }
                }
            }


            // If no immediate win, block, or obvious strategic move, choose a random valid column
            int randomColumn;
            do
            {
                randomColumn = _random.Next(0, board.Cols); // Get a random column index
            } while (board.IsColumnFull(randomColumn)); // Keep trying until an empty column is found

            return randomColumn;
        }

        // Checks if a potential 3-in-a-row can be created, which is a win

        private bool CheckForPotentialThree(Board board, char symbol)
        {
            // Horizontal
            for (int r = 0; r < board.Rows; r++)
            {
                for (int c = 0; c <= board.Cols - 3; c++) // Check for 3, leaving room for 4th
                {
                    int count = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        if (board.GetSymbolAt(r, c + i) == symbol) count++;
                    }

                    if (count == 3)
                    {
                        // Check if an empty slot exists to complete 4 in a row
                        if (c > 0 && board.GetSymbolAt(r, c - 1) == Board.EmptySlot) return true;
                        if (c + 3 < board.Cols && board.GetSymbolAt(r, c + 3) == Board.EmptySlot) return true;
                    }
                }
            }

            // Vertical (less common for 3-in-a-row from player input, but possible for AI planning)
            for (int c = 0; c < board.Cols; c++)
            {
                for (int r = 0; r <= board.Rows - 3; r++)
                {
                    if (board.GetSymbolAt(r, c) == symbol &&
                        board.GetSymbolAt(r + 1, c) == symbol &&
                        board.GetSymbolAt(r + 2, c) == symbol)
                    {
                        // If there's an empty spot directly above and it's reachable for a piece to land there
                        if (r > 0 && board.GetSymbolAt(r - 1, c) == Board.EmptySlot)
                        {
                            // This checks if the slot (r-1, c) is available and would complete 4 vertically.
                            return true;
                        }
                    }
                }
            }

            // Diagonal (top-left to bottom-right)
            for (int r = 0; r <= board.Rows - 3; r++)
            {
                for (int c = 0; c <= board.Cols - 3; c++)
                {
                    int count = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        if (board.GetSymbolAt(r + i, c + i) == symbol) count++;
                    }
                    if (count == 3)
                    {
                        // Check empty slots on either end of the potential 4-in-a-row
                        if (r > 0 && c > 0 && board.GetSymbolAt(r - 1, c - 1) == Board.EmptySlot) return true;
                        if (r + 3 < board.Rows && c + 3 < board.Cols && board.GetSymbolAt(r + 3, c + 3) == Board.EmptySlot) return true;
                    }
                }
            }

            // Diagonal (bottom-left to top-right)
            for (int r = 2; r < board.Rows; r++) // Start from r=2 to allow check for r-1, r-2
            {
                for (int c = 0; c <= board.Cols - 3; c++)
                {
                    int count = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        if (board.GetSymbolAt(r - i, c + i) == symbol) count++;
                    }
                    if (count == 3)
                    {
                        // Check empty slots on either end of the potential 4-in-a-row
                        if (r + 1 < board.Rows && c > 0 && board.GetSymbolAt(r + 1, c - 1) == Board.EmptySlot) return true;
                        if (r - 3 >= 0 && c + 3 < board.Cols && board.GetSymbolAt(r - 3, c + 3) == Board.EmptySlot) return true;
                    }
                }
            }
            return false;
        }
    }

    // Overall Game Flow

    public class Game
    {
        private Board _board; // The game board instance
        private Player[] _players; // Array of players (Human or Computer)
        private int _currentPlayerIndex; // Index of the player whose turn it is
        private GameMode _gameMode; // The selected game mode (OnePlayer or TwoPlayer)
        private Random _random; // For randomizing first player in One-Player m

        // Instance of a game class
        public Game(GameMode mode)
        {
            _board = new Board(); // Create a new board
            _gameMode = mode;
            _random = new Random();
            InitializePlayers(_gameMode); // Set up players based on game mode
        }

        // Initializes the players array based on the selected game mode.

        private void InitializePlayers(GameMode mode)
        {
            if (mode == GameMode.TwoPlayer)
            {
                _players = new Player[2];
                _players[0] = new HumanPlayer('X'); // Player 1 (Human 'X')
                _players[1] = new HumanPlayer('O'); // Player 2 (Human 'O')
                _currentPlayerIndex = 0; // Player 'X' always starts in two-player mode
            }
            else // One-Player Mode (vs. Computer)
            {
                _players = new Player[2];
                // Randomly decide who goes first
                if (_random.Next(2) == 0) // 0 for human first, 1 for computer first
                {
                    _players[0] = new HumanPlayer('X');      // Human 'X'
                    _players[1] = new ComputerPlayer('O');   // Computer 'O'
                    _currentPlayerIndex = 0; // Human starts
                    Console.WriteLine("\n--- You (X) go first! ---");
                }
                else
                {
                    _players[0] = new ComputerPlayer('X');   // Computer 'X'
                    _players[1] = new HumanPlayer('O');      // Human 'O'
                    _currentPlayerIndex = 0; // Computer starts
                    Console.WriteLine("\n--- Computer (X) goes first! ---");
                }
                System.Threading.Thread.Sleep(1000); // Pause briefly // Searched on google.com
            }
        }

        // Start the game

        public void StartGame()
        {
            bool gameOver = false;
            _board.ResetBoard(); // Ensure the board is clean at the start of a new game

            Console.Clear();
            Console.WriteLine($"Starting new Connect Four game ({_gameMode} mode)...");
            System.Threading.Thread.Sleep(1000); // Short pause

            while (!gameOver)
            {
                Console.Clear(); // Clear console for current turn
                _board.DisplayBoard(); // Show the current board state

                Player currentPlayer = _players[_currentPlayerIndex];
                Console.WriteLine($"\nIt's {currentPlayer.GetSymbol()}'s turn.");

                int column = currentPlayer.MakeMove(_board); // Get the move from the current player

                // Process the chosen move
                bool moveSuccessful = _board.DropPiece(column, currentPlayer.GetSymbol());

                if (moveSuccessful)
                {
                    // Check for game end conditions after a successful move
                    gameOver = CheckGameEnd(currentPlayer.GetSymbol());

                    if (!gameOver)
                    {
                        // If game is not over, switch to the next player's turn
                        _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Length;
                    }
                }
                else
                {
                    // If move was not successful (e.g., column full, invalid input),
                    // the current player retains their turn.
                    Console.WriteLine("Move failed. Please try again.");
                    System.Threading.Thread.Sleep(1500); // Pause to show message
                }
            }

            _board.DisplayBoard(); // Display final board state
            Console.WriteLine("\nGame Over!");

            // Ask if players want to play again
            if (AskToPlayAgain())
            {
                // This 'return' effectively exits the current game.        
                return;
            }
            else
            {
                // If they don't want to play again, Program will exit its loop.
                Environment.Exit(0); // Exit the application. Searched on Google.com
            }
        }
        // Checks for win or tie conditions after a piece has been played.
        private bool CheckGameEnd(char lastPlayedSymbol)
        {
            if (_board.CheckWin(lastPlayedSymbol))
            {
                Console.WriteLine($"Player {lastPlayedSymbol} wins!");
                return true; // Game is over due to a win
            }
            else if (_board.IsBoardFull())
            {
                Console.WriteLine("The board is full! It's a tie!");
                return true; // Game is over due to a tie
            }
            return false; // Game is not yet over
        }

        // Asks the user if they want to play another round.

        private bool AskToPlayAgain()
        {
            Console.WriteLine("Do you want to play again? (yes/no)");
            string input = Console.ReadLine().ToLower();
            return input == "yes" || input == "y";
        }

        // Main Entry

        public class Program
        {
            /// <summary>
            /// The main method where the program execution begins.
            /// </summary>
            /// <param name="args">Command line arguments (not used).</param>
            static void Main(string[] args)
            {
                Console.Title = "Connect Four"; // Set the console window title

                // Keep the game running until the user decides to quit
                while (true)
                {
                    Console.Clear(); // Clear console for a fresh start
                    Console.WriteLine("Welcome to Connect Four!");
                    Console.WriteLine("-------------------------");
                    Console.WriteLine("Select game mode:");
                    Console.WriteLine("1. Two-Player Mode");
                    Console.WriteLine("2. One-Player Mode (vs. Computer)");
                    Console.WriteLine("3. Exit");

                    Console.Write("Enter your choice (1, 2, or 3): ");
                    string choice = Console.ReadLine();

                    GameMode mode;
                    if (choice == "1")
                    {
                        mode = GameMode.TwoPlayer;
                    }
                    else if (choice == "2")
                    {
                        mode = GameMode.OnePlayer;
                    }
                    else if (choice == "3")
                    {
                        Console.WriteLine("Exiting game. Goodbye!");
                        break; // Exit the game loop
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                        System.Threading.Thread.Sleep(1500); // Pause to show message
                        continue; // Re-prompt for choice
                    }

                    Game game = new Game(mode); // Create a new game instance
                    game.StartGame(); // Start the game
                    Console.WriteLine("\nPress any key to return to the main menu or exit if you chose 'no' earlier...");
                    Console.ReadKey(); // Wait for user input before returning to main menu
                }
            }
        }
    }
}

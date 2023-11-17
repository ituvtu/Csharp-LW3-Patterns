using System;

public class Game : IGame
{
	#region Fields and Properties

	private readonly GameMemento _memento;
	private GameBoard gameBoard;
	private int currentPlayer = 1;

	public int CurrentPlayer
	{
		get { return currentPlayer; }
		set
		{
			currentPlayer = value;
			// Додаткова логіка, якщо потрібно
		}
	}

	public int Rows => gameBoard.Rows;
	public int Columns => gameBoard.Columns;

	#endregion

	#region Events

	public event Action<int> GameWon;

	#endregion

	#region Constructor

	public Game()
	{
		int rows = 6;
		int columns = 6;
		_memento = new GameMemento(new GameBoard(rows, columns));
		gameBoard = _memento.SavedGameBoard;
	}

	#endregion

	#region Public Methods

	public void StartNewGame()
	{
		gameBoard.ResetBoard();
		currentPlayer = 1;
		// Додаткова ініціалізація за потреби
	}

	public void MakeMoveCross(int x, int y)
	{
		gameBoard.SetValueAt(x, y, 1); // 1 представляє хрестик
		HandleMoveResult(gameBoard.CheckForWin(1));
	}

	public void MakeMoveCircle(int x, int y)
	{
		gameBoard.SetValueAt(x, y, 2); // 2 представляє нолик
		HandleMoveResult(gameBoard.CheckForWin(2));
	}

	public void AddNode(GameBoard.Node newNode)
	{
		gameBoard.AddNode(newNode);
	}

	public void RestoreFromMemento(GameMemento memento)
	{
		if (memento == null)
			throw new ArgumentNullException(nameof(memento));

		gameBoard = memento.SavedGameBoard;
	}

	#endregion

	#region Private Methods

	private void HandleMoveResult(GameResult result)
	{
		switch (result)
		{
			case GameResult.OrderWin:
				GameWon?.Invoke(currentPlayer);
				break;
			case GameResult.ChaosWin:
				GameWon?.Invoke(0);
				break;
				// NoWin не вимагає дій
		}
	}

	protected virtual void OnGameWon(int playerValue)
	{
		GameWon?.Invoke(playerValue);
	}

	#endregion

	#region Interface Implementations

	public int GetValueAt(int x, int y)
	{
		return gameBoard.GetValueAt(x, y);
	}

	public string GetBoardAsString()
	{
		return gameBoard.ToString();
	}

	public int GetCurrentPlayer()
	{
		return currentPlayer;
	}

	public GameBoard GetBoard()
	{
		return gameBoard;
	}

	public GameMemento CreateMemento()
	{
		return new GameMemento(gameBoard);
	}

	#endregion
}

using System;
using System.Text;
public enum GameResult
{
	OrderWin, // Перемога порядку
	ChaosWin, // Перемога хаосу
	NoWin     // Немає переможця (гра продовжується)
}
public class GameBoard
{
	#region Fields
	private Node boardData; // Поле, що буде використовуватись для представлення ігрового поля
	#endregion

	#region Nested Classes
	public class Node
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Value { get; set; }
		public Node Next { get; set; }
		public Node(int x, int y, int value)
		{
			X = x;
			Y = y;
			Value = value;
			Next = null;
		}
	}
	#endregion

	#region Constructors
	public GameBoard(int rows, int columns)
	{
		Rows = rows;
		Columns = columns;
		boardData = null; // Початкове ігрове поле пусте
	}
	#endregion

	#region Properties
	public int Rows { get; private set; }
	public int Columns { get; private set; }
	#endregion

	#region Public Methods
	// Отримати значення комірки за її координатами
	public int GetValueAt(int x, int y)
	{
		if (boardData != null)
		{
			Node node = FindNode(x, y);
			if (node != null)
			{
				return node.Value;
			}
		}
		return 0; // Повертаємо 0 для нульових значень або якщо значення не знайдено.
	}

	// Встановити значення комірки за її координатами
	public void SetValueAt(int x, int y, int value)
	{
		if (x < 0 || x >= Rows || y < 0 || y >= Columns)
		{
			throw new IndexOutOfRangeException("Invalid row or column index.");
		}

		if (boardData == null)
		{
			boardData = new Node(x, y, value);
		}
		else
		{
			Node node = FindNode(x, y);
			if (node != null)
			{
				node.Value = value;
			}
			else
			{
				Node newNode = new Node(x, y, value)
				{
					Next = boardData
				};
				boardData = newNode;
			}
		}
	}

	// Додати новий вузол (ход) на гру
	public void AddNode(Node newNode)
	{
		if (newNode.X < 0 || newNode.X >= Rows || newNode.Y < 0 || newNode.Y >= Columns)
		{
			throw new IndexOutOfRangeException("Invalid row or column index.");
		}

		if (boardData != null)
		{
			Node node = FindNode(newNode.X, newNode.Y);
			if (node != null)
			{
				node.Value = newNode.Value;
			}
			else
			{
				newNode.Next = boardData;
				boardData = newNode;
			}
		}
		else
		{
			boardData = new Node(newNode.X, newNode.Y, newNode.Value);
		}
	}

	// Метод для виведення ігрового поля на консоль
	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();

		for (int i = 0; i < Rows; i++)
		{
			for (int j = 0; j < Columns; j++)
			{
				sb.Append(GetValueAt(i, j) + " ");
			}
			sb.AppendLine(); // Перехід на новий рядок для виводу наступного рядка ігрового поля
		}

		return sb.ToString();
	}
	public void ResetBoard()
	{
		// Очистити пов'язаний список, встановивши boardData на null
		boardData = null;

		// Опціонально, якщо вам потрібно зберегти стан ігрового поля до ресету,
		// ви могли б додати логіку тут для збереження стану до ресету, 
		// наприклад, використовуючи мементо або інший механізм.
	}
	public GameResult CheckForWin(int winValue)
	{
		if (CheckLinesForWin(winValue, 5))
			return GameResult.OrderWin;

		if (IsBoardFull())
			return GameResult.ChaosWin;

		return GameResult.NoWin;
	}
	#endregion

	#region Private Methods
	private Node FindNode(int x, int y)
	{
		Node currentNode = boardData;
		while (currentNode != null)
		{
			if (currentNode.X == x && currentNode.Y == y)
			{
				return currentNode;
			}
			currentNode = currentNode.Next;
		}
		return null;
	}


	private bool CheckLinesForWin(int winValue, int requiredConsecutive)
	{
		// Перевірка для кожного рядка
		for (int row = 0; row < Rows; row++)
		{
			if (CheckLine(row, 0, 0, 1, winValue, requiredConsecutive))
				return true;
		}

		// Перевірка для кожного стовпця
		for (int col = 0; col < Columns; col++)
		{
			if (CheckLine(0, col, 1, 0, winValue, requiredConsecutive))
				return true;
		}

		return false;
	}

	private bool CheckLine(int startX, int startY, int deltaX, int deltaY, int winValue, int requiredConsecutive)
	{
		BoardIterator iterator = new BoardIterator(this);
		int consecutiveCount = 0;

		while (iterator.MoveNext())
		{
			int currentX = iterator.CurrentX;
			int currentY = iterator.CurrentY;

			if (currentX == startX && currentY == startY)
			{
				if ((int)iterator.Current == winValue)
				{
					consecutiveCount++;
					if (consecutiveCount == requiredConsecutive)
						return true;
				}
				else
				{
					consecutiveCount = 0;
				}

				startX += deltaX;
				startY += deltaY;
			}
		}

		return false;
	}

	private bool IsBoardFull()
	{
		BoardIterator iterator = new BoardIterator(this);
		while (iterator.MoveNext())
		{
			if ((int)iterator.Current == 0)
				return false;
		}

		return true;
	}
	#endregion
}

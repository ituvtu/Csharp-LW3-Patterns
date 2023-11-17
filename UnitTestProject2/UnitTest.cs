using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2
{
	[TestClass]
	public class UnitTest
	{
		#region Pattern Iterator
		[TestMethod]
		public void BoardIterator_Current_AfterEnd_ThrowsException()
		{
			GameBoard board = new GameBoard(1, 1);
			BoardIterator iterator = new BoardIterator(board);
			iterator.MoveNext();
			iterator.MoveNext();

			Assert.ThrowsException<InvalidOperationException>(() => { var current = iterator.Current; });
		}

		[TestMethod]
		public void BoardIterator_Reset_ResetsCurrentToInitial()
		{
			GameBoard board = new GameBoard(1, 1);
			BoardIterator iterator = new BoardIterator(board);
			iterator.MoveNext();
			iterator.Reset();
			iterator.MoveNext();

			Assert.AreEqual(0, iterator.Current); // Assuming default value is 0 for unassigned cells
		}
		[TestMethod]
		public void BoardIterator_Current_BeforeFirstMove_ThrowsException()
		{
			GameBoard board = new GameBoard(3, 3);
			BoardIterator iterator = new BoardIterator(board);

			Assert.ThrowsException<InvalidOperationException>(() => { var current = iterator.Current; });
		}

		[TestMethod]
		public void BoardIterator_MoveNext_EmptyBoard_ReturnsFalse()
		{
			GameBoard board = new GameBoard(0, 0);
			BoardIterator iterator = new BoardIterator(board);

			bool result = iterator.MoveNext();

			Assert.IsFalse(result);
		}
		[TestMethod]
		public void BoardIterator_Iteration_CoversAllCells()
		{
			GameBoard board = new GameBoard(3, 3);
			BoardIterator iterator = new BoardIterator(board);

			int cellCount = 0;
			while (iterator.MoveNext())
			{
				cellCount++;
			}

			Assert.AreEqual(9, cellCount); // 3x3 board should have 9 cells
		}

		[TestMethod]
		public void BoardIterator_Current_ReturnsCorrectValue()
		{
			GameBoard board = new GameBoard(3, 3);
			board.SetValueAt(1, 1, 5); // Встановлюємо значення 5 на позиції (1,1)
			BoardIterator iterator = new BoardIterator(board);

			// Пересуваємо ітератор на позицію (1,1)
			iterator.MoveNext(); // (0,0)
			iterator.MoveNext(); // (0,1)
			iterator.MoveNext(); // (0,2)
			iterator.MoveNext(); // (1,0)
			iterator.MoveNext(); // (1,1) - Тут ми очікуємо значення 5

			Assert.AreEqual(5, iterator.Current);
		}
		[TestMethod]
		public void BoardIterator_Reset_ResetsToInitialPosition()
		{
			GameBoard board = new GameBoard(2, 2);
			BoardIterator iterator = new BoardIterator(board);

			iterator.MoveNext();
			iterator.Reset();

			Assert.ThrowsException<InvalidOperationException>(() => { var current = iterator.Current; });
		}

		[TestMethod]
		public void BoardIterator_MoveNext_AfterEnd_ReturnsFalse()
		{
			GameBoard board = new GameBoard(1, 1);
			BoardIterator iterator = new BoardIterator(board);

			iterator.MoveNext();
			bool canMoveNext = iterator.MoveNext();

			Assert.IsFalse(canMoveNext);
		}
		[TestMethod]
		public void BoardIterator_Current_InvalidState_ThrowsException()
		{
			GameBoard board = new GameBoard(3, 3);
			BoardIterator iterator = new BoardIterator(board);

			// Move iterator beyond the end
			while (iterator.MoveNext()) { }

			Assert.ThrowsException<InvalidOperationException>(() => { var current = iterator.Current; });
		}

		[TestMethod]
		public void BoardIterator_MoveNext_OnEmptyBoard_ReturnsFalse()
		{
			GameBoard board = new GameBoard(0, 0);
			BoardIterator iterator = new BoardIterator(board);

			bool result = iterator.MoveNext();

			Assert.IsFalse(result);
		}
		#endregion

		#region Pattern Memento
		[TestMethod]
		public void GameMemento_RestoreFromMemento_RestoresCorrectState()
		{
			Game game = new Game();
			game.MakeMoveCross(0, 0);
			GameMemento memento = game.CreateMemento();

			game.MakeMoveCircle(1, 1);
			game.RestoreFromMemento(memento);

			Assert.AreEqual(1, game.GetBoard().GetValueAt(0, 0));
			Assert.AreEqual(0, game.GetBoard().GetValueAt(1, 1));
		}
		[TestMethod]
		public void GameMemento_CreateMemento_CreatesCorrectMemento()
		{
			GameBoard board = new GameBoard(3, 3);
			board.SetValueAt(1, 1, 5);
			GameMemento memento = new GameMemento(board);

			Assert.AreEqual(5, memento.SavedGameBoard.GetValueAt(1, 1));
		}
		[TestMethod]
		public void Game_CreateMemento_SavesCurrentState()
		{
			Game game = new Game();
			game.MakeMoveCross(0, 0);
			var memento = game.CreateMemento();

			Assert.AreEqual(1, memento.SavedGameBoard.GetValueAt(0, 0));
		}

		[TestMethod]
		public void Game_RestoreFromMemento_RestoresCorrectStateAndPlayer()
		{
			Game game = new Game();
			game.MakeMoveCross(0, 0); // Перший хід хрестиком
			var memento = game.CreateMemento();

			game.MakeMoveCircle(1, 1); // Другий хід ноликом
			game.RestoreFromMemento(memento);

			var currentPlayerAfterRestore = game.GetCurrentPlayer();
			var cellValueAfterRestore = game.GetBoard().GetValueAt(0, 0);

			Assert.AreEqual(1, cellValueAfterRestore); // Перевіряємо стан дошки
			Assert.AreNotEqual(2, currentPlayerAfterRestore); // Перевіряємо, що поточний гравець не є гравцем 2
		}
		#endregion

		#region Game
		[TestMethod]
		public void Game_GetValueAt_AfterMultipleMoves_ReturnsCorrectValues()
		{
			Game game = new Game();
			game.MakeMoveCross(0, 0);
			game.MakeMoveCircle(1, 1);

			Assert.AreEqual(1, game.GetValueAt(0, 0));
			Assert.AreEqual(2, game.GetValueAt(1, 1));
		}
		[TestMethod]
		public void GetCurrentPlayer_AfterCreation_ReturnsPlayer1()
		{
			// Arrange
			Game game = new Game();

			// Act
			int currentPlayer = game.GetCurrentPlayer();

			// Assert
			Assert.AreEqual(1, currentPlayer);
		}


		[TestMethod]
		public void GetValueAt_OutOfBounds_ReturnsZero()
		{
			// Arrange
			Game game = new Game();

			// Act
			int value = game.GetValueAt(3, 3);

			// Assert
			Assert.AreEqual(0, value);
		}

		[TestMethod]
		public void GetBoardAsString_ReturnsNonEmptyString()
		{
			// Arrange
			Game game = new Game();

			// Act
			string boardString = game.GetBoardAsString();

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(boardString));
		}
		[TestMethod]
		public void GetCurrentPlayer_AfterCrossAndCircleMove_ReturnsNextPlayer()
		{
			Game game = new Game();

			// Перший хід гравця 1 (хрестик)
			game.MakeMoveCross(0, 0);
			int currentPlayerAfterCross = game.GetCurrentPlayer();

			// Другий хід гравця 2 (нолик)
			game.MakeMoveCircle(1, 1);
			int currentPlayerAfterCircle = game.GetCurrentPlayer();

			Assert.AreEqual(currentPlayerAfterCross, currentPlayerAfterCircle); // Перевіряємо, чи змінився гравець
		}
		[TestMethod]
		public void GetCurrentPlayer_AfterMove_ReturnsNextPlayer()
		{
			// Arrange
			Game game = new Game();

			// Act
			game.MakeMoveCross(0, 0);
			int currentPlayer = game.GetCurrentPlayer();

			// Assert
			Assert.AreEqual(1, currentPlayer); // Перевіряємо, що наступний гравець не є гравцем 1
		}
		[TestMethod]
		public void Game_GetValueAt_DefaultValue_ReturnsZero()
		{
			Game game = new Game();

			Assert.AreEqual(0, game.GetValueAt(1, 1));
		}
		[TestMethod]
		public void Game_GetBoardAsString_AfterCrossAndCircleMoves_ReturnsUpdatedString()
		{
			Game game = new Game();
			game.MakeMoveCross(0, 0); // Хрестик на (0, 0)
			game.MakeMoveCircle(1, 1); // Нолик на (1, 1)

			string expectedString = "1 0 0 0 0 0 \n0 2 0 0 0 0 \n0 0 0 0 0 0 \n0 0 0 0 0 0 \n0 0 0 0 0 0 \n0 0 0 0 0 0 \n"
				.Replace("\n", Environment.NewLine); // Універсальний формат
			string actualString = game.GetBoardAsString();

			Assert.AreEqual(expectedString, actualString);
		}
		#endregion

		#region GameBoard
		[TestMethod]
		public void GameBoard_Constructor_InitializesEmptyBoard()
		{
			int rows = 3;
			int columns = 3;
			GameBoard board = new GameBoard(rows, columns);

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					Assert.AreEqual(0, board.GetValueAt(i, j));
				}
			}
		}

		[TestMethod]
		public void GameBoard_SetValueAt_SetsCorrectValue()
		{
			GameBoard board = new GameBoard(3, 3);
			board.SetValueAt(1, 1, 5);

			Assert.AreEqual(5, board.GetValueAt(1, 1));
		}

		[TestMethod]
		public void GameBoard_SetValueAt_OutOfBounds_ThrowsException()
		{
			GameBoard board = new GameBoard(3, 3);

			Assert.ThrowsException<IndexOutOfRangeException>(() => board.SetValueAt(3, 3, 5));
		}

		[TestMethod]
		public void GameBoard_AddNode_AddsNodeCorrectly()
		{
			GameBoard board = new GameBoard(3, 3);
			GameBoard.Node newNode = new GameBoard.Node(1, 1, 5);
			board.AddNode(newNode);

			Assert.AreEqual(5, board.GetValueAt(1, 1));
		}
		[TestMethod]
		public void GameBoard_AddNode_NodeAlreadyExists_UpdatesNodeValue()
		{
			GameBoard board = new GameBoard(3, 3);
			board.AddNode(new GameBoard.Node(1, 1, 5));
			board.AddNode(new GameBoard.Node(1, 1, 10));

			Assert.AreEqual(10, board.GetValueAt(1, 1));
		}

		[TestMethod]
		public void GameBoard_ToString_ReturnsCorrectFormat()
		{
			GameBoard board = new GameBoard(2, 2);
			board.SetValueAt(0, 0, 1);
			board.SetValueAt(0, 1, 2);
			board.SetValueAt(1, 0, 3);
			board.SetValueAt(1, 1, 4);

			string expectedString = $"1 2 {Environment.NewLine}3 4 {Environment.NewLine}";

			string actualString = board.ToString();

			Assert.AreEqual(expectedString, actualString);
		}
		[TestMethod]
		public void GameBoard_SetValueAt_ValidRange_ChecksBounds()
		{
			GameBoard board = new GameBoard(3, 3);
			board.SetValueAt(2, 2, 5);

			Assert.AreEqual(5, board.GetValueAt(2, 2));
		}



		[TestMethod]
		public void GameBoard_SetValueAt_NegativeIndex_ThrowsException()
		{
			GameBoard board = new GameBoard(3, 3);

			Assert.ThrowsException<IndexOutOfRangeException>(() => board.SetValueAt(-1, -1, 5));
		}

		[TestMethod]
		public void GameBoard_AddNode_NegativeIndex_ThrowsException()
		{
			GameBoard board = new GameBoard(3, 3);

			Assert.ThrowsException<IndexOutOfRangeException>(() => board.AddNode(new GameBoard.Node(-1, -1, 5)));
		}

		[TestMethod]
		public void GameBoard_ToString_EmptyBoard_ReturnsCorrectFormat()
		{
			GameBoard board = new GameBoard(2, 2);
			string expectedString = $"0 0 {Environment.NewLine}0 0 {Environment.NewLine}";

			string actualString = board.ToString();

			Assert.AreEqual(expectedString, actualString);
		}
		[TestMethod]
		public void GameBoard_SetValueAt_InvalidColumn_ThrowsException()
		{
			GameBoard board = new GameBoard(3, 3);

			Assert.ThrowsException<IndexOutOfRangeException>(() => board.SetValueAt(0, 3, 5));
		}

		[TestMethod]
		public void GameBoard_SetValueAt_InvalidRow_ThrowsException()
		{
			GameBoard board = new GameBoard(3, 3);

			Assert.ThrowsException<IndexOutOfRangeException>(() => board.SetValueAt(3, 0, 5));
		}

		[TestMethod]
		public void GameBoard_GetValueAt_EmptyCell_ReturnsZero()
		{
			GameBoard board = new GameBoard(3, 3);

			Assert.AreEqual(0, board.GetValueAt(1, 1));
		}
		#endregion

		#region CheckWin
		[TestMethod]
		public void CheckForWin_OrderWinHorizontal_ReturnsOrderWin()
		{
			var gameBoard = new GameBoard(5, 5);
			for (int i = 0; i < 5; i++)
			{
				gameBoard.SetValueAt(0, i, 1); // Заповнення рядка однаковими значеннями
			}

			var result = gameBoard.CheckForWin(1);
			Assert.AreEqual(GameResult.OrderWin, result);
		}

		[TestMethod]
		public void CheckForWin_OrderWinVertical_ReturnsOrderWin()
		{
			var gameBoard = new GameBoard(5, 5);
			for (int i = 0; i < 5; i++)
			{
				gameBoard.SetValueAt(i, 0, 1); // Заповнення колонки однаковими значеннями
			}

			var result = gameBoard.CheckForWin(1);
			Assert.AreEqual(GameResult.OrderWin, result);
		}

		[TestMethod]
		public void CheckForWin_ChaosWin_ReturnsChaosWin()
		{
			var gameBoard = new GameBoard(5, 5);
			for (int x = 0; x < 5; x++)
			{
				for (int y = 0; y < 5; y++)
				{
					gameBoard.SetValueAt(x, y, (x + y) % 2 + 1); // Заповнення поля без переможної комбінації
				}
			}

			var result = gameBoard.CheckForWin(1);
			Assert.AreEqual(GameResult.ChaosWin, result);
		}

		[TestMethod]
		public void CheckForWin_NoWin_ReturnsNoWin()
		{
			var gameBoard = new GameBoard(5, 5);
			gameBoard.SetValueAt(0, 0, 1); // Лише одне значення на полі

			var result = gameBoard.CheckForWin(1);
			Assert.AreEqual(GameResult.NoWin, result);
		}
		#endregion

		#region Move
		[TestMethod]
		public void MakeMove_OutOfBounds_ThrowsException()
		{
			// Arrange
			Game game = new Game();

			// Act & Assert
			Assert.ThrowsException<IndexOutOfRangeException>(() => game.MakeMoveCross(7, 7));
		}
		[TestMethod]
		public void MakeMoveCircle_InvalidPosition_ThrowsException()
		{
			Game game = new Game();

			Assert.ThrowsException<IndexOutOfRangeException>(() => game.MakeMoveCircle(-1, -1));
		}
		[TestMethod]
		public void MakeMoveCross_UpdatesGameBoard()
		{
			// Arrange
			Game game = new Game();

			// Act
			game.MakeMoveCross(0, 0);
			int valueAtFirstMove = game.GetBoard().GetValueAt(0, 0);

			// Assert
			Assert.AreEqual(1, valueAtFirstMove); // Перевіряємо, що хрестик розміщено
		}
		[TestMethod]
		public void MakeMoveCircle_SameCellTwice_UpdatesValue()
		{
			Game game = new Game();
			game.MakeMoveCircle(0, 0); // Player 1 (Circle)
			game.MakeMoveCross(0, 0); // Player 2 (Cross)

			Assert.AreEqual(1, game.GetValueAt(0, 0)); // Перевіряємо, що значення оновлено
		}

		[TestMethod]
		public void MakeMoveCross_InvalidRow_ThrowsException()
		{
			Game game = new Game();

			Assert.ThrowsException<IndexOutOfRangeException>(() => game.MakeMoveCross(3, 10));
		}

		[TestMethod]
		public void MakeMoveCircle_InvalidColumn_ThrowsException()
		{
			Game game = new Game();

			Assert.ThrowsException<IndexOutOfRangeException>(() => game.MakeMoveCircle(7, 7));
		}

		[TestMethod]
		public void MakeMoveCrossAndCircle_SwitchesPlayers()
		{
			// Arrange
			Game game = new Game();

			// Act
			game.MakeMoveCross(0, 0);
			int currentPlayerAfterFirstMove = game.GetCurrentPlayer();
			game.MakeMoveCircle(1, 1);
			int currentPlayerAfterSecondMove = game.GetCurrentPlayer();

			// Assert
			Assert.AreEqual(currentPlayerAfterFirstMove, currentPlayerAfterSecondMove);
		}

		[TestMethod]
		public void UndoMove_RestoresPreviousState()
		{
			// Arrange
			Game game = new Game();

			// Act
			game.MakeMoveCross(0, 0);
			GameMemento memento = game.CreateMemento();
			game.MakeMoveCircle(1, 1);
			game.RestoreFromMemento(memento);
			int valueAfterUndo = game.GetBoard().GetValueAt(0, 0);

			// Assert
			Assert.AreEqual(1, valueAfterUndo);
		}
		[TestMethod]
		public void MakeMoveCross_InvalidPosition_ThrowsException()
		{
			Game game = new Game();

			Assert.ThrowsException<IndexOutOfRangeException>(() => game.MakeMoveCross(-1, -1));
		}

		[TestMethod]
		public void MakeMoveCircle_UpdatesGameBoard()
		{
			// Arrange
			Game game = new Game();

			// Act
			game.MakeMoveCircle(1, 1);
			int valueAtSecondMove = game.GetBoard().GetValueAt(1, 1);

			// Assert
			Assert.AreEqual(2, valueAtSecondMove); // Перевіряємо, що нолик розміщено
		}
		[TestMethod]
		public void MakeMoveCircle_SameCellNotUpdatesValue()
		{
			Game game = new Game();
			game.MakeMoveCircle(0, 0); // Player 1 (Circle)
			game.MakeMoveCircle(0, 0); // Player 2 (Circle) спробує зробити хід в те саме місце

			Assert.AreEqual(2, game.GetValueAt(0, 0)); // Значення не повинно змінитися
		}
		#endregion

	}
}

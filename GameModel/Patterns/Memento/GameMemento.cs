public class GameMemento
{
	#region Fields and Properties
	public GameBoard SavedGameBoard { get; set; }
	#endregion

	#region Public Methods
	public GameMemento(GameBoard gameBoard)
	{
		// Копіюємо стан гри для збереження
		SavedGameBoard = new GameBoard(gameBoard.Rows, gameBoard.Columns);
		for (int i = 0; i < gameBoard.Rows; i++)
		{
			for (int j = 0; j < gameBoard.Columns; j++)
			{
				SavedGameBoard.SetValueAt(i, j, gameBoard.GetValueAt(i, j));
			}
		}
	}
	#endregion

}
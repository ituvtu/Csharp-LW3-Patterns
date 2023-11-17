using System;
using System.ComponentModel;
using System.Windows.Input;



public class GameViewModel : INotifyPropertyChanged
{
	private Game _game;

	public ICommand StartGameCommand { get; private set; }
	public ICommand MakeMoveCommand { get; private set; }

	public event PropertyChangedEventHandler PropertyChanged;

	private void StartGame()
	{
		_game.StartNewGame();
		OnPropertyChanged(nameof(GameBoard)); // Припустимо, що у вас є властивість GameBoard
	}

	private void MakeMove(object param)
	{
		// Логіка для ходу гравця
	}

	protected virtual void OnPropertyChanged(string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	// Припустимо, що у вас є властивість GameBoard
	public string GameBoard
	{
		get { return _game.GetBoardAsString(); }
	}

}

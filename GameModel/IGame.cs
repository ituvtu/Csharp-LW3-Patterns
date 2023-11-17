public interface IGame
{
	int Rows { get; }
	int Columns { get; }
	int GetValueAt(int x, int y);
	void AddNode(GameBoard.Node newNode);
	string ToString();
}
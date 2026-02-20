using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Example
{
    public partial class MainWindow : Window
    {
        private char[,] _maze;
        private int _rows;
        private int _cols;
        private const int CellSize = 20;
        private Figur _player;
        private Rectangle _playerRect;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;

            // Key-Events am Fenster registrieren, PreviewKeyDown fängt Tasten auch dann,
            // wenn ein Kind-Fokus hat (robuster in vielen WPF-Szenarien).
            PreviewKeyDown += Window_KeyDown;
            KeyDown += Window_KeyDown;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Versuche den Fokus auf das Fenster zu setzen, damit Key-Events ankommen.
            // Kombination sorgt in den meisten Fällen dafür, dass das Fenster wirklich Fokus bekommt.
            this.Focus();
            Keyboard.Focus(this);
            FocusManager.SetFocusedElement(this, this);

            string path = "maze_6x6.txt";
            if (!File.Exists(path))
            {
                MessageBox.Show($"Datei nicht gefunden: {path}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string[] lines = File.ReadAllLines(path).Select(l => l.Replace("\r", "")).ToArray();
            _rows = lines.Length;
            _cols = lines.Max(l => l.Length);

            _maze = new char[_rows, _cols];

            // Fülle Maze mit Leerzeichen und kopiere die Datei-Inhalte
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    _maze[i, j] = (j < lines[i].Length) ? lines[i][j] : ' ';
                }
            }

            // Größe des Spielfelds anpassen
            Spielfeld.Width = _cols * CellSize;
            Spielfeld.Height = _rows * CellSize;

            // Wände zeichnen und Startposition der Spielfigur finden
            bool foundPlayer = false;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    if (_maze[i, j] == '#')
                    {
                        var wall = new Rectangle
                        {
                            Fill = Brushes.Black,
                            Width = CellSize,
                            Height = CellSize
                        };
                        Canvas.SetTop(wall, i * CellSize);
                        Canvas.SetLeft(wall, j * CellSize);
                        Spielfeld.Children.Add(wall);
                    }
                    else if (_maze[i, j] == 'X' && !foundPlayer)
                    {
                        _player = new Figur(i, j);
                        foundPlayer = true;
                    }
                }
            }

            if (!foundPlayer)
            {
                // Fallback: obere linke Ecke
                _player = new Figur(0, 0);
            }

            // Spieler-Visual anlegen
            _playerRect = new Rectangle
            {
                Fill = Brushes.Red,
                Width = CellSize,
                Height = CellSize
            };
            Spielfeld.Children.Add(_playerRect);
            UpdatePlayerVisual();
        }

        private void UpdatePlayerVisual()
        {
            if (_playerRect == null || _player == null)
                return;

            Canvas.SetTop(_playerRect, _player.Row * CellSize);
            Canvas.SetLeft(_playerRect, _player.Col * CellSize);
        }

        private bool CanMoveTo(int row, int col)
        {
            if (row < 0 || row >= _rows || col < 0 || col >= _cols)
                return false;

            return _maze[row, col] != '#';
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            int dRow = 0;
            int dCol = 0;

            switch (e.Key)
            {
                case Key.W:
                case Key.Up:
                    dRow = -1;
                    break;
                case Key.S:
                case Key.Down:
                    dRow = 1;
                    break;
                case Key.A:
                case Key.Left:
                    dCol = -1;
                    break;
                case Key.D:
                case Key.Right:
                    dCol = 1;
                    break;
                default:
                    return;
            }

            // Guard: falls _player noch nicht gesetzt ist, nichts tun
            if (_player == null)
                return;

            if (_player.TryMove(dRow, dCol, CanMoveTo))
            {
                UpdatePlayerVisual();
                e.Handled = true; // verhindere unerwünschtes Weiterreichen des Events
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Example
{
    public partial class MainWindow : Window
    {
        const int CellSize = 20;

        char[,] maze;
        int rows;
        int cols;

        Figur player;
        Rectangle playerRect;

        List<Wand> waende = new List<Wand>();

        public MainWindow()
        {
            InitializeComponent();

            Loaded += LoadGame;
            KeyDown += Window_KeyDown;
        }

        void LoadGame(object sender, RoutedEventArgs e)
        {
            string[] lines = File.ReadAllLines("maze_6x6.txt");

            rows = lines.Length;
            cols = lines[0].Length;

            maze = new char[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    maze[i, j] = lines[i][j];

                    if (maze[i, j] == '#')
                        CreateWall(i, j);

                    if (maze[i, j] == 'X')
                        player = new Figur(i, j);
                }
            }

            CreatePlayer();
        }

        void CreateWall(int row, int col)
        {
            Wand wand = new Wand(row, col);

            Rectangle rect = new Rectangle
            {
                Width = CellSize,
                Height = CellSize,
                Fill = Brushes.Black
            };

            Canvas.SetTop(rect, row * CellSize);
            Canvas.SetLeft(rect, col * CellSize);

            wand.Rect = rect;

            waende.Add(wand);

            Spielfeld.Children.Add(rect);
        }

        void CreatePlayer()
        {
            playerRect = new Rectangle
            {
                Width = CellSize,
                Height = CellSize,
                Fill = Brushes.Red
            };

            Spielfeld.Children.Add(playerRect);

            UpdatePlayer();
        }

        void UpdatePlayer()
        {
            Canvas.SetTop(playerRect, player.Row * CellSize);
            Canvas.SetLeft(playerRect, player.Col * CellSize);
        }

        bool CanMove(int row, int col)
        {
            foreach (var wand in waende)
            {
                if (wand.Row == row && wand.Col == col)
                    return false;
            }

            return true;
        }

        void Window_KeyDown(object sender, KeyEventArgs e)
        {
            int dRow = 0;
            int dCol = 0;

            if (e.Key == Key.W || e.Key == Key.Up) dRow = -1;
            if (e.Key == Key.S || e.Key == Key.Down) dRow = 1;
            if (e.Key == Key.A || e.Key == Key.Left) dCol = -1;
            if (e.Key == Key.D || e.Key == Key.Right) dCol = 1;

            if (player.TryMove(dRow, dCol, CanMove))
                UpdatePlayer();
        }
    }
}












/*
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Example
{
    public partial class MainWindow : Window
    {
        const int CellSize = 20;

        char[,] maze;
        int rows;
        int cols;

        Figur player;
        Rectangle playerRect;

        List<Wand> waende = new List<Wand>();

        public MainWindow()
        {
            InitializeComponent();

            Loaded += LoadGame;
            KeyDown += Window_KeyDown;
        }

        void LoadGame(object sender, RoutedEventArgs e)
        {
            string[] lines = File.ReadAllLines("maze_6x6.txt");

            rows = lines.Length;
            cols = lines[0].Length;

            maze = new char[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    maze[i, j] = lines[i][j];

                    if (maze[i, j] == '#')
                        CreateWall(i, j);

                    if (maze[i, j] == 'X')
                        player = new Figur(i, j);
                }
            }

            CreatePlayer();
        }

        void CreateWall(int row, int col)
        {
            Wand wand = new Wand(row, col);

            Rectangle rect = new Rectangle
            {
                Width = CellSize,
                Height = CellSize,
                Fill = Brushes.Black
            };

            Canvas.SetTop(rect, row * CellSize);
            Canvas.SetLeft(rect, col * CellSize);

            wand.Rect = rect;

            waende.Add(wand);

            Spielfeld.Children.Add(rect);
        }

        void CreatePlayer()
        {
            playerRect = new Rectangle
            {
                Width = CellSize,
                Height = CellSize,
                Fill = Brushes.Red
            };

            Spielfeld.Children.Add(playerRect);

            UpdatePlayer();
        }

        void UpdatePlayer()
        {
            Canvas.SetTop(playerRect, player.Row * CellSize);
            Canvas.SetLeft(playerRect, player.Col * CellSize);
        }

        bool CanMove(int row, int col)
        {
            foreach (var wand in waende)
            {
                if (wand.Row == row && wand.Col == col)
                    return false;
            }

            return true;
        }

        void Window_KeyDown(object sender, KeyEventArgs e)
        {
            int dRow = 0;
            int dCol = 0;

            if (e.Key == Key.W || e.Key == Key.Up) dRow = -1;
            if (e.Key == Key.S || e.Key == Key.Down) dRow = 1;
            if (e.Key == Key.A || e.Key == Key.Left) dCol = -1;
            if (e.Key == Key.D || e.Key == Key.Right) dCol = 1;

            if (player.TryMove(dRow, dCol, CanMove))
                UpdatePlayer();
        }
    }
}
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Example
{
    public partial class MainWindow : Window
    {
        
        List<Goodie> goodies = new List<Goodie>();

        const int CellSize = 20;
        const int ActiveGoodieCount = 3; 

        char[,] maze;
        int rows;
        int cols;

        Figur player;
        Rectangle playerRect;

        List<Wand> waende = new List<Wand>();

        Random rnd = new Random();

        int goodieCount = 0;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += LoadGame;
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
         
            if (player == null)
                player = new Figur(0, 0);

            CreatePlayer();

            EnsureActiveGoodies();

            UpdateGoodieCounter();
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

                Fill = new ImageBrush(new BitmapImage(new Uri("C:\\Github Zamberl\\aiit-labor-Domsi43\\Lab06_Labyrinth\\Musi.jpg")))
                {
                    Stretch = Stretch.UniformToFill
                }
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
            {
                UpdatePlayer();

                var collected = goodies.Find(g => g.Row == player.Row && g.Col == player.Col);
                if (collected != null)
                {
                    OnGoodieCollected(collected);
                }
            }
        }

        
        void OnGoodieCollected(Goodie collected)
        {
            if (collected.Rect != null && Spielfeld.Children.Contains(collected.Rect))
                Spielfeld.Children.Remove(collected.Rect);

            goodies.Remove(collected);
            goodieCount++;
            UpdateGoodieCounter();
            EnsureActiveGoodies();
        }
        void UpdateGoodieCounter()
        {
            if (GoodieCounter != null)
            {
                GoodieCounter.Text = goodieCount.ToString();
            }
        }
        void EnsureActiveGoodies()
        {
            while (goodies.Count < ActiveGoodieCount)
            {
                PlaceGoodieRandom();
            }
        }
        void PlaceGoodieRandom()
        {
            if (maze == null || rows == 0 || cols == 0)
                return;

            for (int attempt = 0; attempt < 1000; attempt++)
            {
                int r = rnd.Next(rows);
                int c = rnd.Next(cols);

                if (maze[r, c] == '#')
                    continue;

                if (r == player.Row && c == player.Col)
                    continue;

                bool occupiedByGoodie = false;
                foreach (var g in goodies)
                {
                    if (g.Row == r && g.Col == c)
                    {
                        occupiedByGoodie = true;
                        break;
                    }
                }
                if (occupiedByGoodie)
                    continue;

                var rect = new Rectangle
                {
                    Width = CellSize,
                    Height = CellSize,
                    Fill = new ImageBrush(new BitmapImage(new Uri("C:\\Github Zamberl\\aiit-labor-Domsi43\\Lab06_Labyrinth\\Lebakas.Sömme.jpg")))
                    {
                        Stretch = Stretch.UniformToFill
                    }
                };

                Canvas.SetTop(rect, r * CellSize);
                Canvas.SetLeft(rect, c * CellSize);

                var goodie = new Goodie(r, c, rect);
                goodies.Add(goodie);
                Spielfeld.Children.Add(rect);
                return;
            }
        }
    }
}


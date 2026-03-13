    using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Example
{
    public partial class MainWindow : Window
    {
        List<Goodie> goodies = new List<Goodie>();
        List<Enemy> enemies = new List<Enemy>();

        const int CellSize = 20;
        const int ActiveGoodieCount = 3;
        const int ActiveEnemyCount = 2;
        const int InitialPoints = 5;
        const int DamagePerHit = 1;
        const int WinThreshold = 25; 

        char[,] maze;
        int rows;
        int cols;

        Figur player;
        Rectangle playerRect;

        List<Wand> waende = new List<Wand>();

        Random rnd = new Random();

        int goodieCount = 0;
        int playerPoints = InitialPoints;

        DispatcherTimer enemyTimer;

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

            EnsureActiveEnemies();
            StartEnemyTimer();

            UpdateGoodieCounter();
            UpdatePointsCounter();
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
            if (GameOverOverlay.Visibility == Visibility.Visible || WinOverlay.Visibility == Visibility.Visible)
                return;

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

                var ecoll = enemies.Find(en => en.Row == player.Row && en.Col == player.Col);
                if (ecoll != null)
                {
                    OnPlayerEaten();
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

            if (goodieCount >= WinThreshold)
            {
                Win();
                return;
            }

            EnsureActiveGoodies();
        }

        void UpdateGoodieCounter()
        {
            if (GoodieCounter != null)
            {
                GoodieCounter.Text = goodieCount.ToString();
            }
        }
        void UpdatePointsCounter()
        {
            if (PointsCounter != null)
            {
                PointsCounter.Text = playerPoints.ToString();
            }
        }

        void EnsureActiveGoodies()
        {
            if (WinOverlay.Visibility == Visibility.Visible || GameOverOverlay.Visibility == Visibility.Visible)
                return;

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

                bool occupiedByGoodie = goodies.Any(g => g.Row == r && g.Col == c);
                if (occupiedByGoodie)
                    continue;

                bool occupiedByEnemy = enemies.Any(en => en.Row == r && en.Col == c);
                if (occupiedByEnemy)
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

        void EnsureActiveEnemies()
        {
            while (enemies.Count < ActiveEnemyCount)
            {
                PlaceEnemyRandom();
            }
        }

        void PlaceEnemyRandom()
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

                if (goodies.Any(g => g.Row == r && g.Col == c))
                    continue;

                if (enemies.Any(en => en.Row == r && en.Col == c))
                    continue;

                var rect = new Rectangle
                {
                    Width = CellSize,
                    Height = CellSize,
                    Fill = new ImageBrush(new BitmapImage(new Uri("C:\\Github Zamberl\\aiit-labor-Domsi43\\Lab06_Labyrinth\\Salat.jpg")))
                    {
                        Stretch = Stretch.UniformToFill
                    }
                };

                Canvas.SetTop(rect, r * CellSize);
                Canvas.SetLeft(rect, c * CellSize);

                var enemy = new Enemy(r, c, rect);
                enemies.Add(enemy);
                Spielfeld.Children.Add(rect);
                return;
            }
        }
        void StartEnemyTimer()
        {
            enemyTimer = new DispatcherTimer();
            enemyTimer.Interval = TimeSpan.FromMilliseconds(500);
            enemyTimer.Tick += EnemyTimer_Tick;
            enemyTimer.Start();
        }
        void StopEnemyTimer()
        {
            if (enemyTimer != null)
            {
                enemyTimer.Stop();
                enemyTimer.Tick -= EnemyTimer_Tick;
                enemyTimer = null;
            }
        }
        void EnemyTimer_Tick(object sender, EventArgs e)
        {
            MoveEnemies();
        }
        void MoveEnemies()
        {
            if (GameOverOverlay.Visibility == Visibility.Visible || WinOverlay.Visibility == Visibility.Visible)
                return;

            foreach (var en in enemies.ToList())
            {
                var options = new List<Tuple<int,int>>();

                var deltas = new (int dr, int dc)[] { (-1,0), (1,0), (0,-1), (0,1) };
                foreach (var d in deltas)
                {
                    int nr = en.Row + d.dr;
                    int nc = en.Col + d.dc;
                    if (nr < 0 || nr >= rows || nc < 0 || nc >= cols)
                        continue;
                    if (maze[nr, nc] == '#')
                        continue;

                    if (enemies.Any(other => other != en && other.Row == nr && other.Col == nc))
                        continue;
                    options.Add(Tuple.Create(nr, nc));
                }

                if (options.Count == 0)
                    continue;

                var pick = options[rnd.Next(options.Count)];
                en.Row = pick.Item1;
                en.Col = pick.Item2;

                if (en.Rect != null)
                {
                    Canvas.SetTop(en.Rect, en.Row * CellSize);
                    Canvas.SetLeft(en.Rect, en.Col * CellSize);
                }

                if (en.Row == player.Row && en.Col == player.Col)
                {
                    OnPlayerEaten();
                }
            }
        }

        void OnPlayerEaten()
        {
            playerPoints -= DamagePerHit;
            if (playerPoints < 0) playerPoints = 0;
            UpdatePointsCounter();

            if (playerPoints <= 0)
            {
                GameOver();
            }
        }

        void GameOver()
        {
            StopEnemyTimer();
            GameOverOverlay.Visibility = Visibility.Visible;
        }

        void Win()
        {
            StopEnemyTimer();

            goodies.Clear();

            WinDetails.Text = $"Gesammelte Goodies: {goodieCount}";
            WinOverlay.Visibility = Visibility.Visible;
        }
    }
}


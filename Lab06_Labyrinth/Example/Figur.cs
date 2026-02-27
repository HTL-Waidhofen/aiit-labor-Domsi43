using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Example
{
    internal class Figur
    {
        public int Row { get; private set; }
        public int Col { get; private set; }

        public Ellipse e { get;}

        public Figur(int row, int col)
        {

            e = new Ellipse();
            e.Fill = Brushes.Blue;
            e.Width = 16;
            e.Height = 16;
            Canvas.SetLeft(e, col * 10);
            Canvas.SetTop(e, row * 10);

            Row = row;
            Col = col;
        }

       
        public bool TryMove(int dRow, int dCol, Func<int, int, bool> canMove)
        {
            int newRow = Row + dRow;
            int newCol = Col + dCol;
            if (canMove(newRow, newCol))
            {
                Row = newRow;
                Col = newCol;
                return true;
            }

            return false;
        }
    }
}

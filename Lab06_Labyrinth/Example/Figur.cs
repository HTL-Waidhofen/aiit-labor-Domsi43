using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Example
{
    public class Figur
    {
        public int Row;
        public int Col;

        public Figur(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public bool TryMove(int dRow, int dCol, Func < int, int, bool > CanMove)
        {
            int newRow = Row + dRow;
            int newCol = Col + dCol;

            if (CanMove(newRow, newCol))
            {
                Row = newRow;
                Col = newCol;
                return true;
            }

            return false;
        }
    }
}









/*
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Example
{
    public class Figur
    {
        public int Row;
        public int Col;

        public Figur(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public bool TryMove(int dRow, int dCol, Func<int, int, bool> CanMove)
        {
            int newRow = Row + dRow;
            int newCol = Col + dCol;

            if (CanMove(newRow, newCol))
            {
                Row = newRow;
                Col = newCol;
                return true;
            }

            return false;
        }
    }
}
*/
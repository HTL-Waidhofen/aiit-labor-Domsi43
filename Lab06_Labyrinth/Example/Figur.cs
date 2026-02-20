using System;

namespace Example
{
    internal class Figur
    {
        public int Row { get; private set; }
        public int Col { get; private set; }

        public Figur(int row, int col)
        {
            Row = row;
            Col = col;
        }

        // Versucht, die Figur um (dRow,dCol) zu bewegen.
        // canMove entscheidet, ob das Ziel betreten werden darf.
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

using System.Windows.Shapes;

namespace Example
{
    public class Goodie
    {
        public int Row;
        public int Col;
        public Rectangle Rect;

        public Goodie(int row, int col, Rectangle rect)
        {
            Row = row;
            Col = col;
            Rect = rect;
        }
    }
}
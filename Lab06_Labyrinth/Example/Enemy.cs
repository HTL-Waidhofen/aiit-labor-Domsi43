using System.Windows.Shapes;

namespace Example
{
    public class Enemy
    {
        public int Row;
        public int Col;
        public Rectangle Rect;

        public Enemy(int row, int col, Rectangle rect)
        {
            Row = row;
            Col = col;
            Rect = rect;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO2_P1
{
    public class Tile
    {
        public Checker? c;
        public bool HasChecker;
        public Tile(Checker c)
        {
            this.c = c;
            HasChecker = true;
        }
        public Tile()
        {
            this.c = null; 
            this.HasChecker = false;
        }
        public Tile(Tile other)
        {
            if (other.c != null) this.c = new Checker(other.c);
            else this.c = null;
            this.HasChecker = other.HasChecker;
        }

        public void Move(int x, int y, Tile[,] board) {
            if (x < 0 || y < 0 || x > 8 || y > 8) return;
            if (!board[x, y].HasChecker&&this.HasChecker)
            {
                Checker ch = this.c;
                this.c = null;
                this.HasChecker = false;
                board[x, y].HasChecker = true;
                ch.Move(y, x);
                board[x, y].c = ch;
            }
        }
    }
}

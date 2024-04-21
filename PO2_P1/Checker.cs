using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO2_P1
{
    public class Checker
    {
        const int offsetx = 26;
        const int offsety = 36;
        int x;
        int y;
        bool active;
        public int color;
        public SFML.Graphics.Sprite Sprite {  get; set; }
        public void Move(int x, int y)
        {
                this.x = x;
                this.y = y;
                Sprite.Position = new Vector2f(offsetx + 64 * x, offsety + 64 * y);
        }
        public Checker(SFML.Graphics.Texture t, int color, int pixels, int x, int y)
        {
            Sprite = new Sprite(t);
            Sprite.TextureRect = new IntRect(color*32, 0, 16, 16);
            Sprite.Scale = new Vector2f(1 / 16, 1 / 16);
            Sprite.Scale = new Vector2f(pixels / 16, pixels/ 16);
            this.x = x;
            this.y = y;
            active = true;
            Sprite.Position = new Vector2f(offsetx + 64 * x, offsety + 64*y);//1 field is 64 px long, starting on 26/36
            this.color = color;
        }
        public Checker(Checker other)
        {
            Sprite = new Sprite(other.Sprite);
            x = other.x;
            y = other.y;
            color = other.color;
            active = other.active;
        }
    }
}

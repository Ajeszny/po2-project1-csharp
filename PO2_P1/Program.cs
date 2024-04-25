using PO2_P1;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

static class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Press ESC key to close window");
        var window = new SimpleWindow();
        window.Run();

        Console.WriteLine("All done");
    }
}

class SimpleWindow
{
    const int offsetx = 26;
    const int offsety = 36;
    private Tile[,] checkers;
    SFML.Graphics.RenderWindow window;

    public void Run()
    {
        uint x = 600, y = 600;
        var mode = new SFML.Window.VideoMode(x - 142/4, y-142/4);
        window = new SFML.Graphics.RenderWindow(mode, "SFML works!");
        window.SetMouseCursorVisible(false);
        window.KeyPressed += Window_KeyPressed;
        window.Closed += Window_Closed;
        var boardtexture = new SFML.Graphics.Texture("Assets/boards/board_plain_05.png");
        var checkertexture = new SFML.Graphics.Texture("Assets/checkers.png");
        var cursortexture = new SFML.Graphics.Texture("Assets/cursor.png");
        var selectiontexture = new SFML.Graphics.Texture("Assets/redness.png");
        var boardsprite = new SFML.Graphics.Sprite(boardtexture);
        var cursorsprite = new SFML.Graphics.Sprite(cursortexture);
        var selectionsprite = new SFML.Graphics.Sprite(selectiontexture);
        cursorsprite.TextureRect = new IntRect(16, 0, 16, 16);
        cursorsprite.Scale = new Vector2f(2, 2);
        Checker c = new Checker(checkertexture, 1, 64, 1, 2);
        checkers = new Tile[8,8];
        Sprite? selectedchecker = null;
        int sx = -1, sy = -1;
        int current_color = 1;

        for (int i = 0;i < 8;i++)//x-es
        {
            for(int j = 0;j < 3;j++)//y-s
            {
                if ((i % 2 == 0&& j != 1)|| (i % 2 != 0&& j == 1))
                {
                    checkers [i, j] = new Tile(new Checker(checkertexture, 1, 64, i, j));
                } else
                {
                    checkers[i, j] = new Tile();
                }
            }
        }

        for(int i = 0; i < 8;i++)
        {
            for (int j = 3; j < 5;j++)
            {
                checkers[i, j] = new Tile();
            }
        }

        for (int i = 0; i < 8; i++)//x-es
        {
            for (int j = 5; j < 8; j++)//y-s
            {
                if (
                    (i % 2 != 0 && j != 6) 
                    ||
                    (i % 2 == 0 && j == 6)
                    )
                {
                    checkers[i, j] = new Tile(new Checker(checkertexture, 0, 64, i, j));
                }
                else
                {
                    checkers[i, j] = new Tile();
                }
            }
        }

        boardsprite.Scale = new Vector2f((x)/142, (y) / 142);

        // Start the game loop
        while (window.IsOpen)
        {
            // Process events
            window.DispatchEvents();
            window.Draw(boardsprite);
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (checkers[i, j].c != null) window.Draw(checkers[i, j].c.Sprite);
                }
            }
            Vector2i zalupa = get_mouse_coords(window);
            if (zalupa.X >= 0 && zalupa.Y >= 0) {
                selectionsprite.Position = new Vector2f(zalupa.X*64 + offsetx + 2, zalupa.Y*64 - offsety + 64);
                window.Draw(selectionsprite);
            }
            Vector2i pos = SFML.Window.Mouse.GetPosition(window);
            cursorsprite.Position = new Vector2f(pos.X, pos.Y);
            cursorsprite.TextureRect = new IntRect(16, 0, 16, 16);
            if (SFML.Window.Mouse.IsButtonPressed(0))
            {
                if (zalupa.X >= 0&& zalupa.Y >= 0&& zalupa.X < 8&& zalupa.Y < 8){
                    if (checkers[zalupa.X, zalupa.Y].c != null || selectedchecker != null)
                    {
                        if (checkers[zalupa.X, zalupa.Y].c != null && selectedchecker == null)
                        {
                            selectedchecker = new Sprite(checkers[zalupa.X, zalupa.Y].c.Sprite);
                            sx = zalupa.X;
                            sy = zalupa.Y;
                        }
                        selectedchecker.Position = new Vector2f(pos.X, pos.Y);

                        window.Draw(selectedchecker);
                    }
                }
                cursorsprite.TextureRect = new IntRect(32, 0, 16, 16);
                
            } else
            {
                if (selectedchecker != null)
                {
                    if (current_color == 0)
                    {
                        if (Math.Abs(zalupa.X - sx) == 1 && (zalupa.Y - sy) == 1)
                        {
                            if (Convert.ToBoolean(move_black_checker(zalupa.X - sx, sx, sy)))
                            {
                                current_color = 1;
                            }
                        }
                    } else if (current_color == 1) {

                        if (Math.Abs(zalupa.X - sx) == 1 && (zalupa.Y - sy) == -1)
                        {
                            if (Convert.ToBoolean(move_white_checker(zalupa.X - sx, sx, sy)))
                            {
                                current_color = 0;
                            }
                        }
                    }
                }
                sx = 0;
                sy = 0;
                selectedchecker = null;
            }
            window.Draw(cursorsprite);



            // Finally, display the rendered frame on screen
            window.Display();
        }
    }

    /// <summary>
    /// Move a checker
    /// </summary>
    /// <param name="dir">Either 1 or -1</param>
    /// <param name="x">x of dest checker</param>
    /// <param name="y">y of dest checker</param>
    private int move_black_checker(int dir, int x, int y)
    {
        if (x < 0 || y < 0 || x > 8 || y > 8|| checkers[x, y].c.color != 1) return 0;
        if (checkers[x + dir, y + 1].c != null)
        {
            if (((x + dir * 2 >= 8)
            || (y + 2 >= 8)
            || (x + dir * 2 < 0)
            || (y + 2 < 0)))
            {
                return 0;
            }
            if (checkers[x + dir * 2, y + 2].c != null) { return 0; }
            checkers[x + dir * 2, y + 2] = new Tile(checkers[x, y]);
            checkers[x + dir * 2, y + 2].c.Move(x + dir * 2, y + 2);
            checkers[x, y] = new Tile();
            checkers[x + dir, y + 1] = new Tile();
            return 1;
        }
        checkers[x+dir, y+1] = new Tile(checkers[x, y]);
        checkers[x + dir, y + 1].c.Move(x + dir, y + 1);
        checkers[x, y] = new Tile();
        return 1;
    }

    Vector2i local_input(int color)
    {
        return get_mouse_coords(window);
    }

    private int move_white_checker(int dir, int x, int y)
    {
        if (x < 0 || y < 0 || x > 8 || y > 8 || checkers[x, y].c.color != 0) return 0;
        if (checkers[x + dir, y - 1].c != null)
        {
            if (((x + dir * 2 >= 8)
            || (y + 2 >= 8)
            || (x + dir * 2 < 0)
            || (y + 2 < 0)))
            {
                return 0;
            }
            if (checkers[x + dir * 2, y - 2].c != null) { return 0; }
            checkers[x + dir * 2, y - 2] = new Tile(checkers[x, y]);
            checkers[x + dir * 2, y - 2].c.Move(x + dir * 2, y - 2);
            checkers[x, y] = new Tile();
            checkers[x + dir, y - 1] = new Tile();
            return 0;
        }
        checkers[x + dir, y - 1] = new Tile(checkers[x, y]);
        checkers[x + dir, y - 1].c.Move(x + dir, y - 1);
        checkers[x, y] = new Tile();
        return 1;
    }

    private Vector2i get_mouse_coords(Window window)
    {
        Vector2i res = SFML.Window.Mouse.GetPosition(window);
        res.X = (res.X - offsetx) / 64;
        res.Y = (res.Y - offsety) / 64;
        return res;
    }

    /// <summary>
    /// Function called when a key is pressed
    /// </summary>
    private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
    {
        var window = (SFML.Window.Window)sender;
        if (e.Code == SFML.Window.Keyboard.Key.Escape)
        {
            window.Close();
        }
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        var window = (SFML.Window.Window)sender;
        window.Close();
    }
}
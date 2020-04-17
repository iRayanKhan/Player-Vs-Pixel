using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Clicker
{
    public class LevelCreator : IDisposable
    {
        private Platform[,] platforms;
        private Dictionary<string, Texture2D> platformTextures;
        private Player player;
        private Game1 toPass;
        private enemy ene;

        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        public int PlatWidth
        {
            get { return platforms.GetLength(0); }
        }

        public int PlatHeight
        {
            get { return platforms.GetLength(1); }
        }

        public LevelCreator(IServiceProvider provider, string path, Game1 pass)
        {
            content = new ContentManager(provider, "Content");
            platformTextures = new Dictionary<string, Texture2D>();
            platformTextures.Add("Texture", Content.Load<Texture2D>("Platform Setter/Sample"));
            toPass = pass;
            LoadPlatforms(path);
        }

        //Load Levels into
        private void LoadPlatforms(string path)
        {
            int numOfPlat = 0;
            List<string> lines = new List<string>();
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line = reader.ReadLine();
                    numOfPlat = line.Length;
                    while (line != null)
                    {
                        lines.Add(line);
                        int nextLineWidth = line.Length;
                        line = reader.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file couldn't be read:");
                Console.WriteLine(e.Message);
            }
            platforms = new Platform[numOfPlat, lines.Count];

            for (int y = 0; y < PlatHeight; y++)
            {
                for (int x = 0; x < PlatWidth; x++)
                {
                    string currentRow = lines[y];
                    char charType = currentRow[x];
                    platforms[x, y] = PlatType(charType, x, y);
                }
            }
        }

        //Where the platforms are set
        private Platform PlatType(char platform, int x, int y)
        {
            switch (platform)
            {
                case '.':
                    return LoadPlatform(String.Empty, new Rectangle(), Platform.PlatformType.incollidable);
                case '_':
                    return LoadPlatform("Texture", new Rectangle(0 + (x * 100), 0 + (y * 50), 100, 50), Platform.PlatformType.collidable);
                case '-':
                    return LoadPlatform("Texture", new Rectangle(0 + (x * 100), 0 + (y * 50), 50, 100), Platform.PlatformType.incollidable);
                case '|':
                    return LoadPlatform("Texture", new Rectangle(0 + (x * 50), 0 + (y * 100), 50, 100), Platform.PlatformType.verticallyCollidable);
                case 's':
                    return LoadSpawn(x,y);
                case 'e':
                    return LoadEnemy(x, y);
                default:
                    throw new NotSupportedException(String.Format(
                        "Unsupported type character detected, {0}, please remove said illegal character at {1}, {2}.", platform, x, y));
            }
        }

        private Platform LoadEnemy(int x, int y) {
            ene = new enemy(new Vector2(x, y));
            return new Platform(String.Empty, new Rectangle(x, y, PlatWidth, PlatHeight), Platform.PlatformType.incollidable);
        }

        private Platform LoadSpawn(int _x, int _y) {
            if (player != null)
                throw new NotSupportedException("a level can only have one starting point");
            Vector2 start = new Vector2((_x * 32) + 48, (_y * 32) + 16);
            player = new Player(this, start, toPass);
            return new Platform(String.Empty, new Rectangle((int)start.X,(int)start.Y,PlatWidth, PlatHeight), Platform.PlatformType.incollidable);
        }

        private Platform LoadPlatform(string texture, Rectangle rec, Platform.PlatformType platType)
        {
            return new Platform(texture, rec, platType);
        }
        //Draws Platforms
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawPlatforms(spriteBatch);
            player.Draw(gameTime, spriteBatch);
            ene.Draw(spriteBatch, Content.Load<Texture2D>("click"));
        }

        public Platform.PlatformType GetCollision(int x, int y) {
            return platforms[x, y].PlatState;
        }

        public void Update(GameTime gameTime, SpriteBatch spriteBatch) {
            player.Update(gameTime);
            ene.Update(gameTime, player);

        }

        private void DrawPlatforms(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < PlatHeight; y++)
            {
                for (int x = 0; x < PlatWidth; x++)
                {
                    if (platformTextures.ContainsKey(platforms[x, y].PlatTex))
                    {
                        Vector2 pos = new Vector2(x, y) * platforms[x,y].getSize();
                        spriteBatch.Draw(platformTextures[platforms[x, y].PlatTex], platforms[x,y].PlatRec, Color.White);
                    }
                }
            }
        }

        

        

        public Rectangle GetBounds(int _x, int _y) {
            if (_x < 0 || _y < 0 || _x >= PlatWidth || _y >= PlatHeight)
                return new Rectangle(_x * Platform.WIDTH, _y * Platform.HEIGHT, Platform.WIDTH, Platform.HEIGHT);
            if (platforms[_x, _y].PlatState == Platform.PlatformType.collidable)
                return new Rectangle(_x * Platform.WIDTH, (_y * Platform.HEIGHT) + 20, Platform.WIDTH, Platform.HEIGHT - 20);

            return new Rectangle(_x * Platform.WIDTH, (_y * Platform.HEIGHT) + 5, Platform.WIDTH, Platform.HEIGHT - 5);
        }

        public void Dispose()
        {
            Content.Unload();
        }
    }
}
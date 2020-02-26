using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game1
{
    public class PlayState
    {
        private static readonly Camera camera;
        private readonly LightDiskPlayer player;
        private readonly List<Block> blocks;
        private readonly List<Light1> lights;
        private readonly Background background;

        static PlayState()
        {
            camera = new Camera();
        }

        public static void EarlyInitialize(GraphicsDeviceManager graphics)
        {
            LightPolygon.EarlyInitialize(graphics);
        }

        public static void Initialize(GraphicsDevice GraphicsDevice, ContentManager Content)
        {
            Block.Initialize(GraphicsDevice, camera, 32, 32, Color.Black);
            Light1.Initialize(GraphicsDevice, camera);
            Image.Initialize(Content);
        }

        public PlayState()
        {
            player = new LightDiskPlayer(new Vector2(-20, -50), 32, 0, Keys.Up, Keys.Left, Keys.Down, Keys.Right, Color.Yellow, 1f, Color.Yellow);

            blocks = new List<Block>();
            //{
            //    new Block(new Vector2(100, 100)),
            //};

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    blocks.Add(new Block(new Vector2(3 * i * (Block.width + 10), j * (Block.height + 10))));

            lights = new List<Light1>()
            {
                player.light,
                //new Light1(new Vector2(-100, -100), 2, Color.White),
                //new Light(new Vector2(-100, 0), 2, Color.White),
            };

            //lights[0].AddObject(player);
            //foreach (Block block in blocks)
            //    lights[0].AddObject(block);


            foreach (Light1 light in lights)
            {
                foreach (Block block in blocks)
                    light.AddObject(block);
            }

            background = new Background(new Point(0, 0), "ground", Color.White);
        }

        public void Update(float elapsed)
        {
            player.Update(elapsed);
            foreach (Block block in blocks)
                block.Collide(player);

            camera.Update(player.Position);

            foreach (Light1 light in lights)
                light.Update(elapsed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            camera.BeginDraw(spriteBatch);

            background.Draw(spriteBatch);

            spriteBatch.End();

            Light1.BeginDraw();
            foreach (Light1 light in lights)
                light.Draw();
            Light1.EndDraw(spriteBatch);

            camera.BeginDraw(spriteBatch);

            player.Draw(spriteBatch);
            foreach (Block block in blocks)
                block.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}

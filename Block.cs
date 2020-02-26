using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game1
{
    public class Block : PolygonObst
    {
        public static float width, height;
        private static Color color;

        public static void Initialize(GraphicsDevice GraphicsDevice, Camera camera, float newWidth, float newHeight, Color newColor)
        {
            width = newWidth;
            height = newHeight;
            color = newColor;
            Polygon.Initialize(GraphicsDevice, camera);
        }

        public Block(Vector2 center)
            : base(new List<Vector2>()
            {
                center + new Vector2(-width, -height) * 0.5f,
                center + new Vector2(-width, height) * 0.5f,
                center + new Vector2(width, height) * 0.5f,
                center + new Vector2(width, -height) * 0.5f
            }, 0, color)
        { }
    }
}

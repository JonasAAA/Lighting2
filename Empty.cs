using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game1
{
    public class Empty : IShadowCastingObject
    {
        private readonly float maxDist;

        public Empty(float maxDist)
        {
            this.maxDist = maxDist;
        }

        public void RelAngles(Vector2 lightPos, List<AngleObj> relAngles)
        {
            for (int i = 0; i < 4; i++)
                relAngles.Add(new AngleObj(i * MathHelper.TwoPi / 4, this));
        }
        //void RelAngles(Vector2 lightPos, List<float> relAngles);

        public void InterPoint(Vector2 lightPos, Vector2 lightDir, List<float> interPoints)
        {
            interPoints.Add(maxDist);
        }

        public void Draw(SpriteBatch spriteBatch)
        { }
    }
}

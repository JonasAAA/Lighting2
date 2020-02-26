﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game1
{
    public class Segment : IShadowCastingObject
    {
        public readonly Vector2 startPos, dir, orthDir;
        public readonly float rotation, length, radius;
        
        private readonly Disk startDisk, endDisk;
        private readonly Image image;

        public Segment(Vector2 startPos, Vector2 endPos, float radius, Color color)
        {
            float small = 0.1f;
            this.startPos = startPos;
            dir = endPos - startPos;
            dir.Normalize();
            startPos -= dir * small;
            length += 2 * small;
            orthDir = new Vector2(-dir.Y, dir.X);
            rotation = (float)Math.Atan2(dir.Y, dir.X);
            length = Vector2.Distance(startPos, endPos);
            this.radius = radius;

            startDisk = new Disk(startPos, radius, 0, "disk", color);
            endDisk = new Disk(endPos, radius, 0, "disk", color);

            image = new Image("pixel", length, 2 * radius, color);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            startDisk.Draw(spriteBatch);
            endDisk.Draw(spriteBatch);
            image.Draw(spriteBatch, startPos + dir * length / 2, rotation);
        }

        //public void RelAngles(Vector2 lightPos, List<float> relAngles)
        //{
        //    if (radius >= 1.5f)
        //    {
        //        startDisk.RelAngles(lightPos, relAngles);
        //        endDisk.RelAngles(lightPos, relAngles);
        //    }
        //    else
        //    {
        //        Vector2 startRelPos = startPos - lightPos,
        //                endRelPos = startPos + length * dir - lightPos;
        //        relAngles.Add((float)Math.Atan2(startRelPos.Y, startRelPos.X));
        //        relAngles.Add((float)Math.Atan2(endRelPos.Y, endRelPos.X));
        //    }
        //}

        public void RelAngles(Vector2 lightPos, List<AngleObj> relAngles)
        {
            if (radius >= 1.5f)
            {
                throw new NotImplementedException();
                ////startDisk.RelAngles(lightPos, relAngles);
                ////endDisk.RelAngles(lightPos, relAngles);
            }
            else
            {
                Vector2 startRelPos = startPos - lightPos,
                        endRelPos = startPos + length * dir - lightPos;
                relAngles.Add(new AngleObj((float)Math.Atan2(startRelPos.Y, startRelPos.X), this));
                relAngles.Add(new AngleObj((float)Math.Atan2(endRelPos.Y, endRelPos.X), this));
            }
        }

        public void InterPoint(Vector2 lightPos, Vector2 lightDir, List<float> interPoints)
        {
            if (radius >= 1.5f)
            {
                startDisk.InterPoint(lightPos, lightDir, interPoints);
                endDisk.InterPoint(lightPos, lightDir, interPoints);
            }

            float a = dir.X,
                  b = -lightDir.X,
                  c = lightPos.X - startPos.X,
                  d = dir.Y,
                  e = -lightDir.Y,
                  f = lightPos.Y - startPos.Y;

            // now we have to solve
            // t1 * a + t2 * b = c
            // t1 * d + t2 * e = f
            // where t2 is what we want and t1 tell us if they intersect at all

            float det = a * e - b * d;
            if (det == 0)
                return;

            float t1 = (c * e - b * f) / det,
                  t2 = (a * f - c * d) / det;

            if (t1 < 0 || t1 > length)
                return;

            //if (t1 < -0.001f || t1 > length + 0.001f)
            //    return;

            if (radius >= 1.5f)
            {
                // this is not exactly correct but should be close enough
                Disk temporary = new Disk(startPos + dir * t1, radius, 0, "disk", image.color);
                temporary.InterPoint(lightPos, lightDir, interPoints);
            }
            else
            {
                if (t2 > 0)
                    interPoints.Add(t2);
            }
        }
    }
}

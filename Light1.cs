using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game1
{
    public class Light1
    {
        public Vector2 position;

        protected float mainAngle;

        private readonly float maxAngleDiff;
        private readonly List<IShadowCastingObject> castObjects;
        private readonly LightPolygon polygon;

        private float angle;

        public static void EarlyInitialize(GraphicsDeviceManager graphics)
        {
            LightPolygon.EarlyInitialize(graphics);
        }

        public static void Initialize(GraphicsDevice GraphicsDevice, Camera camera)
        {
            LightPolygon.Initialize(GraphicsDevice, camera);
        }

        public Light1(Vector2 position, float strength, Color color)
            : this(position, 0, MathHelper.TwoPi, strength, color)
        { }

        public Light1(Vector2 position, float mainAngle, float maxAngleDiff, float strength, Color color)
        {
            this.position = position;

            this.mainAngle = mainAngle;

            this.maxAngleDiff = maxAngleDiff;
            castObjects = new List<IShadowCastingObject>();
            polygon = new LightPolygon(strength, color);
        }

        public void AddObject(IShadowCastingObject castObject)
        {
            castObjects.Add(castObject);
        }

        public virtual void Update(float elapsed)
        {
            List<AngleObj> angles = new List<AngleObj>();
            foreach (var castObject in castObjects)
                castObject.RelAngles(position, angles);

            const float small = 0.0001f;
            int oldAngleCount = angles.Count;

            for (int i = 0; i < oldAngleCount; i++)
            {
                angles.Add(new AngleObj(angles[i].angle + small, angles[i].castingObject));
                //angles.Add(angles[i] - small);
                angles[i].angle -= small;
            }

            float maxDist = 2000;
            Empty empty = new Empty(maxDist);
            empty.RelAngles(position, angles);
            for (int i = 0; i < angles.Count; i++)
                angles[i].angle = MathHelper.WrapAngle(angles[i].angle);
            angles.Sort();

            //PrepareAngles(ref angles);

            List<Vector2> vertices = new List<Vector2>();
            HashSet<IShadowCastingObject> curObj = new HashSet<IShadowCastingObject>()
            {
                empty,
            };

            foreach (var castObject in castObjects)
            {
                angle = -MathHelper.Pi;
                if (!IfMisses(castObject))
                    curObj.Add(castObject);
            }

            for (int i = 0; i < angles.Count; i++)
            {
                angle = angles[i].angle;
                curObj.RemoveWhere(IfMisses);
                curObj.Add(angles[i].castingObject);

                if (/*Keyboard.GetState().IsKeyDown(Keys.Space) && */curObj.Count > 50)
                {
                    ;
                }

                Vector2 rayDir = Dir(angle);
                List<float> dists = new List<float>();
                float minDist = maxDist;
                foreach (var castObject in curObj)
                    castObject.InterPoint(position, rayDir, dists);
                foreach (float dist in dists)
                {
                    float d = dist;
                    if (d >= 0 && d < minDist)
                        minDist = d;
                }
                vertices.Add(position + minDist * rayDir);
            }

            if (maxAngleDiff * 2 < MathHelper.TwoPi)
                vertices.Add(position);

            polygon.Update(position, vertices);
        }

        private bool IfMisses(IShadowCastingObject castingObject)
        {
            List<float> dists = new List<float>();
            castingObject.InterPoint(position, Dir(angle), dists);
            return dists.Count == 0;
        }

        private static Vector2 Dir(float angle)
            => new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

        public static void BeginDraw()
        {
            LightPolygon.BeginDraw();
        }

        public void Draw()
        {
            polygon.Draw();
        }

        public static void EndDraw(SpriteBatch spriteBatch)
        {
            LightPolygon.EndDraw(spriteBatch);
        }

        //private void PrepareAngles(ref List<(float angle, IShadowCastingObject castingObject)> angles)
        //{
        //    Empty empty = new Empty();
        //    empty.RelAngles(position, angles);
        //    for (int i = 0; i < angles.Count; i++)
        //        angles[i].angle = MathHelper.WrapAngle(angles[i].angle);
        //    for (int i = 0; i < 4; i++)
        //        angles.Add(i * MathHelper.TwoPi / 4);

        //    for (int i = 0; i < angles.Count; i++)
        //        angles[i] = MathHelper.WrapAngle(angles[i]);

        //    angles.Sort();

        //    //List<(float angle, IShadowCastingObject castingObject)> prepAngles = new List<(float angle, IShadowCastingObject castingObject)>();
        //    //foreach (var angle in angles)
        //    //{
        //    //    float prepAngle = MathHelper.WrapAngle(angle - mainAngle);
        //    //    if (Math.Abs(prepAngle) <= maxAngleDiff)
        //    //        prepAngles.Add(prepAngle + mainAngle);
        //    //}
        //    //prepAngles.Add(mainAngle + MathHelper.WrapAngle(maxAngleDiff));
        //    //prepAngles.Add(mainAngle - MathHelper.WrapAngle(maxAngleDiff));
        //    //prepAngles.Sort();

        //    //angles = new List<float>();
        //    //for (int i = 0; i < prepAngles.Count; i++)
        //    //{
        //    //    if (i == 0 || prepAngles[i - 1] != prepAngles[i])
        //    //        angles.Add(prepAngles[i]);
        //    //}
        //}
    }
}

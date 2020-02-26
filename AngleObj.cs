using System;

namespace Game1
{
    public class AngleObj : IComparable
    {
        public float angle;
        public IShadowCastingObject castingObject;

        public AngleObj(float angle, IShadowCastingObject castingObject)
        {
            this.angle = angle;
            this.castingObject = castingObject;
        }

        public int CompareTo(object obj)
        {
            AngleObj other = obj as AngleObj;
            return angle.CompareTo(other.angle);
        }
    }
}

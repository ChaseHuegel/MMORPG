namespace Mmorpg.Data
{
    public class Entity
    {
        public int ID;

        public float X;

        public float Y;

        public float Z;

        public float Heading;

        public float Size;

        public string Name;

        public string Label;

        public string Title;

        public string Description;

        public virtual void Tick(float deltaTime) {}
    }
}

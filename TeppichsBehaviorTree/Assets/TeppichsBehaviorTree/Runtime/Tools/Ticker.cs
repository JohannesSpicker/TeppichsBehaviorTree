namespace Tools
{
    public class Ticker
    {
        private readonly float duration;
        private          float counter;

        public Ticker(float duration) { this.duration = duration; }

        public virtual bool Tick(float delta)
        {
            counter += delta;

            return duration <= counter;
        }

        public void Reset() => counter = 0;
    }
}
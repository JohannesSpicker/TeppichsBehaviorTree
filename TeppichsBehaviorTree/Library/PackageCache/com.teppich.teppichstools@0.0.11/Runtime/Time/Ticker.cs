namespace TeppichsTools.Time
{
    public class Ticker
    {
        private float counter;
        private float duration;

        public Ticker(float duration) { this.duration = duration; }

        public float Remaining => duration - counter;

        public virtual bool Tick(float delta)
        {
            counter += delta;

            return duration <= counter;
        }

        public void Reset() => counter = 0;

        public void ChangeDuration(float newDuration) => duration = newDuration;
    }
}
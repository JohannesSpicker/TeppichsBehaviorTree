using System;

namespace TeppichsTools.Behavior
{
    public class Observable<T>
    {
        private T value;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke(Value);
            }
        }

        private event Action<T> OnValueChanged;

        public void Subscribe(Action<T>   observer) => OnValueChanged += observer;
        public void Unsubscribe(Action<T> observer) => OnValueChanged -= observer;
    }
}
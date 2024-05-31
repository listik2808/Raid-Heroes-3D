using System;
using System.Diagnostics;
using TMPro;

namespace Assets.Scripts.Economics
{
    public abstract class Currency
    {
        public float Count;
        public float AllCount;
        public event Action OnValueChanged;
        public event Action<float> ShowValue;
        public event Action<float> Value;
        public virtual void Add(float value)
        {
            if (IsCorrect(value) == false) return;
            Count += value;
            AllCount += value;
            Value?.Invoke(value);
           OnValueChanged?.Invoke();
            ShowValue?.Invoke(Count);
        }

        public virtual bool IsCorrect(float value)
        {
            return value > 0;
        }

        public virtual void Reduce(float value)
        {
            if (IsCorrect(value) == false) return;
            Count -= value;
            OnValueChanged?.Invoke();
            ShowValue?.Invoke(Count);
        }
    }
}

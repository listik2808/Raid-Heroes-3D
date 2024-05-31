using Scripts.Enemy;
using Scripts.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private EnemyHeaith _health;
    [SerializeField] private Slider _sliderHp;

    public Slider SliderHp => _sliderHp;

    private void OnEnable()
    {
        _health.HealthChanged += View;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= View;
    }

    public void View()
    {
        _sliderHp.value = _health.Current / _health.Max;
    }
}

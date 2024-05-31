using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class BattleSpeed : MonoBehaviour
{
    [SerializeField] private List<Image> _speedIcon = new List<Image>();
    [SerializeField] private AudioSource _audioSource;
    private float[] _speeds = new float[] { 0.5f, 1f, 2f };
    private int _currentIndex;
    private float _currentSpeed = 1f;

    public float CurrentSpeed => _currentSpeed;

    private void Start()
    {
        NextSpeed();
    }

    public void NextSpeed()
    {
        foreach (var icon in _speedIcon)
        {
            icon.gameObject.SetActive(false);
        }
        _currentIndex++;
        _currentIndex %= _speeds.Length;
        Time.timeScale = _speeds[_currentIndex];
        _speedIcon[_currentIndex].gameObject.SetActive(true);
        _currentSpeed = _speeds[_currentIndex];
    }

    public void PlaySoundClik()
    {
        _audioSource.Play();
    }
}

using System;
using UnityEngine;

public class ShowPopup : MonoBehaviour
{
    [SerializeField] private GameObject[] _gameObjectsToActivate;
    [SerializeField] private GameObject[] _gameObjectsToDeactivate;
    public event Action OnShow;
    public event Action OnHide;

    private void Start()
    {
        Deactivate();
    }

    public void Activate()
    {
        SetActive(true);
        OnShow?.Invoke();
    }

    public void Deactivate()
    {
        SetActive(false);
        OnHide?.Invoke();
    }

    private void SetActive(bool active)
    {
        foreach (var go in _gameObjectsToActivate)
        {
            go.SetActive(active);
        }

        foreach (var go in _gameObjectsToDeactivate)
        {
            go.SetActive(!active);
        }
    }
}

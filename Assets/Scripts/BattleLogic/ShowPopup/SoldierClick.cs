using Scripts.Army.TypesSoldiers;
using System;
using UnityEngine;

public class SoldierClick : MonoBehaviour
{
    private Camera _camera;
    private const string LayerHero = "Hero";
    private const string LayerEnemy = "Enemy";
    private const string LayerUI = "UI";
    private Soldier _soldier;
    private Canvas _canvas;


    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectPart();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DeselectPart();
        }
    }


    private void SelectPart()
    {
        RaycastHit hit;
        Ray cameraRay = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out hit, 1000f, LayerMask.GetMask(LayerHero, LayerEnemy, LayerUI)))
        {
            var newSoldier = hit.collider.GetComponent<Soldier>();
            _canvas = hit.collider.GetComponent<Canvas>();
            if (_soldier != newSoldier && newSoldier != null)
            {
                HidePopup();
                _soldier = newSoldier;
            }
        }
        else
        {
            HidePopup();
            _soldier = null;
        }
    }

    private void DeselectPart()
    {
        if (_soldier == null) return;

        ShowPopup();
    }
    private void ShowPopup()
    {
        _soldier?.GetComponent<ShowPopup>().Activate();
    }

    private void HidePopup()
    {
        _soldier?.GetComponent<ShowPopup>().Deactivate();
    }
}

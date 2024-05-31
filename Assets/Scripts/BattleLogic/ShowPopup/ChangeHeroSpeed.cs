using Assets.Scripts.BattleLogic.StateMachine;
using Scripts.Army.TypesSoldiers;
using Scripts.Data;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//located on the player
public class ChangeHeroSpeed : MonoBehaviour, ISavedProgress
{
    [SerializeField] private NavMeshAgent _navMesh;
    [SerializeField] private TMP_Text _textSpeed;
    [SerializeField] private Soldier _soldier;
    [SerializeField] private Slider _slider;
    [SerializeField] private AIAgentConfig _agentConfig;

    private float _startSpeed;
    private IPersistenProgressService _progressService;

    private void Start()
    {
        _progressService = AllServices.Container.Single<IPersistenProgressService>();
        if (SceneManager.GetActiveScene().name != "SandBox")
        {
            LoadProgress(_progressService.Progress);
        }
        else
        {
            _startSpeed = _agentConfig.MaxSpeed;
            // _navMesh.speed = _startSpeed;
            _slider.value = _navMesh.speed / _startSpeed;
            //float res = (float)Math.Round((double)_navMesh.speed, 1);
            _textSpeed.text = _navMesh.speed.ToString();
        }
    }

    public void Change(float speed)
    {
        //float result = speed * _startSpeed;
        //result = (float)Math.Round((double)result, 1);
        _navMesh.speed = speed * _startSpeed;
        _navMesh.speed = (float)Math.Round((double)_navMesh.speed, 1);
        UpdateProgress(_progressService.Progress);
        ShowSpeed();
    }

    private void ShowSpeed()
    {
        float result = (float)Math.Round((double)_navMesh.speed, 1);
        _slider.value = _navMesh.speed / _startSpeed;
        _textSpeed.text = result.ToString();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        foreach (var item in progress.PlayerData.TypeHero.AllHerosType.ListTypsHeros)
        {
            if(_soldier.HeroTypeId == item.TypeId)
            {
                item.ValueSpeed = _navMesh.speed;
            }
        }
    }

    public void LoadProgress(PlayerProgress progress)
    {
        _navMesh.speed = _agentConfig.MaxSpeed;
        _startSpeed = _navMesh.speed;
        foreach (var item in progress.PlayerData.TypeHero.AllHerosType.ListTypsHeros)
        {
            if (_soldier.HeroTypeId == item.TypeId && item.ValueSpeed >=0)
            {
                _startSpeed = _navMesh.speed;
                _navMesh.speed = item.ValueSpeed;
                _slider.value = _navMesh.speed / _startSpeed;
            }
        }
        ShowSpeed();
    }
}

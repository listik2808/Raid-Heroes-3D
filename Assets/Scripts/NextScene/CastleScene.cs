using Scripts.Economics.Buildings;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.States;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Logic.CastleConstruction;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Scripts.NextScene
{
    public class CastleScene : MonoBehaviour
    {
        private const string SceneCastle = "CastleProtection";

        [SerializeField] private BookmarkButton _button;
        [SerializeField] private BuildingWindow _buildingWindow;
        [SerializeField] private List<ButtonOpenBulletinBoard> _buttonOpenBulletinBoards = new List<ButtonOpenBulletinBoard>();
        [SerializeField] private List<ConstructionCastle> _constructionCastles = new List<ConstructionCastle>();
        private int _numberScene = 1;
        private int _maxCountCastle;
        private IGameStateMachine _stateMachine;

        private void OnEnable()
        {
            _button.ButtonOnClic += LoadSceneCastle;
            foreach (ButtonOpenBulletinBoard button in _buttonOpenBulletinBoards)
            {
                button.ClicButtoncastle += OpenCanvasCastle;
            }
        }

        private void OnDisable()
        {
            _button.ButtonOnClic -= LoadSceneCastle;
            foreach (ButtonOpenBulletinBoard button in _buttonOpenBulletinBoards)
            {
                button.ClicButtoncastle -= OpenCanvasCastle;
            }
        }

        private void Awake()
        {
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
            _maxCountCastle = _constructionCastles.Count;
        }

        public void SetNextCastleLeft(ConstructionCastle constructionCastle)
        {
            int index;
            index = IndexSearch(constructionCastle);
            if (index - 1 >= 0)
            {
                index--;
                _buildingWindow.SetComponent(_constructionCastles[index]);
            }
            else
            {
                index = _maxCountCastle - 1;
                _buildingWindow.SetComponent(_constructionCastles[index]);
            }
        }

        public void SetNextCastleRight(ConstructionCastle constructionCastle)
        {
            int index;
            index = IndexSearch(constructionCastle);
            if (index + 1 <= _maxCountCastle -1)
            {
                index++;
                _buildingWindow.SetComponent(_constructionCastles[index]);
            }
            else
            {
                index = 0;
                _buildingWindow.SetComponent(_constructionCastles[index]);
            }
        }

        private int IndexSearch(ConstructionCastle constructionCastle)
        {
            for (int i = 0; i < _constructionCastles.Count; i++)
            {
                if (_constructionCastles[i].Building == constructionCastle.Building)
                {
                    return i;
                }
            }

            return 0;
        }

        private void LoadSceneCastle()
        {
            _stateMachine.Enter<LoadLevelStateCastle, string, int>(SceneCastle, _numberScene);
        }

        private void OpenCanvasCastle(ConstructionCastle constructionCastle)
        {
            _buildingWindow.SetComponent(constructionCastle,this);
            _buildingWindow.gameObject.SetActive(true);
        } 
    }
}

using Scripts.Data;
using Scripts.Infrastructure.Player;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.Screens;
using Scripts.Logic;
using Scripts.Logic.EnemySpawners;
using Scripts.RaidScreen;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Scripts.CameraMuve
{
    public class CameraMovement : MonoBehaviour
    {
        protected int CameraMove = Animator.StringToHash("Move");

        [SerializeField] private ListEnemyUnits _listEnemyUnits;
        [SerializeField] private ScreenHeroes _screenHeroes;
        private List<SpawnMarker> _spawnMarkerId;
        private Animator _animator;
        private Camera _camera;
        //private float _yPoint;
        private float _zPoint;

        private void OnEnable()
        {
            _listEnemyUnits.ChangedPosition += SetPositionCamera;
        }

        private void OnDisable()
        {
            _listEnemyUnits.ChangedPosition -= SetPositionCamera;
        }


        public void MoveCamera()
        {
            _animator = _camera.GetComponent<Animator>();
            _animator.enabled = true;
            _animator.SetTrigger(CameraMove);
        }

        private void SetPositionCamera(List<SpawnMarker> uniqueIds,int value)
        {
            _camera = Camera.main;
            //_yPoint = _camera.transform.position.y;
            _zPoint = _camera.transform.position.z;
            _spawnMarkerId = uniqueIds;

            foreach (SpawnMarker unit in _spawnMarkerId)
            {
                if (value > 1 &&  unit.Id == value)
                {
                    _camera.transform.position = new Vector3(unit.transform.position.x - 3.5f, 1.531f, _zPoint);
                    break;
                }
                else if (YandexGame.savesData.SelectionWindow && value == 1)
                {
                    _camera.transform.position = new Vector3(-5.32f, 1.531f, -5.52f);
                    break;
                }
            }
        }
    }
}

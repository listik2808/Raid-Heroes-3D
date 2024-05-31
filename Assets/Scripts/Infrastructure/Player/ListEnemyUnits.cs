using Scripts.Data;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Factory;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using Scripts.Logic;
using Scripts.Logic.EnemySpawners;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Infrastructure.Player
{
    public class ListEnemyUnits : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private MainScreen _mainScreen;
        private IGameFactory _gameFactory;
        private List<SpawnPoint> _enemySpawns = new List<SpawnPoint>();
        private int _idSpawner;

        public List<SpawnPoint> SpawnPoints => _enemySpawns;
        public int IdSpawner => _idSpawner;

        public event Action<int> NumberBattlesChanged;
        public event Action<List <SpawnMarker>,int> ChangedPosition;

        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (SceneManager.GetActiveScene().name == AssetPath.SceneMain)
            {
                if (progress.PointSpawn.Reset == false)
                {
                    progress.PointSpawn.IdSpawnerEnemy = _idSpawner;
                    progress.PointSpawn.SetBestIdEnemy(_idSpawner);
                }
            }
        }

        public void LoadProgress(PlayerProgress progress)
        {
            progress.PointSpawn.Reset = false;
            _idSpawner = progress.PointSpawn.IdSpawnerEnemy;
            NumberBattlesChanged?.Invoke(_idSpawner);
        }

        private void Start()
        {
            if (_mainScreen.Diactivate == false)
                TryGetId();
            else
            {
                ChangedPosition?.Invoke(_mainScreen.RaidObject.SpawnMarkers, _idSpawner);
            }
        }

        public void SetResetIdSpawner(int value)
        {
            _idSpawner = value;
           ClearRaidZone();
            ClearSpawners();
        }

        private void ClearSpawners()
        {
            foreach (var item in _mainScreen.RaidZones.ListRaids)
            {
                foreach (var spawn in item.SpawnMarkers)
                {
                    spawn.Slain = false;
                }
            }
        }

        private void ClearRaidZone()
        {
            foreach (var item in _mainScreen.RaidZones.ListRaids)
            {
                if (item.Passed)
                {
                    item.Slay(false);
                }
            }
        }

        private void TryGetId()
        {
            SetListSpawnPoint();
            NumberBattlesChanged?.Invoke(_idSpawner);
            ChangedPosition?.Invoke(_mainScreen.RaidObject.SpawnMarkers, _idSpawner);
            //SetArmy();
        }

        private void SetListSpawnPoint()
        {
            for (int i = 0; i < _gameFactory.SpawnPoints.Count; i++)
            {
                if (_gameFactory.SpawnPoints[i]._slain == false)
                {
                    _enemySpawns.Add(_gameFactory.SpawnPoints[i]);
                }
            }
        }

        private void SetArmy()
        {

            foreach (SpawnPoint spawner in _enemySpawns)
            {
                if (spawner._slain == false)
                {
                    _idSpawner = spawner.Id;
                    NumberBattlesChanged?.Invoke(_idSpawner);
                    ChangedPosition?.Invoke(_mainScreen.RaidObject.SpawnMarkers, _idSpawner);
                    break;
                }
            }
        }
    }
}
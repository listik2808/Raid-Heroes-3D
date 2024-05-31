using Scripts.Army.TypesSoldiers;
using Scripts.Data;
using Scripts.Enemy;
using Scripts.Infrastructure.Factory;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using Scripts.StaticData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Logic
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId MonsterTypeId;
        //private EnemyDeath _enemyDeath;
        public int Id { get; set; }
        public bool _slain;
        public int IdRaid;
        private Camera _camera;
        private IGameFactory _gameFactory;
        private MainScreen _mainScreen;

        public void Construct(IGameFactory factory,MainScreen mainScreen)
        {
            _gameFactory = factory;
            _mainScreen = mainScreen;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Count == 0 && Id == 0)
            {
                Spawn(progress);
            }
            else
            {
                if (progress.KillData.CheckingRecordPassing(Id))
                {
                    _slain = true;
                }
                else
                {
                    Spawn(progress);
                }
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_slain && _mainScreen.PersistenProgressService.Progress.PointSpawn.Reset == false)
            {
                if (progress.KillData.ClearedSpawners.Contains(Id))
                {
                    return;
                }
                else
                {
                    progress.KillData.ClearedSpawners.Add(Id);
                }
            }
        }


        public void Spawn(PlayerProgress progress)
        {
            if (_slain ==false && Id == progress.PointSpawn.IdSpawnerEnemy)
            {
                GameObject monster = _gameFactory.CreateModelMonster(MonsterTypeId, transform,Id,IdRaid,_mainScreen);
            }
        }
    }
}
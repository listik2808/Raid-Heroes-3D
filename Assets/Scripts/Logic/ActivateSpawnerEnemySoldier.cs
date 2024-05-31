using Scripts.Army.TypesSoldiers;
using Scripts.Army.TypesSoldiers.CharacteristicsSoldier;
using Scripts.BattleLogic.GameResult;
using Scripts.BattleTactics;
using Scripts.Data;
using Scripts.Infrastructure;
using Scripts.Infrastructure.Factory;
using Scripts.Infrastructure.Services.PersistentProgress;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Logic
{
    public class ActivateSpawnerEnemySoldier : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private List<EnemyCell> _cells = new List<EnemyCell>();
        [SerializeField] private List<Soldier> _soldiers = new List<Soldier>();
        [SerializeField] private Power _power;
        private int _idSolder;
        private int _idRaid;
        private RaidZones _raidZones;
        private int _currentCountSolier = 0;
        private Coroutine _instantiateSoldier;
        private IGameFactory _factory;
        private GameResultsWatcher _watcher;
        private Coroutine _coroutine;
        private float _powerValue;
        private float _maxPower;
        private List<EnemyCell> _enemyCells = new List<EnemyCell>();
        private List<Soldier> _soldierEnemy = new List<Soldier>();
        public List<EnemyCell> EnemyCells => _cells;
        private IPersistenProgressService _progressService;

        public event Action SpawnEnd;

        public void Construct(RaidZones raidZone,IGameFactory gameFactory, GameResultsWatcher resultWotcher,IPersistenProgressService persistenProgressService)
        {
            _raidZones = raidZone;
            _factory = gameFactory;
            _watcher = resultWotcher;
            _progressService = persistenProgressService;
        }

        private void OnEnable()
        {
            _power.Finish += StartSetHpSoldierEnemy;
            //_power.Finish += SetTutorDamage;
        }

        private void OnDisable()
        {
            _power.Finish -= StartSetHpSoldierEnemy;
            //_power.Finish -= SetTutorDamage;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if(progress.PointSpawn.IdRaid == 0)
            {
                progress.PointSpawn.IdRaid = 1;
            }

            _idRaid = progress.PointSpawn.IdRaid;
            _idSolder = progress.PointSpawn.IdSpawnerEnemy;
            if (progress.PointSpawn.NextIdEnemy != 0)
            {
                _idSolder = progress.PointSpawn.NextIdEnemy;
            }
        }

        private void Start()
        {
            GetArmy();
            if (_soldiers != null)
            {
                SpawnEnd += SetPower;
                _currentCountSolier = _soldiers.Count;
                RandomPoint();
            }
        }

        public void RandomPoint()
        {
            if (_instantiateSoldier != null)
            {
                StopCoroutine(_instantiateSoldier);
                _instantiateSoldier = null;
            }

            _instantiateSoldier = StartCoroutine(SoldierSpawn());
        }

        public void SetTutorDamage()
        {
            if (_progressService.Progress.Training.Tutor == false)
            {
                foreach (var item in _soldierEnemy)
                {
                    item.TutorMelldamage(2);
                    item.Agent.TutorDamage();
                }
            }
        }

        private void StartSetHpSoldierEnemy()
        {
            _watcher.SetHpSquadEnemy(_soldierEnemy, _idSolder);
            SetTutorDamage();
        }

        private void SetPower()
        {
            SpawnEnd -= SetPower;
            foreach (var item in _soldierEnemy)
            {
                item.SoldiersStatsLevel.ResetLevel();
                item.BaseData();
            }
            StartCoroutine(_power.UpLevelEnemy(_soldierEnemy, _maxPower));
        }

        private void GetArmy()
        {
            foreach (RaidScreen.RaidsObject zonaRaid in _raidZones.ListRaids)
            {
                if (zonaRaid.Id == _idRaid)
                {
                    zonaRaid.SetMaxId();
                    foreach (EnemySpawners.SpawnMarker soldier in zonaRaid.SpawnMarkers)
                    {
                        if (soldier.Id == _idSolder)
                        {
                            _soldiers = soldier.ArmyEnemySoldiers;
                            _maxPower = _power.GetPower(_idSolder, false);
                        }
                    }
                }
            }
        }

        private IEnumerator SoldierSpawn()
        {
            int index = 0;
            while (_currentCountSolier > 0)
            {
                var point = Random.Range(0, _cells.Count);
                if (_currentCountSolier > 0 && _cells[point].IsBusy == false)
                {
                    Soldier soldier = _cells[point].Spawn(_soldiers[index]);
                    index++;
                    _soldierEnemy.Add(soldier);
                    _currentCountSolier--;
                }
                yield return null;
            }
            SpawnEnd?.Invoke();
        }
    }
}
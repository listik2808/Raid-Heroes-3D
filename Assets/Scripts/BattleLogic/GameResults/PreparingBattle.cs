using Scripts.Data;
using Scripts.Infrastructure;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.NextScene;
using UnityEngine;

namespace Scripts.BattleLogic.GameResult
{
    public class PreparingBattle : MonoBehaviour, ISavedProgress
    {
        RandomChanceCard _randomChanceCard;
        private RaidZones _raidZones;
        private MainStage _mainStage;
        private int _idEnemy;
        private int _nextIdenemy=0;
        private int _idRaid;
        private bool _nextbattle = false;
        private bool _restartLevel = false;
        private bool _reternScenemain = false;
        private bool _isDaed;
        private IPersistenProgressService _progressService;
        private float _price;
        private int _countCard =0;

        public int IdEnemy => _idEnemy;
        public int IdRaid => _idRaid;
        private float _rewardStars = 0;
        public IPersistenProgressService ProgressService => _progressService;
        public RaidZones RaidZones => _raidZones;
        public RandomChanceCard RandomChanceCard => _randomChanceCard;
        public MainStage MainStage => _mainStage;

        public void Construct(RaidZones raidZone,MainStage mainStage,RandomChanceCard randomChanceCard,IPersistenProgressService progressService)
        {
            _raidZones = raidZone;
            _mainStage = mainStage;
            _mainStage.NextIdBattle += SetNextId;
            _mainStage.RestartLevelBattle += RestartLevel;
            _mainStage.ReturnMainScene += ReturnMain;
            _randomChanceCard = randomChanceCard;
            _randomChanceCard.Construct(mainStage);
            _progressService = progressService;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _idEnemy = progress.PointSpawn.IdSpawnerEnemy;
            _idRaid = progress.PointSpawn.IdRaid;

        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_nextbattle)
            {
                progress.PointSpawn.NextIdEnemy = _nextIdenemy;
                progress.PointSpawn.IdSpawnerEnemy = _nextIdenemy;
            }
            if (_reternScenemain)
            {
                progress.PointSpawn.NextIdEnemy = 0;
                progress.WorldData.IsMainScren = false;
            }
            if (_restartLevel)
            {
                progress.PointSpawn.NextIdEnemy = 0;
                progress.WorldData.IsMainScren = true;
            }
            AddReward(progress);
            Time.timeScale = 1f;
            progress.PointSpawn.IdRaid = _idRaid;
            progress.Achievements.AllValueClearTerritoriesAch = _idRaid;
        }

        public int TryGetIdNextFighter(bool value)
        {
            _isDaed = value;
            if (_idEnemy == 10 && _isDaed)
            {
                _progressService.Progress.Training.Tutor = true;
            }
            foreach (var raidZone in _raidZones.ListRaids)
            {
                if(raidZone.Id == _idRaid)
                {
                    raidZone.SetMaxId();
                    if(_idEnemy >= raidZone.MaxId)
                    {
                        raidZone.Slay(true);
                        _idRaid++;
                        _progressService.Progress.PointSpawn.IdRaid = _idRaid;
                        break;
                    }
                }
            }

            if(_progressService.Progress.Training.Tutor == false && _progressService.Progress.Training.CountCard < 5)
            {
                if (_idEnemy == 1 || _idEnemy == 3 || _idEnemy == 5 || _idEnemy == 7 || _idEnemy == 9)
                {
                    _countCard = _randomChanceCard.TryGetCardRandom(_idEnemy, _progressService);
                }
                else
                {
                    _countCard = -1;
                }
            }
            else
            {
                _countCard = _randomChanceCard.TryGetCardRandom(_idEnemy, _progressService);
            }
            _mainStage.CardIssued(_countCard);
            return _countCard;
        }

        public void SetPrice(float price,float rewardStar)
        {
            _price = price;
            _rewardStars = rewardStar;
        }

        private void AddReward(PlayerProgress progress)
        {
            progress.Wallet.Coins.Add(_price);
            progress.Wallet.Stars.Add(_rewardStars);
        }

        private void TrySaveKillSoldier(PlayerProgress progress)
        {
            if (_isDaed)
            {
                progress.KillData.ClearedSpawners.Add(progress.PointSpawn.IdSpawnerEnemy);
                progress.PointSpawn.SetBestIdEnemy(progress.PointSpawn.IdSpawnerEnemy);
                int index = _raidZones.ListRaids.Count;
                _raidZones.ListRaids[index - 1].SetMaxId();
                int maxID = _raidZones.ListRaids[index - 1].MaxId;
                if (maxID >=_idEnemy + 1)
                {
                    progress.PointSpawn.IdSpawnerEnemy = _idEnemy + 1;
                }
            }
        }

        private void SetNextId()
        {
            _nextIdenemy = _idEnemy + 1;
            TrySaveKillSoldier(_progressService.Progress);
            _nextbattle = true;
            _mainStage.NextIdBattle -= SetNextId;
        }

        private void RestartLevel()
        {
            _isDaed = false;
            TrySaveKillSoldier(_progressService.Progress);
            _restartLevel = true;
            _mainStage.RestartLevelBattle -= RestartLevel;
        }

        private void ReturnMain()
        {
            TrySaveKillSoldier(_progressService.Progress);
            _reternScenemain = true;
            _mainStage.ReturnMainScene -= ReturnMain;
        }
    }
}
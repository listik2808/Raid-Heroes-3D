using Scripts.Army.PlayerSquad;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.Player;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Infrastructure.States;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Logic.CastleConstruction;
using Scripts.Music;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Scripts.Infrastructure.UIWindows.UIProgressReid
{
    public class NewRaid : MonoBehaviour
    {
        [SerializeField] private SoundEffectsUi _soundEffectsUi;
        [SerializeField] private SoundMenu _soundMenu;
        [SerializeField] private FightNumber _flightNumber;
        [SerializeField] private TMP_Text _textStarts;
        [SerializeField] private Canvas _canvasReset;
        [SerializeField] private BookmarkButton _rewardStarsAds;
        [SerializeField] private BookmarkButton _buttonReset;
        [SerializeField] private BookmarkButton _buttonCloseReset;
        [SerializeField] private BookmarkButton _buttonOpenReset;
        [SerializeField] private Squad _squad;
        [SerializeField] private Heroes _heroes;
        [SerializeField] private ListEnemyUnits _enemyUnits;
        [SerializeField] private List<ConstructionCastle> _constructionCastles = new List<ConstructionCastle>();
        [SerializeField] private BookmarkButton _buttonAdsReward;
        [SerializeField] private Image _imageReset;
        [SerializeField] private Image _bacgroundImageReward;
        [SerializeField] private TMP_Text _rewardAds;
        [SerializeField] private Button _bacgroundRewardResetScreen;
        private IGameStateMachine _stateMachine;
        private ISaveLoadService _saveLoadService;
        private IPersistenProgressService _progressService;
        private int _numberScene = 1;
        private float _rewardStars;

        private void OnEnable()
        {
            _buttonOpenReset.ButtonOnClic += OpenCanvasReset;
            _buttonCloseReset.ButtonOnClic += CloseReset;
            _buttonReset.ButtonOnClic += Reset;
            _rewardStarsAds.ButtonOnClic += AdsStars;
            YandexGame.RewardVideoEvent += RewardedScene;
        }

        private void OnDisable()
        {
            YandexGame.CloseVideoEvent -= Reset;
            _buttonOpenReset.ButtonOnClic -= OpenCanvasReset;
            _buttonCloseReset.ButtonOnClic -= CloseReset;
            _buttonReset.ButtonOnClic -= Reset;
            _rewardStarsAds.ButtonOnClic -= AdsStars;
            YandexGame.RewardVideoEvent -= RewardedScene;
            _buttonAdsReward.ButtonOnClic -= Reset;
        }

        private void Awake()
        {
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
        }

        private void OpenCanvasReset()
        {
            _rewardStars = GetStarsReset();
            _textStarts.text = _rewardStars.ToString();
            _canvasReset.gameObject.SetActive(true);
        }

        private void CloseReset()
        {
            _canvasReset.gameObject.SetActive(false);
        }

        private void Reset()
        {
            //YandexGame.savesData.pointSpawn
            _progressService.Progress.Achievements.AllNewCountRaid++;
            //List<int>list = new List<int>();
            _progressService.Progress.OptionData.CountRaid += 1;
            _progressService.Progress.Wallet.Stars.Add(_rewardStars);
            _progressService.Progress.KillData.ClearedSpawners.Clear();
            _progressService.Progress.WorldData.IsMainScren = false;
            //_progressService.Progress.KillData.ClearedSpawners = list;
            _progressService.Progress.KillData.ClireRaidZoneId.Clear();
            _progressService.Progress.PointSpawn.IdRaid = 1;
            _progressService.Progress.PointSpawn.IdSpawnerEnemy = 1;
            _progressService.Progress.PointSpawn.NextIdEnemy = 0;
            _progressService.Progress.PointSpawn.Reset = true;
            _enemyUnits.SetResetIdSpawner(1);
            _progressService.Progress.WorldData.PositionOnLevel.NumberRaid = 1;
            ResetHerosLevel();
            ResetSquadLevel();
            ResetLevelConstructionCastle();
            //_enemyUnits.ResetSpawn();
            _rewardStars = 0;
            _saveLoadService.SaveProgress();
            LoadScene();
        }

        private float GetStarsReset(int multiplier = 1)
        {
            float value = (float)Math.Round((_flightNumber.CurrentNumber + 0.01 * _flightNumber.CurrentNumber * _flightNumber.CurrentNumber) * multiplier);
            return value;
        }

        private void LoadScene()
        {
            _stateMachine.Enter<LoadLevelState, string, int>(AssetPath.SceneMain, _numberScene);
        }

        private void ResetLevelConstructionCastle()
        {
            foreach (ConstructionCastle item in _constructionCastles)
            {
                if (item.IsOpen)
                {
                    item.Reset();
                }
            }
        }

        private void ResetSquadLevel()
        {
            foreach (Army.TypesSoldiers.Soldier squad in _squad.Soldiers)
            {
                squad.SoldiersStatsLevel.CurrentLevelSpecialSkill = 1;
                squad.SoldiersStatsLevel.CurrentMeleelevel = 1;
                squad.SoldiersStatsLevel.CurrentSurvivabilityLevel = 1;
                squad.SpecialSkillLevelData.ResetStepLevelSkill();
                squad.SurvivabilityLevelData.ResetStepLevelSkill();
                squad.MeleeDamageLevelData.ResetStepLevelSkill();
                foreach (var item in _progressService.Progress.PlayerData.TypeHero.AllHerosType.ListTypsHeros)
                {
                    if (item.TypeId == squad.HeroTypeId)
                    {
                        item.Reset();
                    }
                }

            }
           
        }

        private void ResetHerosLevel()
        {
            foreach (Army.TypesSoldiers.Soldier item in _heroes.Soldiers)
            {
                item.SoldiersStatsLevel.CurrentLevelSpecialSkill = 1;
                item.SoldiersStatsLevel.CurrentMeleelevel = 1;
                item.SoldiersStatsLevel.CurrentSurvivabilityLevel = 1;
                item.SpecialSkillLevelData.ResetStepLevelSkill();
                item.SurvivabilityLevelData.ResetStepLevelSkill();
                item.MeleeDamageLevelData.ResetStepLevelSkill();
            }
        }

        private void AdsStars()
        {
            _soundEffectsUi.PauseEffect();
            _soundMenu.PauseSound();
            YandexGame.RewVideoShow(0);
        }

        public void RewardedScene(int index)
        {
            _soundEffectsUi.PlayEffect();
            _soundMenu.PlaySound();
            if(index == 0)
            {
                YandexGame.timerShowAd = 0;
                _bacgroundRewardResetScreen.enabled = false;
                _imageReset.gameObject.SetActive(false);
                _bacgroundImageReward.gameObject.SetActive(true);
                _rewardStars = GetStarsReset(2);
                _rewardAds.text = _rewardStars.ToString();
                _buttonAdsReward.ButtonOnClic += Reset;
            }
        }
    }
}
using Scripts.Army.TypesSoldiers;
using Scripts.NextScene;
using Scripts.StaticData;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Scripts.BattleLogic.GameResult
{
    public class GameResultsWatcher : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioLose;
        [SerializeField] private AudioSource _audioWin;
        [SerializeField] private TimerBattle _timerBattle;
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Slider _enemyHpSlider;
        [SerializeField] private Slider _playerHpSlider;
        [SerializeField] private TMP_Text _textHpEnemy;
        [SerializeField] private TMP_Text _textIdEnemy;
        [SerializeField] private TMP_Text _textHpHero;
        private List<Soldier> _enemySoldiers = new List<Soldier>();
        private List<Soldier> _heroSoldiers = new List<Soldier>();
        private bool _enemyDead;
        private float _enemyMaxHp =0;
        private float _enemyCurrentHp =0;
        private float _enemyShortHp = 0;
        private float _enemyShortMaxHp = 0;
        private float _heroMaxHp = 0;
        private float _heroCurrentHp =0;
        private float _heroShortHp = 0;
        private float _heroShortMaxHp = 0;
        private int _idEnemy;

        public GameObject Screen => _gameObject;

        public event Action<bool> ChangetEnemyDaed;
       // public event Action OnChangetSlider;

        private void OnEnable()
        {
            HeroEnemyList.OnEnemyRemove += OnEnemyRemove;
            HeroEnemyList.OnHeroRemove += OnHeroRemove;
        }

        private void OnDisable()
        {
            HeroEnemyList.OnHeroRemove -= OnHeroRemove;
            HeroEnemyList.OnEnemyRemove -= OnEnemyRemove;
            foreach (var item in _enemySoldiers)
            {
                item.DataSoldier.EnemyHealth.OnHit -= SetCurentHpEnemy;
            }
            foreach (var item in _heroSoldiers)
            {
                item.DataSoldier.EnemyHealth.OnHit -= SetCurrentHpSquadHero;
            }
        }

        public void SetSquadHpHero(List<Soldier> soldiers)
        {
            _heroSoldiers = soldiers;
            float resultMaxHpHero = 0;
            float resultCurrentHpHero = 0;
            foreach (Soldier item in _heroSoldiers)
            {
                item.DataSoldier.EnemyHealth.OnHit += SetCurrentHpSquadHero;
                item.SetCharacteristics();
                resultMaxHpHero += item.DataSoldier.EnemyHealth.Max;
                resultCurrentHpHero += item.DataSoldier.EnemyHealth.Current;
            }
            _heroCurrentHp = resultCurrentHpHero;
            _heroMaxHp = resultMaxHpHero;
            string resMax = AbbreviationsNumbers.ShortNumber(resultMaxHpHero);
            _heroShortMaxHp = AbbreviationsNumbers.Value;
            string resCurrent = AbbreviationsNumbers.ShortNumber(resultCurrentHpHero);
            _heroShortHp = AbbreviationsNumbers.Value;
            _playerHpSlider.value = _heroShortHp / _heroShortMaxHp;
            _textHpHero.text = resCurrent + "/" + resMax;
        }

        public void SetHpSquadEnemy(List<Soldier> soldierEnemy,int id)
        {
            float resultMaxHpEnemy = 0;
            float resultCurrenthpEnemy = 0;
            _idEnemy = id;
            _textIdEnemy.text = "#" + _idEnemy.ToString();
            _enemySoldiers = soldierEnemy;
            foreach (Soldier item in _enemySoldiers)
            {
                item.DataSoldier.EnemyHealth.OnHit += SetCurentHpEnemy;
                resultMaxHpEnemy += item.DataSoldier.EnemyHealth.Max;
                resultCurrenthpEnemy += item.DataSoldier.EnemyHealth.Current;
            }
            _enemyMaxHp = resultMaxHpEnemy;
            _enemyCurrentHp = resultCurrenthpEnemy;
            string enemyMax = AbbreviationsNumbers.ShortNumber(resultMaxHpEnemy);
            _enemyShortMaxHp = AbbreviationsNumbers.Value;
            string enemyCurrent = AbbreviationsNumbers.ShortNumber(resultCurrenthpEnemy);
            _enemyShortHp = AbbreviationsNumbers.Value;
            _enemyHpSlider.value = _enemyShortHp / _enemyShortMaxHp;
            _textHpEnemy.text = enemyCurrent + "/" + enemyMax;
        }

        public void GameOver()
        {
            foreach (var item in _heroSoldiers)
            {
                item.DataSoldier.EnemyHealth.Damage(9999999999999999);
            }
            HeroEnemyList.Heroes.Clear();
            OnHeroRemove();
        }

        public void ShowResults()
        {
            _timerBattle.enabled = false;
            if (_enemyDead)
            {
                if(YandexGame.savesData.IsSoundEffects)
                    _audioWin.Play();
            }
            else
            {
                if(YandexGame.savesData.IsSoundEffects)
                    _audioLose.Play();
            }
            _gameObject.SetActive(true);
            InvisibleComponents();
        }

        private void SetCurrentHpSquadHero(float damage)
        {
            _heroCurrentHp -= damage;
            string resHeroMax = AbbreviationsNumbers.ShortNumber(_heroMaxHp);
            _heroShortMaxHp = AbbreviationsNumbers.Value;
            if ( _heroCurrentHp < 0 )
                _heroCurrentHp = 0;
            string resHeroHp = AbbreviationsNumbers.ShortNumber(_heroCurrentHp);
            _heroShortHp = AbbreviationsNumbers.Value;
            _playerHpSlider.value = _heroShortHp / _heroShortMaxHp;
            _textHpHero.text = resHeroHp + "/" + resHeroMax;
        }

        private void InvisibleComponents()
        {
            _enemyHpSlider.gameObject.SetActive(false);
            _playerHpSlider.gameObject.SetActive(false);
        }

        private void SetCurentHpEnemy(float damage)
        {
            _enemyCurrentHp -= damage;
            string resMax = AbbreviationsNumbers.ShortNumber(_enemyMaxHp);
            _enemyShortMaxHp = AbbreviationsNumbers.Value;
            if(_enemyCurrentHp < 0)
                _enemyCurrentHp = 0;

            string res = AbbreviationsNumbers.ShortNumber(_enemyCurrentHp);
            _enemyShortHp = AbbreviationsNumbers.Value;
            _enemyHpSlider.value = _enemyShortHp / _enemyShortMaxHp;
            _textHpEnemy.text = res + "/" + resMax;
        }

        private void OnEnemyRemove()
        {
            if (AllSoldiersDied(HeroEnemyList.Enemies))
            {
                _timerBattle.enabled = false;
                //Invoke("ShowResults",1.5f);
                //ShowResults();
                _enemyDead = true;
                ChangetEnemyDaed?.Invoke(_enemyDead);
            }
            //_saveLoadService.SaveProgress();
        }

        private void OnHeroRemove()
        {
            if (AllSoldiersDied(HeroEnemyList.Heroes))
            {
                _timerBattle.enabled = false;
                //Invoke("ShowResults", 1.5f);
                //ShowResults();

                _enemyDead = false;
                ChangetEnemyDaed?.Invoke(_enemyDead);
            }
            //_saveLoadService.SaveProgress();
        }

        private bool AllSoldiersDied(List<Soldier> soldiers)
        {
            return soldiers.Count <= 0;
        }

        private void HideResults()
        {
            _gameObject.SetActive(false);
        }
    }
}
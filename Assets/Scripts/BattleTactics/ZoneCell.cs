using Scripts.Army.AllCadsHeroes;
using Scripts.Army.TypesSoldiers;
using Scripts.BattleLogic.GameResult;
using Scripts.Data;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using Scripts.StaticData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BattleTactics
{
    public class ZoneCell : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private List<PlayerCell> _playerCells = new List<PlayerCell>();
        private int _currentCountSoldier = 0;
        private List<Soldier> _playerSoldier = new List<Soldier>();
        private List<HeroTypeId> _saveProgressSoldiers = new List<HeroTypeId>();
        private List<int> _id = new List<int>();
        private Coroutine _instantiateSoldier;
        private GameResultsWatcher _watcher;
        private List<Soldier> _heroes = new List<Soldier>();
        private HerosCards _herosCards;
        public List<PlayerCell> PlayerCells => _playerCells;

        public void Constructor(GameResultsWatcher gameResultsWatcher,HerosCards herosCards)
        {
            _watcher = gameResultsWatcher;
            _herosCards = herosCards;
        }

        private void Awake()
        {
            int id = 0;
            AssigningNumber(id);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            _id.Clear();
            _saveProgressSoldiers.Clear();
            SetProgress(progress);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            LoadLists(progress);
            if (_saveProgressSoldiers.Count > 0)
                Load(progress);
            if (_currentCountSoldier > 0)
            {
                StartSpawnCorutine(_playerSoldier);
            }
            _watcher.SetSquadHpHero(_heroes);
        }

        public void LoadLists(PlayerProgress progress)
        {
            //_playerSoldier = progress.SquadPlayer.SoldiersPlayer;
            SetArmySoldier(progress);
            _saveProgressSoldiers = progress.PlayerCellData.FieldSoldiers;
            _id = progress.PlayerCellData.Id;
            //_currentCountSoldier = _playerSoldier.Count;
            _currentCountSoldier = progress.SquadPlayer.HeroTypeIds.Count;
            foreach (Soldier soldier in _playerSoldier)
            {
                soldier.DiactivateInstalled();
            }
        }

        private void SetArmySoldier(PlayerProgress progress)
        {
            foreach (var item in progress.SquadPlayer.HeroTypeIds)
            {
                foreach (var cardAll in _herosCards.SoldierСardsAll)
                {
                    if(cardAll.SoldierCardViewer.Card.Soldier.HeroTypeId == item)
                    {
                        _playerSoldier.Add(cardAll.SoldierCardViewer.Card.Soldier);
                    }
                }
            }
        }

        private void Load(PlayerProgress progress)
        {
            SoldierLoad();
        }

        private void SoldierLoad()
        {
            for (int i = 0; i < _playerSoldier.Count; i++)
            {
                PlayerCell cell = CheckForSimilarities(_playerSoldier[i]);
                if (cell != null)
                {
                    _heroes.Add(cell.Spawn(_playerSoldier[i]));
                    _playerSoldier[i].SetInstalled();
                    _currentCountSoldier--;
                }
            }
        }

        private PlayerCell CheckForSimilarities(Soldier soldier)
        {
            for (int i = 0; i < _saveProgressSoldiers.Count; i++)
            {
                if (soldier.HeroTypeId == _saveProgressSoldiers[i])
                {
                    PlayerCell cell = CellIdSet(i);
                    return cell;
                }
            }
            return null;
        }

        private PlayerCell CellIdSet(int i)
        {
            for (int j = 0; j < _playerCells.Count; j++)
            {
                if (_playerCells[j].ID == _id[i])
                {
                    return _playerCells[j];
                }
            }
            return null;
        }

        private void StartSpawnCorutine(List<Soldier> soldiers)
        {
            if (_instantiateSoldier != null)
                _instantiateSoldier = null;
            _instantiateSoldier = StartCoroutine(SpawnSoldier(soldiers));
        }

        private IEnumerator SpawnSoldier(List<Soldier> soldiers)
        {
            while (_currentCountSoldier > 0)
            {
                int point = Random.Range(0, _playerCells.Count);
                for (int i = 0; i < soldiers.Count; i++)
                {
                    if (_playerCells[point].IsBusy == false && soldiers[i].InstalledInCell == false)
                    {
                        _heroes.Add(_playerCells[point].Spawn(soldiers[i]));
                        soldiers[i].SetInstalled();
                        _currentCountSoldier--;
                    }
                }
                yield return null;
            }
        }

        private void AssigningNumber(int id)
        {
            foreach (PlayerCell cell in _playerCells)
            {
                id++;
                cell.SetId(id);
            }

        }

        private void SetProgress(PlayerProgress progress)
        {
            foreach (PlayerCell cell in _playerCells)
            {
                if (cell.IsBusy)
                {
                    _id.Add(cell.ID);
                    _saveProgressSoldiers.Add(cell.Soldier.HeroTypeId);
                    cell.Soldier.DiactivateInstalled();
                }
            }
            progress.PlayerCellData.Id = _id;
            progress.PlayerCellData.FieldSoldiers = _saveProgressSoldiers;
        }
    }
}
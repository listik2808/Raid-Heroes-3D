using Scripts.Army.PlayerSquad;
using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using Scripts.Logic;
using Scripts.StaticData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Army
{
    public class TroopsEnemy : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private Canvas _canvas;

        private List<CameraParentEnemy> _cameraParentEnemies = new List<CameraParentEnemy>();
       // private MainScreen _mainScreen;
        private List<Soldier> _soldiers = new List<Soldier>();
        private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();
        private int _id;
        private int _idRaid;
        private float _goldReward;
        private int _starsReward;
       // private Camera _camera;

        public List<CameraParentEnemy> CameraParentEnemies => _cameraParentEnemies;
        public List<SpawnPoint> SpawnPoints => _spawnPoints;
        public List<Soldier> Soldiers => _soldiers;
        public Transform Container => _container;
        public int StarsReward => _starsReward;
       // public MainScreen MainScreen => _mainScreen;
        public float GoldReward => _goldReward;

        public int Id => _id;

        private void Start()
        {
            _goldReward = CountingRewards.GetPveRewardGold(_id);
            if (_idRaid != 0 && _id % 10 == 0)
            {
                _starsReward = _id / 10;
            }
        }

        public void AddSolder(List<Soldier> soldiers, int id, int idRaid,List<SpawnPoint> spaws,MainScreen mainScreen,List<CameraParentEnemy> cameraParentEnemies)
        {
            _cameraParentEnemies = cameraParentEnemies;
            _spawnPoints = spaws;
            _soldiers = soldiers;
            _id = id;
            _idRaid = idRaid;
        }
    }
}

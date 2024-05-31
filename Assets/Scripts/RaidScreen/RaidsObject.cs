using Scripts.CameraMuve;
using Scripts.Data;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Logic;
using Scripts.Logic.EnemySpawners;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.RaidScreen
{
    public class RaidsObject : MonoBehaviour
    {
        [SerializeField] private List<SpawnMarker> _spawnMarkers = new List<SpawnMarker>();
        [SerializeField] private int _id;

        private int _maxId;
        private bool _passed = false;

        public int Id => _id;
        public int MaxId => _maxId;
        public List<SpawnMarker > SpawnMarkers => _spawnMarkers;
        public bool Passed => _passed;


        private void Start()
        {
            SetMaxId();
        }

        public void SetMaxId()
        {
            int count = _spawnMarkers.Count;
            count -= 1;
            _maxId = _spawnMarkers[count].Id;
        }

        public void Slay(bool value)
        {
            _passed = value;
        }
    }
}

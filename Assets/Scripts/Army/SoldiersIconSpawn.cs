using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.StaticData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Army
{
    public class SoldiersIconSpawn : MonoBehaviour
    {
        [SerializeField] private TroopsEnemy _troops;
        [SerializeField] private BookmarkButton _bookmarkButton;
        [SerializeField] private ImageSoldierStats _soldierStats;
        [SerializeField] private AudioSource _audioSource;
        private List<ImageSoldierStats> _enemy = new List<ImageSoldierStats>();
        public List<ImageSoldierStats> Enemy => _enemy;

        private void OnEnable()
        {
            _bookmarkButton.ButtonOnClic += CloseScreen;
        }

        private void OnDisable()
        {
            _bookmarkButton.ButtonOnClic -= CloseScreen;
            //foreach (var item in _enemy)
            //{
            //    item.OpenStatsSoldier -= CloseStats;
            //}
        }

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            int index = 0;
            int _countTroopsSoldier = _troops.Soldiers.Count;
            while (_countTroopsSoldier != index)
            {
                ImageSoldierStats enemy = Instantiate(_soldierStats, _troops.Container);
                enemy.OpenStatsSoldier += CloseStats;
                enemy.SetSpriteEnemy(_troops.Soldiers[index], index, _troops.CameraParentEnemies, _audioSource);
                _enemy.Add(enemy);
                index++;
                yield return null;
            }
        }

        private void CloseScreen()
        {
            foreach (var item in _enemy)
            {
                item.Deactivate();
            }
        }

        private void CloseStats(int number)
        {
            foreach (var item in _enemy)
            {
                if (item.Number != number)
                {
                    item.Deactivate();
                }
            }
        }
    }
}

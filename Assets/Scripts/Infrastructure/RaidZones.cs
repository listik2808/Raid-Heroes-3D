using Scripts.Data;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using Scripts.Logic;
using Scripts.RaidScreen;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Infrastructure
{
    public class RaidZones : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private List<RaidsObject> _raidsList = new List<RaidsObject>();
        [SerializeField] private List<CameraParentEnemy> _cameraParentEnemies = new List<CameraParentEnemy>();

        public List<CameraParentEnemy> CameraParentEnemies => _cameraParentEnemies;

        public List<RaidsObject> ListRaids => _raidsList;

        public void LoadProgress(PlayerProgress progress)
        {
            foreach (RaidsObject item in _raidsList)
            {
                if (progress.KillData.SetClireZoneRaidId(item.Id))
                {
                    item.Slay(true);
                }
                //if (progress.KillData.ClireRaidZoneId.Contains(item.Id))
                //{
                    
                //    item.Slay(true);
                //}

                foreach (var item2 in item.SpawnMarkers)
                {
                    if (progress.KillData.CheckingRecordPassing(item2.Id))
                    {
                        item2.Slain = true;
                    }

                    //if (progress.KillData.ClearedSpawners.Contains(item2.Id))
                    //{
                    //    item2.Slain = true;
                    //}
                }
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            foreach (RaidsObject item in _raidsList)
            {
                if (item.Passed)
                {
                    if (progress.KillData.SetClireZoneRaidId(item.Id) == false)
                    {
                        progress.KillData.ClireRaidZoneId.Add(item.Id);
                    }
                }
            }
        }
    }
}
using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using Scripts.Logic;
using Scripts.StaticData;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<SpawnPoint> SpawnPoints { get; }
        List<ISavedProgessReader> ProgessReaders { get; }
        List<ISavedProgress> ProgessWriters { get; }
        void Cleanup();
        GameObject CreateHud();
        GameObject CreateHudBattle();
        RaidZones CreateRaidZones();
        GameObject CreateRaid(int number);
        GameObject CreateBattle(int number);
        GameObject CreateModelMonster(MonsterTypeId monsterTypeId, Transform parent,int id,int idRaid,MainScreen mainScreen);
        SpawnPoint CreateSpawner(Vector3 at, int spawnerId, MonsterTypeId monsterTypeId,int idRaid,MainScreen mainScreen);
    }
}
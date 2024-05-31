using Scripts.Army.PlayerSquad;
using Scripts.Army.TypesSoldiers;
using Scripts.Army.TypesSoldiers.TypeCardClass;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Infrastructure.Services;
using UnityEngine;

public enum CardType
{
    Simple = 0, 
    Rare = 1,
    Epic = 2,
}

public class CardViewer : SoldierCardViewer
{
    [SerializeField] private Soldier _hero;
    [SerializeField] private SoldierCard _soldierCard;

    public override void CloseUpgradeSoldier()
    {
        throw new System.NotImplementedException();
    }

    public override void OpenSoldierUpgradeScreen()
    {
        throw new System.NotImplementedException();
    }

    public override void SaveDataHero()
    {
        throw new System.NotImplementedException();
    }

    public override void SetComponent()
    {
        throw new System.NotImplementedException();
    }

    public override void Start()
    {
        SetbaseComponent();
        _hero = Card.Soldier;
    }

    public override void SetComponentSevices()
    {
        //if (_progressService == null)
        //    _progressService = AllServices.Container.Single<IPersistenProgressService>();
        //if (_saveLoadService == null)
        //    _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
    }
}

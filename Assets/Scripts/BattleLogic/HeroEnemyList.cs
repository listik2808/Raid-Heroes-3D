using Scripts.Army.TypesSoldiers;
using Scripts.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class HeroEnemyList
{
    private static List<Soldier> _heroes = new();
    private static List<Soldier> _enemies = new();
    public static event Action OnHeroRemove;
    public static event Action OnEnemyRemove;

    public static List<Soldier> Heroes => _heroes;
    public static List<Soldier> Enemies => _enemies;

    //calls after hero death
    public static void RemoveHero(Soldier soldier)
    {
        if (_heroes.Remove(soldier))
            OnHeroRemove?.Invoke();
    }

    //calls after enemy death
    public static void RemoveEnemy(Soldier soldier)
    {
        if (_enemies.Remove(soldier))
            OnEnemyRemove?.Invoke();
    }

    public static AIAgentBase FindStrongestAgentWithoutMotivation(HeroType type)
    {
        var collection = type == HeroType.Hero ? _heroes : _enemies;
        var list = collection.Where(x=>x.SpecialAttack != SpecialAttack.Motivation).ToList();

        if(list.Count == 0)
            return null;

        var index = UnityEngine.Random.Range(0, list.Count);

        return list[index].GetComponent<AIAgentBase>();
    }
}

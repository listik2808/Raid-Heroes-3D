using Scripts.Army.TypesSoldiers;
using Scripts.Enemy;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public HeroType Type;
    [SerializeField] private AIAgentBase _agent;
    private float _timer;

    //void Update()
    //{
    //    _timer += Time.deltaTime;
    //    if (_timer > _agent.SpecAttackCooldown)
    //    {
    //        _timer = 0;

    //        if (Type == HeroType.Hero)
    //        {
    //            EnemyHeaith heroToHeal = FindHeroToHeal(HeroEnemyList.Heroes);
    //            if (heroToHeal != null)
    //            {
    //                heroToHeal.Damage(-_agent.SpecDamage); // heal hero
    //                _agent.Animation.AttackSpec();
    //                _agent.ResetTimeSpecAttack();
    //            }
    //        }
    //        else
    //        {
    //            EnemyHeaith enemyToHeal = FindHeroToHeal(HeroEnemyList.Enemies);
    //            if (enemyToHeal != null)
    //            {
    //                enemyToHeal.Damage(-_agent.SpecDamage); // heal enemy
    //                _agent.Animation.AttackSpec();
    //                _agent.ResetTimeSpecAttack();
    //            }
    //        }
    //    }
    //}

    public static EnemyHeaith FindHeroToHeal(List<Soldier> collection)
    {
        EnemyHeaith heroToHeal = null;
        float smallestPercentageHealth = 1f;
        foreach (var hero in collection)
        {
            if (hero.Agent!=null)
            {
                if (hero.Agent.IsDead)
                    continue;
            }

            if (hero.Agent.Health != null)
            {
                var percentage = hero.Agent.Health.Current / hero.Agent.Health.Max;
                if (percentage < smallestPercentageHealth)
                {
                    smallestPercentageHealth = percentage;
                    heroToHeal = hero.Agent.Health;
                }
            }
        }

        return heroToHeal;
    }

}

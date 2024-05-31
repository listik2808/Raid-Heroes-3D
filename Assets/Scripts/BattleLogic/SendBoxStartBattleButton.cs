using Scripts.BattleTactics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SendBoxStartBattleButton : MonoBehaviour
{
    [SerializeField] private ZoneCell zone;
    [SerializeField] private SoldierClick soldierClick;
    public void RestartScene()
    {
        HeroEnemyList.Enemies.Clear();
        HeroEnemyList.Heroes.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Battle()
    {
        soldierClick.enabled = false;
        DiacivateCollider(zone);
        foreach (var item in HeroEnemyList.Enemies)
        {
            var enemy = item.GetComponent<FindTarget>();
            enemy.FindNearestOpponent();
            enemy.Agent.StateMachine.ChangeState(AIStateId.ChasePlayer);
        }
        foreach (var item in HeroEnemyList.Heroes)
        {
            var hero = item.GetComponent<FindTarget>();
            hero.FindNearestOpponent();
            hero.Agent.StateMachine.ChangeState(AIStateId.ChasePlayer);
            hero.gameObject.layer = LayerMask.NameToLayer("HeroBox");
        }
    }
    private void DiacivateCollider(ZoneCell zoneCell)
    {
        foreach (PlayerCell item in zoneCell.PlayerCells)
        {
            item.Collider.enabled = false;
        }
    }
}

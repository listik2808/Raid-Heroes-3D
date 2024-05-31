using Assets.Scripts.BattleLogic.StateMachine.Agents;
using Source.Scripts.Logic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody RigidbodyBullet;
    private ShootAgent _shootAgent;
    private HealAgent _healAgent;
    private int _mask;
    private GameObject _target;
    float _dif;

    public void SetShoot(ShootAgent shootAgent, int mask,GameObject target)
    {
        _target = target;
        _mask = mask;
        _shootAgent = shootAgent;
    }

    public void SetHeal(HealAgent heal)
    {
        _healAgent = heal;
    }

    private void Update()
    {
        if(_target != null)
        {
            transform.forward = RigidbodyBullet.velocity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _mask || other.TryGetComponent(out Wall wall))
        {
            Damage();
        }
    }

    private void Damage()
    {
        if (_shootAgent != null)
            _shootAgent.OnAttack(true);

        Destroy(gameObject);
    }
}

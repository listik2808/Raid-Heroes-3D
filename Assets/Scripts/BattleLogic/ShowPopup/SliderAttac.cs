using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderAttac : MonoBehaviour
{
    [SerializeField] private AIAgentBase _agent;
    [SerializeField] private Image _iconSpecAttack;
    [SerializeField] private Image _iconAttackMeleeDamage;
    private float _elepsedTimeSpecAttack = 0;
    private float _elepsedTimeMeleeDamage = 0;
    private bool _isFillTimeMeleeDamage =false;
    private bool _isFillTimeSpecSkill = false;

    private void Update()
    {
        SetTimeMeleeDamage(_elepsedTimeMeleeDamage);
        SetTimeSpec(_elepsedTimeSpecAttack);
    }

    private void SetTimeSpec(float timeSpec)
    {
        if (_isFillTimeSpecSkill == false && _agent.IsStun == false)
        {
            _elepsedTimeSpecAttack += Time.deltaTime;
            _iconSpecAttack.fillAmount = timeSpec / _agent.SpecAttackCooldown;
            if (_iconSpecAttack.fillAmount == 1)
            {
                _agent.TimeSpecAttac(true);
                _isFillTimeSpecSkill = true;
            }
        }
    }

    private void SetTimeMeleeDamage(float timedamage)
    {
        if (_isFillTimeMeleeDamage == false && _agent.IsStun == false)
        {
            _elepsedTimeMeleeDamage += Time.deltaTime;
            _iconAttackMeleeDamage.fillAmount = timedamage / _agent.AttackCooldown;
            if (_iconAttackMeleeDamage.fillAmount == 1)
            {
                _agent.TimeAttac(true);
                _isFillTimeMeleeDamage = true;
            }
        }
        
    }

    public void ResetTimeMeleeDamageSkill()
    {
        _elepsedTimeMeleeDamage = 0;
        _isFillTimeMeleeDamage = false;
        _agent.TimeAttac(false);
    }

    public void ResetTimeSpecialSkill()
    {
        _elepsedTimeSpecAttack = 0;
        _isFillTimeSpecSkill = false;
        _agent.TimeSpecAttac(false);
    }
}

using System.Collections;
using UnityEngine;

public class Motivation : MonoBehaviour
{
    [HideInInspector] public float TimeLeft;
    private float _storedMultiplayer;

    public void StartMotivation(AIAgentBase agent, float multiplayer, float time)
    {
        _storedMultiplayer = multiplayer;
        agent.BuffAndDebuff.Motivation.gameObject.SetActive(true);
        agent.DamageMultiplayer *= multiplayer;
        agent.Health.DamageMultiplayer *= 1 / multiplayer;
        if (agent.Motivation == null || time > agent.Motivation?.TimeLeft)
        {
            agent.BuffAndDebuff.Motivation.gameObject.SetActive(false);
            agent.Motivation?.StopMotivation(agent);
            agent.Motivation = this;
            agent.Motivation.TimeLeft = time;
            StartCoroutine(CountdownMotivation(agent, time));
        }
    }

    public void StopMotivation(AIAgentBase agent)
    {
        agent.BuffAndDebuff.Motivation.gameObject.SetActive(false);
        StopCoroutine("CountdownMotivation");
    }

    private IEnumerator CountdownMotivation(AIAgentBase agent, float time)
    {
        TimeLeft -= Time.deltaTime;
        agent.BuffAndDebuff.Motivation.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        agent.DamageMultiplayer /= _storedMultiplayer;
        agent.BuffAndDebuff.Motivation.gameObject.SetActive(false);
        agent.Health.DamageMultiplayer *= _storedMultiplayer;
    }
}

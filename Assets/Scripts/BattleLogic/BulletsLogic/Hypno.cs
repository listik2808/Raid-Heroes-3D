using System.Collections;
using UnityEngine;

public class Hypno : MonoBehaviour
{
    [HideInInspector] public float HypnoTime;
    [HideInInspector] public float HypnoTimeLeft;// сколько времени гипноза осталось

    public void StartHypno(AIAgentBase agent, float time)
    {
        bool replaceHypno = true;
        if (time > agent.Hypno.HypnoTimeLeft)
        {
            agent.Hypno.StopHypno(agent);
        }
        else
        {
            replaceHypno = false;
        }

        if (!replaceHypno)
            return;

        if (replaceHypno)
        {
            agent.BuffAndDebuff.Hyposis.gameObject.SetActive(true);
            agent.Hypno = this;
            agent.Hypno.HypnoTime = time;
            agent.UnderHypno = true;
            agent.FindNewOpponent();
            StartCoroutine(UnhypnoAgent(agent, time));
        }
    }

    private void StopHypno(AIAgentBase agent)
    {
        StopCoroutine("UnhypnoAgent");
    }

    private IEnumerator UnhypnoAgent(AIAgentBase agent, float time)
    {
        agent.Hypno.HypnoTimeLeft -= Time.deltaTime;
        yield return new WaitForSeconds(time);

        agent.UnderHypno = false;
        agent.BuffAndDebuff.Hyposis.gameObject.SetActive(false);
        agent.FindNewOpponent();
    }
}

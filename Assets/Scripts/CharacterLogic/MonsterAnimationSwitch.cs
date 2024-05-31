using UnityEngine;

public class MonsterAnimationSwitch : AnimationSwitch
{
    protected int _idle = Animator.StringToHash("Idle");
    public void Idle()
    {
        Animator.SetTrigger(_idle);
    }
}

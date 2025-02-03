using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharactorAnimationEventListener : MonoBehaviour
{
    public UnityEvent startAttackEvent;
    public UnityEvent endAttackAnimationEvent;

    public void OnStartAttack()
    {
        startAttackEvent?.Invoke();
    }

    public void OnEndAttackAnimation()
    {
        endAttackAnimationEvent?.Invoke();
    }
}

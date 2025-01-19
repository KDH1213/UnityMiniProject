using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AttackPattern : MonoBehaviour
{
    public UnityAction attackStartEvent;
    public UnityAction attackEndEvent;
    
    [SerializeField]
    public AttackInfoData attackInfoData;

    public abstract void Excute();
    public abstract void FixedExcute();
}
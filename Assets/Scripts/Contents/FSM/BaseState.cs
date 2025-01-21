using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseState<T> : MonoBehaviour
{
    protected T stateType;
    public T StateType { get { return stateType; } }   

    public UnityEvent enterStateEvent;
    public UnityEvent excuteUpdateStateEvent;
    public UnityEvent excuteFixedUpdateStateEvent;
    public UnityEvent exitStateEvent;

    public abstract void Enter();
    public abstract void ExcuteUpdate();
    public abstract void ExcuteFixedUpdate();

    public abstract void Exit();
}

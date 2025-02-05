using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorFSM : FSMController<CharactorStateType>
{
    [field: SerializeField] 
    public CharactorData CharactorData { get; set; }

    [field: SerializeField] 
    public AttackData AttackData { get; set; }

    [field: SerializeField] 
    public Animator Animator { get; private set; }
    [field: SerializeField] 
    public CharactorProfile CharactorProfile { get; private set; }

    [SerializeField]
    private Transform rendererTransform;

    private void Update()
    {
        StateTable[currentStateType].ExecuteUpdate();
    }

    public void OnFlip()
    {
        var scale = rendererTransform.localScale;
        scale.x *= -1f;
        rendererTransform.localScale = scale;
    }

    public bool IsFlip()
    {
        return rendererTransform.localScale.x > 0f ? false : true;
    }
}

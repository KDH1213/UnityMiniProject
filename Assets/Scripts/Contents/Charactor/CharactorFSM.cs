using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorFSM : FSMController<CharactorStateType>
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharactorProfile CharactorProfile { get; private set; }


    private void Update()
    {
        StateTable[currentStateType].ExcuteUpdate();
    }
}

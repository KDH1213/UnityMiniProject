using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorFSM : FSMController<CharactorFSM>
{
    // private OverlapCollider attackCollider;


    [SerializeField] CharactorProfile charactorProfile;
    [SerializeField] private AttackInfoData attackInfoData;
    [SerializeField] private AttackType attackType;

    [SerializeField] private Vector2 currentPosition;
    [SerializeField] private int targetColliderSize = 10;


    private Collider2D[] targetColliders;

    private void Update()
    {
        
    }

    protected virtual void StartAttack()
    {
        int count = Physics2D.OverlapCircleNonAlloc(currentPosition, charactorProfile.AttackRange, targetColliders , GetLayerMasks.Enemy);

        if (count == 0)
            return;

        var createObject = Instantiate(attackInfoData.AttackObjectPrefab);
        createObject.transform.position = transform.position + attackInfoData.CreateOffsetPos;

        //var dagedObject = createObject.GetComponent<ProjectTile>();
        //dagedObject.TargetShooting(transform, targetTransform, OnAttackTarget);

        // targetTransform.position;
    }
}

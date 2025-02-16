using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorAttackRangeObject : MonoBehaviour
{
    [SerializeField]
    private GameTouchManager gameTouchManager;

    private CharactorTileController targetTileController;
    private CharactorFSM targetCharactor;
    public CharactorTileController TargetTileController { get { return targetTileController; } }

    private Vector3 targetOffsetPosition;
    private bool isMove = false;

    public void OnActiveObject(CharactorTileController charactorTileController)
    {
        if (charactorTileController.CharactorCount == 0)
        {
            OnDisableObject();
            return;
        }

        //if (isMove && targetTileController.CharactorCount != 0 && targetCharactor == targetTileController.CharacterControllers[0])
        //    return;

        isMove = false;
        gameObject.SetActive(true);
        targetTileController = charactorTileController;
        targetCharactor = targetTileController.CharacterControllers[0];

        if(targetCharactor.CurrentStateType == CharactorStateType.Move)
        {
            OnTargetMove();
        }
        SetTargetInfo();
    }

    public void OnDisableObject()
    {
        gameObject.SetActive(false);
    }

    private void SetTargetInfo()
    {
        transform.position = targetTileController.transform.position;
        float attackRange = targetCharactor.CharactorData.RealAttackRange;
        transform.localScale = Vector2.one * attackRange;
    }

    private void Update()
    {
        if(isMove)
        {
            transform.position = targetCharactor.transform.position + targetOffsetPosition;
        }
    }

    public void OnTargetMove()
    {
        isMove = true;

        if (targetCharactor.CharactorData.CharactorClassType == CharactorClassType.N)
            targetOffsetPosition = -targetTileController.CharactorDeploymentData.deploymentPositionList[0];
        else
            targetOffsetPosition = Vector3.zero;
    }

    //private void OnChangeCharactorTile(CharactorTileController moveTile)
    //{

    //}
}

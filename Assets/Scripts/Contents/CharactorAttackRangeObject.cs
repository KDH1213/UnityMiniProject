using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorAttackRangeObject : MonoBehaviour
{
    [SerializeField]
    private GameTouchManager gameTouchManager;

    private CharactorTileController targetTileController;
    private CharactorFSM targetCharactor;

    private Vector3 targetOffsetPosition;
    private bool isMove = false;

    public void OnActiveObject(CharactorTileController charactorTileController)
    {
        if (charactorTileController.CharactorCount == 0)
        {
            OnDisableObject();
            return;
        }

        isMove = false;
        gameObject.SetActive(true);
        targetTileController = charactorTileController;
        targetCharactor = targetTileController.CharacterControllers[0];
        SetTargetInfo();
    }

    public void OnDisableObject()
    {
        gameObject.SetActive(false);
    }

    private void SetTargetInfo()
    {
        transform.position = targetTileController.transform.position;
        float attackRange = targetCharactor.CharactorData.AttackRange;
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
        targetOffsetPosition = targetTileController.transform.position - targetCharactor.transform.position;
    }

    //private void OnChangeCharactorTile(CharactorTileController moveTile)
    //{

    //}
}

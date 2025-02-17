using System.Collections.Generic;
using UnityEngine;

public class CharactorTileController : MonoBehaviour
{
    [SerializeField]
    private CharactorTileManager charactorTileManager;

    [SerializeField] 
    private CharactorDeploymentData charactorDeploymentData;
    public CharactorDeploymentData CharactorDeploymentData { get { return charactorDeploymentData; } }


    public CharactorClassType CharactorClassType { get; private set; } = CharactorClassType.End;
    public int CharactorID { get; private set; } = -1;

    private List<CharactorFSM> characterControllerList = new List<CharactorFSM>();
    public List<CharactorFSM> CharacterControllers { get { return characterControllerList; } }

    private int charactorCount = 0;
    public int CharactorCount { get { return charactorCount; } }

    public void CreateCharactor(CharactorFSM characterController)
    {
        if(charactorCount == 0)
        {
            charactorTileManager.ChangeUsetTileCount(1);
        }

        ResetPosition();
        characterControllerList.Add(characterController);
        characterController.AttackDetectionPoint = transform.position;
        characterController.transform.position = transform.position;

        CharactorID = characterController.CharactorData.Id;
        CharactorClassType = characterController.CharactorData.CharactorClassType;
        ++charactorCount;
        ChangePosition();
    }

    public void AddCharactor(CharactorFSM characterController)
    {
        characterControllerList.Add(characterController);
        characterController.AttackDetectionPoint = transform.position;
        ++charactorCount;
    }

    public void OnChangeCharactorCount()
    {

    }

    private void ResetPosition()
    {
        if ((charactorDeploymentData.OverlappingClassTypeMask & (CharactorClassTypeMask)(1 << (int)CharactorClassType)) == 0)
            return;

        for (int i = 0; i < charactorCount; ++i)
        {
            characterControllerList[i].transform.position -= charactorDeploymentData.deploymentPositionList[i];
        }
    }

    private void ChangePosition()
    {
        if ((charactorDeploymentData.OverlappingClassTypeMask & (CharactorClassTypeMask)(1 << (int)CharactorClassType)) == 0)
            return;

        for (int i = 0; i < charactorCount; ++i)
        {
            characterControllerList[i].transform.position += charactorDeploymentData.deploymentPositionList[i];
        }
    }
    
    public bool IsCreateCharactor()
    {
        return charactorCount < charactorDeploymentData.maxDeploymentCount;
    }

    public void OnSellCharactor()
    {
        if (charactorCount == 0)
            return;

        ResetPosition();
        RemoveCharactor(1, true);
        charactorTileManager.OnSellCharactor(this);
        ChangePosition();
    }

    public void OnSynthesisCharactor()
    {
        if (charactorCount < charactorDeploymentData.maxDeploymentCount)
            return;

        charactorTileManager.OnSynthesisCharactor(this);
    }

    public void RemoveCharactor(int count, bool isDestroy)
    {
        if (count > charactorCount)
            return;

        if(isDestroy)
        {
            for (int i = 0; i < count; ++i)
            {
                Destroy(characterControllerList[charactorCount - 1 - i].gameObject);
            }
        }
        charactorCount -= count;
        characterControllerList.RemoveRange(charactorCount, count);
        if(charactorCount == 0)
            charactorTileManager.ChangeUsetTileCount(-1);
    }

    public void OnChangeCharactorInfo(CharactorTileController endCharactorTileObject)
    {
        int leftCount = characterControllerList.Count;
        int rightCount = endCharactorTileObject.characterControllerList.Count; 

        for (int i = 0; i < leftCount; ++i)
        {
            if ((charactorDeploymentData.OverlappingClassTypeMask & (CharactorClassTypeMask)(1 << (int)CharactorClassType)) == 0)
                ((CharactorMoveState)characterControllerList[i].StateTable[CharactorStateType.Move]).OnSetMovePoint(endCharactorTileObject.transform.position);
            else
                ((CharactorMoveState)characterControllerList[i].StateTable[CharactorStateType.Move]).OnSetMovePoint(endCharactorTileObject.transform.position + charactorDeploymentData.deploymentPositionList[i]);
            characterControllerList[i].ChangeState(CharactorStateType.Move);
            characterControllerList[i].AttackDetectionPoint = endCharactorTileObject.transform.position;
        }

        for (int i = 0; i < rightCount; ++i)
        {
            if ((charactorDeploymentData.OverlappingClassTypeMask & (CharactorClassTypeMask)(1 << (int)endCharactorTileObject.CharactorClassType)) == 0)
                ((CharactorMoveState)endCharactorTileObject.characterControllerList[i].StateTable[CharactorStateType.Move]).OnSetMovePoint(transform.position);
            else
                ((CharactorMoveState)endCharactorTileObject.characterControllerList[i].StateTable[CharactorStateType.Move]).OnSetMovePoint(transform.position + charactorDeploymentData.deploymentPositionList[i]);
            endCharactorTileObject.characterControllerList[i].ChangeState(CharactorStateType.Move);

            endCharactorTileObject.characterControllerList[i].AttackDetectionPoint = transform.position;
        }

        (characterControllerList, endCharactorTileObject.characterControllerList) = (endCharactorTileObject.characterControllerList, characterControllerList);
        (charactorCount, endCharactorTileObject.charactorCount) = (endCharactorTileObject.charactorCount, charactorCount);
        (CharactorClassType, endCharactorTileObject.CharactorClassType) = (endCharactorTileObject.CharactorClassType, CharactorClassType);
        (CharactorID, endCharactorTileObject.CharactorID) = (endCharactorTileObject.CharactorID, CharactorID);
    }

    public void OnSetReinforcedLevel(float damagePercent)
    {
        float reinforcedDamage = damagePercent * characterControllerList[0].CharactorData.Damage;
        foreach (var characterController in characterControllerList)
        {
            characterController.SetReinforcedDamage(reinforcedDamage);
            characterController.OnPlayReinforcedEffect();
        }
    }
}

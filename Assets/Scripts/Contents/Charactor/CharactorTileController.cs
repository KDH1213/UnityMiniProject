using System.Collections.Generic;
using UnityEngine;

public class CharactorTileController : MonoBehaviour
{
    [SerializeField]
    private CharactorTileManager charactorTileManager;

    [SerializeField] 
    private CharactorDeploymentData charactorDeploymentData;
    
    public CharactorClassType CharactorClassType { get; private set; }
    public string CharactorID { get; private set; } = string.Empty;

    private List<CharactorFSM> characterControllerList = new List<CharactorFSM>();
    public List<CharactorFSM> CharacterControllers { get { return characterControllerList; } }

    private int charactorCount = 0;
    public int CharactorCount { get { return charactorCount; } }

    public void AddCharactor(CharactorFSM characterController)
    {
        ResetPosition();
        characterControllerList.Add(characterController);
        characterController.transform.position = transform.position;

        CharactorID = characterController.CharactorData.Id;
        CharactorClassType = characterController.CharactorData.CharactorClassType;
        ++charactorCount;
        ChangePosition();
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

    public void OnSaleCharactor()
    {
        if (charactorCount == 0)
            return;

        ResetPosition();
        RemoveCharactor(1);
        ChangePosition();
    }

    public void OnSynthesisCharactor()
    {
        if (charactorCount < charactorDeploymentData.maxDeploymentCount)
            return;

        charactorTileManager.OnSynthesisCharactor(this);
    }

    public void RemoveCharactor(int count)
    {
        if (count > charactorCount)
            return;

        for (int i = 0; i < count; ++i)
        {
            Destroy(characterControllerList[--charactorCount].gameObject);
        }
        characterControllerList.RemoveRange(charactorCount, count);
    }

    public void OnChangeCharactorInfo(CharactorTileController endCharactorTileObject)
    {
        foreach (var characterController in characterControllerList)
        {
            var currentPos = characterController.transform.position - transform.position ;
            ((CharactorMoveState)characterController.StateTable[CharactorStateType.Move]).OnSetMovePoint(endCharactorTileObject.transform.position + currentPos);
            characterController.ChangeState(CharactorStateType.Move);
        }

        foreach (var characterController in endCharactorTileObject.characterControllerList)
        {
            var currentPos = characterController.transform.position - endCharactorTileObject.transform.position;
            ((CharactorMoveState)characterController.StateTable[CharactorStateType.Move]).OnSetMovePoint(transform.position + currentPos);
            characterController.ChangeState(CharactorStateType.Move);
        }

        (characterControllerList, endCharactorTileObject.characterControllerList) = (endCharactorTileObject.characterControllerList, characterControllerList);
        (charactorCount, endCharactorTileObject.charactorCount) = (endCharactorTileObject.charactorCount, charactorCount);
        (CharactorClassType, endCharactorTileObject.CharactorClassType) = (endCharactorTileObject.CharactorClassType, CharactorClassType);
        (CharactorID, endCharactorTileObject.CharactorID) = (endCharactorTileObject.CharactorID, CharactorID);
    }
}

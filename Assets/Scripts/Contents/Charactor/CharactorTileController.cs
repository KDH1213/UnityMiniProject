using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorTileController : MonoBehaviour
{
    [SerializeField]
    private CharactorTileManager charactorTileManager;

    [SerializeField] 
    private GroundSocketPositionData positionData;
    
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
        if (charactorCount == 1)
            return;

        var positionList = charactorCount == 2 ? positionData.twoSocketoffsetList : positionData.threeSocketList;

        for (int i = 0; i < charactorCount; ++i)
        {
            characterControllerList[i].transform.position -= positionList[i];
        }
    }

    private void ChangePosition()
    {
        if (charactorCount == 1)
            return;

        var positionList = charactorCount == 2 ? positionData.twoSocketoffsetList : positionData.threeSocketList;

        for (int i = 0; i < charactorCount; ++i)
        {
            characterControllerList[i].transform.position += positionList[i];
        }
    }
    
    public bool IsCreateCharactor()
    {
        return charactorCount < positionData.NumberOfCharactersPerTile;
    }

    public void OnSaleCharactor()
    {
        if (charactorCount == 0)
            return;

        ResetPosition();
        RemoveCharactor(1);
        // Destroy(characterControllerList[--charactorCount].gameObject);
        // characterControllerList.RemoveAt(charactorCount);
        ChangePosition();
    }

    public void OnSynthesisCharactor()
    {
        if (charactorCount < positionData.NumberOfCharactersPerTile)
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
}

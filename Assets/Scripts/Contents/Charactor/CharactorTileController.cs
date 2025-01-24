using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorTileController : MonoBehaviour
{
    [SerializeField] 
    private int charactorCount = 0;
    [SerializeField] 
    private int maxCharactorCount = 3;

    [SerializeField] 
    private List<CharactorFSM> characterControllers;
    [SerializeField] 
    private GroundSocketPositionData positionData;
    
    public CharactorClassType CharactorClassType { get; private set; }
    public string CharactorID { get; private set; } = string.Empty;
    public List<CharactorFSM> CharacterControllers { get { return characterControllers; } }


    public int CharactorCount { get { return charactorCount; } }

    public void AddCharactor(CharactorFSM characterController)
    {
        ResetPosition();
        characterControllers.Add(characterController);
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
            characterControllers[i].transform.position -= positionList[i];
        }
    }

    private void ChangePosition()
    {
        if (charactorCount == 1)
            return;

        var positionList = charactorCount == 2 ? positionData.twoSocketoffsetList : positionData.threeSocketList;

        for (int i = 0; i < charactorCount; ++i)
        {
            characterControllers[i].transform.position += positionList[i];
        }
    }
}

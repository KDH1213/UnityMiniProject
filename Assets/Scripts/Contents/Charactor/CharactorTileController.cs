using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorTileController : MonoBehaviour
{
    [SerializeField] private int charactorCount = 0;
    [SerializeField] private int maxCharactorCount = 3;

    [SerializeField] private List<CharactorFSM> characterControllers;
    public List<CharactorFSM> CharacterControllers { get { return characterControllers; } }


    public int CharactorCount { get { return charactorCount; } }

    public void AddCharactor(CharactorFSM characterController)
    {
        characterControllers.Add(characterController);
        characterController.transform.position = transform.position;
        ++charactorCount;
    }

    public void OnChangeCharactorCount()
    {

    }

}

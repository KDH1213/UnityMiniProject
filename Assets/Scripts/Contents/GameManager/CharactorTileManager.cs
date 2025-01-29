using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharactorTileManager : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private List<CharactorTileController> charactorTileObjects = new List<CharactorTileController>();
    public List<CharactorTileController> CharactorTileObjects { get { return charactorTileObjects; } }

    [field: SerializeField]
    public CharactorDeploymentData CharactorDeploymentData { get; private set; }

    [SerializeField]
    private readonly int maxCharactorCount = 20;


    private Dictionary<string, int> charactorCountTable = new Dictionary<string, int>();

    private int tileControllerCount;
    private int useTileCharactorCount = 0;
    private int totalCharactorCount = 0;

    public UnityEvent<int, int> changeCharatorCountEvent;


    private void Awake()
    {
        StartSortcharactorTiles();
        tileControllerCount = charactorTileObjects.Count;
    }

    private void Start()
    {
        changeCharatorCountEvent?.Invoke(totalCharactorCount, maxCharactorCount);
    }

    public void StartSortcharactorTiles()
    {
        charactorTileObjects.Sort((CharactorTileController left, CharactorTileController right) =>
        {
            if (left.transform.position.x == right.transform.position.x)
            {
                return left.transform.position.y < right.transform.position.y ? -1 : 1;
            }
            else
            {
                return left.transform.position.x < right.transform.position.x ? -1 : 1;
            }
        });
    }

    public void CreateCharactor(CharactorFSM createCharactor)
    {
        if (IsFindDeploymentTile(ref createCharactor, out var charactorTileController))
        {
            charactorTileController.AddCharactor(createCharactor);
            AddCharactorTable(createCharactor.CharactorData.Id);
            return;
        }

        foreach (var tile in charactorTileObjects)
        {
            if(tile.CharactorCount == 0 
                || ((((CharactorClassTypeMask)(1 << (int)tile.CharactorClassType) & CharactorDeploymentData.OverlappingClassTypeMask) != 0)
                && tile.CharactorID == createCharactor.CharactorData.Id && tile.CharactorCount < CharactorDeploymentData.maxDeploymentCount))
            {

                tile.AddCharactor(createCharactor);
                AddCharactorTable(createCharactor.CharactorData.Id);
                break;
            }
        }
    }

    public bool IsCreateCharactor()
    {
        return (totalCharactorCount < maxCharactorCount && useTileCharactorCount < tileControllerCount);
    }

    public void OnSaleCharactor(CharactorTileController charactorTileController)
    {
        if (!charactorCountTable.ContainsKey(charactorTileController.CharactorID))
            return;

        --charactorCountTable[charactorTileController.CharactorID];
    }

    public void OnSynthesisCharactor(CharactorTileController charactorTileController)
    {
        if (!charactorCountTable.ContainsKey(charactorTileController.CharactorID))
            return;

        int charactorCount = charactorTileController.CharactorCount;
        charactorCountTable[charactorTileController.CharactorID] -= charactorCount;
        totalCharactorCount -= charactorCount;

        var synthesisCharactor = gameController.GetCreateSynthesisCharactor(charactorTileController.CharactorClassType);
        var createCharactor = synthesisCharactor.GetComponent<CharactorFSM>();

        charactorTileController.RemoveCharactor(charactorCount);

        if (IsCharactorDeployment(ref createCharactor, out var tile))
        {
            tile.AddCharactor(createCharactor);
        }
        else
        {
            charactorTileController.AddCharactor(createCharactor);
        }
        AddCharactorTable(createCharactor.CharactorData.Id);
    }

    private void AddCharactorTable(string charactorID)
    {
        if (charactorCountTable.ContainsKey(charactorID))
            ++charactorCountTable[charactorID];
        else
        {
            charactorCountTable.Add(charactorID, 1);
            ++useTileCharactorCount;
        }

        ++totalCharactorCount;

        // if (tile.CharactorCount == 0)
        changeCharatorCountEvent?.Invoke(totalCharactorCount, maxCharactorCount);
    }

    private bool IsCharactorDeployment(ref CharactorFSM synthesisCharactor, out CharactorTileController charactorTileController)
    {
        charactorTileController = null;

        if (((CharactorClassTypeMask)(1 << (int)synthesisCharactor.CharactorData.CharactorClassType) & CharactorDeploymentData.OverlappingClassTypeMask) == 0
            || !IsFindDeploymentTile(ref synthesisCharactor, out charactorTileController))
            return false;

        return true;
    }

    private bool IsFindDeploymentTile(ref CharactorFSM synthesisCharactor, out CharactorTileController charactorTileController)
    {
        charactorTileController = null;

        foreach (var tile in charactorTileObjects)
        {
            if (tile.CharactorCount > 0 && tile.CharactorCount < CharactorDeploymentData.maxDeploymentCount
                 && tile.CharactorID == synthesisCharactor.CharactorData.Id)
            {
                charactorTileController = tile;
                return true;
            }
        }

        return false;
    }
}

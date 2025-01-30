using System;
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


    private Dictionary<int, int> charactorCountTable = new Dictionary<int, int>();

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
        if (((CharactorClassTypeMask)(1 << (int)createCharactor.CharactorData.CharactorClassType) & CharactorDeploymentData.OverlappingClassTypeMask) != 0
            && IsFindDeploymentTile(ref createCharactor, out var charactorTileController))
        {
            charactorTileController.CreateCharactor(createCharactor);
            AddCharactorTable(createCharactor.CharactorData.Id);
            return;
        }

        foreach (var tile in charactorTileObjects)
        {
            if(tile.CharactorCount == 0 
                || ((((CharactorClassTypeMask)(1 << (int)tile.CharactorClassType) & CharactorDeploymentData.OverlappingClassTypeMask) != 0)
                && tile.CharactorID == createCharactor.CharactorData.Id && tile.CharactorCount < CharactorDeploymentData.maxDeploymentCount))
            {

                tile.CreateCharactor(createCharactor);
                AddCharactorTable(createCharactor.CharactorData.Id);
                break;
            }
        }
    }

    public bool IsCreateCharactor()
    {
        return (totalCharactorCount < maxCharactorCount && useTileCharactorCount < tileControllerCount);
    }

    public void OnSellCharactor(CharactorTileController charactorTileController)
    {
        if (!charactorCountTable.ContainsKey(charactorTileController.CharactorID))
            return;

        --charactorCountTable[charactorTileController.CharactorID];

        if ((((CharactorClassTypeMask)(1 << (int)charactorTileController.CharactorClassType) & CharactorDeploymentData.OverlappingClassTypeMask) == 0))
            return;

        var list = IsFindDeploymentPossibleCharactorTiles(charactorTileController);
        if (list.Count == 0)
            return;

        var charactor = list[0].CharacterControllers[list[0].CharactorCount - 1];
        list[0].RemoveCharactor(1, false);

        ((CharactorMoveState)charactor.StateTable[CharactorStateType.Move]).OnSetMovePoint(charactorTileController.transform.position + CharactorDeploymentData.deploymentPositionList[charactorTileController.CharactorCount]);
        charactorTileController.AddCharactor(charactor);
        charactor.ChangeState(CharactorStateType.Move);
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

        charactorTileController.RemoveCharactor(charactorCount, true);

        if (IsCharactorDeployment(ref createCharactor, out var tile))
        {
            tile.CreateCharactor(createCharactor);
        }
        else
        {
            charactorTileController.CreateCharactor(createCharactor);
        }
        AddCharactorTable(createCharactor.CharactorData.Id);
    }

    private void AddCharactorTable(int charactorID)
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

    private bool IsFindDeploymentTile(ref CharactorFSM findCharactor, out CharactorTileController charactorTileController)
    {
        charactorTileController = null;

        foreach (var tile in charactorTileObjects)
        {
            if (tile.CharactorCount > 0 && tile.CharactorCount < CharactorDeploymentData.maxDeploymentCount
                 && tile.CharactorID == findCharactor.CharactorData.Id)
            {
                charactorTileController = tile;
                return true;
            }
        }

        return false;
    }

    private List<CharactorTileController> IsFindDeploymentPossibleCharactorTiles(CharactorTileController charactorTileController)
    {
        var list = new List<CharactorTileController>();
        var count = charactorTileObjects.Count;

        for(int i = count -1; i >= 0; --i)
        {
            if (charactorTileObjects[i] == charactorTileController
                || charactorTileObjects[i].CharactorCount == 0 || charactorTileObjects[i].CharactorCount == 3
                || charactorTileObjects[i].CharactorID != charactorTileController.CharactorID)
                continue;

            list.Add(charactorTileObjects[i]);
        }

        return list;
    }

    public List<(int, bool)> GetHoldingsStatus(CombinationData combinationData)
    {
        List<(int, bool)> holingsStatusList = new List<(int, bool)>();

        foreach (var item in combinationData.IngredientTable)
        {
            if (!charactorCountTable.ContainsKey(item.Key))
            {
                for(int i = 0; i < item.Value; ++i)
                {
                    holingsStatusList.Add( new (item.Key, false));
                }

                continue;
            }

            int holdingCount = Mathf.Min(charactorCountTable[item.Key], item.Value);

            for(int i = 0;i < holdingCount; ++i)
            {
                holingsStatusList.Add(new(item.Key, true));
            }

            for (int i = holdingCount; i < item.Value; ++i)
            {
                holingsStatusList.Add(new(item.Key, false));
            }
        }

        return holingsStatusList;
    }

    public int GetHoldingsStatusPercent(CombinationData combinationData)
    {
        float percent = 0;
        float maxPercent = 0;

        foreach (var item in combinationData.IngredientTable)
        {
            if (!charactorCountTable.ContainsKey(item.Key))
            {
                if (DataTableManager.CharactorDataTable.Get(item.Key).CharactorClassType == CharactorClassType.N)
                {
                    maxPercent += item.Value * 10;
                }
                else if (DataTableManager.CharactorDataTable.Get(item.Key).CharactorClassType == CharactorClassType.A)
                {
                    maxPercent += item.Value * 30;
                }
                else
                {
                    maxPercent += item.Value * 50;
                }
                continue;
            }


            int holdingCount = charactorCountTable[item.Key];
             
            if(DataTableManager.CharactorDataTable.Get(item.Key).CharactorClassType == CharactorClassType.N)
            {
                percent += Mathf.Min(charactorCountTable[item.Key], item.Value) * 10;
                maxPercent += item.Value * 10;
            }
            else if (DataTableManager.CharactorDataTable.Get(item.Key).CharactorClassType == CharactorClassType.A)
            {
                percent += Mathf.Min(charactorCountTable[item.Key], item.Value) * 30;
                maxPercent += item.Value * 30;
            }
            else
            {
                percent += Mathf.Min(charactorCountTable[item.Key], item.Value) * 50;
                maxPercent += item.Value * 50;
            }
        }

        return (int)(percent / maxPercent * 100f);
    }

    public void OnCreateCombinationCharactor(CombinationData combinationData)
    {
        foreach (var item in combinationData.IngredientTable)
        {
            charactorCountTable[item.Key] -= item.Value;
            totalCharactorCount -= item.Value;
            var list = charactorTileObjects.FindAll(tile => tile.CharactorID == item.Key);
            int destroyCount = 0;

            list.Sort((CharactorTileController left, CharactorTileController right) =>
            {
                if (left.CharactorCount < right.CharactorCount)
                    return -1;
                else if (left.CharactorCount > right.CharactorCount)
                    return 1;
                else
                {
                    if (left.transform.position.x == right.transform.position.x)
                    {
                        return left.transform.position.y > right.transform.position.y ? -1 : 1;
                    }
                    else
                    {
                        return left.transform.position.x > right.transform.position.x ? -1 : 1;
                    }
                }
            });

            for (var i = 0; i < list.Count; i++)
            {
                int count = Mathf.Min(item.Value - destroyCount, list[i].CharactorCount);
                list[i].RemoveCharactor(count, true);
                destroyCount += count;

                if(destroyCount == item.Value)
                    break;
            }
        }

        var createCharactor = Instantiate(DataTableManager.CharactorDataTable.Get(combinationData.Id).PrefabObject); 
        CreateCharactor(createCharactor.GetComponent<CharactorFSM>());
    }
}

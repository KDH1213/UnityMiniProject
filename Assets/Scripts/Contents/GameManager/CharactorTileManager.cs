using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharactorTileManager : MonoBehaviour
{
    [SerializeField]
    private List<CharactorTileController> charactorTileObjects = new List<CharactorTileController>();
    public List<CharactorTileController> CharactorTileObjects { get { return charactorTileObjects; } }

    private Dictionary<string, int> charactorCountTable = new Dictionary<string, int>();

    [SerializeField]
    private readonly int maxCharactorCount = 20;

    [SerializeField]
    private readonly int maxCharactorTileCount = 3;

    [SerializeField]
    private int tileControllerCount;
    private int useCharactorTileCount = 0;
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
        foreach (var tile in charactorTileObjects)
        {
            if(tile.CharactorCount == 0 || (tile.CharactorClassType == CharactorClassType.N 
                && tile.CharactorID == createCharactor.CharactorData.Id && tile.CharactorCount < maxCharactorTileCount))
            {
                tile.AddCharactor(createCharactor);

                var charactorID = createCharactor.CharactorData.Id;
                if (charactorCountTable.ContainsKey(charactorID))
                    ++charactorCountTable[charactorID];
                else
                {
                    charactorCountTable.Add(charactorID, 1);
                    ++useCharactorTileCount;
                }

                ++totalCharactorCount;

                // if (tile.CharactorCount == 0)
                changeCharatorCountEvent?.Invoke(totalCharactorCount, maxCharactorCount);
                break;
            }
        }
    }

    public bool IsCreateCharactor()
    {
        return (totalCharactorCount < maxCharactorCount && useCharactorTileCount < tileControllerCount);
    }
}

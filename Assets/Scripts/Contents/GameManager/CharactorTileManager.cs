using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorTileManager : MonoBehaviour
{
    [SerializeField]
    private List<CharactorTileController> charactorTileObjects = new List<CharactorTileController>();
    public List<CharactorTileController> CharactorTileObjects { get { return charactorTileObjects; } }

    private Dictionary<string, int> charactorCountTable = new Dictionary<string, int>();

    [SerializeField] private int maxCharactorTileCount = 3;
    private int useCharactorTileCount = 0;

    private void Awake()
    {
        StartSortcharactorTiles();
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
                    charactorCountTable.Add(charactorID, 1);

                ++useCharactorTileCount;
                break;
            }
        }
    }

    public bool IsCreateCharactor()
    {
        return useCharactorTileCount == CharactorTileObjects.Count;
    }
}

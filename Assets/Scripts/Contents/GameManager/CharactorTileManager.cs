using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorTileManager : MonoBehaviour
{
    [SerializeField]
    private List<Transform> charactorTileObjects = new List<Transform>();
    public List<Transform> CharactorTileObjects { get { return charactorTileObjects; } }

    private void Awake()
    {
        StartSortcharactorTiles();
    }

    public void StartSortcharactorTiles()
    {
        charactorTileObjects.Sort((Transform left, Transform right) =>
        {
            if (left.position.x == right.position.x)
            {
                return left.position.y < right.position.y ? -1 : 1;
            }
            else
            {
                return left.position.x < right.position.x ? -1 : 1;
            }
        });
    }
}

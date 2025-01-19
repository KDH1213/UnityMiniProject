using UnityEngine;


[CreateAssetMenu(fileName = "TowerData ", menuName = "System/TowerData", order = 0)]
public class CreateTowerData : ScriptableObject
{
    [SerializeField]
    private GameObject towerPrefab;
    public GameObject TowerPrefab {  get { return towerPrefab; } }

    [SerializeField]
    private Vector3 createLocalPosition;
    public Vector3 CreateLocalPosition() { return createLocalPosition; }

    [SerializeField]
    private int price;
    public int Price { get { return price; } }

}

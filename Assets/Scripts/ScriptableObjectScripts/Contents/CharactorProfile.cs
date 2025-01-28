using UnityEngine;

[CreateAssetMenu(fileName = "CharactorProfile", menuName = "Profile/CharactorProfile", order = 3)]
[System.Serializable]
public class CharactorProfile : ScriptableObject
{
    [field: SerializeField]
    public float MoveSpeed {  get; private set; }

}

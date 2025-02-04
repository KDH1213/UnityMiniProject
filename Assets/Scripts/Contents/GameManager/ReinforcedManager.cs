using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReinforcedManager : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private InGameUiController inGameUiController;

    private ReinforcedTable reinforcedTable;

    public UnityEvent<CharactorClassType, int> reinforcedLevelTypeNEvent;
    public UnityEvent<CharactorClassType, int> reinforcedLevelTypeAEvent;
    public UnityEvent<CharactorClassType, int> reinforcedLevelTypeSEvent;
    public UnityEvent<int> reinforcedLevelTypeCellEvent;

    private int[] currentReinforcedLevelTypes = new int[(int)CharactorClassType.End];
    private float[] currentReinforcedDamageDamagePercent = new float[(int)CharactorClassType.End];

    private int currentReinforcedLevelTypeCall = 0;

    private void Awake()
    {
        reinforcedLevelTypeNEvent.AddListener(inGameUiController.OnChangeCharactorClassReinforced);
        reinforcedLevelTypeAEvent.AddListener(inGameUiController.OnChangeCharactorClassReinforced);
        reinforcedLevelTypeSEvent.AddListener(inGameUiController.OnChangeCharactorClassReinforced);
        reinforcedLevelTypeCellEvent.AddListener(inGameUiController.OnChangeCoinDrawLevel);
    }

    public int GetReinforcedLevel(CharactorClassType type)
    {
        return currentReinforcedLevelTypes[(int)type];
    }

    public float GetCurrentReinforcedDamageDamagePercent(CharactorClassType type)
    {
        return currentReinforcedDamageDamagePercent[(int)type];
    }

    public void OnLevelUpReinforcedLevelType(int index)
    {
        var type = (CharactorClassType)index;
        var value = DataTableManager.ReinforcedTable.GetKeyData(type).valueList[currentReinforcedLevelTypes[index]];
        if (gameController.CurrentCoint < value)
            return;
        gameController.OnAddCoin(-value);
        ++currentReinforcedLevelTypes[index];
        currentReinforcedDamageDamagePercent[index] = DataTableManager.ReinforcedTable.GetKeyData(type).damagePercentList[currentReinforcedLevelTypes[index]];

        switch (type)
        {
            case CharactorClassType.N:
                reinforcedLevelTypeNEvent?.Invoke(type, currentReinforcedLevelTypes[index]);
                break;
            case CharactorClassType.A:
                reinforcedLevelTypeAEvent?.Invoke(type, currentReinforcedLevelTypes[index]);
                break;
            case CharactorClassType.S:
                reinforcedLevelTypeSEvent?.Invoke(type, currentReinforcedLevelTypes[index]);
                break;
            case CharactorClassType.End:
                break;
            default:
                break;
        }
    }
    public void OnLevelUpReinforcedLevelTypeCall()
    {
        var value = DataTableManager.CoinDrawTable.Get(currentReinforcedLevelTypeCall).Value;
        if (gameController.CurrentCoint < value)
            return;

        gameController.OnAddCoin(-value);
        ++currentReinforcedLevelTypeCall;
        reinforcedLevelTypeCellEvent?.Invoke(currentReinforcedLevelTypeCall);
    }
}


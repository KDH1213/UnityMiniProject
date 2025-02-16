using UnityEngine;
using UnityEngine.Events;

public class ReinforcedManager : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    [field: SerializeField]
    public UIReinforcedView ReinforcedView { get; private set; }

    [SerializeField]
    private ParticleSystem[] upgradeEffects;

    private ReinforcedTable reinforcedTable;

    public UnityEvent<CharactorClassType, int> reinforcedLevelTypeNEvent;
    public UnityEvent<CharactorClassType, int> reinforcedLevelTypeAEvent;
    public UnityEvent<CharactorClassType, int> reinforcedLevelTypeSEvent;
    public UnityEvent<int> reinforcedLevelTypeCellEvent;

    private int[] currentReinforcedLevelTypes = new int[(int)CharactorClassType.End];
    private float[] currentReinforcedDamageDamagePercent = new float[(int)CharactorClassType.End];

    private int currentReinforcedLevelTypeCall = 0;
    public int CurrentReinforcedLevelTypeCall { get { return currentReinforcedLevelTypeCall; } }

    private void Awake()
    {
        reinforcedLevelTypeNEvent.AddListener(ReinforcedView.OnChangeCharactorClassReinforced);
        reinforcedLevelTypeAEvent.AddListener(ReinforcedView.OnChangeCharactorClassReinforced);
        reinforcedLevelTypeSEvent.AddListener(ReinforcedView.OnChangeCharactorClassReinforced);
        reinforcedLevelTypeCellEvent.AddListener(ReinforcedView.OnChangeCoinDrawLevel);
    }

    public int GetReinforcedLevel(CharactorClassType type)
    {
        return currentReinforcedLevelTypes[(int)type];
    }

    public float GetCurrentReinforcedDamagePercent(CharactorClassType type)
    {
        return currentReinforcedDamageDamagePercent[(int)type];
    }

    public void OnLevelUpReinforcedLevelType(int index)
    {
        var type = (CharactorClassType)index;

        if (DataTableManager.ReinforcedTable.GetKeyData(type).CurrencyType == CurrencyType.Coin)
        {
            var value = DataTableManager.ReinforcedTable.GetKeyData(type).valueList[currentReinforcedLevelTypes[index]];
            if (gameController.CurrentCoin < value)
                return;

            gameController.OnAddCoin(-value);
        }
        else
        {
            var value = DataTableManager.ReinforcedTable.GetKeyData(type).valueList[currentReinforcedLevelTypes[index]];
            if (gameController.CurrentJewel < value)
                return;

            gameController.OnAddJewel(-value);
        }

        ++currentReinforcedLevelTypes[index];
        upgradeEffects[index].Stop();
        upgradeEffects[index].gameObject.SetActive(true);
        upgradeEffects[index].Play();
        currentReinforcedDamageDamagePercent[index] = DataTableManager.ReinforcedTable.GetKeyData(type).damagePercentList[currentReinforcedLevelTypes[index]];
        gameController.CharactorTileManager.OnPlayReinforcedEffect(type);
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
        var value = DataTableManager.CoinDrawTable.Get(currentReinforcedLevelTypeCall).UpgradeCost;
        if (gameController.CurrentCoin < value)
            return;

        gameController.OnAddCoin(-value);
        upgradeEffects[3].Stop();
        upgradeEffects[3].gameObject.SetActive(true);
        upgradeEffects[3].Play();
        ++currentReinforcedLevelTypeCall;
        reinforcedLevelTypeCellEvent?.Invoke(currentReinforcedLevelTypeCall);
    }
}


using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class UIHpBar : MonoBehaviour
{
    private IObjectPool<UIHpBar> hpBarObjectPool;

    [SerializeField]
    private Slider hpBarSlider;

    private MonsterStatus targetStats;
    private Transform targetTransform;
    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnChangeHpBar(float persent)
    {
        hpBarSlider.value = persent;

        if(persent <= 0f)
        {
            Release();
        }
    }

    public void SetPool(IObjectPool<UIHpBar> hpBarObjectPool)
    {
        this.hpBarObjectPool = hpBarObjectPool;
    }

    public void SetTarget(MonsterStatus charactorStats)
    {
        var hpStat = charactorStats.CurrentValueTable[StatType.HP];
        hpStat.OnChangeValuePerscent += OnChangeHpBar;
        OnChangeHpBar(hpStat.PersentValue);

        targetStats = charactorStats;
        targetTransform = targetStats.transform;
    }

    private void LateUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetTransform.position);
        screenPos.z = 0f;
        transform.position = screenPos;
    }

    public void Release()
    {
        targetStats.CurrentValueTable[StatType.HP].OnChangeValuePerscent -= OnChangeHpBar;
        hpBarObjectPool.Release(this);
    }

}

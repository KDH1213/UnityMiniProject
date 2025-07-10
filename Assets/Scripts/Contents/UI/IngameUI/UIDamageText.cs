using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using TMPro;
using Unity.Burst;
using UnityEngine;
using UnityEngine.Pool;

public class UIDamageText : MonoBehaviour
{
    [SerializeField]
    private DamageTextEffectData damageEffectData;

    [SerializeField]
    private RectTransform target;

    [SerializeField]
    private TextMeshProUGUI damageText;

    //private UniTask uiDamageTask;
    //private CancellationTokenSource uiDamageCoroutineSource = new();

    private IObjectPool<UIDamageText> uiDamageTextPool;

    private Vector3 position;
    private Vector3 endPosition;

    private Vector3 startScale;
    private Vector3 targetScale;

    private Vector3 scale;
    private Color color;
    private float currentTime;
    private int currentDamage;

    private void OnDisable()
    {
        damageText.color = color;
        target.localScale = startScale;
    }

    private void Awake()
    {
        color = damageText.color;
        startScale = transform.localScale;
        targetScale = startScale * damageEffectData.TargetScaleSize;
    }

    private void OnEnable()
    {
        scale = target.localScale;
        currentTime = 0f;
    }


    [BurstCompile]
    private void Update()
    {
        currentTime += Time.deltaTime;
        var ratio = currentTime / damageEffectData.Duration;
        target.position = Vector3.Lerp(position, endPosition, ratio);
        target.localScale = Vector3.Lerp(scale, targetScale, ratio);
        damageText.color = Color.Lerp(color, damageEffectData.TargetColor, ratio);

        if(currentTime > damageEffectData.Duration)
            DestroyUIDamageText();
    }

    #region 미사용 코드
    //private async UniTask CoUIDamageEffect()
    //{
    //    float currentTime = 0f;

    //    Vector3 position = target.position;
    //    Vector3 endPosition = target.position + direction * distance;
    //    Vector3 scale = target.localScale;
    //    Color color = damageText.color;

    //    while (currentTime < time)
    //    {
    //        currentTime += Time.deltaTime;
    //        var ratio = currentTime / time;
    //        target.position = Vector3.Lerp(position, endPosition, ratio);
    //        target.localScale = Vector3.Lerp(scale, targetScale, ratio);
    //        damageText.color = Color.Lerp(color, targetColor, ratio);
    //        await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: this.GetCancellationTokenOnDestroy());
    //    }

    //    DestroyUIDamageText();
    //}

    //private IEnumerator CoEffect()
    //{
    //    float currentTime = 0f;

    //    Vector3 position = target.position;
    //    Vector3 endPosition = target.position + direction * distance;
    //    Vector3 scale = target.localScale;
    //    Color color = damageText.color;

    //    while (currentTime < time)
    //    {
    //        currentTime += Time.deltaTime;
    //        var ratio = currentTime / time;
    //        target.position = Vector3.Lerp(position, endPosition, ratio);
    //        target.localScale = Vector3.Lerp(scale, targetScale, ratio);
    //        damageText.color = Color.Lerp(color, targetColor, ratio);
    //        yield return new WaitForEndOfFrame();
    //    }

    //    DestroyUIDamageText();
    //}
    #endregion
    public void SetDamage(int damage)
    {
        if(currentDamage != damage)
        {
            currentDamage = damage;
            damageText.text = damage.ToString();
        }

        position = target.position + damageEffectData.OffsetPosition;
        endPosition = target.position + damageEffectData.Direction * damageEffectData.Distance;
    }

    public void SetPool(IObjectPool<UIDamageText> uiDamageTextPool)
    {
        this.uiDamageTextPool = uiDamageTextPool;
    }

    public IObjectPool<UIDamageText> GetObjectPool()
    {
        return uiDamageTextPool;
    }

    public void DestroyUIDamageText()
    {
        uiDamageTextPool.Release(this);
    }
}

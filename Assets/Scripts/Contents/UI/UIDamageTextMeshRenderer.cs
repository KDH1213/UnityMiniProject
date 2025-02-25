using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class UIDamageTextMeshRenderer : MonoBehaviour
{
    [SerializeField]
    private DamageTextEffectData damageEffectData;

    [SerializeField]
    private RectTransform target;

    [SerializeField]
    private TextMeshPro damageText;

    //private UniTask uiDamageTask;
    //private CancellationTokenSource uiDamageCoroutineSource = new();

    private IObjectPool<UIDamageTextMeshRenderer> uiDamageTextPool;

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

    private void Update()
    {
        currentTime += Time.deltaTime;
        var ratio = currentTime / damageEffectData.Duration;
        target.position = Vector3.Lerp(position, endPosition, ratio);
        target.localScale = Vector3.Lerp(scale, targetScale, ratio);
        damageText.color = Color.Lerp(color, damageEffectData.TargetColor, ratio);

        if (currentTime > damageEffectData.Duration)
            DestroyUIDamageText();
    }
    public void SetDamage(int damage)
    {
        if (currentDamage != damage)
        {
            currentDamage = damage;
            damageText.text = damage.ToString();
        }

        position = target.position + damageEffectData.OffsetPosition;
        endPosition = target.position + damageEffectData.Direction * damageEffectData.Distance;
    }

    public void SetPool(IObjectPool<UIDamageTextMeshRenderer> uiDamageTextPool)
    {
        this.uiDamageTextPool = uiDamageTextPool;
    }

    public IObjectPool<UIDamageTextMeshRenderer> GetObjectPool()
    {
        return uiDamageTextPool;
    }

    public void DestroyUIDamageText()
    {
        uiDamageTextPool.Release(this);
    }
}

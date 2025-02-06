using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class UIDamageText : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 direction;

    [SerializeField]
    private float distance;

    [SerializeField]
    private float time;

    [SerializeField] 
    private Vector3 startScale;
    [SerializeField]
    private Vector3 targetScale;

    [SerializeField]
    private Color targetColor;

    [SerializeField]
    private TextMeshProUGUI damageText;

    private IObjectPool<UIDamageText> uiDamageTextPool;

    private Vector3 position;
    private Vector3 endPosition;
    private Vector3 scale;
    private Color color;
    private float currentTime;

    private void OnDisable()
    {
        target.localScale = startScale;
    }

    private void OnEnable()
    {
        position = target.position;
        endPosition = target.position + direction * distance;
        scale = target.localScale;
        color = damageText.color;
    }
    //private void Start()
    //{
    //    StartCoroutine(CoEffect());
    //}

    private void Update()
    {
        currentTime += Time.deltaTime;
        var ratio = currentTime / time;
        target.position = Vector3.Lerp(position, endPosition, ratio);
        target.localScale = Vector3.Lerp(scale, targetScale, ratio);
        damageText.color = Color.Lerp(color, targetColor, ratio);

        if(currentTime > time)
            DestroyUIDamageText();
    }

    private IEnumerator CoEffect()
    {
        float currentTime = 0f;

        Vector3 position = target.position;
        Vector3 endPosition = target.position + direction * distance;
        Vector3 scale = target.localScale;
        Color color = damageText.color;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            var ratio = currentTime / time;
            target.position = Vector3.Lerp(position, endPosition, ratio);
            target.localScale = Vector3.Lerp(scale, targetScale, ratio);
            damageText.color = Color.Lerp(color, targetColor, ratio);
            yield return new WaitForEndOfFrame();
        }

        DestroyUIDamageText();
    }

    public void SetDamage(string damage)
    {
        damageText.text = damage;
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

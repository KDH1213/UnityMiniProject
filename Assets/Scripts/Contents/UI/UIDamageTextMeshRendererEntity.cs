using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Pool;

public class UIDamageTextMeshRendererEntity : MonoBehaviour
{
    [SerializeField]
    private DamageTextEffectData damageEffectData;

    [SerializeField]
    private RectTransform target;

    [SerializeField]
    private TextMeshPro damageText;

    private Vector3 position;
    private Vector3 endPosition;

    private Vector3 startScale;
    private Vector3 targetScale;

    private Vector3 scale;
    private Color color;
    private float currentTime;
    private int currentDamage;

    class Baker : Baker<UIDamageTextMeshRendererEntity>
    {
        public override void Bake(UIDamageTextMeshRendererEntity src)
        {
            var data = new UIDamageTextEntity()
            {
                // damageEffectData = src.damageEffectData,
                // damageText = src.damageText,
                // target = src.target,
                position = src.position,
                endPosition = src.endPosition,
                startScale = src.startScale,
                targetScale = src.targetScale,
                color = src.color,
                scale = src.scale,
                currentTime = src.currentTime,
                currentDamage = src.currentDamage,
                damageEffectInfo = new DamageTextEffectInfo(src.damageEffectData)
            };

            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, data);
        }
    }

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
}

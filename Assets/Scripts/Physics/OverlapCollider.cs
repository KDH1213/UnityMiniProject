using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum OverlapType
{
    Box,
    Circle,

}

public class OverlapCollider : MonoBehaviour
{
    [SerializeField] private int colliderCount;
    [SerializeField] protected Collider2D[] hitColliders;
    public Collider2D[] HitColliders { get { return hitColliders; } }

    [SerializeField] protected LayerMask hitLayerMasks;
    [SerializeField] protected OverlapType overlapType;

    private void Awake()
    {
        hitColliders = new Collider2D[colliderCount];
    }

    public int StartOverlapCircle(float radin)
    {
        return Physics2D.OverlapCircleNonAlloc(transform.position, radin, hitColliders);
    }

    public int StartOverlapBox(Vector2 scale)
    {
        return Physics2D.OverlapBoxNonAlloc(transform.position, scale, 0f, hitColliders);
    }
}

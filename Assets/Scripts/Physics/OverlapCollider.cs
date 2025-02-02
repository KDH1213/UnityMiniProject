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
    [SerializeField] 
    protected List<Collider2D> hitColliderList;

    [SerializeField]
    private int colliderSize;
    public List<Collider2D> HitColliderList { get { return hitColliderList; } }

    [SerializeField] 
    protected LayerMask hitLayerMasks;
    [SerializeField] 
    protected OverlapType overlapType;

    [SerializeField]
    ContactFilter2D contactFilter2D = new ContactFilter2D().NoFilter();

    [SerializeField]
    ContactFilter2D defalutContactFilter2D = new ContactFilter2D().NoFilter();
    private void Awake()
    {
        defalutContactFilter2D.SetLayerMask(hitLayerMasks);
        hitColliderList.Capacity = colliderSize;
    }

    public int StartOverlapCircle(float radin)
    {
        return Physics2D.OverlapCircle(transform.position, radin, defalutContactFilter2D, hitColliderList);
    }

    public int StartOverlapBox(Vector2 scale)
    {
        return Physics2D.OverlapBox(transform.position, scale, 0f, defalutContactFilter2D, hitColliderList);
    }

    public int StartOverlapCircle(Vector2 position, float radin, LayerMask layerMask)
    {
        contactFilter2D.SetLayerMask(layerMask);
        return Physics2D.OverlapCircle(position, radin, contactFilter2D, hitColliderList);
    }

    public int StartOverlapBox(Vector2 position, Vector2 scale, LayerMask layerMask)
    {
        contactFilter2D.SetLayerMask(layerMask);
        return Physics2D.OverlapBox(position, scale, 0f, contactFilter2D, hitColliderList);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapCollider : MonoBehaviour
{
    protected List<Collider> colliderLists;
    public List<Collider> ColliderList { get { return colliderLists; } }

    [SerializeField]
    protected LayerMask hitLayerMasks;


}

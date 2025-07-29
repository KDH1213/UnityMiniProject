using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    private Transform targetTransform;

    public void SetTarget(Transform tatget)
    {
        targetTransform = tatget;
    }

    private void Update()
    {
        transform.position = targetTransform.position;
    }
}

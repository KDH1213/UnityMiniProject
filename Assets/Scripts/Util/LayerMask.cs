using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity�� LayerMask ������ �޾ƿɴϴ�.
/// </summary>
public static class GetLayerMasks
{
    public static readonly int Ground = 1 << GetLayer.Ground;
    public static readonly int Enemy = 1 << GetLayer.Enemy;
    public static readonly int UI = 1 << GetLayer.UI;
    public static readonly int InteractionUI = 1 << GetLayer.InteractionUI;
}


/// <summary>
/// Unity�� Layer ������ �޾ƿɴϴ�.
/// </summary>
public static class GetLayer
{
    public static readonly int Ground = UnityEngine.LayerMask.NameToLayer("Ground");
    public static readonly int Enemy = UnityEngine.LayerMask.NameToLayer("Enemy");
    public static readonly int UI = UnityEngine.LayerMask.NameToLayer("UI");
    public static readonly int InteractionUI = UnityEngine.LayerMask.NameToLayer("InteractionUI");
}
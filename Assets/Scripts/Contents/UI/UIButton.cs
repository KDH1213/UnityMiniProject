using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIButton : MonoBehaviour
{
    [SerializeField]
    private Color interactableColor;

    [SerializeField]
    private new Graphic[] renderer;


    public void OnInTeractable(bool isOn)
    {
        var button = GetComponent<Button>();
        button.interactable = isOn;
        
        if (!button.interactable)
        {
            foreach (Graphic g in renderer)
            {
                g.color = interactableColor;
            }
        }
    }
}

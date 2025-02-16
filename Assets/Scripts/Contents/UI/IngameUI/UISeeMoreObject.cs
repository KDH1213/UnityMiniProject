using UnityEngine;

public class UISeeMoreObject : MonoBehaviour
{
    [SerializeField]
    private GameObject seeMoreObject;

    private void OnDisable()
    {
        seeMoreObject.SetActive(false);
    }

    public void OnSetActive()
    {
        seeMoreObject.SetActive(!seeMoreObject.activeSelf);
    }
}

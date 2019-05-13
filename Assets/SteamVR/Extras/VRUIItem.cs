using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VRUIItem : MonoBehaviour
{
    private BoxCollider boxCollider;
    private RectTransform rectTransform;
    public UIType uiType;

    public enum UIType
    {
        Button,
        Image,
        Slider,
        Panel,
        Text
    }
    private void OnEnable()
    {
        ValidateCollider();
    }

    private void OnValidate()
    {
        ValidateCollider();
    }

    private void ValidateCollider()
    {
        rectTransform = GetComponent<RectTransform>();

        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
        }

        boxCollider.size = rectTransform.sizeDelta;
    }

    public void Click()
    {
        if (uiType==UIType.Button)
        transform.GetComponent<Button>().onClick.Invoke();
    }
    public void OnEnter()
    {
        if (uiType == UIType.Button)
            transform.GetComponent<Image>().color= transform.GetComponent<Button>().colors.pressedColor;
    }
    public void OnExit()
    {
        if (uiType == UIType.Button)
            transform.GetComponent<Image>().color = transform.GetComponent<Button>().colors.normalColor;
    }
}
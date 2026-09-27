using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ButtonHoverTrigger : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    [SerializeField] private MenuHandController handController;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TriggerMove();
    }

    public void OnSelect(BaseEventData eventData)
    {
        TriggerMove();
    }

    private void TriggerMove()
    {
        if (handController != null)
        {
            handController.MoveToButton(rectTransform);
        }
    }
}

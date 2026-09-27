using UnityEngine;

public class MenuHandController : MonoBehaviour
{
    [Header("Hand Reference")]
    [SerializeField] private RectTransform hands;

    [Header("Settings")]
    [SerializeField] private bool smoothMove = true;
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float yOffset = 20f;

    private float targetY;
    private bool isActive = false;

    private void Update()
    {
        if (!isActive) return;

        Vector3 handPos = hands.anchoredPosition;

        float newY = smoothMove
            ? Mathf.Lerp(handPos.y, targetY, Time.deltaTime * moveSpeed)
            : targetY;

        hands.anchoredPosition = new Vector2(0f, newY);
    }

    public void MoveToButton(RectTransform buttonRect)
    {
        isActive = true;

        // Align Y position with the hovered button relative to the UI container
        targetY = buttonRect.anchoredPosition.y + yOffset;
    }
}

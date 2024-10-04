using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class ArcLayoutGroup : MonoBehaviour
{
    public float radius = 200f;        // Radius of the arc
    public float arcAngle = 180f;      // Total angle of the arc (in degrees)
    public float spacing = 10f;        // Spacing between elements
    public float yOffset = 0f;         // Y Offset to adjust the vertical position of the arc
    public float angOffset = 0f;         // Y Offset to adjust the vertical position of the arc
    public float animationDuration = 0.5f; // Duration for the smooth transition
    [SerializeField] bool autoUpdate;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void AlignChildrenInArc()
    {
        int childCount = rectTransform.childCount;

        if (childCount == 0)
            return;

        float totalSpacing = (childCount) * spacing;
        float angleOffset = arcAngle / Mathf.Max(1, childCount);
        float startAngle = -arcAngle / 2;

        for (int i = 0; i < childCount; i++)
        {
            RectTransform child = rectTransform.GetChild(i) as RectTransform;
            if (child != null)
            {
                float angle = (startAngle + angOffset) + (i * angleOffset);
                float angleInRadians = angle * Mathf.Deg2Rad;

                float xPos = Mathf.Sin(angleInRadians) * (radius + spacing * i);
                float yPos = Mathf.Cos(angleInRadians) * radius;

                child.DOLocalMove(new Vector2(xPos, yPos - yOffset), animationDuration);
                child.DOLocalRotate(new Vector3(0, 0, -angle / 2), animationDuration);
            }
        }
    }

    private void LateUpdate()
    {
        if (autoUpdate) AlignChildrenInArc();
    }
}

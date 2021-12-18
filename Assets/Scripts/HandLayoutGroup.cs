using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HandLayoutGroup : LayoutGroup
{
    [SerializeField]
    protected float _spacing = 0f;

    //[SerializeField]
    //protected float _angleDeg = 20f;

    [SerializeField]
    protected float _parabolaHeight = 200f;

    [SerializeField]
    protected float _parabolaPow = 2f;

    [SerializeField]
    protected float _parabolaRotationCoef = 1f;


    public float Spacing { get => _spacing; set => SetProperty(ref _spacing, value); }

    //public float AngleDeg { get => _angleDeg; set => SetProperty(ref _angleDeg, value); }

    public float ParabolaHeight { get => _parabolaHeight; set => SetProperty(ref _parabolaHeight, value); }

    public float ParabolaPow { get => _parabolaPow; set => SetProperty(ref _parabolaPow, value); }

    public float ParabolaRotationCoef { get => _parabolaRotationCoef; set => SetProperty(ref _parabolaRotationCoef, value); }


    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        CalcAlongAxis(0, false);
    }

    public override void CalculateLayoutInputVertical()
    {
        CalcAlongAxis(1, false);
    }

    public override void SetLayoutHorizontal()
    {
        SetChildrenX();
    }

    public override void SetLayoutVertical()
    {
        SetChildrenY();
    }

    protected void CalcAlongAxis(int axis, bool isVertical)
    {
        float combinedPadding = (axis == 0 ? padding.horizontal : padding.vertical);

        float totalMin = combinedPadding;
        float totalPreferred = combinedPadding;
        float totalFlexible = 0;

        bool alongOtherAxis = (isVertical ^ (axis == 1));
        for (int i = 0; i < rectChildren.Count; i++)
        {
            RectTransform child = rectChildren[i];
            float min = LayoutUtility.GetMinSize(child, axis);
            float preferred = LayoutUtility.GetPreferredSize(child, axis);
            float flexible = LayoutUtility.GetFlexibleSize(child, axis);

            //if ((axis == 0 ? childForceExpandWidth : childForceExpandHeight))
            //    flexible = Mathf.Max(flexible, 1);

            if (alongOtherAxis)
            {
                totalMin = Mathf.Max(min + combinedPadding, totalMin);
                totalPreferred = Mathf.Max(preferred + combinedPadding, totalPreferred);
                totalFlexible = Mathf.Max(flexible, totalFlexible);
            }
            else
            {
                totalMin += min + Spacing;
                totalPreferred += preferred + Spacing;

                // Increment flexible size with element's flexible size.
                totalFlexible += flexible;
            }
        }

        if (!alongOtherAxis && rectChildren.Count > 0)
        {
            totalMin -= Spacing;
            totalPreferred -= Spacing;
        }
        totalPreferred = Mathf.Max(totalMin, totalPreferred);
        SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, axis);
    }

    protected void SetChildrenX()
    {
        var c = rectChildren.Count;
        if (c < 1)
        {
            return;
        }

        var size = rectTransform.rect.width;
        var innerSize = size - padding.horizontal;
        var allChildrenWidth = rectChildren.Sum(c => c.sizeDelta.x);
        var allChildrenWidthWithSpacing = allChildrenWidth + Spacing * (c - 1);

        var useSpacing = allChildrenWidthWithSpacing <= innerSize;
        var overlapCoef = innerSize / allChildrenWidth;

        var pos = GetStartOffset(0, Mathf.Min(innerSize, allChildrenWidthWithSpacing));

        for (var i = 0; i < c; i++)
        {
            var child = rectChildren[i];

            // Rotation:

            // circle (arc)
            //child.rotation = Quaternion.Euler(0, 0, GetArcRotation(i, c, AngleDeg));

            // parabola
            child.rotation = Quaternion.Euler(0, 0, GetParabolaRotation(i, c, _parabolaPow, ParabolaRotationCoef));

            var childWidth = child.sizeDelta.x;
            if (childWidth == 0)
            {
                childWidth = LayoutUtility.GetPreferredWidth(child);
            }

            SetChildAlongAxis(child, 0, pos, childWidth);
            pos += useSpacing
                ? childWidth + Spacing
                : childWidth * overlapCoef;
        }
    }

    protected void SetChildrenY()
    {
        var c = rectChildren.Count;
        if (c < 1)
        {
            return;
        }

        var size = rectTransform.rect.height;
        var innerSize = size - padding.vertical;
        var maxChildHeight = rectChildren.Max(c => c.sizeDelta.y);
        var minHeightForParabola = Mathf.Min(innerSize - maxChildHeight, ParabolaHeight);

        for (var i = 0; i < rectChildren.Count; i++)
        {
            var child = rectChildren[i];
            var childHeight = child.sizeDelta.y;
            if (childHeight == 0)
            {
                childHeight = LayoutUtility.GetPreferredHeight(child);
            }

            var startOffset = GetStartOffset(1, minHeightForParabola + maxChildHeight);

            // Arc Y
            //startOffset -= GetArcDelta(i, c, AngleDeg, rectTransform.rect.width).y;

            // Parabola's Y
            startOffset += GetParabolaY(i, c, _parabolaPow, minHeightForParabola);

            SetChildAlongAxis(child, 1, startOffset, childHeight);
        }
    }

    private static float GetArcRotation(int currentIndex, int count, float angleDeg) =>
        angleDeg / 2 - angleDeg / (count - 1) * currentIndex;

    private static Vector2 GetArcDelta(int currentIndex, int count, float angleDeg, float width)
    {
        var angleRad = angleDeg * Mathf.Deg2Rad;
        var alpha = angleRad / 2;
        var gama = angleRad * currentIndex / (count - 1);
        var beta = alpha - gama;
        var omega = (Mathf.PI - alpha - beta) / 2;
        var r = width / (2 * Mathf.Sin(alpha));
        var l = gama * r;
        var dx = l * Mathf.Sin(omega);
        var dy = l * Mathf.Cos(omega);
        return new Vector2(dx, dy);
    }

    private static float GetParabolaRotation(int currentIndex, int count, float pow, float correction)
    {
        // We can use tangents for parabola which is 1st derivative of the function
        var symmetricIndex = currentIndex - (count - 1f) / 2f;
        var fraction = symmetricIndex / count;
        var powSafe = pow == 1 ? 1 : pow - 1;
        var tgAlpha = Mathf.Sign(fraction) * pow * Mathf.Pow(Mathf.Abs(fraction), powSafe);
        var alpha = -Mathf.Atan(tgAlpha) * Mathf.Rad2Deg * correction;
        return alpha;
    }

    private static float GetParabolaY(int currentIndex, int count, float pow, float height)
    {
        var median = (count - 1f) / 2f;
        var symmetricIndex = currentIndex - median;
        var y = height * Mathf.Pow(Mathf.Abs(symmetricIndex / median), pow);
        return y;
    }

}

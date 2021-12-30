// we can switch arc shape between based on circle or parabola 
#define ARC_AS_PARABOLA

using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CardsInHand.Scripts.UI
{
    public class HandLayoutGroup : LayoutGroup
    {
        [SerializeField]
        protected float _spacing = 0f;

        [SerializeField]
        protected float _arcHeight = 80f;

#if ARC_AS_PARABOLA
        [SerializeField]
        protected float _parabolaPow = 2f;

        [SerializeField]
        protected float _cardRotationCoef = .4f;
#else
        [SerializeField]
        protected float _angleDeg = 40f;
#endif

        [SerializeField]
        protected float _cardRotationCorrection = 0f;


        [Header("Animation")]
        [SerializeField]
        protected float _animationDuration = .5f;


        public float Spacing { get => _spacing; set => SetProperty(ref _spacing, value); }

        public float ArcHeight { get => _arcHeight; set => SetProperty(ref _arcHeight, value); }

#if ARC_AS_PARABOLA

        public float ParabolaPow { get => _parabolaPow; set => SetProperty(ref _parabolaPow, value); }


        public float CardRotationCoef { get => _cardRotationCoef; set => SetProperty(ref _cardRotationCoef, value); }
#else
        public float AngleDeg { get => _angleDeg; set => SetProperty(ref _angleDeg, value); }
#endif

        public float CardRotationCorrection { get => _cardRotationCorrection; set => SetProperty(ref _cardRotationCorrection, value); }


        public override void CalculateLayoutInputVertical()
        {
        }

        public override void SetLayoutHorizontal()
        {
            SetChildrenX();
        }

        public override void SetLayoutVertical()
        {
            SetChildrenY();
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
            var allChildrenWidth = rectChildren.Sum(rt => rt.sizeDelta.x);
            var allChildrenWidthWithSpacing = allChildrenWidth + Spacing * (c - 1);
            var lastChildWidth = rectChildren[c - 1].sizeDelta.x;

            var useSpacing = allChildrenWidthWithSpacing <= innerSize;
            var overlapCoef = allChildrenWidth <= innerSize
                ? innerSize / allChildrenWidth
                : (innerSize - lastChildWidth) / (allChildrenWidth - lastChildWidth);

            var pos = GetStartOffset(0, Mathf.Min(innerSize, allChildrenWidthWithSpacing));

            for (var i = 0; i < c; i++)
            {
                var child = rectChildren[i];

                // Rotation:
#if ARC_AS_PARABOLA
                // parabola
                var zAngle = GetParabolaRotation(i, c, ParabolaPow, CardRotationCoef);
#else
                // circle (arc)
                var zAngle = GetArcRotation(i, c, AngleDeg);
#endif

                child.DORotateQuaternion(Quaternion.Euler(0, 0, zAngle + CardRotationCorrection), _animationDuration);

                var childWidth = child.sizeDelta.x;
                if (childWidth == 0)
                {
                    childWidth = LayoutUtility.GetPreferredWidth(child);
                }

                DOTween.To(
                    () => child.anchoredPosition.x - childWidth * child.pivot.x,
                    x => SetChildAlongAxis(child, 0, x, childWidth),
                    pos,
                    _animationDuration);

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
            var maxChildHeight = rectChildren.Max(rt => rt.sizeDelta.y);
            var minHeightForParabola = Mathf.Min(innerSize - maxChildHeight, ArcHeight);

            for (var i = 0; i < rectChildren.Count; i++)
            {
                var child = rectChildren[i];
                var childHeight = child.sizeDelta.y;
                if (childHeight == 0)
                {
                    childHeight = LayoutUtility.GetPreferredHeight(child);
                }

                var startOffset = GetStartOffset(1, minHeightForParabola + maxChildHeight);

                // arrange children by Y to make an arc
#if ARC_AS_PARABOLA
                // Parabola's Y
                startOffset += GetParabolaY(i, c, ParabolaPow, minHeightForParabola);
#else
                // Arc Y
                startOffset -= GetArcDelta(i, c, AngleDeg, rectTransform.rect.width).y;
#endif


                SetChildAlongAxis(child, 1, startOffset, childHeight);
            }
        }

#if ARC_AS_PARABOLA
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
            if (count <= 1)
            {
                return height;
            }

            var median = (count - 1f) / 2f;
            var symmetricIndex = currentIndex - median;
            var y = height * Mathf.Pow(Mathf.Abs(symmetricIndex / median), pow);
            return y;
        }
#else
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
#endif
    }
}
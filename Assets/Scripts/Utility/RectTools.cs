using UnityEngine;
using UnityEngine.UI;

namespace CardsInHand.Scripts.Utility
{
    public class RectTools
    {
        public static (int width, int height) GetRectSize(Rect rect) =>
            ((int)rect.width, (int)rect.height);

        public static (int width, int height) GetRectTransformSize(RectTransform rectTransform) =>
            GetRectSize(rectTransform.rect);

        public static Rect CreateRectFromImage(Graphic image)
        {
            var (w, h) = GetRectTransformSize(image.rectTransform);
            return new Rect(0, 0, w, h);
        }
    }
}
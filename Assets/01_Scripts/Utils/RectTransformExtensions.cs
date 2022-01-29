using UnityEngine;
public static class RectTransformExtensions
{
    public static void SetLeft( RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight( RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop( RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom( RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

    public static void SetRectExceptBottom(RectTransform rt, float left = 0, float right = 0, float top = 0)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }
    public static void SetRect(RectTransform rt, float left = 0, float right = 0, float top = 0, float bottom = 0)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorSettings
{
    /// <summary>
    /// ColorSettings ::
    /// predefined color 적용 위해서 정적으로 선언
    /// </summary>
    public static readonly Color normalColor = new Color(1f, 1f, 1f);
    public static readonly Color darkColor = new Color(50f / 255f, 50f / 255f, 50f / 255f);
    public static readonly Color greenColor = new Color(0f, 1f, 0f);
    public static readonly Color yellowColor = new Color(1f, 1f, 0f);

    public static readonly Color cc1 = new Color(1f, 1f, 0f);
    public static readonly Color cc2 = new Color(175f / 255f, 175f / 255f, 0f);
    public static readonly Color cc3 = new Color(125f / 255f, 125f / 255f, 0f);
    public static readonly Color[] colorArr = { cc1, cc2, cc3 };


    public static readonly Color[] cardTypeColor = { Color.gray, Color.magenta, Color.cyan, Color.yellow };
    public static readonly Color[] efxColors = { Color.cyan, Color.gray, Color.green, Color.red, Color.magenta};

}

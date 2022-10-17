using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GamingColor
{
    // This variable is needed to set a value from Unity Editor
    public GamingColors color;
    public GamingColors Color => color;
    public Material Material => Resources.Load<Material>($"Materials/{_colorMapping[Color]}");

    private Dictionary<GamingColors, String> _colorMapping = new Dictionary<GamingColors, string>()
    {
        { GamingColors.Red, "Red color" },
        { GamingColors.Green, "Green color" },
        { GamingColors.Blue, "Blue color" }
    };
}

public enum GamingColors
{
    Red,
    Green,
    Blue
}

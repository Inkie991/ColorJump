using System;
using UnityEngine;

public class ColorChanger : ColorableObject
{
    [SerializeField]
    [Range(0f, 1f)]
    private float alpha;

    public override void SetColor(GamingColor value)
    {
        gamingColor = value;
        GetComponent<Renderer>().material.SetColor("_Color", ConvertColor(value.Material.color));
    }

    private Color ConvertColor(Color color)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}
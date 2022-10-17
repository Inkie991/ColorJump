using UnityEngine;

public abstract class ColorableObject : MonoBehaviour
{
    [SerializeField] protected GamingColor gamingColor;
    public GamingColor GamingColor
    {
        get => gamingColor;
        protected set => SetColor(value);
    }
    
    private void Awake()
    {
        SetColor(gamingColor);
    }

    public virtual void SetColor(GamingColor color)
    {
        gamingColor = color;
        GetComponent<Renderer>().material = color.Material;
    }
}
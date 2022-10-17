using System;
using UnityEngine;

public class Sector : ColorableObject
{
    public event EventHandler<GamingColor> SectorIsBroken;
    public bool IsBroken { get; private set; }
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        Managers.Audio.AttachSectorSounds(this);
        Managers.Vibration.AttachSectorVibration(this);
    }

    public void Break()
    {
        IsBroken = true;
        _animator.SetTrigger("Destroy");
        SectorIsBroken?.Invoke(this, gamingColor);
        Invoke("DestroySelf", 1f);
    }

    public override void SetColor(GamingColor color)
    {
        gamingColor = color;
        Renderer[] children = GetComponentsInChildren<Renderer>();

        foreach (Renderer child in children)
        {
            child.material = color.Material;
        } 
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    } 
}
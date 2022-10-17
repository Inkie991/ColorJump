using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ColorableObject
{
    [SerializeField] private GamingColors color;
    public event EventHandler Died;
    public event EventHandler Won;
    public GamingColors Color => color;

    public int Score { get; private set; }

    private void Start()
    {
        Score = 0;
        Managers.Audio.AttachPlayerSounds(this);
    }

    // TODO: Сделать красиво
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Finish"))
        {
            Won?.Invoke(this, null);
            Managers.UI.ToggleWin(true);
            gameObject.SetActive(false);
            return;
        }

        Sector sector = collider.GetComponent<Sector>();

        if (sector != null)
        {
            if (sector.GamingColor.Color != GamingColor.Color)
            {
                Death();
            }
            else
            {
                if (!sector.IsBroken)
                {
                    sector.Break();
                    Score++;
                }
            }   
        }

        ColorChanger colorChanger = collider.GetComponent<ColorChanger>();

        if (colorChanger != null)
        {
            GamingColor = colorChanger.GamingColor;   
        }
    }

    void Death()
    {
        Died?.Invoke(this, null);
        Managers.UI.ToggleLose(true);
        gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    private Bounceable _sphere;
    private bool _motionLocked = true;
    [SerializeField] private float yOffset = 1f;

    public Bounceable Sphere
    {
        get
        {
            return _sphere;
        }
        set
        {
            if (_sphere != null)
            {
                _sphere.BouncedOffPlatform -= LockMotion;
                _sphere.PassedOffPlatform -= UnlockMotion;
            }

            _motionLocked = true;
            _sphere = value;

            if (_sphere != null)
            {
                _sphere.BouncedOffPlatform += LockMotion;
                _sphere.PassedOffPlatform += UnlockMotion;
            }
        }
    }

    private void Start()
    {
        if (_sphere == null)
        {
            Sphere = GameObject.FindWithTag("Player").GetComponent<Bounceable>();
        }
    }

    void Update()
    {
        if (!_motionLocked)
        {
            Utils.SetY(gameObject, _sphere.transform.position.y + yOffset);
        }
    }

    private void LockMotion(object sender, float y)
    {
        _motionLocked = true;
        Utils.SetY(gameObject, y + yOffset);
    }

    private void UnlockMotion(object sender, float y)
    {
        _motionLocked = false;
        // TODO: fix minor glitch
    }

    public void ResetY()
    {
        Utils.SetY(gameObject, yOffset);
    }
}

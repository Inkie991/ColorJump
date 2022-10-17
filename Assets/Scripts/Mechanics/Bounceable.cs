using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounceable : MonoBehaviour
{
    private const int platformLayerMask = 1 << 6;
    
    [SerializeField] private float bounceFactor = 50f;
    [SerializeField] float gravity = 160f;
    [SerializeField] private float minimalPlatformDistance = 0.25f;

    private BoxCollider _boxCollider;
    public event EventHandler<float> BouncedOffPlatform;
    public event EventHandler<float> PassedOffPlatform;
    private float _velocity = 50f;
    private float radius;

    private GameObject _smudgePrefab;

    private void Start()
    {
        Managers.Audio.AttachBounceableSounds(this);
        _smudgePrefab = Resources.Load<GameObject>("Prefabs/Smudge");

        _boxCollider = GetComponent<BoxCollider>();
        radius = _boxCollider.bounds.size.y / 2;
    }

    private void FixedUpdate()
    {
        _velocity -= gravity * Time.fixedDeltaTime;

        Vector3 oldPosition = transform.position;

        oldPosition.y += _velocity * Time.fixedDeltaTime;
        
        transform.position = oldPosition;
        
        RaycastHit hit;
        if (Physics.Raycast(
                transform.position,
                Vector3.down,
                out hit,
                radius + minimalPlatformDistance,
                platformLayerMask))
        {
            var other = hit.collider;
            _velocity = bounceFactor; 
            BouncedOffPlatform?.Invoke(this, other.gameObject.transform.position.y); 
            DrawSmudge(other);
        }
            
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PassCollider"))
        {
            PassedOffPlatform?.Invoke(this, other.gameObject.transform.position.y);
        }
    }

    // TODO: Сделать нормально
    void DrawSmudge(Collider collider)
    {
        float offset = collider.bounds.size.y / 2;
        float yPos = collider.transform.position.y + offset;
        Vector3 smudgePosition = transform.position;
        smudgePosition.y = yPos;

        var smudge = Instantiate(_smudgePrefab, smudgePosition, Quaternion.Euler(90f, 0, 0));
        smudge.transform.parent = GameObject.FindWithTag("Column").transform;
    }
}

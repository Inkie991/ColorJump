using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    void Start()
    {
        Invoke("DestroySelf", 3f);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}

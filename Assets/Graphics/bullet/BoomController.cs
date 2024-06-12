using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomController : MonoBehaviour
{
    private float time;
    public float lifeTime = 2;
    private void Awake()
    {
        time = Time.time;
    }

    private void Update()
    {
        if (time + lifeTime < Time.time)
            Destroy(gameObject);
    }
}

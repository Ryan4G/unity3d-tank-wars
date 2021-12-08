using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayToDestory : MonoBehaviour
{
    private float _delaySeconds= 1.0f;

    public void Init(float seconds)
    {
        _delaySeconds = seconds;

        StartCoroutine(DelayDestory());
    }

    private void Start()
    {
    }

    private IEnumerator DelayDestory()
    {
        yield return new WaitForSeconds(_delaySeconds);

        Destroy(gameObject);
    }
}
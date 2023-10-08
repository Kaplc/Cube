using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
    public MeshRenderer mr;
    public MeshRenderer childMr;
    public Rigidbody rg;

    private bool isStart;

    private Coroutine pushCoroutine;

    private void OnEnable()
    {
        if (pushCoroutine != null)
        {
            StopCoroutine(pushCoroutine);
        }

        if (rg)
        {
            rg.useGravity = false;
            rg.velocity = Vector3.zero; 
            rg.angularVelocity = Vector3.zero;
        }
    }

    public void ChangeColor(Color color)
    {
        if (mr) mr.material.color = color;
        if (childMr) childMr.material.color = color;
    }

    /// <summary>
    /// 开始下落
    /// </summary>
    public void FallDown()
    {
        rg.useGravity = true;
        rg.AddTorque(new Vector3(Random.Range(1f, 30f), Random.Range(1f, 30f), Random.Range(1f, 30f)) * 20);
        pushCoroutine = StartCoroutine(Push());
    }

    private IEnumerator Push()
    {
        yield return new WaitForSeconds(2f);
        PoolManager.Instance.PushObject(gameObject);
    }
}
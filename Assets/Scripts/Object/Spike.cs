using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Random = UnityEngine.Random;


public class Spike : MonoBehaviour
{
    private Transform childTransform;
    private Coroutine triggerCoroutine;
    private Coroutine pushCoroutine;
    public Rigidbody rg;
    public MeshRenderer mr;
    private Color mrColor;
    
    // 陷阱动画偏移
    private float offSetZ;
    // 陷阱间隔时间
    private float cd;

    private bool isStart;

    private void OnEnable()
    {
        if (isStart)
        {
            StopCoroutine(pushCoroutine);
            // 关闭重力
            rg.useGravity = false;
            // 速度置零
            rg.velocity = Vector3.zero;
            rg.angularVelocity = Vector3.zero; 
            // 启动动画协程
            triggerCoroutine = StartCoroutine(TriggerSpike());
            // 随机陷阱时间间隔
            cd = Random.Range(0.8f, 2.0f);

            mr.material.color = mrColor;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isStart = true;
        // 找到子物体
        if (gameObject.CompareTag("GroundSpike"))
        {
            childTransform = gameObject.transform.Find("moving_spikes_b").GetComponent<Transform>();
            offSetZ = 0.15f;
        }
        else
        {
            childTransform = gameObject.transform.Find("smashing_spikes_b").GetComponent<Transform>();
            offSetZ = 0.6f;
        }

        // 启动动画协程
        triggerCoroutine = StartCoroutine(TriggerSpike());
        // 随机陷阱时间间隔
        cd = Random.Range(0.8f, 2.0f);

        mrColor = mr.material.color;
    }

    public void FallDown()
    {
        StopCoroutine(triggerCoroutine);
        // 启动重力
        rg.useGravity = true;
        // 加力随机旋转
        rg.AddTorque(new Vector3(Random.Range(1f, 30f), Random.Range(1f, 30f), Random.Range(1f, 30f)) * 20);
        pushCoroutine = StartCoroutine(Push());
    }

    private IEnumerator TriggerSpike()
    {
        while (true)
        {
            childTransform.localPosition = new Vector3(childTransform.localPosition.x, childTransform.localPosition.y, offSetZ);
            yield return new WaitForSeconds(cd);
            
            childTransform.localPosition = new Vector3(childTransform.localPosition.x, childTransform.localPosition.y, 0);
            yield return new WaitForSeconds(cd);
        }
    }

    private IEnumerator Push()
    {
        yield return new WaitForSeconds(2f);
        PoolManager.Instance.PushObject(gameObject);
    }
}
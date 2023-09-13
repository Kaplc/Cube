using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Spike : MonoBehaviour
{
    private Transform childTransform;
    private Coroutine triggerCoroutine;

    // 陷阱动画偏移
    private float offSetZ;

    // 陷阱间隔时间
    private float cd;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    public void ObjDestroy()
    {
        StopCoroutine(triggerCoroutine);
        Destroy(gameObject, 2);
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
}
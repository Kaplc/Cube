using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreatMapItem();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CreatMapItem()
    {
        // 奇数行
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject tile = Instantiate(Resources.Load<GameObject>("Tile"), new Vector3(0, 0, 0), Quaternion.Euler(-90, 45, 0), transform);
                // 每个方块偏移0.35
                tile.transform.position = new Vector3(0.35f + i * 0.35f, 0, j * 0.35f);
                // rgb数值/255f
                tile.GetComponent<MeshRenderer>().material.color = new Color(90 / 255f, 77 / 255f, 115 / 255f);
                tile.transform.Find("normal_a2").GetComponent<MeshRenderer>().material.color = new Color(90 / 255f, 77 / 255f, 115 / 255f);
            }
        }

        // 偶数行 
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject tile = Instantiate(Resources.Load<GameObject>("Tile"), new Vector3(0, 0, 0), Quaternion.Euler(-90, 45, 0), transform);
                // 先偏移0.175f
                tile.transform.position = new Vector3(0.175f + (i + 1) * 0.35f, 0, 0.175f + j * 0.35f);
                tile.GetComponent<MeshRenderer>().material.color = new Color(103 / 255f, 85 / 255f, 127 / 255f);
                tile.transform.Find("normal_a2").GetComponent<MeshRenderer>().material.color = new Color(103 / 255f, 85 / 255f, 127 / 255f);
            }
        }

        // 墙
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject tile = Instantiate(Resources.Load<GameObject>("Wall"), new Vector3(0, 0, 0), Quaternion.Euler(-90, 45, 0), transform);

                tile.transform.position = new Vector3(0.175f + (i * 5) * 0.35f, 0, 0.175f + j * 0.35f);
                tile.GetComponent<MeshRenderer>().material.color = new Color(82 / 255f, 59 / 255f, 102 / 255f);
            }
        }
    }
}
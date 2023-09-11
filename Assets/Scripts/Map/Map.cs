using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // 砖块深浅颜色
    public Color color = new Color(103 / 255f, 85 / 255f, 127 / 255f);
    public Color depthColor = new Color(90 / 255f, 77 / 255f, 115 / 255f);
    public Color wallColor = new Color(82 / 255f, 59 / 255f, 102 / 255f);

    // 砖块坐标数据集合
    public List<GameObject[]> tilePos = new List<GameObject[]>();

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
        for (int j = 0; j < 10; j++)
        {
            GameObject[] OddTiles = new GameObject[5];
            // 奇数行
            for (int i = 0; i < 5; i++)
            {
                GameObject tile = Instantiate(Resources.Load<GameObject>("Tile"), new Vector3(0, 0, 0), Quaternion.Euler(-90, 45, 0), transform);
                // 每个方块偏移0.35
                tile.transform.position = new Vector3(0.35f + i * 0.35f, 0, j * 0.35f);
                // rgb数值/255f
                tile.GetComponent<MeshRenderer>().material.color = depthColor;
                tile.transform.Find("normal_a2").GetComponent<MeshRenderer>().material.color = depthColor;
                // 命名
                tile.name = $"({j*2},{i})";

                OddTiles[i] = tile;
            }

            tilePos.Add(OddTiles);

            // 偶数行 
            GameObject[] evenTiles = new GameObject[6];
            for (int i = 0; i < 6; i++)
            {
                GameObject tile;
                if (i == 0 || i == 5)
                {
                    // 墙
                    tile = Instantiate(Resources.Load<GameObject>("Wall"), new Vector3(0, 0, 0), Quaternion.Euler(-90, 45, 0), transform);
                    tile.transform.position = new Vector3(0.175f + i * 0.35f, 0, 0.175f + j * 0.35f);
                    tile.GetComponent<MeshRenderer>().material.color = color;
                }
                else
                {
                    // 砖块
                    tile = Instantiate(Resources.Load<GameObject>("Tile"), new Vector3(0, 0, 0), Quaternion.Euler(-90, 45, 0), transform);
                    // 先偏移0.175f
                    tile.transform.position = new Vector3(0.175f + i * 0.35f, 0, 0.175f + j * 0.35f);
                    tile.GetComponent<MeshRenderer>().material.color = color;
                    tile.transform.Find("normal_a2").GetComponent<MeshRenderer>().material.color = color;
                }

                tile.name = $"({j*2+1},{i})";
                evenTiles[i] = tile;
            }

            tilePos.Add(evenTiles);
        }
    }
}
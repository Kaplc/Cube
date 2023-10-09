using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMap : MonoBehaviour
{
    private static ShopMap instance;
    public static ShopMap Instance => instance;


    public Color color;
    public Color depthColor;
    public Color wallColor;

    private float diagonal = 0.35921f;

    // 砖块坐标数据集合
    private List<GameObject[]> tilePos = new List<GameObject[]>();
    private List<Transform> createCubePos = new List<Transform>();
    private List<GameObject> allCube = new List<GameObject>();
    private Dictionary<int, int> cubePosDic = new Dictionary<int, int>();

    private bool moveCube;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (moveCube)
        {
            MoveCube();
        }
    }

    public void RightCube()
    {
        // 末尾放到第一, 其他元素向后移
        allCube.Insert(0, allCube[allCube.Count - 1]);
        allCube.RemoveAt(allCube.Count - 1);
        allCube[0].transform.position = createCubePos[0].position;
    }

    public void LeftCube()
    {
        // 原第一加到末尾，并移除原第一位置
        allCube.Add(allCube[0]);
        allCube.RemoveAt(0);
        allCube[allCube.Count - 1].transform.position = createCubePos[createCubePos.Count - 1].position;
    }

    private void CreateCube()
    {
        for (int i = 0; i < DataManager.Instance.shopData.cubeInfos.Count; i++)
        {
            var cube = Instantiate(Resources.Load<GameObject>(DataManager.Instance.shopData.cubeInfos[i].path));
            cube.transform.position = createCubePos[0].position + new Vector3(0, 0.124f, 0);
            allCube.Add(cube);
            // 位置映射
            cubePosDic.Add(i, i);
        }
    }

    private void MoveCube()
    {
        for (int i = 0; i < createCubePos.Count; i++)
        {
            allCube[i].transform.position = Vector3.Lerp(allCube[i].transform.position, createCubePos[i].position + new Vector3(0, 0.125f, 0),
                Time.deltaTime * 10);
        }
    }

    public void CreateMap()
    {
        // 10次x2=20行
        for (int z = 0; z < 10; z++)
        {
            GameObject[] OddTiles = new GameObject[5];
            // 偶数行
            for (int x = 0; x < 5; x++)
            {
                GameObject tile;

                // 创建普通方块
                tile = CreateNormalTile(depthColor);


                // 每个方块偏移0.35
                tile.transform.position = new Vector3(diagonal + x * diagonal, 0, z * diagonal);

                OddTiles[x] = tile;
            }

            tilePos.Add(OddTiles);

            // 奇数行 
            GameObject[] evenTiles = new GameObject[6];
            for (int x = 0; x < 6; x++)
            {
                GameObject tile;
                if (x == 0 || x == 5)
                {
                    // 挖空墙壁生成地砖
                    if (z >= 3 && z <= 5)
                    {
                        tile = CreateNormalTile(color);
                    }
                    else
                    {
                        // 墙
                        tile = PoolManager.Instance.GetObject("Tile/Wall");
                        tile.transform.SetParent(transform);
                        tile.transform.localPosition = Vector3.zero;
                        tile.transform.rotation = Quaternion.Euler(-90, 45, 0);
                        tile.transform.position = new Vector3(diagonal / 2 + x * diagonal, 0, diagonal / 2 + z * diagonal);
                        tile.GetComponent<Tile>().ChangeColor(wallColor);
                    }
                }
                else
                {
                    // 生成普通地砖
                    tile = CreateNormalTile(color);
                }

                // 先偏移0.175f,每个方块偏移0.35
                tile.transform.position = new Vector3(diagonal / 2 + x * diagonal, 0, diagonal / 2 + z * diagonal);

                evenTiles[x] = tile;
            }

            tilePos.Add(evenTiles);
        }

        // 地图创建完成创建cube的位置
        Transform leftHideCubePos = new GameObject("leftHideCubePos").transform;
        leftHideCubePos.position = tilePos[9][1].transform.position - new Vector3(diagonal * 2, 0, 0);
        createCubePos.Add(leftHideCubePos);
        createCubePos.Add(tilePos[9][1].transform);
        createCubePos.Add(tilePos[6][2].transform);
        createCubePos.Add(tilePos[9][4].transform);
        Transform rightHideCubePos = new GameObject("rightHideCubePos").transform;
        rightHideCubePos.position = tilePos[9][4].transform.position + new Vector3(diagonal * 2, 0, 0);
        createCubePos.Add(rightHideCubePos);

        CreateCube();
        moveCube = true;
    }

    private GameObject CreateNormalTile(Color tileColor)
    {
        GameObject normalTile = PoolManager.Instance.GetObject("Tile/Tile");

        normalTile.transform.SetParent(transform);
        normalTile.transform.localPosition = Vector3.zero;
        normalTile.transform.rotation = Quaternion.Euler(-90, 45, 0);
        normalTile.GetComponent<Tile>().ChangeColor(tileColor);
        return normalTile;
    }

    public void PushMap()
    {
        for (int i = 0; i < tilePos.Count; i++)
        {
            for (int j = 0; j < tilePos[i].Length; j++)
            {
                PoolManager.Instance.PushObject(tilePos[i][j]);
            }
        }

        tilePos.Clear();
    }
}
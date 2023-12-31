﻿using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class CubeInfo
{
    [XmlAttribute] public int id;
    [XmlAttribute] public string path;
    [XmlAttribute] public int unlockGemCount;
}

public class ShopData
{
    public List<CubeInfo> cubeInfos;
}
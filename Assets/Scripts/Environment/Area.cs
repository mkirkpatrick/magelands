using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Area
{
    public enum AreaShape { Box, Circle}
    public AreaShape shape;

    public int[] position;
    public int[] size;
    public int height;

    public int[,] areaMap;
    public int margin;

    public Area(int[] _position, int[] _size, int _height) {
        position = _position;
        size = _size;
        height = _height;
    }
}

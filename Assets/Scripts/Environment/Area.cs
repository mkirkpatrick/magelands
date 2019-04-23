using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area
{
    public enum AreaShape { Box, Circle}
    public AreaShape shape;

    public Vector2 position;
    public Vector2 size;
    public int height;

    public int margin;

    public Area(Vector2 _position, Vector2 _size, int _height) {
        position = _position;
        size = _size;
        height = _height;
    }
}

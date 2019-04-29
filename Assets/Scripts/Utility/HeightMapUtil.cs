using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HeightMapUtil
{
    // Creation
    public static int[,] RaiseSquare(int[,] _map, int[] _position, int[] _size, int height, bool ignoreZero = false) {
        int[,] newMap = _map;

        for (int x = _position[0]; x < _position[0] + _size[0]; x++) {
            for (int y = _position[1]; y < _position[1] + _size[1]; y++) {
                if (ignoreZero == true && newMap[x, y] == 0)
                    continue;

                newMap[x, y] = height;
            }
        }
        return newMap;
    }
    public static int[,] RoundMapCorners(int[,] _map, int insetValue, int iterations = 1)
    {
        int[,] newMap = _map;
        int[] size = new int[] { newMap.GetLength(0), newMap.GetLength(1) };
        int xCount = 0;
        int zCount = 0;

        Vector2 area;

        //Bottom Left
        for (int i = 0; i < iterations; i++)
        {
            if (i > 0)
            {
                int smallerInset = insetValue - (i * 2);
                area = new Vector2(Random.Range(smallerInset - 1, smallerInset + 2), Random.Range(smallerInset - 1, smallerInset + 2));

                for (int x = xCount; x < xCount + area.x; x++)
                {
                    for (int z = 0; z < area.y; z++)
                    {
                        newMap[x, z] = 0;
                    }
                }
                xCount += (int)area.x;

                area = new Vector2(Random.Range(smallerInset - 1, smallerInset + 2), Random.Range(smallerInset - 1, smallerInset + 2));

                for (int x = 0; x < area.x; x++)
                {
                    for (int z = zCount; z < zCount + area.y; z++)
                    {
                        newMap[x, z] = 0;
                    }
                }
                zCount += (int)area.y;
            }
            else
            {
                area = new Vector2(Random.Range(insetValue - 1, insetValue + 2), Random.Range(insetValue - 1, insetValue + 2));

                for (int x = 0; x < area.x; x++)
                {
                    for (int z = 0; z < area.y; z++)
                    {
                        _map[x, z] = 0;
                    }
                }
                xCount = (int)area.x;
                zCount = (int)area.y;
            }
        }

        //Top Left
        for (int i = 0; i < iterations; i++)
        {
            if (i > 0)
            {
                int smallerInset = insetValue - (i * 2);
                area = new Vector2(Random.Range(smallerInset - 1, smallerInset + 2), Random.Range(smallerInset - 1, smallerInset + 2));

                for (int x = xCount; x < xCount + area.x; x++)
                {
                    for (int z = size[1] - (int)area.y; z < size[1]; z++)
                    {
                        newMap[x, z] = 0;
                    }
                }
                xCount += (int)area.x;

                area = new Vector2(Random.Range(smallerInset - 1, smallerInset + 2), Random.Range(smallerInset - 1, smallerInset + 2));

                for (int x = 0; x < area.x; x++)
                {
                    for (int z = size[1] - ((int)area.y + zCount); z < size[1]; z++)
                    {
                        newMap[x, z] = 0;
                    }
                }
                zCount += (int)area.y;
            }
            else
            {
                area = new Vector2(Random.Range(insetValue - 1, insetValue + 2), Random.Range(insetValue - 1, insetValue + 2));

                for (int x = 0; x < area.x; x++)
                {
                    for (int z = size[1] - (int)area.y; z < size[1]; z++)
                    {
                        newMap[x, z] = 0;
                    }
                }
                xCount = (int)area.x;
                zCount = (int)area.y;
            }
        }

        //Top Right
        for (int i = 0; i < iterations; i++)
        {
            if (i > 0)
            {
                int smallerInset = insetValue - (i * 2);
                area = new Vector2(Random.Range(smallerInset - 1, smallerInset + 2), Random.Range(smallerInset - 1, smallerInset + 2));

                for (int x = size[0] - ((int)area.x + xCount); x < size[0]; x++)
                {
                    for (int z = size[1] - (int)area.y; z < size[1]; z++)
                    {
                        newMap[x, z] = 0;
                    }
                }
                xCount += (int)area.x;

                area = new Vector2(Random.Range(smallerInset - 1, smallerInset + 2), Random.Range(smallerInset - 1, smallerInset + 2));

                for (int x = size[0] - (int)area.x; x < size[0]; x++)
                {
                    for (int z = size[1] - ((int)area.y + zCount); z < size[1]; z++)
                    {
                        newMap[x, z] = 0;
                    }
                }
                zCount += (int)area.y;
            }
            else
            {
                area = new Vector2(Random.Range(insetValue - 1, insetValue + 2), Random.Range(insetValue - 1, insetValue + 2));

                for (int x = size[0] - (int)area.x; x < size[0]; x++)
                {
                    for (int z = size[1] - (int)area.y; z < size[1]; z++)
                    {
                        newMap[x, z] = 0;
                    }
                }
                xCount = (int)area.x;
                zCount = (int)area.y;
            }
        }

        //Bottom Right
        for (int i = 0; i < iterations; i++)
        {
            if (i > 0)
            {
                int smallerInset = insetValue - (i * 2);
                area = new Vector2(Random.Range(smallerInset - 1, smallerInset + 2), Random.Range(smallerInset - 1, smallerInset + 2));

                for (int x = size[0] - ((int)area.x + xCount); x < size[0]; x++)
                {
                    for (int z = 0; z < area.y; z++)
                    {
                        newMap[x, z] = 0;
                    }
                }
                xCount += (int)area.x;

                area = new Vector2(Random.Range(smallerInset - 1, smallerInset + 2), Random.Range(smallerInset - 1, smallerInset + 2));

                for (int x = size[0] - (int)area.x; x < size[0]; x++)
                {
                    for (int z = zCount; z < area.y + zCount; z++)
                    {
                        newMap[x, z] = 0;
                    }
                }
                zCount += (int)area.y;
            }
            else
            {
                area = new Vector2(Random.Range(insetValue - 1, insetValue + 2), Random.Range(insetValue - 1, insetValue + 2));

                for (int x = size[0] - (int)area.x; x < size[0]; x++)
                {
                    for (int z = 0; z < area.y; z++)
                    {
                        newMap[x, z] = 0;
                    }
                }
                xCount = (int)area.x;
                zCount = (int)area.y;
            }
        }

        return newMap;
    }
    public static void RaiseMountain(Land _land, int[] _position, int[] _size, int height) {

        int[] offSetPosition = new int[2] { _position[0] - (_size[0] / 2), _position[1] - (_size[1] / 2) };

        int[,] newMap = ExtractMapArea(_land.heightMap, offSetPosition, _size);

        int xValue = _size[0];
        int yValue = _size[1];

        int xOffset = 0;
        int yOffset = 0;

        for (int h = _land.levelHeight + 1; h < _land.levelHeight + height + 1; h++)
        {

            for (int x = 0; x < xValue; x++) {
                for (int y = 0; y < yValue; y++)
                {

                    if (newMap[x + xOffset, y + yOffset] != 0 && newMap[x + xOffset, y + yOffset] < h)
                        newMap[x + xOffset, y + yOffset] = h;
                }
            }
            int[,] innerMap = ExtractMapArea(newMap, new int[] { xOffset, yOffset }, new int[] { xValue, yValue });

            innerMap = RoughMapEdges(innerMap, 2, .04f);

            newMap = InsertMapIntoMap(newMap, innerMap, new int[] { xOffset, yOffset });

            xValue = Mathf.FloorToInt(xValue * .85f);
            yValue = Mathf.FloorToInt(yValue * .85f);
            xValue -= Random.Range(1, 3);
            yValue -= Random.Range(1, 3);

            int xDiff = (_size[0] - xValue) / 2;
            int yDiff = (_size[1] - yValue) / 2;

            xOffset = xDiff + Random.Range(-xDiff/2, (xDiff/2) + 1);
            yOffset = yDiff + +Random.Range(-yDiff / 2, (yDiff / 2) + 1);

            if (xValue < 2 || yValue < 2)
                break;
        }

        _land.heightMap = InsertMapIntoMap(_land.heightMap, newMap, offSetPosition);
    }

    // Manipulation
    public static int[,] RoughMapEdges(int[,] _map, int _cutDepth = 1, float _scale = .15f, bool setToZero = false)
    {

        int[,] newMap = _map;

        float scale = _scale;
        float randomX = Random.Range(0f, 1f);
        float randomY = Random.Range(0f, 1f);

        int xSize = newMap.GetLength(0);
        int ySize = newMap.GetLength(1);


        for (int x = 0; x < xSize; x++)
        {
            int cutDepth = Mathf.FloorToInt(Mathf.PerlinNoise((x * scale) + randomX, randomY) * (_cutDepth + 1));

            for (int depth = 0; depth < cutDepth; depth++)
            {
                if (setToZero == true || newMap[x, ySize - (depth + 1)] == 0)
                    newMap[x, ySize - (depth + 1)] = 0;
                else
                    newMap[x, ySize - (depth + 1)] -= 1;
            }
        }
        randomX = Random.Range(0f, 1f);
        randomY = Random.Range(0f, 1f);
        for (int z = 0; z < ySize; z++)
        {
            int cutDepth = Mathf.FloorToInt(Mathf.PerlinNoise(randomX, (z * scale) + randomY) * (_cutDepth + 1));

            for (int depth = 0; depth < cutDepth; depth++)
            {
                if (setToZero == true || newMap[xSize - (depth + 1), z] == 0)
                    newMap[xSize - (depth + 1), z] = 0;
                else
                    newMap[xSize - (depth + 1), z] -= 1;
            }
        }
        randomX = Random.Range(0f, 1f);
        randomY = Random.Range(0f, 1f);
        for (int x = 0; x < xSize; x++)
        {
            int cutDepth = Mathf.FloorToInt(Mathf.PerlinNoise((x * scale) + randomX, randomY) * (_cutDepth + 1));

            for (int depth = 0; depth < cutDepth; depth++)
            {
                if (setToZero == true || newMap[x, depth] == 0)
                    newMap[x, depth] = 0;
                else
                    newMap[x, depth] -= 1;
            }
        }
        randomX = Random.Range(0f, 1f);
        randomY = Random.Range(0f, 1f);
        for (int z = 0; z < ySize; z++)
        {
            int cutDepth = Mathf.FloorToInt(Mathf.PerlinNoise(randomX, (z * scale) + randomY) * (_cutDepth + 1));

            for (int depth = 0; depth < cutDepth; depth++)
            {
                if (setToZero == true || newMap[depth, z] == 0)
                    newMap[depth, z] = 0;
                else
                    newMap[depth, z] -= 1;
            }
        }
        return newMap;
    }
    public static int[,] RaiseFloorValues(int[,] _map, int lowestValue) {
        int[,] newMap = _map;

        for (int x = 0; x < _map.GetLength(0); x++) {
            for (int y = 0; y < _map.GetLength(1); y++)
            {
                if (newMap[x,y] < lowestValue && newMap[x,y] != 0)
                    newMap[x, y] = lowestValue;
            }
        }
        return newMap;  
    }
    public static int[,] LowerCeilingValues(int[,] _map, int highestValue) {
        int[,] newMap = _map;

        for (int x = 0; x < _map.GetLength(0); x++)
        {
            for (int y = 0; y < _map.GetLength(1); y++)
            {
                if (newMap[x, y] > highestValue)
                    newMap[x, y] = highestValue;
            }
        }
        return newMap;
    }
    public static int[,] SmoothMap(int[,] _map) {
        int[,] newMap = _map;
        bool[] smoothingFound;
        bool smoothingFinished = false;

        while (smoothingFinished == false) {

            smoothingFound = new bool[3];

            for (int x = 0; x < newMap.GetLength(0); x++)
            {
                for (int y = 0; y < newMap.GetLength(1); y++)
                {
                    int[] neighbors = GetMapNeighbors(newMap, new int[] { x, y });

                    // Check Holes
                    int counter = 0;
                    foreach (int i in neighbors) {
                        if (i > newMap[x, y])
                            counter++;
                    }
                    if (counter >= 3) {
                        newMap[x, y] += 1;  // Fill Hole
                        smoothingFound[0] = true;
                        break;
                    }

                    // Check Juts
                    counter = 0;
                    foreach (int i in neighbors)
                    {
                        if (i < newMap[x, y])
                            counter++;
                    }
                    if (counter >= 3)
                    {
                        newMap[x, y] -= 1;  // Remove Jut
                        smoothingFound[1] = true;
                        break;
                    }

                    // Check Grooves
                    if ( (neighbors[0] > newMap[x,y] && neighbors[2] > newMap[x, y]) || (neighbors[1] > newMap[x, y] && neighbors[3] > newMap[x, y]) ) {
                        newMap[x, y] += 1;  // Remove Groove
                        smoothingFound[2] = true;
                        break;
                    }
                    // Check Lines
                    /*
                    if ((neighbors[0] < newMap[x, y] && neighbors[2] < newMap[x, y]) || (neighbors[1] < newMap[x, y] && neighbors[3] < newMap[x, y]) )
                    {
                        newMap[x, y] -= 1;  // Remove Line
                        smoothingFound[3] = true;
                        break;
                    }
                    */
                }
            }

            smoothingFinished = true;
            foreach (bool check in smoothingFound) {
                if (check == true) {
                    smoothingFinished = false;
                }     
            }
        }

        return newMap;
    }

    // Helpers
    public static int[,] InsertMapIntoMap(int[,] _parentMap, int[,] _childMap, int[] _position) {

        int[,] newMap = _parentMap;

        for (int x = 0; x < _childMap.GetLength(0); x++) {

            for (int y = 0; y < _childMap.GetLength(1); y++) {

                newMap[_position[0] + x, _position[1] + y] = _childMap[x, y];  
            }
        }
        return newMap;
    }
    public static int[,] ExtractMapArea(int[,] _parentMap, int[] _position, int[] _size) {

        int[,] newMap = new int[_size[0], _size[1]];

        for (int x = 0; x < _size[0]; x++) {
            if (_position[0] + x == _parentMap.GetLength(0))
                break;

            for (int y = 0; y < _size[1]; y++) {
                if (_position[1] + y == _parentMap.GetLength(1))
                    break;

                newMap[x, y] = _parentMap[_position[0] + x, _position[1] + y];  
            }
        }
        return newMap;
    }

    public static int[] GetMapNeighbors(int[,] _map, int[] _position) {
        int[] neighbors = new int[4] { 0, 0, 0, 0 };

        if (_position[1] != _map.GetLength(1) - 1)
            neighbors[0] = _map[_position[0], _position[1] + 1];
        if (_position[0] != _map.GetLength(0) - 1)
            neighbors[1] = _map[_position[0] + 1, _position[1]];
        if (_position[1] != 0)
            neighbors[2] = _map[_position[0], _position[1] - 1];
        if (_position[0] != 0)
            neighbors[3] = _map[_position[0] - 1, _position[1]];

        return neighbors;
    }
}

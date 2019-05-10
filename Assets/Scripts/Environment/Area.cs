using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Area
{

    public int[] position;
    public int[] size;
    public int height;

    public int[,] areaMap; // 0: Not in area, 1: Area playable, 2: Area border
    public int margin;

    public Area(int[] _position, int[] _size, int _height, int _margin = 2) {
        position = _position;
        size = _size;
        height = _height;
        margin = _margin;
    }

    public void SetAreaBorder() {
        int[,] borderMap = new int[ size[0], size[1] ];

        //Find the border
        for (int i = 0; i < margin; i++) {
            for (int x = 0; x < size[0]; x++) {
                for (int y = 0; y < size[1]; y++) {
                    if (areaMap[x, y] >= 1) {

                        int[] borderNeighbors = HeightMapUtil.GetMapNeighbors(areaMap, new int[2] { x, y });

                        for (int n = 0; n < 4; n++) {
                            if (borderNeighbors[n] == 0) {
                                AddBorderNeighbors(x, y, i);
                            }
                        }
                    }
                }
            }
        }

        //Assign the border to areaMap
        for (int x = 0; x < size[0]; x++) {
            for (int y = 0; y < size[1]; y++) {
                if (borderMap[x, y] >= 1) {
                    areaMap[x, y] = 2;
                }
            }
        }

        void AddBorderNeighbors(int x, int y, int iteration){

            if (iteration < margin)
                borderMap[x, y] = iteration + 1;
            else
                return;

            int[] mapNeighbors = HeightMapUtil.GetMapNeighbors(areaMap, new int[2] { x, y });

            if (mapNeighbors[0] != 0)
                AddBorderNeighbors(x, y + 1, iteration + 1);
            if (mapNeighbors[1] != 0)
                AddBorderNeighbors(x + 1, y, iteration + 1);
            if (mapNeighbors[2] != 0)
                AddBorderNeighbors(x, y - 1, iteration + 1);
            if (mapNeighbors[3] != 0)
                AddBorderNeighbors(x - 1, y, iteration + 1);
        }
    }
}

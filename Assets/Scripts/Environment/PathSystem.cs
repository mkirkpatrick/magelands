using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathSystem
{
    public Vector2[] crossroads;
    public Path[] paths;
    public int[,] pathMap;

    public PathSystem(Land _land, Vector2[] _linkPoints, Vector2 _crossroadPoints) {

        crossroads = new Vector2[1] { _crossroadPoints };
        paths = new Path[_linkPoints.Length];
        pathMap = new int[_land.XSize, _land.ZSize];

        for (int i = 0; i < paths.Length; i++) {
            paths[i] = new Path( new Vector2[2] { _linkPoints[i], _crossroadPoints } );
        }
        GeneratePathMap();
        SmoothPaths(_land);
    }

    void GeneratePathMap() {
        foreach (Path path in paths) {
            for (float f = 0; f <= 1; f += .005f) {
                Vector2 bezierVector = MathUtil.CalculateCubicBezierPoint(f, path.pathPoints[0], path.bezierHandles[0], path.bezierHandles[1], path.pathPoints[1]);
                int[] mapPositions = new int[2] { Mathf.Clamp( Mathf.RoundToInt(bezierVector.x), 0, pathMap.GetLength(0) - 1), Mathf.Clamp(Mathf.RoundToInt(bezierVector.y), 0, pathMap.GetLength(1) - 1 ) };

                pathMap[ mapPositions[0], mapPositions[1] ] = 1;
                int[] neighbors = HeightMapUtil.GetMapNeighbors(pathMap, new int[2] { mapPositions[0], mapPositions[1] });

                if(mapPositions[0] != 0)
                    pathMap[ mapPositions[0] - 1, mapPositions[1] ] = 1;
                if (mapPositions[0] != pathMap.GetLength(0) - 1)
                    pathMap [mapPositions[0] + 1, mapPositions[1] ] = 1;
                if (mapPositions[1] != 0)
                    pathMap[ mapPositions[0], mapPositions[1] - 1 ] = 1;
                if (mapPositions[1] != pathMap.GetLength(1) - 1)
                    pathMap[mapPositions[0], mapPositions[1] + 1] = 1;
            }
        }     
    }

    void SmoothPaths(Land _land) {

        bool smoothed = true;

        while (smoothed == true) {
            smoothed = false;

            for (int x = 0; x < _land.XSize; x++) {
                for (int z = 0; z < _land.ZSize; z++) {
                    if (pathMap[x,z] == 1) {

                        if ( z != _land.ZSize - 1 && pathMap[x, z + 1] == 1) {
                            if (_land.heightMap[x, z] - _land.heightMap[x, z + 1] >= 2 && _land.heightMap[x, z + 1] != 0) {
                                _land.heightMap[x, z] -= 1;
                                smoothed = true;
                            }   
                        }
                        if ( x != _land.XSize - 1 && pathMap[x + 1, z] == 1)
                        {
                            if (_land.heightMap[x, z] - _land.heightMap[x + 1, z] >= 2 && _land.heightMap[x + 1, z] != 0)
                            {
                                _land.heightMap[x, z] -= 1;
                                smoothed = true;
                            }
                        }
                        if ( z != 0 && pathMap[x, z - 1] == 1)
                        {
                            if (_land.heightMap[x, z] - _land.heightMap[x, z - 1] >= 2 && _land.heightMap[x, z - 1] != 0) {
                                _land.heightMap[x, z] -= 1;
                                smoothed = true;
                            }
                                
                        }
                        if ( x != 0 && pathMap[x - 1, z] == 1)
                        {
                            if (_land.heightMap[x, z] - _land.heightMap[x - 1, z] >= 2 && _land.heightMap[x - 1, z] != 0) {
                                _land.heightMap[x, z] -= 1;
                                smoothed = true;
                            }     
                        }
                    }
                }
            }              
        }
    }

    [System.Serializable]
    public class Path {
        public Vector2[] pathPoints;
        public Vector2[] bezierHandles;

        public Path(Vector2[] _points) {
            pathPoints = _points;

            bezierHandles = new Vector2[2];
            Vector2 straightVector = pathPoints[1] - pathPoints[0];
            float[] offsets = new float[2] { straightVector[0] * Random.Range(.1f, .6f), straightVector[1] * Random.Range(.1f, .6f) };
            bezierHandles[0] = new Vector2(pathPoints[0].x + offsets[0], pathPoints[0].y + offsets[1] );

            straightVector = pathPoints[0] - pathPoints[1];
            offsets = new float[2] { straightVector[0] * Random.Range(.1f, .6f), straightVector[1] * Random.Range(.1f, .6f) };
            bezierHandles[1] = new Vector2(pathPoints[1].x + offsets[0], pathPoints[1].y + offsets[1]);
        }
    }
}


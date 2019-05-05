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

        //Update Crossraods
        UpdatePathMap(new int[2] { (int)crossroads[0].x, (int)crossroads[0].y });
        
        foreach (Path path in paths) {

            int xDiff = (int)path.pathPoints[1].x - (int)path.pathPoints[0].x;
            int yDiff = (int)path.pathPoints[1].y - (int)path.pathPoints[0].y;

            List<int> xRuns = new List<int>();
            List<int> yRuns = new List<int>();

            int xAbs = Mathf.Abs(xDiff);
            int yAbs = Mathf.Abs(yDiff);

            while (xAbs != 0) {
                if (xAbs > 12)
                {
                    int newRun = Random.Range(4, 9);
                    xRuns.Add(newRun);
                    xAbs -= newRun;
                }
                else {
                    if (xAbs <= 8)
                    {
                        xRuns.Add(xAbs);
                        xAbs -= xAbs;
                    }
                    else {
                        xRuns.Add( Mathf.FloorToInt(xAbs / 2) );

                        if(xAbs % 2 != 0)
                            xRuns.Add(Mathf.FloorToInt(xAbs / 2) + 1);
                        else
                            xRuns.Add(Mathf.FloorToInt(xAbs / 2));

                        xAbs -= xAbs;
                    }
                }   
            }

            int yVal = Mathf.FloorToInt(yAbs / xRuns.Count);
            int yRemainder = yAbs % xRuns.Count;

            for (int i = 0; i < xRuns.Count; i++) {
                yRuns.Add(yVal);
            }
            for (int i = 0; i < xRuns.Count; i++)
            {
                int randomOffset = Random.Range(-2, 3);

                if (i == xRuns.Count - 1)
                {
                    yRuns[i] += randomOffset;
                    yRuns[0] += -randomOffset;
                }
                else {
                    yRuns[i] += randomOffset;
                    yRuns[i + 1] += -randomOffset;
                }
            }
            //Add remainder to smallest run
            int lowestVal = 1000;
            int lowestIndex = 0;

            for (int i = 0; i < xRuns.Count; i++) {
                if (yRuns[i] < lowestVal) {
                    lowestIndex = i;
                    lowestVal = yRuns[i];
                }       
            }
            yRuns[lowestIndex] += yRemainder;

            int[] currentPosition = new int[2] { (int)path.pathPoints[0].x, (int)path.pathPoints[0].y };

            for (int i = 0; i < xRuns.Count; i++)
            {
                for (int x = 0; x < xRuns[i]; x++) {
                    UpdatePathMap(currentPosition);

                    if(xDiff >= 0)
                        currentPosition[0]++;
                    else
                        currentPosition[0]--;

                }
                for (int y = 0; y < yRuns[i]; y++)
                {
                    UpdatePathMap(currentPosition);

                    if (yDiff >= 0)
                        currentPosition[1]++;
                    else
                        currentPosition[1]--;
                }
            }

           
        }
        void UpdatePathMap(int[] _mapPositions)
        {

            pathMap[_mapPositions[0], _mapPositions[1]] = 1;

            if (_mapPositions[0] != 0) {
                pathMap[_mapPositions[0] - 1, _mapPositions[1]] = 1;
                if (_mapPositions[1] != 0)
                    pathMap[_mapPositions[0] - 1, _mapPositions[1] - 1] = 1;
                if (_mapPositions[1] != pathMap.GetLength(1) - 1)
                    pathMap[_mapPositions[0] - 1, _mapPositions[1] + 1] = 1;
            }

            if (_mapPositions[0] != pathMap.GetLength(0) - 1) {
                pathMap[_mapPositions[0] + 1, _mapPositions[1]] = 1;
                if (_mapPositions[1] != 0)
                    pathMap[_mapPositions[0] + 1, _mapPositions[1] - 1] = 1;
                if (_mapPositions[1] != pathMap.GetLength(1) - 1)
                    pathMap[_mapPositions[0] + 1, _mapPositions[1] + 1] = 1;
            }

            if (_mapPositions[1] != 0) {
                pathMap[_mapPositions[0], _mapPositions[1] - 1] = 1;

                if (_mapPositions[0] != 0)
                    pathMap[_mapPositions[0] - 1, _mapPositions[1] - 1] = 1;
                if (_mapPositions[0] != pathMap.GetLength(1) - 1)
                    pathMap[_mapPositions[0] + 1, _mapPositions[1] - 1] = 1;
            }

            if (_mapPositions[1] != pathMap.GetLength(1) - 1) {
                pathMap[_mapPositions[0], _mapPositions[1] + 1] = 1;
                if (_mapPositions[0] != 0)
                    pathMap[_mapPositions[0] - 1, _mapPositions[1] + 1] = 1;
                if (_mapPositions[0] != pathMap.GetLength(1) - 1)
                    pathMap[_mapPositions[0] + 1, _mapPositions[1] + 1] = 1;
            }
                

        }
    }

    void SmoothPaths(Land _land) {

        //Remove Path Juts
        for (int x = 0; x < pathMap.GetLength(0); x++)
        {
            for (int z = 0; z < pathMap.GetLength(1); z++)
            {
                if (pathMap[x, z] == 1)
                {
                    int[] neighbors = HeightMapUtil.GetMapNeighbors(pathMap, new int[2] { x, z });

                    int counter = 0;

                    foreach (int i in neighbors)
                        if (i == 1)
                            counter++;

                    if (counter <= 1)
                    {
                        pathMap[x, z] = 0;
                    }
                }
            }
        }

        bool smoothed = true;

        while (smoothed == true) {
            smoothed = false;

            for (int x = 0; x < _land.XSize; x++) {
                for (int z = 0; z < _land.ZSize; z++) {
                    if (pathMap[x,z] == 1) {

                        if ( z != _land.ZSize - 1) {
                            if (_land.heightMap[x, z] - _land.heightMap[x, z + 1] >= 2 && _land.heightMap[x, z + 1] != 0) {
                                _land.heightMap[x, z] -= 1;
                                smoothed = true;
                            }   
                        }
                        if ( x != _land.XSize - 1)
                        {
                            if (_land.heightMap[x, z] - _land.heightMap[x + 1, z] >= 2 && _land.heightMap[x + 1, z] != 0)
                            {
                                _land.heightMap[x, z] -= 1;
                                smoothed = true;
                            }
                        }
                        if ( z != 0)
                        {
                            if (_land.heightMap[x, z] - _land.heightMap[x, z - 1] >= 2 && _land.heightMap[x, z - 1] != 0) {
                                _land.heightMap[x, z] -= 1;
                                smoothed = true;
                            }
                                
                        }
                        if ( x != 0)
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


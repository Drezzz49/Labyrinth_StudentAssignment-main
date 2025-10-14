using System.Collections.Generic;
using UnityEngine;

public static class PathfindingAlgorithm
{
    /* <summary>
     TODO: Implement pathfinding algorithm here
     Find the shortest path from start to goal position in the maze.
     
     Dijkstra's Algorithm Steps:
     1. Initialize distances to all nodes as infinity
     2. Set distance to start node as 0
     3. Add start node to priority queue
     4. While priority queue is not empty:
        a. Remove node with minimum distance
        b. If it's the goal, reconstruct path
        c. For each neighbor:
           - Calculate new distance through current node
           - If shorter, update distance and add to queue
     
     MAZE FEATURES TO HANDLE:
     - Basic movement cost: 1.0 between adjacent cells
     - Walls: Some have infinite cost (impassable), others have climbing cost
     - Vents (teleportation): Allow instant travel between distant cells with usage cost
     
     AVAILABLE DATA STRUCTURES:
     - Dictionary<Vector2Int, float> - for tracking distances
     - Dictionary<Vector2Int, Vector2Int> - for tracking previous nodes (path reconstruction)
     - SortedSet<T> or List<T> - for priority queue implementation
     - mapData provides methods to check walls, vents, and boundaries
     
     HINT: Start simple with BFS (ignore wall costs and vents), then extend to weighted Dijkstra
     </summary> */

    private static List<Vector2Int> path = new List<Vector2Int>();
    private static Dictionary<Vector2Int, Vector2Int> previousSteps = new Dictionary<Vector2Int, Vector2Int>();
    private static Dictionary<Vector2Int, float> distances = new Dictionary<Vector2Int, float>();


    public static List<Vector2Int> FindShortestPath(Vector2Int start, Vector2Int goal, IMapData mapData)
    {
        // TODO: Implement your pathfinding algorithm here

        //path.Add(start + Vector2Int.down);
        //path.Add(start + 2 * Vector2Int.down);

        Vector2Int[] dirs = new Vector2Int[4] { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };
        Vector2Int previousStep = start;

        int maxFail = 1000;
        int failCount = 0;

        while (true)
        {
            Vector2Int newStep = new Vector2Int(-1, -1);
            List<Vector2Int> possibleSteps = new List<Vector2Int>();

            foreach (Vector2Int dir in dirs)
            {
                Vector2Int newStepTotest = previousStep + dir;

                if (!CheckWall(mapData, previousStep, dir))
                    possibleSteps.Add(newStepTotest);
            }

            if (possibleSteps.Count > 0)
                Debug.LogError("Hjälp");

            if (possibleSteps.Count > 1)
            {
                (Vector2Int pos, float distance) minDistance = (Vector2Int.zero, -1);

                foreach (Vector2Int step in possibleSteps)
                {
                    if (step == previousStep && possibleSteps.Count != 3)
                        continue;

                    if (minDistance.distance == -1)
                    {
                        minDistance = (step, Vector2Int.Distance(step, goal));
                        continue;
                    }

                    float checkDistance = Vector2Int.Distance(step, goal);
                    if (checkDistance < minDistance.distance)
                        minDistance = (step, checkDistance);
                }
            }
            else
                newStep = possibleSteps[0];

            if (newStep != new Vector2Int(-1, -1))
            {
                path.Add(newStep);
                failCount = 0;
            }
            else
                failCount++;

            if (previousStep == goal || failCount == maxFail)
                break;
        }

        foreach (Vector2Int step in path)
            Debug.Log(step);

        //Debug.LogWarning("FindShortestPath not implemented yet!");
        return path;
    }

    public static bool IsMovementBlocked(Vector2Int from, Vector2Int to, IMapData mapData)
    {
        // TODO: Implement movement blocking logic
        // For now, allow all movement so character can move while you work on pathfinding
        return false;
    }

    private static bool CheckWall(IMapData mapData, Vector2Int pos, Vector2Int dir)
    {
        {
            Vector2Int toCheck = pos + dir;
            if (toCheck.x < 0 || toCheck.y < 0 || toCheck.x >= mapData.Width || toCheck.y >= mapData.Height)
                return true;
        }

        if (dir == Vector2Int.up)
        {
            Vector2Int toCheck = pos + Vector2Int.up;
            return mapData.HasHorizontalWall(toCheck.x, toCheck.y);
        }
        else if (dir == Vector2Int.down)
        {
            Vector2Int toCheck = pos;
            return mapData.HasHorizontalWall(toCheck.x, toCheck.y);
        }
        else if (dir == Vector2Int.right)
        {
            Vector2Int toCheck = pos + Vector2Int.right;
            return mapData.HasVerticalWall(toCheck.x, toCheck.y);
        }
        else
        {
            Vector2Int toCheck = pos;
            return mapData.HasVerticalWall(toCheck.x, toCheck.y);
        }
    }
}
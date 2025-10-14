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

        //Dictionary<Vector2Int, Vector2Int> paths = new Dictionary<Vector2Int, Vector2Int>();


        Vector2Int myPos = start;
        int failsafe = 0;

        while (myPos != goal && failsafe <= 200)
        {
            //if (!CheckWall(mapData, myPos, Vector2Int.up))
            //{
            //    path.Add(myPos + Vector2Int.up); 
            //    myPos = myPos + Vector2Int.up;
            //}
            //else if (!CheckWall(mapData, myPos, Vector2Int.down))
            //{
            //    path.Add(myPos + Vector2Int.down);
            //    myPos = myPos + Vector2Int.down;
            //}
            //else if (!CheckWall(mapData, myPos, Vector2Int.right))
            //{
            //    path.Add(myPos + Vector2Int.right);
            //    myPos = myPos + Vector2Int.right;
            //}
            //else //vänster
            //{
            //    Debug.Log("entered");
            //    path.Add(myPos + Vector2Int.left);
            //    myPos = myPos + Vector2Int.left;
            //}
            //failsafe++;
            
            Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down , Vector2Int.left, Vector2Int.right };
            Vector2Int newDir = dirs[Random.Range(0, 4)];
            if (!CheckWall(mapData, myPos, newDir))
            {
                path.Add(myPos + newDir);
                myPos = myPos + newDir;
                //failsafe = 0;
            }
            failsafe++;
        }

        foreach (var item in path)
        {
            Debug.Log(item);
        }
        
       



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
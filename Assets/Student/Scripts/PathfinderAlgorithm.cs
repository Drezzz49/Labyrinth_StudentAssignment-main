using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    //private static Dictionary<Vector2Int, Vector2Int> previousSteps = new Dictionary<Vector2Int, Vector2Int>();
    //private static Dictionary<Vector2Int, float> distances = new Dictionary<Vector2Int, float>();


    public static List<Vector2Int> FindShortestPath(Vector2Int start, Vector2Int goal, IMapData mapData)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };




        AStar(start, goal, mapData);
        //BFS(start, goal, mapData);

        return path;
    }

    public static bool IsMovementBlocked(Vector2Int from, Vector2Int to, IMapData mapData)
    {
        // TODO: Implement movement blocking logic
        // For now, allow all movement so character can move while you work on pathfinding
        return false;
    }

    private static void AStar(Vector2Int start, Vector2Int goal, IMapData mapData)
    {
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        List<Vector2Int> OpenList = new List<Vector2Int> { start };
        HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();

        Dictionary<Vector2Int, float> gCost = new Dictionary<Vector2Int, float> { [start]=0 }; //beskriver kostnaden att komma givna positionen från start
        Dictionary<Vector2Int, float> hCost = new Dictionary<Vector2Int, float> { [start]=Heuristic(start, goal) }; //beskriver kostnaden för fågelvägen från givna positionen till målet
        Dictionary<Vector2Int, Vector2Int> tempPath = new Dictionary<Vector2Int, Vector2Int>();

        while (OpenList.Count > 0)
        {
            OpenList.OrderBy(pos => gCost[pos] + hCost[pos]); //sortera listan beroende på hur högt värde de har på gCost och hCost på pos (vi räknar ut f kostnaden g+h=f)
            Vector2Int currentPosition = OpenList[0]; //hämtar positionen med bäst kostnad (lägst kostnad)

            if (currentPosition == goal) { break; }

            OpenList.Remove(currentPosition);
            closedList.Add(currentPosition);


            foreach (var dir in dirs)
            {
                var newPos = currentPosition + dir;
                if (CheckWall(mapData, currentPosition, dir) || closedList.Contains(newPos)) //om det finns en vägg eller vi redan varit på positionen
                {
                    continue;
                }

                float newGScore = gCost[currentPosition] + 1;

                if (!gCost.ContainsKey(newPos) || newGScore < gCost[newPos]) //vi kollar om vi har räknat ut gCost tidigare, eller om det nya gCost är bättre, då uppdaterar vi det 
                {
                    gCost[newPos] = newGScore;
                    hCost[newPos] = Heuristic(newPos, goal);

                    tempPath[newPos] = currentPosition;

                    if (!OpenList.Contains(newPos))
                    {
                        OpenList.Add(newPos);
                    }
                }
            }
        }

        Vector2Int tempPosition = goal;

        while (tempPath.ContainsKey(tempPosition)) //bygger upp path genom att gå baklänges igenom tempPath
        {
            tempPosition = tempPath[tempPosition];
            path.Add(tempPosition);
        }
        path.Reverse();
        path.Add(goal);

    }

    private static float Heuristic(Vector2Int current, Vector2Int goal) //kostnad fågelvägen
    {
        var x = Mathf.Abs(current.x - goal.x);
        var y = Mathf.Abs(current.y - goal.y);

        return x + y;
    }


    private static void BFS(Vector2Int start, Vector2Int goal, IMapData mapData)
    {
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        List<Vector2Int> haveBeen = new List<Vector2Int>();

        stack.Push(start);
        haveBeen.Add(start);

        while (true)
        {
            var currentPos = stack.Peek();
            if (currentPos == goal)
            {
                break;
            }

            bool noNewPath = true;

            foreach (var dir in dirs)
            {
                if (!CheckWall(mapData, currentPos, dir) && !haveBeen.Contains(currentPos + dir))
                {
                    noNewPath = false; // vi kunde gå
                    stack.Push(currentPos + dir);
                    haveBeen.Add(currentPos + dir);
                    break;
                }

            }
            if (noNewPath == true)
            {
                stack.Pop();
            }
        }
        while (stack.Count > 0)
        {
            path.Add(stack.Pop());
        }
        path.Reverse();
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
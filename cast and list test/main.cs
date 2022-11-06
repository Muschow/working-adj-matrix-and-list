using Godot;
using System;
using System.Collections.Generic;

public class main : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    List<Vector2> nodeList = new List<Vector2>();
    int width = 11;
    int height = 8;
    const int wall = 1;
    const int node = 0;
    // Called when the node enters the scene tree for the first time.
    private int convertVectorToInt(Vector2 temp)
    {

        if (temp.x == 0)
        {
            return (int)Math.Abs(temp.y);
        }
        else
        {
            return (int)Math.Abs(temp.x);
        }
    }

    private void PrintNodeList()
    {
        GD.Print("Printing NodeList: ");
        foreach (var thing in nodeList)
        {
            GD.Print(thing);
        }
    }

    private void GenerateNodeList()
    {
        TileMap nodeTilemap = GetNode<TileMap>("NodeTilemap");
        for (int i = 1; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (nodeTilemap.GetCell(j, i) == node)
                {
                    nodeList.Add(new Vector2(j, i));
                }
            }
        }
        PrintNodeList();
    }

    private bool IfWallOrNodeBetween(Vector2 vec1, Vector2 vec2)
    {
        //vec1 should be the smaller vector
        GD.Print("IfOnWall: " + " Vec1: " + vec1 + ", Vec2: " + vec2);
        TileMap mazeTilemap = GetNode<TileMap>("MazeTilemap");
        TileMap nodeTilemap = GetNode<TileMap>("NodeTilemap");
        if (vec1.x == vec2.x)
        {
            for (int y = (int)vec1.y; (int)y < vec2.y; y++)
            {
                if ((mazeTilemap.GetCell((int)vec1.x, y) == wall) || (nodeTilemap.GetCell((int)vec1.x, y) == node && y != vec1.y && y != vec2.y))
                {
                    GD.Print("reached get cell x: " + vec1.x + ",y: " + y);
                    return true;
                }
            }
            return false;
        }
        else if (vec1.y == vec2.y)
        {
            for (int x = (int)vec1.x; (int)x < vec2.x; x++)
            {
                if (mazeTilemap.GetCell(x, (int)vec1.y) == wall || (nodeTilemap.GetCell(x, (int)vec1.y) == node && x != vec1.x && x != vec2.x))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return true;
        }

    }


    private int[,] GenerateAdjMatrix()
    {
        TileMap mazeTilemap = GetNode<TileMap>("MazeTilemap");

        int[,] adjMatrix = new int[nodeList.Count, nodeList.Count];

        for (int i = 0; i < nodeList.Count; i++)
        {
            //int nodeCounter = 0;
            for (int j = 0; j < nodeList.Count; j++)
            {

                Vector2 v1 = nodeList[i];
                Vector2 v2 = nodeList[j];
                if (v1.x == v2.x || v1.y == v2.y)
                {
                    Vector2 vec1;
                    Vector2 vec2;
                    //swaps so v1 is smaller and v2 is bigger
                    if (v1 <= v2)
                    {
                        vec1 = v1;
                        vec2 = v2;
                    }
                    else
                    {
                        vec1 = v2;
                        vec2 = v1;
                    }

                    //if on wall, no edge, else put weight
                    if (IfWallOrNodeBetween(vec1, vec2))
                    {
                        adjMatrix[i, j] = 0;
                    }
                    else
                    {
                        adjMatrix[i, j] = convertVectorToInt(vec2 - vec1);
                        //nodeCounter++;
                    }

                }
            }
        }

        return adjMatrix;
    }

    private void PrintAdjMatrix(int[,] adjMatrix)
    {
        GD.Print("Actual adj matrix :");
        for (int i = 0; i < nodeList.Count; i++)
        {
            GD.PrintRaw("\n");
            for (int j = 0; j < nodeList.Count; j++)
            {
                GD.PrintRaw(adjMatrix[i, j]);
            }
        }
    }

    private List<Tuple<Vector2, int>>[] GenerateAdjList()
    {
        TileMap mazeTilemap = GetNode<TileMap>("MazeTilemap");

        List<Tuple<Vector2, int>>[] adjList = new List<Tuple<Vector2, int>>[nodeList.Count]; //adjList is an array, size number of nodes, containing a list of tuples (neighbour nodes, weight)
        for (int i = 0; i < adjList.Length; i++) //init list
        {
            adjList[i] = new List<Tuple<Vector2, int>>();
        }



        for (int i = 0; i < nodeList.Count; i++)
        {
            for (int j = 0; j < nodeList.Count; j++)
            {

                Vector2 v1 = nodeList[i];
                Vector2 v2 = nodeList[j];
                if (v1.x == v2.x || v1.y == v2.y)
                {
                    Vector2 vec1;
                    Vector2 vec2;
                    //swaps so v1 is smaller (vec1) and v2 is bigger (vec2)
                    if (v1 <= v2)
                    {
                        vec1 = v1;
                        vec2 = v2;
                    }
                    else
                    {
                        vec1 = v2;
                        vec2 = v1;
                    }

                    int neighbourVal = convertVectorToInt(vec2 - vec1);
                    //if on wall, no edge, else put weight
                    if ((!IfWallOrNodeBetween(vec1, vec2)) && (neighbourVal != 0))
                    {
                        Tuple<Vector2, int> newTuple = new Tuple<Vector2, int>(nodeList[j], convertVectorToInt(vec2 - vec1));
                        adjList[i].Add(newTuple); //this is not working for some reason, object reference not set to instance of object
                    }
                }
            }
        }

        return adjList;
    }

    private void PrintAdjList(List<Tuple<Vector2, int>>[] adjList)
    {
        GD.Print("\nPrinting adj list");
        for (int i = 0; i < adjList.Length; i++)
        {
            GD.Print("\n");
            GD.PrintRaw(nodeList[i] + " | ");
            foreach (var item in adjList[i])
            {
                GD.PrintRaw(item + ", ");
            }
        }
    }
    public override void _Ready()
    {
        GenerateNodeList();

        int[,] adjMatrix = GenerateAdjMatrix();
        PrintAdjMatrix(adjMatrix);

        List<Tuple<Vector2, int>>[] adjList = GenerateAdjList();
        PrintAdjList(adjList);

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}

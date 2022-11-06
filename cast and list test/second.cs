using Godot;
using System;
using System.Collections.Generic;
public class second : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Node2D main = GetNode<Node2D>("/root/main");

        // //List<int> newIntList = (List<int>)main.Get("intList"); //for some reason it either cant node.Get a list or something else idk
        // GD.Convert(main.Get("intArray2D"), Variant.Type.IntArray);
        // int[,] newIntArray = (int[,])main.Get("intArray2D");
        // foreach (int item in newIntArray)
        // {
        //     GD.PrintRaw(item);
        // }


        // int newNumber = (int)main.Get("number");
        // GD.Print("number " + newNumber);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}

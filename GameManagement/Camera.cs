﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Camera : Object3D
{

    public float viewAngleX, viewAngleY;
    Vector3 viewVertex;
    Vector2 prevMousePos, mouseDiff;


    public Camera(string id = "")
        : base("", id)
    {
        prevMousePos = new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2);
    }

    public Vector3 ViewVertex
    {
        get { return viewVertex; }
    }

    public override void HandleInput(InputHelper input)
    {
        mouseDiff.X = input.MousePosition.X - prevMousePos.X;
        mouseDiff.Y = input.MousePosition.Y - prevMousePos.Y;
        viewAngleX += mouseDiff.X * 0.005f;
        viewAngleY -= mouseDiff.Y * 0.005f;
        if (viewAngleY > 1)
            viewAngleY = 1;
        if (viewAngleY < -1)
            viewAngleY = -1;
        viewVertex = new Vector3(position.X + (float)(Math.Cos(viewAngleX) * Math.Cos(viewAngleY)),
                                position.Y + (float)Math.Sin(viewAngleY),
                                position.Z + (float)(Math.Sin(viewAngleX) * Math.Cos(viewAngleY)));
        prevMousePos = new Vector2(input.MousePosition.X, input.MousePosition.Y);
    }
}
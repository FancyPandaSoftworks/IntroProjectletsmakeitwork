﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

class RandomLevel
{
    private Dictionary<Point, Tile> tileList;
    private List<Point> keyList;
    private Tile newTile;
    private Point position;
    private Random random;
    private int tiles;

    public static void Main()
    {
        RandomLevel level = new RandomLevel(20);

    }

    public Dictionary<Point, Tile> TileList
    {
        get { return tileList; }

    }

    public List<Point> KeyList
    {
        get
        {
            return keyList;
        }
    }
    public RandomLevel(int tiles = 10)
    {
        tileList = new Dictionary<Point, Tile>();
        keyList = new List<Point>();
        newTile = new EntryTile();
        random = new Random();

        newTile.tilePosition = Point.Zero;
        position = newTile.tilePosition;
        tileList.Add(position, newTile);
        keyList.Add(position);

        this.tiles = tiles;

        CreateMainPath();

        for (int i = random.Next(1, tiles / 4); i > 0; i--)
            CreateSidePath(random.Next(3, tiles / 2));

    }

    private void CreateMainPath()
    {
        while (tiles >= 0)
        {
            List<Point> possiblePositions = new List<Point>();

            //Look where a Tile can be placed
            Point position = new Point(newTile.tilePosition.X - 1, newTile.tilePosition.Y);

            if (CanPlaceMainPathTile(position))
                possiblePositions.Add(position);

            position.X = newTile.tilePosition.X;
            position.Y = newTile.tilePosition.Y + 1;

            if (CanPlaceMainPathTile(position))
                possiblePositions.Add(position);

            position.X = newTile.tilePosition.X + 1;
            position.Y = newTile.tilePosition.Y;

            if (CanPlaceMainPathTile(position))
                possiblePositions.Add(position);

            position.X = newTile.tilePosition.X;
            position.Y = newTile.tilePosition.Y - 1;

            if (CanPlaceMainPathTile(position))
                possiblePositions.Add(position);


            if (tiles > 0)
            {
                //Choose where to place the Tile
                newTile = CreateMainPathTile(possiblePositions[random.Next(0, possiblePositions.Count - 1)]);
                tileList.Add(newTile.tilePosition, newTile);
            }
            else
            {
                //Choose where to place the Tile
                newTile = CreateExitTile(possiblePositions[random.Next(0, possiblePositions.Count - 1)]);
                tileList.Add(newTile.tilePosition, newTile);
            }

            keyList.Add(newTile.tilePosition);

            Console.WriteLine("X:" + newTile.tilePosition.X + "\nY:" + newTile.tilePosition.Y + "");

            tiles--;

        }

    }


    //Creating SidePath From here
    private void CreateSidePath(int tiles)
    {
        Point nextPosition = keyList[random.Next(0, keyList.Count - 1)];
        Tile nextTile = CanCreateSidePath(nextPosition);

        if (nextTile != null)
        {

            tileList.Add(nextTile.tilePosition, nextTile);
            keyList.Add(nextTile.tilePosition);

            while (tiles > 0)
            {
                List<Point> possiblePositions = GetPossiblePositions(nextTile.tilePosition);

                if (possiblePositions.Count > 0)
                {
                    //Choose where to place the Tile
                    nextTile = new SidePathTile(possiblePositions[random.Next(0, possiblePositions.Count - 1)]);
                    tileList.Add(nextTile.tilePosition, nextTile);
                    keyList.Add(nextTile.tilePosition);
                    Console.WriteLine("Side: \nX: " + nextTile.tilePosition.X + "\nY: " + nextTile.tilePosition.Y);
                }
                else
                {
                    break;
                }
                tiles--;
            }

        }
        else
        {
            CreateSidePath(tiles);
        }
    }

    private Tile CanCreateSidePath(Point nextPosition)
    {
        List<Point> possibleEntrys = new List<Point>();
        Point test = nextPosition;

        test.X = nextPosition.X - 1;
        if (CanPlaceSideEntry(test))
        {
            possibleEntrys.Add(test);
        }
        test.X = nextPosition.X + 1;
        if (CanPlaceSideEntry(test))
        {
            possibleEntrys.Add(test);
        }
        test.X = nextPosition.X;
        test.Y = nextPosition.Y - 1;
        if (CanPlaceSideEntry(test))
        {
            possibleEntrys.Add(test);
        }
        test.Y = nextPosition.Y + 1;
        if (CanPlaceSideEntry(test))
        {
            possibleEntrys.Add(test);
        }
        return (new SidePathEntryTile(possibleEntrys[random.Next(0, possibleEntrys.Count - 1)]));
    }

    private bool CanPlaceSideEntry(Point currentPosition)
    {
        return (!tileList.ContainsKey(currentPosition) && GetPossiblePositions(currentPosition).Count > 0);
    }

    //Checking if a tile can be placed
    private List<Point> GetPossiblePositions(Point currentPosition)
    {
        List<Point> possiblePositions = new List<Point>();

        Point position = new Point(currentPosition.X - 1, currentPosition.Y);

        if (!tileList.ContainsKey(position))
            possiblePositions.Add(position);

        position.X = currentPosition.X;
        position.Y = currentPosition.Y + 1;

        if (!tileList.ContainsKey(position))
            possiblePositions.Add(position);

        position.X = currentPosition.X + 1;
        position.Y = currentPosition.Y;

        if (!tileList.ContainsKey(position))
            possiblePositions.Add(position);

        position.X = currentPosition.X;
        position.Y = currentPosition.Y - 1;

        if (!tileList.ContainsKey(position))
            possiblePositions.Add(position);

        return possiblePositions;

    }

    private bool CanPlaceMainPathTile(Point currentPosition)
    {
        return (!tileList.ContainsKey(currentPosition) && GetPossiblePositions(currentPosition).Count == 3);
    }

    private Tile CreateMainPathTile(Point point)
    {
        return new MainPathTile(point);
    }

    private Tile CreateExitTile(Point point)
    {
        return new ExitTile(point);
    }

}
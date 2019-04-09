using System;
using System.Collections.Generic;
using Tulum.Models;

namespace Tulum.Bussiness
{
    public class Board
    {


        public Board()
        {
        }

        private void InitializeBoard()
        {
            // We start with three tiles at the top
            int rowSize = 3;
            const int maxRowSize = 5;
            const int boardSize = 18;

            int i = 0;

            while(i < boardSize)
            {
                rowSize = 
                i++;
            }
        }

    private IDictionary<TileType, int> GetAvailableTiles()
        {
            var availableTiles = new Dictionary<TileType, int>();

            availableTiles.Add(TileType.Brick, 3);
            availableTiles.Add(TileType.Grain, 4);
            availableTiles.Add(TileType.Stone, 3);
            availableTiles.Add(TileType.Wood, 4);
            availableTiles.Add(TileType.Wool, 4);
            availableTiles.Add(TileType.None, 1);

            return availableTiles;
        }
    }

    public class Tile
    {
        private ResourceType _resourceType;
        private int _index;
        // First neighbor is the top and we proceed clockwise
        private Tile[] _neighbors = new Tile[6];

        public Tile(ResourceType resourceType, int index)
        {
            this._resourceType = resourceType;
            this._index = index;
        }

        public void SetNeighbor(Tile neighbor, int index)
        {
            if (this._neighbors[index] == null)
            {
                this._neighbors[index] = neighbor;
            }

            throw new InvalidOperationException("Neighbor cannot be reset");
        }
    }
}



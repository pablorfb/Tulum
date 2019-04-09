using System;
using System.Collections.Generic;
using Tulum.Models;

namespace Tulum.Bussiness
{
	public class Board
	{

        private readonly int BOARD_SIZE = 18;

		public Board()
		{
		}

        private InitializeBoard()
        {

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

        public AddNeighbor(Tile neighbor, int index)
        {
            if (this._neighbors[index])
            {

            }
        }
    }
}



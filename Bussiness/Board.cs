using System;
using System.Collections.Generic;
using Tulum.Models;

namespace Tulum.Bussiness
{
    public class Board
    {
        // We start with three tiles at the top
        private readonly int[] BoardMeasurements = new int[] { 3, 4, 5, 4, 3 };

        private IDictionary<Tuple<int, int>, Tile> _boardTilesMap = new Dictionary<Tuple<int, int>, Tile>();

        public Board()
        {
        }

        public bool TrySetSettlement(PlayerColor color, CornerLocation location)
        {


            return true;
        }

        private void InitializeBoard()
        {
            var tileTypes = this.GetTileTypeOrder();

            var tileIndex = 0;
            for (var i = 0; i < this.BoardMeasurements.Length; i++)
            {
                int rowSize = this.BoardMeasurements[i];
                for (var j = 0; j < rowSize; j++)
                {
                    var location = Tuple.Create(i, j);
                    this._boardTilesMap[location] = new Tile(tileTypes[tileIndex++]);
                }
            }
        }

        private TileType[] GetTileTypeOrder()
        {
            var availableTiles = new Dictionary<TileType, int>();

            availableTiles.Add(TileType.Brick, 3);
            availableTiles.Add(TileType.Grain, 4);
            availableTiles.Add(TileType.Stone, 3);
            availableTiles.Add(TileType.Wood, 4);
            availableTiles.Add(TileType.Wool, 4);
            availableTiles.Add(TileType.None, 1);

            // 19 is the number of tiles
            var tiles = new TileType[19];

            var i = 0;
            foreach (var tileType in availableTiles)
            {
                for (var j = 0; j < tileType.Value; j++)
                {
                    tiles[i++] = tileType.Key;
                }
            }

            this.Shuffle(tiles);

            return tiles;
        }

        private void Shuffle(TileType[] tiles)
        {
            var random = new Random();
            for (var i = 0; i < tiles.Length; i++)
            {
                var j = random.Next(tiles.Length - i - 1) + i;
                var t = tiles[i];
                tiles[i] = tiles[j];
                tiles[j] = t;
            }
        }

        private bool ValidateSettlementAvailability(CornerLocation location)
        {
            var tile = this.GetTileByCornerLocation(location);
            if (tile == null || !tile.IsCornerEmpty(location.Corner))
            {
                return false;
            }

            switch(location.Corner)
            {
                case TileCorner.Top:
                    if(location.Row == 0)
                    {
                        return true;
                    }
                    else if
                case TileCorner.TopRight:
                case TileCorner.BottomRight:
                case TileCorner.BottomLeft:
                case TileCorner.TopLeft:
            }
        }

        private Tile GetTileByCornerLocation(CornerLocation cornerLocation)
        {
            Tile tile = null;
            this._boardTilesMap.TryGetValue(Tuple.Create(cornerLocation.Row, cornerLocation.Column), out tile);
            return tile;
        }
    }

    public class Tile
    {
        private TileType _resourceType;
        // First neighbor is the top and we proceed clockwise
        private PlayerColor?[] _bridges = new PlayerColor?[6];
        private PlayerColor?[] _settlements = new PlayerColor?[6];

        public Tile(TileType resourceType)
        {
            this._resourceType = resourceType;
        }

        public void SetBridge(PlayerColor player, TileSide tileSide)
        {
            var index = (int)tileSide;
            if (this._bridges[index] == null)
            {
                this._bridges[index] = player;
            }

            throw new InvalidOperationException("Location already contains a bridge");
        }

        public void SetSettlement(PlayerColor player, TileCorner tileCorner)
        {
            var index = (int)tileCorner;
            if (this._settlements[index] == null)
            {
                this._settlements[index] = player;
            }

            throw new InvalidOperationException("Location already contains a settlement");
        }

        public bool IsSideEmpty(TileSide tileSide)
        {
            return this._bridges[(int)tileSide] == null;
        }

        public bool IsCornerEmpty(TileCorner tileCorner)
        {
            return this._settlements[(int)tileCorner] == null;
        }
    }
}




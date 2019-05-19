
using System.Linq;
using System;
using System.Collections.Generic;
using Tulum.Models;
namespace Tulum.Bussiness
{
    public class Board
    {
        // We start with three tiles at the top
        private readonly int[] BoardMeasurements = new int[] { 3, 4, 5, 4, 3 };

        private readonly int[] RowOffset = new int[] { 1, 1, 0, 1, 1 };


        private IDictionary<BoardCoordinates, Tile> _boardTilesMap = new Dictionary<BoardCoordinates, Tile>();

        public Board()
        {
        }

        public bool TrySetSettlement(PlayerColor player, CornerLocation location)
        {
            var tile = this.GetTileByLocation(this.CornerLocationToBoardCoordinates(location));
            if (tile == null || !tile.IsCornerEmpty(location.Corner))
            {
                return false;
            }

            var neighbours = tile.GetNeighbours(location.Corner);

            if (!neighbours.All(x => x.Tile.IsCornerEmpty(x.Corner))) {
                return false;
            }

            tile.SetSettlement(player, location.Corner);
            neighbours.ToList().ForEach(neighbour => neighbour.Tile.SetSettlement(player, neighbour.Corner));

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
                    this._boardTilesMap[new BoardCoordinates(i, j)] = new Tile(tileTypes[tileIndex++]);
                }
            }
        }

        private void SetNeighbours(Tile tile, BoardCoordinates location)
        {
            foreach(TileSide tileSide in Enum.GetValues(typeof(TileSide)))
            {
                Tile neighbourTile = null;
                TileSide oppositeTileSide = TileSide.BottomRight; ;
                switch (tileSide)
                {
                    case TileSide.TopLeft:
                        var topLeftLocation = new BoardCoordinates(location.Row - 1, location.Column - this.RowOffset[location.Row]);
                        neighbourTile = this.GetTileByLocation(topLeftLocation);
                        oppositeTileSide = TileSide.BottomRight;
                        break;
                    case TileSide.TopRight:
                        var topRightLocation = new BoardCoordinates(location.Row - 1, location.Column);
                        neighbourTile = this.GetTileByLocation(topRightLocation);
                        oppositeTileSide = TileSide.BottomLeft;
                        break;
                    case TileSide.Right:
                        var rightLocation = new BoardCoordinates(location.Row, location.Column + 1);
                        neighbourTile = this.GetTileByLocation(rightLocation);
                        oppositeTileSide = TileSide.Left;
                        break;
                    case TileSide.BottomRight:
                        var bottomRightLocation = new BoardCoordinates(location.Row + 1, location.Column);
                        neighbourTile = this.GetTileByLocation(bottomRightLocation);
                        oppositeTileSide = TileSide.TopLeft;
                        break;
                    case TileSide.BottomLeft:
                        var bottomLeftLocation = new BoardCoordinates(location.Row + 1, location.Column - this.RowOffset[location.Row]);
                        neighbourTile = this.GetTileByLocation(bottomLeftLocation);
                        oppositeTileSide = TileSide.TopRight;
                        break;
                    case TileSide.Left:
                        var leftLocation = new BoardCoordinates(location.Row, location.Column - 1);
                        neighbourTile = this.GetTileByLocation(leftLocation);
                        oppositeTileSide = TileSide.Right;
                        break;
                }

                if (neighbourTile == null)
                {
                    neighbourTile.SetNeighbour(tile, tileSide);
                    tile.SetNeighbour(neighbourTile, oppositeTileSide);
                }

            }
        }

        private int GetRowOffset(BoardCoordinates boardLocation, TileSide tileSide)
        {

            var midBoard = BoardMeasurements.Length / 2;

            switch (tileSide)
            {
                case TileSide.TopLeft:
                    return boardLocation.Row < midBoard ? 1 : 0;
                case TileSide.TopRight:
                    return boardLocation.Row > midBoard ? 1 : 0;
                case TileSide.BottomRight:
                    return boardLocation.Row < midBoard ? 1 : 0;
                case TileSide.BottomLeft:
                    return boardLocation.Row > midBoard ? 1 : 0;
                default:
                    return 0;
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


        private BoardCoordinates CornerLocationToBoardCoordinates(CornerLocation location) => new BoardCoordinates(location.Column, location.Row);

        private Tile GetTileByLocation(BoardCoordinates location)
        {
            Tile tile = null;
            this._boardTilesMap.TryGetValue(location, out tile);
            return tile;
        }
    }

    public class Tile
    {
        private TileType _resourceType;
        // First neighbor is the top and we proceed clockwise
        private PlayerColor?[] _bridges = new PlayerColor?[6];
        private PlayerColor?[] _settlements = new PlayerColor?[6];
        private IDictionary<TileSide, Tile> _neighbours = new Dictionary<TileSide, Tile>();

        public Tile(TileType resourceType)
        {
            this._resourceType = resourceType;
        }

        public void SetNeighbour(Tile tile, TileSide tileSide)
        {
            Tile existingNeighbour;
            this._neighbours.TryGetValue(tileSide, out existingNeighbour);

            if (existingNeighbour != null && existingNeighbour != tile)
            {
                throw new InvalidOperationException("Neigbours cannot be reset");
            }

            this._neighbours[tileSide] = tile;
        }

        public IEnumerable<NeighbourTile> GetNeighbours(TileCorner corner)
        {
            var neighbourTileSides = this.GetTileSidesInCorner(corner);

            IList<NeighbourTile> tiles = new List<NeighbourTile>();

            neighbourTileSides.ToList().ForEach(tileSide =>
            {
                Tile tile = null;
                if (this._neighbours.TryGetValue(tileSide, out tile))
                {
                    tiles.Add(new NeighbourTile() {
                        Tile = tile,
                        Side = tileSide,
                        Corner = this.GetMatchingCorner(corner, tileSide)
                    });
                }
            });

            return tiles;
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

        private TileCorner GetMatchingCorner(TileCorner thisCorner, TileSide neighboursTileSide)
        {
            var errorMessage = "There are no matching corners for this corner and tile side match";
            switch (thisCorner)
            {
                case TileCorner.Top:
                    if (neighboursTileSide == TileSide.TopLeft) {
                        return TileCorner.BottomLeft;
                    } else if (neighboursTileSide == TileSide.TopRight)
                    {
                        return TileCorner.BottomRight;
                    }
                    else
                    {
                        throw new InvalidOperationException(errorMessage);
                    }
                case TileCorner.TopRight:
                    if (neighboursTileSide == TileSide.TopRight)
                    {
                        return TileCorner.Bottom;
                    }
                    else if (neighboursTileSide == TileSide.Right)
                    {
                        return TileCorner.TopLeft;
                    }
                    else
                    {
                        throw new InvalidOperationException(errorMessage);
                    }
                case TileCorner.BottomRight:
                    if (neighboursTileSide == TileSide.Right)
                    {
                        return TileCorner.BottomLeft;
                    }
                    else if (neighboursTileSide == TileSide.BottomRight)
                    {
                        return TileCorner.Top;
                    }
                    else
                    {
                        throw new InvalidOperationException(errorMessage);
                    }
                case TileCorner.Bottom:
                    if (neighboursTileSide == TileSide.BottomRight)
                    {
                        return TileCorner.TopLeft;
                    }
                    else if (neighboursTileSide == TileSide.BottomLeft)
                    {
                        return TileCorner.TopRight;
                    }
                    else
                    {
                        throw new InvalidOperationException(errorMessage);
                    }
                case TileCorner.BottomLeft:
                    if (neighboursTileSide == TileSide.BottomLeft)
                    {
                        return TileCorner.Top;
                    }
                    else if (neighboursTileSide == TileSide.Left)
                    {
                        return TileCorner.BottomRight;
                    }
                    else
                    {
                        throw new InvalidOperationException(errorMessage);
                    }
                case TileCorner.TopLeft:
                    if (neighboursTileSide == TileSide.Left)
                    {
                        return TileCorner.TopRight;
                    }
                    else if (neighboursTileSide == TileSide.TopLeft)
                    {
                        return TileCorner.Bottom;
                    }
                    else
                    {
                        throw new InvalidOperationException(errorMessage);
                    }
                default:
                    throw new InvalidOperationException(errorMessage);
            }
        }

        private IEnumerable<TileSide> GetTileSidesInCorner(TileCorner corner)
        {
            switch(corner)
            {
                case TileCorner.Top:
                    return new TileSide[] { TileSide.TopLeft, TileSide.TopRight };
                case TileCorner.TopRight:
                    return new TileSide[] { TileSide.TopRight, TileSide.Right };
                case TileCorner.BottomRight:
                    return new TileSide[] { TileSide.Right, TileSide.BottomRight };
                case TileCorner.Bottom:
                    return new TileSide[] { TileSide.BottomRight, TileSide.BottomLeft };
                case TileCorner.BottomLeft:
                    return new TileSide[] { TileSide.BottomLeft, TileSide.Left };
                case TileCorner.TopLeft:
                    return new TileSide[] { TileSide.Left, TileSide.TopLeft };
                default:
                    throw new InvalidOperationException("Invalid Tile side");
            }
        }
    }
    public struct NeighbourTile
    {
        public Tile Tile;
        public TileCorner Corner;
        public TileSide Side;
    }
}








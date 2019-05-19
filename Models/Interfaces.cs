using System;
using System.Collections.Generic;
using System.Text;

namespace Tulum.Models
{

    public enum GameState
    {
        SettingUp, Running, Finished
    }

    public enum PlayerState
    {
        Waiting, ReadyToStartTurn, InTurn, InTurnWaitingForOfferResponse, EvaluatingOffer
    }

    public enum PlayerColor
    {
        Red, White, Blue, Orange
    }

    public interface ICoordinator
    {
        bool ChangeTurn(IPlayer player);

    }

    public interface IBoard
    {
        bool TryAddBridge(BoardCoordinates boardCoordinates, IPlayer player);

        bool TryAddTown(BoardCoordinates boardCoordinates, IPlayer player);

        bool TryAddCity(BoardCoordinates boardCoordinates, IPlayer player);
    }

    public interface IPlayer
    {
        PlayerState GetState();

        IDictionary<ResourceType, int> GetResources();

        bool UseResources(IDictionary<ResourceType, int> resourceQuantity);

        void NotifyTurnStart();

        void NotifyTurnEnd();

        void PresentOffer(Deal offer);

        bool TakeOffer(Deal Offer);

        bool MakeOffer(Deal deal, IPlayer player);

        bool TryAddBridge(BoardCoordinates boardCoordinates);

        bool TryAddTown(BoardCoordinates boardCoordinates);

        bool TryAddCity(BoardCoordinates boardCoordinates);

    }

    public struct Deal
    {
        public ResourceQuantity[] Requests;
        public ResourceQuantity[] Offers;
    }

    public struct ResourceQuantity
    {
        public readonly ResourceType ResourceType;
        public readonly int Quantity;

        public ResourceQuantity(ResourceType resourceType, int quantity)
        {
            this.ResourceType = resourceType;
            this.Quantity = quantity;
        }
    }

    public enum ResourceType
    {
        Wool, Wood, Brick, Stone, Grain
    }

    public enum TileType
    {
        Wool, Wood, Brick, Stone, Grain, None
    }

    public class BoardCoordinates
    {
        public int Row;
        public int Column;

        public BoardCoordinates(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BoardCoordinates))
            {
                return false;
            }
            var other = (BoardCoordinates)obj;
            return this.Column == other.Column && this.Row == other.Row;
        }
    }

    public struct IPlayerInfo
    {
        string Id;
        PlayerColor color;
    }

    public struct CornerLocation
    {
        public int Row;
        public int Column;
        public TileCorner Corner;
    }


    public enum TileSide
    {
        TopLeft, TopRight, Right, BottomRight, BottomLeft, Left
    }

    public enum TileCorner
    {
        Top, TopLeft, BottomLeft, Bottom, BottomRight, TopRight

    }
}


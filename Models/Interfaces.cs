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

    public enum ResourceType {
        Wool, Wood, Brick, Stone, Grain
    }

    public enum TileType {
        Wool, Wood, Brick, Stone, Grain, None
    }

    public struct BoardCoordinates
    {
        int Row;
    }

    public struct IPlayerInfo
    {
        string Id;
    }


}

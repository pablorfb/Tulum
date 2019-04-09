using System;
using System.Collections.Generic;
using System.Text;
using Tulum.Models;

namespace Tulum.Bussiness
{
    class Player : IPlayer
    {
        private IPlayerInfo _info;
        private IBoard _board;
        private PlayerState _playerState = PlayerState.ReadyToStartTurn;

        IDictionary<ResourceType, int> _resources = new Dictionary<ResourceType, int>();

        public Player(IPlayerInfo info, IBoard board)
        {
            this._info = info;
            this._board = board;
        }

        public IDictionary<ResourceType, int> GetResources()
        {
            return this._resources;
        }

        public PlayerState GetState()
        {
            return this._playerState;
        }

        public void NotifyTurnEnd()
        {
            this.ValidateState(PlayerState.InTurn);
            this._playerState = PlayerState.Waiting;
        }

        public void NotifyTurnStart()
        {
            this.ValidateState(PlayerState.Waiting);
            this._playerState = PlayerState.InTurn;
        }

        public void PresentOffer(Deal deal)
        {
            throw new NotImplementedException();
        }

        public bool TakeOffer(Deal deal)
        {
            this.ValidateState(PlayerState.Waiting);

            foreach (var request in deal.Requests) {
                if (this._resources[request.ResourceType] < request.Quantity)
                {
                    return false;
                }
            }

            foreach (var request in deal.Requests)
            {
                this._resources[request.ResourceType] -= request.Quantity;
            }

            foreach (var offer in deal.Offers)
            {
                this._resources[offer.ResourceType] += offer.Quantity;
            }

            return true;
        }


        public bool MakeOffer(Deal deal, IPlayer player)
        {
            this.ValidateState(PlayerState.InTurn);

            foreach (var request in deal.Offers)
            {
                if (this._resources[request.ResourceType] < request.Quantity)
                {
                    return false;
                }
            }

            player.PresentOffer(deal);

            this._playerState = PlayerState.InTurnWaitingForOfferResponse;

            return true;
        }

        public bool UseResources(IDictionary<ResourceType, int> resourceQuantities)
        {
            this.ValidateState(PlayerState.InTurn);

            foreach (KeyValuePair<ResourceType, int> entry in resourceQuantities)
            {
                if (this._resources[entry.Key] < entry.Value)
                {
                    return false;
                }
            }

            foreach (KeyValuePair<ResourceType, int> entry in resourceQuantities)
            {
                this._resources[entry.Key] -= entry.Value;
            }

            return true;
        }


        public bool TryAddBridge(BoardCoordinates boardCoordinates)
        {

        }

        public bool TryAddTown(BoardCoordinates boardCoordinates)
        {

        }

        public bool TryAddCity(BoardCoordinates boardCoordinates)
        {

        }

        private void ValidateState(PlayerState desiredState)
        {
            if (this._playerState != desiredState)
            {
                throw new InvalidOperationException(string.Format("Invalid operationf for state: {0}", desiredState));
            }
        }
    }
}

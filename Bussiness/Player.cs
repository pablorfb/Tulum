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

        IDictionary<ResourceTypes, int> _resources = new Dictionary<ResourceTypes, int>();

        public Player(IPlayerInfo info, IBoard board)
        {
            this._info = info;
            this._board = board;
        }

        public IDictionary<ResourceTypes, int> GetResources()
        {
            return this._resources;
        }

        public PlayerState GetState()
        {
            return this._playerState;
        }

        public void NotifyTurnEnd()
        {
            if ()
            {

            }
            this._playerState = PlayerState.Waiting;
        }

        public void NotifyTurnStart()
        {
            this._playerState = PlayerState.InTurn;
        }

        public void PresentOffer(Deal deal)
        {
            throw new NotImplementedException();
        }

        public bool TakeOffer(Deal deal)
        {   
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
            if (this._playerState != PlayerState.InTurn)
            {
                throw new InvalidOperationException();
            }

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

        public bool UseResources(IDictionary<ResourceTypes, int> resourceQuantities)
        {
            foreach (KeyValuePair<ResourceTypes, int> entry in resourceQuantities)
            {
                if (this._resources[entry.Key] < entry.Value)
                {
                    return false;
                }
            }

            foreach (KeyValuePair<ResourceTypes, int> entry in resourceQuantities)
            {
                this._resources[entry.Key] -= entry.Value;
            }

            return true;
        }

        private void ValidateState(PlayerState desiredState)
        {
            if (this._playerState != desiredState)
            {
                throw new InvalidOperationException();
            }
        }
    }
}

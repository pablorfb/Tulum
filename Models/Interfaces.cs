using System;
using System.Collections.Generic;
using System.Text;

namespace Tulum.Models
{

    enum GameStates
    {
        Initializing, Running, Finished
    }

    enum PlayerStates
    {
        Waiting, ReadyToStartTurn, InTurn
    }

    interface ITulum
    {
        void StartGame();

        void NotifyTurnStart();

        void NotifyTurnEnd();

    }

    interface IPlayer
    {
        PlayerStates GetState();

        void NotifyTurnStart();

        void NotifyTurnEnd();

        bool PresentOffer(Deal offer);

    }

    struct Deal
    {
        public [] Requests;
        object[] Offers;
    }

    struct
    {
        ResourceType
    }

    enum ResourceTypes {
        
    }


}

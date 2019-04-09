using System;
using System.Collections.Generic;
using System.Linq;
using Tulum.Models;

namespace Tulum.Bussiness
{
    public class Coordinator : ICoordinator
    {


        private readonly IEnumerable<IPlayer> _players;
        private GameState _gameState;

        private int _currentTurn = 0;
        private int _elapsedRounds = 0;
        
        public Coordinator(IPlayerInfo[] playersInfo)
        {
            if (playersInfo == null || playersInfo.Length < 2 || playersInfo.Length > 5)
            {
                throw new ArgumentException("Number of players needs to be between 2-5");
            }

            this._players = playersInfo.Select(info =>
            {
                return new Player(info);
            });
            this._gameState = GameState.SettingUp;
        }

        public bool ChangeTurn(IPlayer requestingPlayer)
        {
            if (requestingPlayer == this._players[this._currentTurn])
            {

                this._currentTurn++;

                if (this._currentTurn == this._players.Length)
                {
                    this._currentTurn = 0;
                    this._elapsedRounds++;
                }

                if (this._elapsedRounds == 2 && this._gameState == GameState.SettingUp)
                {
                    this._gameState = GameState.Running;
                }

                return true;
            }

            return false;
        }
    }
}

/*
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GameObject;
using UniRx;
using UniRx.Triggers;

using static Studio.MeowToon.Env;
using static Studio.MeowToon.Utils;

namespace Studio.MeowToon {
    /// <summary>
    /// status system
    /// </summary>
    /// <author>h.adachi (STUDIO MeowToon)</author>
    public class NoticeSystem : MonoBehaviour {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // References [bool => is+adjective, has+past participle, can+verb prototype, triad verb]

        [SerializeField] Text _message_text, _targets_text, _points_text, _mode_text;

        /// <remarks>
        /// for development.
        /// </remarks>
        [SerializeField] Text _energy_text, _power_text;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [noun, adjectives] 

        GameSystem _game_system;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // update Methods

        // Awake is called when the script instance is being loaded.
        void Awake() {
            _game_system = Find(name: GAME_SYSTEM).Get<GameSystem>();

            /// <summary>
            /// game system pause on.
            /// </summary>
            _game_system.OnPauseOn += () => { _message_text.text = MESSAGE_GAME_PAUSE; };

            /// <summary>
            /// game system pause off.
            /// </summary>
            _game_system.OnPauseOff += () => { _message_text.text = string.Empty; };

            // get home.
            Home home = Find(name: HOME_TYPE).Get<Home>();

            /// <summary>
            /// came back home.
            /// </summary>
            home.OnCameBack += () => { _message_text.text = MESSAGE_LEVEL_CLEAR; };
        }

        // Start is called before the first frame update
        void Start() {
            // update text ui.
            this.UpdateAsObservable().Subscribe(onNext: _ => {
                updateGameStatus();
                updateVehicleStatus();
            }).AddTo(this);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        /// <summary>
        /// update game status
        /// </summary>
        void updateGameStatus() {
            //_targets_text.text = string.Format("TGT {0}/{1}", _game_system.targetTotal - _game_system.targetRemain, _game_system.targetTotal);
            //_points_text.text = string.Format("POINT {0}", _game_system.pointTotal);
            _mode_text.text = string.Format("Mode: {0}", _game_system.mode);
            switch (_game_system.mode) {
                case MODE_EASY: _mode_text.color = yellow; break;
                case MODE_NORMAL: _mode_text.color = green; break;
                case MODE_HARD: _mode_text.color = purple; break;
            }
        }

        /// <summary>
        /// update vehicle status
        /// </summary>
        void updateVehicleStatus() {
        }
    }
}
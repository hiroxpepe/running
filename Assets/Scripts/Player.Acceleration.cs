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

using static System.Math;
using UnityEngine;

namespace Studio.MeowToon {
    /// <summary>
    /// player controller
    /// </summary>
    /// <author>h.adachi (STUDIO MeowToon)</author>
    public partial class Player : InputMaper {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        #region inner Classes

        class Acceleration {

            ///////////////////////////////////////////////////////////////////////////////////////
            // Fields [noun, adjectives] 

            Player _player;

            float _current_speed, _previous_speed;

            ///////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjectives] 

            public float currentSpeed { get => _current_speed; set => _current_speed = value; }

            public float previousSpeed { get => _previous_speed; set => _previous_speed = value; }

            public bool canWalk { get => _current_speed < _player._forward_speed_limit; }

            public bool canRun { get => _current_speed < _player._run_speed_limit; }

            public bool canBackward { get => _current_speed < _player._backward_speed_limit; }

            public bool freeze {
                get {
                    if (Round(value: _previous_speed, digits: 2) < 0.02 &&
                        Round(value: _current_speed, digits: 2) < 0.02 &&
                        Round(value: _previous_speed, digits: 2) == Round(value: _current_speed, digits: 2)) {
                        return true;
                    }
                    return false;
                }
            }

            public float jumpPower  {
                get {
                    float value = 0f;
                    //if (_current_speed > 3.9f) {
                    //    value = _jump_power * 1.35f;
                    //}
                    //else 
                    if (_player._y_button.isPressed || _player._do_update.virtualControllerMode) {
                        value = _player._jump_power * 1.25f;
                    }
                    else if (_player._up_button.isPressed || _player._down_button.isPressed) {
                        value = _player._jump_power;
                    }
                    else if (!_player._up_button.isPressed && !_player._down_button.isPressed) {
                        value = _player._jump_power * 1.25f;
                    }
                    //Debug.Log($"value: {value}");
                    return value;
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////
            // Constructor

            /// <summary>
            /// hide the constructor.
            /// </summary>
            Acceleration(Player player) {
                _player = player;
            }

            /// <summary>
            /// returns an initialized instance.
            /// </summary>
            public static Acceleration GetInstance(Player player) {
                return new Acceleration(player);
            }
        }

        #endregion
    }
}
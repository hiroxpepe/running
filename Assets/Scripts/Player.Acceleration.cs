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

            float _current_speed, _previous_speed, _forward_speed_limit, _run_speed_limit, _backward_speed_limit, _jump_power;

            ///////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjectives] 

            public float currentSpeed { get => _current_speed; set => _current_speed = value; }

            public float previousSpeed { get => _previous_speed; set => _previous_speed = value; }

            public bool canWalk { get => _current_speed < _forward_speed_limit; }

            public bool canRun { get => _current_speed < _run_speed_limit; }

            public bool canBackward { get => _current_speed < _backward_speed_limit; }

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
                    Debug.Log($"_current_speed: {_current_speed}");
                    float value = 0f;
                    if (_current_speed > 3.9f) {
                        value = _jump_power * 1.35f;
                    }
                    else if (_current_speed > 2.5f) {
                        value = _jump_power * 1.25f;
                    }
                    else if (_current_speed > 0) {
                        value = _jump_power;
                    }
                    else if (_current_speed == 0) {
                        value = _jump_power * 1.25f;
                    }
                    return value;
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////
            // Constructor

            /// <summary>
            /// hide the constructor.
            /// </summary>
            Acceleration(float forward_speed_limit, float run_speed_limit, float backward_speed_limit, float jump_power) {
                _forward_speed_limit = forward_speed_limit;
                _run_speed_limit = run_speed_limit;
                _backward_speed_limit = backward_speed_limit;
                _jump_power = jump_power;
            }

            /// <summary>
            /// returns an initialized instance.
            /// </summary>
            public static Acceleration GetInstance(float forward_speed_limit, float run_speed_limit, float backward_speed_limit, float jump_power) {
                return new Acceleration(forward_speed_limit, run_speed_limit, backward_speed_limit, jump_power);
            }
        }

        #endregion
    }
}
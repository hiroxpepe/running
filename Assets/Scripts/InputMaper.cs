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

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static UnityEngine.GameObject;
using UniRx;
using UniRx.Triggers;

namespace Studio.MeowToon {
    /// <summary>
    /// to map physical gamepad
    /// </summary>
    /// <author>h.adachi (STUDIO MeowToon)</author>
    public class InputMaper : MonoBehaviour {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [noun, adjectives] 

        protected GameObject _v_controller_object;

        protected ButtonControl _a_button, _b_button, _x_button, _y_button, _up_button, _down_button, _left_button, _right_button;

        protected ButtonControl _left_1_button, _right_1_button, _left_2_button, _right_2_button;

        protected ButtonControl _right_stick_up_button, _right_stick_down_button, _right_stick_left_button, _right_stick_right_button, _right_stick_button;

        protected ButtonControl _start_button, _select_button;

        bool _use_vibration = true;

        bool _use_v_controller;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, adjectives] 

        /// <summary>
        /// whether to use virtual controllers.
        /// </summary>
        public bool useVirtualController { get => _use_v_controller; }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // update Methods

        // Start is called before the first frame update
        protected void Start() {
            // get virtual controller object.
            _v_controller_object = Find(name: "VController");

            // Update is called once per frame.
            this.UpdateAsObservable().Subscribe(onNext: _ => {
                mapGamepad();
            }).AddTo(this);

            #region mobile phone vibration.

            // vibrate the smartphone when the button is pressed.
            this.UpdateAsObservable().Where(predicate: _ => _v_controller_object && _use_vibration &&
                (_a_button.wasPressedThisFrame || _b_button.wasPressedThisFrame || _x_button.wasPressedThisFrame || _y_button.wasPressedThisFrame ||
                _up_button.wasPressedThisFrame || _down_button.wasPressedThisFrame || _left_button.wasPressedThisFrame || _right_button.wasPressedThisFrame ||
                _left_1_button.wasPressedThisFrame || _right_1_button.wasPressedThisFrame || 
                _select_button.wasPressedThisFrame || _start_button.wasPressedThisFrame)).Subscribe(onNext: _ => {
                    AndroidVibrator.Vibrate(milliseconds: 50L);
            }).AddTo(this);

            // no vibration of the smartphone by pressing the start and X buttons at the same time.
            this.UpdateAsObservable().Where(predicate: _ => (_x_button.isPressed && _start_button.wasPressedThisFrame) || 
                (_x_button.wasPressedThisFrame && _start_button.isPressed)).Subscribe(onNext: _ => {
                _use_vibration = !_use_vibration;
            }).AddTo(this);

            #endregion
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        void mapGamepad() {
            // check a physical gamepad connected.
            string[] controller_names = Input.GetJoystickNames();
            if (controller_names.Length == 0 || controller_names[0] == "") {
                _v_controller_object.SetActive(value: true);
                _use_v_controller = true;
            } else {
                _v_controller_object.SetActive(value: false);
                _use_v_controller = false;
            }

            // identifies the OS.
            _up_button = Gamepad.current.dpad.up;
            _down_button = Gamepad.current.dpad.down;
            _left_button = Gamepad.current.dpad.left;
            _right_button = Gamepad.current.dpad.right;
            _start_button = Gamepad.current.startButton;
            _select_button = Gamepad.current.selectButton;
            if (Application.platform == RuntimePlatform.Android) {
                // Android OS
                _a_button = Gamepad.current.aButton;
                _b_button = Gamepad.current.bButton;
                _x_button = Gamepad.current.xButton;
                _y_button = Gamepad.current.yButton;
            } else if (Application.platform == RuntimePlatform.WindowsPlayer) {
                // Windows OS
                _a_button = Gamepad.current.bButton;
                _b_button = Gamepad.current.aButton;
                _x_button = Gamepad.current.yButton;
                _y_button = Gamepad.current.xButton;
            } else {
                // FIXME: can't get it during development with Unity?
                _a_button = Gamepad.current.bButton;
                _b_button = Gamepad.current.aButton;
                _x_button = Gamepad.current.yButton;
                _y_button = Gamepad.current.xButton;
            }
            _left_1_button = Gamepad.current.leftShoulder;
            _right_1_button = Gamepad.current.rightShoulder;
            _left_2_button = Gamepad.current.leftTrigger;
            _right_2_button = Gamepad.current.rightTrigger;
            _right_stick_up_button = Gamepad.current.rightStick.up;
            _right_stick_down_button = Gamepad.current.rightStick.down;
            _right_stick_left_button = Gamepad.current.rightStick.left;
            _right_stick_right_button = Gamepad.current.rightStick.right;
            _right_stick_button = Gamepad.current.rightStickButton;
        }
    }
}
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

namespace Studio.MeowToon {
    /// <summary>
    /// player controller
    /// </summary>
    /// <author>h.adachi (STUDIO MeowToon)</author>
    public partial class Player : InputMaper {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        #region inner Classes

        /// <summary>
        /// class for the FixedUpdate() method.
        /// </summary>
        class DoFixedUpdate {

            ///////////////////////////////////////////////////////////////////////////////////////
            // Fields [noun, adjectives] 

            bool _idol, _run, _walk, _jump, _backward;

            ///////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjectives] 

            public bool idol { get => _idol; }

            public bool run { get => _run; }

            public bool walk { get => _walk; }

            public bool jump { get => _jump; }

            public bool backward { get => _backward; }

            ///////////////////////////////////////////////////////////////////////////////////////
            // Constructor

            /// <summary>
            /// returns an initialized instance.
            /// </summary>
            public static DoFixedUpdate GetInstance() {
                return new DoFixedUpdate();
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // public Methods [verb]

            public void ApplyIdol() { _idol = true; _run = _walk = _backward = _jump = false; }

            public void ApplyRun() { _idol = _walk = _backward = false; _run = true; }

            public void CancelRun() { _run = false; }

            public void ApplyWalk() { _idol = _run = _backward = false; _walk = true; }

            public void CancelWalk() { _walk = false; }

            public void ApplyBackward() { _idol = _run = _walk = false; _backward = true; }

            public void CancelBackward() { _backward = false; }

            public void ApplyJump() { _jump = true; }

            public void CancelJump() { _jump = false; }
        }

        #endregion
    }
}
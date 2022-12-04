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
using UnityEngine.SceneManagement;
using static UnityEngine.GameObject;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

using static Studio.MeowToon.Env;
using static Studio.MeowToon.Utils;

namespace Studio.MeowToon {
    /// <summary>
    /// select scene
    /// </summary>
    /// <author>h.adachi (STUDIO MeowToon)</author>
    public class Select : InputMaper {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constants

        const int SELECT_COUNT = 3;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // References [bool => is+adjective, has+past participle, can+verb prototype, triad verb]

        [SerializeField] Image _easy, _normal, _hard;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [noun, adjectives]

        GameSystem _game_system;

        Map<int, string> _focus = new();

        string _selected = MODE_NORMAL;

        int _idx = 0;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // update Methods

        // Awake is called when the script instance is being loaded.
        void Awake() {
            _game_system = Find(name: GAME_SYSTEM).Get<GameSystem>();
            // set default focus.
            _focus.Add(key: 0, value: MODE_EASY);
            _focus.Add(key: 1, value: MODE_NORMAL);
            _focus.Add(key: 2, value: MODE_HARD);
            _idx = 1; // FIXME:
            _selected = _focus[_idx];
            changeSelectedColor();
        }

        // Start is called before the first frame update
        new void Start() {
            base.Start();

            /// <summary>
            /// select up.
            /// </summary>
            this.UpdateAsObservable().Where(predicate: _ => _up_button.wasPressedThisFrame).Subscribe(onNext: _ => {
                _idx--;
                if (_idx == -1) {
                    _idx = SELECT_COUNT - 1;
                }
                _selected = _focus[_idx];
                _game_system.mode = _selected;
                changeSelectedColor();
            }).AddTo(this);

            /// <summary>
            /// select down.
            /// </summary>
            this.UpdateAsObservable().Where(predicate: _ => _down_button.wasPressedThisFrame || _select_button.wasPressedThisFrame).Subscribe(onNext: _ => {
                _idx++;
                if (_idx == SELECT_COUNT) {
                    _idx = 0;
                }
                _selected = _focus[_idx];
                _game_system.mode = _selected;
                changeSelectedColor();
            }).AddTo(this);

            /// <summary>
            /// return title.
            /// </summary>
            this.UpdateAsObservable().Where(predicate: _ => _start_button.wasPressedThisFrame || _a_button.wasPressedThisFrame).Subscribe(onNext: _ => {
                SceneManager.LoadScene(sceneName: SCENE_TITLE);
            }).AddTo(this);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        void changeSelectedColor() {
            switch (_selected) {
                case MODE_EASY:
                    _easy.color = yellow;
                    _normal.color = white;
                    _hard.color = white;
                    break;
                case MODE_NORMAL:
                    _easy.color = white;
                    _normal.color = yellow;
                    _hard.color = white;
                    break;
                case MODE_HARD:
                    _easy.color = white;
                    _normal.color = white;
                    _hard.color = yellow;
                    break;
            }
        }
    }
}

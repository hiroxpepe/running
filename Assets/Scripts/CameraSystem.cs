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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Vector3;
using UniRx;
using UniRx.Triggers;

using static Studio.MeowToon.Env;

namespace Studio.MeowToon {
    /// <summary>
    /// camera controller
    /// </summary>
    /// <author>h.adachi (STUDIO MeowToon)</author>
    public class CameraSystem : InputMaper {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // References [bool => is+adjective, has+past participle, can+verb prototype, triad verb]

        [SerializeField] GameObject _horizontal_axis, _vertical_axis, _main_camera, _look_target;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [noun, adjectives] 

        Vector3 _default_local_position;

        Quaternion _default_local_rotation;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // update Methods

        // Start is called before the first frame update
        new void Start() {
            base.Start();

            /// <summary>
            /// hold the default position and rotation of the camera.
            /// </summary>
            _default_local_position = transform.localPosition;
            _default_local_rotation = transform.localRotation;

            /// <summary>
            /// rotate the camera view.
            /// </summary>
            this.UpdateAsObservable().Subscribe(onNext: _ => {
                rotateView();
            }).AddTo(this);

            /// <summary>
            /// reset the camera view.
            /// </summary>
            this.UpdateAsObservable().Where(predicate: _ => _right_stick_button.wasPressedThisFrame).Subscribe(onNext: _ => {
                resetRotateView();
            }).AddTo(this);

            /// <summary>
            /// when touching the back wall.
            /// </summary>
            this.OnTriggerEnterAsObservable().Where(predicate: x => x.Like(WALL_TYPE)).Subscribe(onNext: x => {
                List<Material> material_list = x.gameObject.Get<MeshRenderer>().materials.ToList();
                material_list.ForEach(material => { material.ToTransparent(); });
            }).AddTo(this);

            /// <summary>
            /// when leaving the back wall.
            /// </summary>
            this.OnTriggerExitAsObservable().Where(predicate: x => x.Like(WALL_TYPE)).Subscribe(onNext: x => {
                List<Material> material_list = x.gameObject.Get<MeshRenderer>().materials.ToList();
                material_list.ForEach(material => { material.ToOpaque(); });
            }).AddTo(this);

            /// <summary>
            /// when touching the ground.
            /// </summary>
            this.OnTriggerEnterAsObservable().Where(predicate: x => x.Like(GROUND_TYPE)).Subscribe(onNext: x => {
                List<Material> material_list = x.gameObject.Get<MeshRenderer>().materials.ToList();
                material_list.ForEach(material => { material.ToTransparent(); });
            }).AddTo(this);

            /// <summary>
            /// when leaving the ground.
            /// </summary>
            this.OnTriggerExitAsObservable().Where(predicate: x => x.Like(GROUND_TYPE)).Subscribe(onNext: x => {
                List<Material> material_list = x.gameObject.Get<MeshRenderer>().materials.ToList();
                material_list.ForEach(material => { material.ToOpaque(); });
            }).AddTo(this);

            /// <summary>
            /// when touching the block.
            /// </summary>
            this.OnTriggerEnterAsObservable().Where(predicate: x => x.Like(BLOCK_TYPE)).Subscribe(onNext: x => {
                List<Material> material_list = x.gameObject.Get<MeshRenderer>().materials.ToList();
                material_list.ForEach(material => { material.ToTransparent(); });
            }).AddTo(this);

            /// <summary>
            /// when leaving the block.
            /// </summary>
            this.OnTriggerExitAsObservable().Where(predicate: x => x.Like(BLOCK_TYPE)).Subscribe(onNext: x => {
                List<Material> material_list = x.gameObject.Get<MeshRenderer>().materials.ToList();
                material_list.ForEach(material => { material.ToOpaque(); });
            }).AddTo(this);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        /// <summary>
        /// rotate the camera view.
        /// </summary>
        void rotateView() {
            const float ADJUST = 120.0f;
            Vector3 player_position = transform.parent.gameObject.transform.position;
            // up.
            if (_right_stick_up_button.isPressed) {
                transform.RotateAround(point: player_position, axis: right, angle: 1.0f * ADJUST * Time.deltaTime);
                transform.LookAt(target: _look_target.transform);
            }
            // down.
            else if (_right_stick_down_button.isPressed) {
                transform.RotateAround(point: player_position, axis: right, angle: -1.0f * ADJUST * Time.deltaTime);
                transform.LookAt(target: _look_target.transform);
            }
            // left.
            else if (_right_stick_left_button.isPressed) {
                transform.RotateAround(point: player_position, axis: up, angle: 1.0f * ADJUST * Time.deltaTime);
            }
            // right
            else if (_right_stick_right_button.isPressed) {
                transform.RotateAround(point: player_position, axis: up, angle: -1.0f * ADJUST * Time.deltaTime);
            }
        }

        /// <summary>
        /// reset the camera view.
        /// </summary>
        void resetRotateView() {
            transform.localPosition = _default_local_position;
            transform.localRotation = _default_local_rotation;
        }
    }
}
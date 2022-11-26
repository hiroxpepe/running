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
using static System.Math;
using UnityEngine;
using static UnityEngine.Vector3;
using UniRx;
using UniRx.Triggers;

using static Studio.MeowToon.Env;

namespace Studio.MeowToon {
    /// <summary>
    /// player controller
    /// </summary>
    /// <author>h.adachi (STUDIO MeowToon)</author>
    public partial class Player : InputMaper {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // References [bool => is+adjective, has+past participle, can+verb prototype, triad verb]

        [SerializeField] float _jump_power = 15.0f;

        [SerializeField] float _rotational_speed = 10.0f;

        [SerializeField] float _forward_speed_limit = 1.1f;

        [SerializeField] float _run_speed_limit = 3.25f;

        [SerializeField] float _backward_speed_limit = 0.75f;

        [SerializeField] SimpleAnimation _simple_anime;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [noun, adjectives] 

        DoUpdate _do_update;

        DoFixedUpdate _do_fixed_update;

        Acceleration _acceleration;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, adjectives]

        /// <summary>
        /// transform position.
        /// </summary>
        public Vector3 position { get => transform.position; set { transform.position = value; Updated?.Invoke(this, new(nameof(position))); }}

        /// <summary>
        /// transform rotation.
        /// </summary>
        public Quaternion rotation { get => transform.rotation; set { transform.rotation = value; Updated?.Invoke(this, new(nameof(rotation))); }}


        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Events [verb, verb phrase]

        public event Action? OnGrounded;

        /// <summary>
        /// changed event handler.
        /// </summary>
        public event Changed? Updated;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // update Methods

        // Awake is called when the script instance is being loaded.
        void Awake() {
            _do_update = DoUpdate.GetInstance();
            _do_fixed_update = DoFixedUpdate.GetInstance();
            _acceleration = Acceleration.GetInstance(_forward_speed_limit, _run_speed_limit, _backward_speed_limit);
        }

        // Start is called before the first frame update
        new void Start() {
            base.Start();

            const float ADD_FORCE_VALUE = 12.0f;

            /// <remarks>
            /// Rigidbody should be only used in FixedUpdate.
            /// </remarks>
            Rigidbody rb = transform.Get<Rigidbody>();

            // FIXME: to integrate with Energy function.
            this.FixedUpdateAsObservable().Subscribe(onNext: _ => {
                _acceleration.previousSpeed = _acceleration.currentSpeed;// hold previous speed.
                _acceleration.currentSpeed = rb.velocity.magnitude; // get speed.
            }).AddTo(this);

            /// <summary>
            /// idol.
            /// </summary>
            this.UpdateAsObservable().Where(predicate: _ => _do_update.grounded && !_up_button.isPressed && !_down_button.isPressed).Subscribe(onNext: _ => {
                //_simpleAnime.Play("Default");
                _do_fixed_update.ApplyIdol();
            }).AddTo(this);

            this.FixedUpdateAsObservable().Where(predicate: _ => _do_fixed_update.idol).Subscribe(onNext: _ => {
                rb.useGravity = true;
            }).AddTo(this);

            /// <summary>
            /// walk.
            /// </summary>
            this.UpdateAsObservable().Where(predicate: _ => _do_update.grounded && _up_button.isPressed && !_y_button.isPressed).Subscribe(onNext: _ => {
                /*_simpleAnime.Play("Walk");*/
                _do_fixed_update.ApplyWalk();
            }).AddTo(this);

            this.FixedUpdateAsObservable().Where(predicate: _ => _do_fixed_update.walk && _acceleration.canWalk).Subscribe(onNext: _ => {
                const float ADJUST_VALUE = 7.5f;
                rb.AddFor​​ce(force: transform.forward * ADD_FORCE_VALUE * ADJUST_VALUE, mode: ForceMode.Acceleration);
                _do_fixed_update.CancelWalk();
            }).AddTo(this);

            /// <summary>
            /// run.
            /// </summary>
            this.UpdateAsObservable().Where(predicate: _ => _do_update.grounded && _up_button.isPressed && _y_button.isPressed).Subscribe(onNext: _ => {
                /*_simpleAnime.Play("Run");*/
                _do_fixed_update.ApplyRun();
            }).AddTo(this);

            this.FixedUpdateAsObservable().Where(predicate: _ => _do_fixed_update.run && _acceleration.canRun).Subscribe(onNext: _ => {
                const float ADJUST_VALUE = 7.5f;
                rb.AddFor​​ce(force: transform.forward * ADD_FORCE_VALUE * ADJUST_VALUE, mode: ForceMode.Acceleration);
                _do_fixed_update.CancelRun();
            }).AddTo(this);

            /// <summary>
            /// backward.
            /// </summary>
            this.UpdateAsObservable().Where(predicate: _ => _do_update.grounded && _down_button.isPressed).Subscribe(onNext: _ => {
                /*_simpleAnime.Play("Walk");*/
                _do_fixed_update.ApplyBackward();
            }).AddTo(this);

            this.FixedUpdateAsObservable().Where(predicate: _ => _do_fixed_update.backward && _acceleration.canBackward).Subscribe(onNext: _ => {
                const float ADJUST_VALUE = 7.5f;
                rb.AddFor​​ce(force: -transform.forward * ADD_FORCE_VALUE * ADJUST_VALUE, mode: ForceMode.Acceleration);
                _do_fixed_update.CancelBackward();
            }).AddTo(this);

            /// <summary>
            /// jump.
            /// </summary>
            this.UpdateAsObservable().Where(predicate: _ => _b_button.wasPressedThisFrame && _do_update.grounded).Subscribe(onNext: _ => {
                _do_update.grounded = false;
                //_simpleAnime.Play("Jump");
                _do_fixed_update.ApplyJump();
            }).AddTo(this);

            this.FixedUpdateAsObservable().Where(predicate: _ => _do_fixed_update.jump).Subscribe(onNext: _ => {
                const float ADJUST_VALUE = 2.0f;
                rb.useGravity = true;
                rb.AddRelativeFor​​ce(force: up * _jump_power * ADD_FORCE_VALUE * ADJUST_VALUE, mode: ForceMode.Acceleration);
                _do_fixed_update.CancelJump();
            }).AddTo(this);

            /// <summary>
            /// rotate(yaw).
            /// </summary>
            this.UpdateAsObservable().Subscribe(onNext: _ => {
                int axis = _right_button.isPressed ? 1 : _left_button.isPressed ? -1 : 0;
                transform.Rotate(xAngle: 0, yAngle: axis * (_rotational_speed * Time.deltaTime) * ADD_FORCE_VALUE, zAngle: 0);
            }).AddTo(this);

            /// <summary>
            /// freeze.
            /// </summary>
            this.OnCollisionStayAsObservable().Where(predicate: x => x.Like(BLOCK_TYPE) && (_up_button.isPressed || _down_button.isPressed) && _acceleration.freeze).Subscribe(onNext: _ => {
                double reach = getReach();
                if (_do_update.grounded && (reach < 0.5d || reach >= 0.99d)) {
                    moveLetfOrRight(direction: getDirection(forward_vector: transform.forward));
                }
                else if (reach >= 0.5d && reach < 0.99d) {
                    rb.useGravity = false;
                    moveTop();
                }
            }).AddTo(this);

            /// <summary>
            /// when touching blocks.
            /// TODO: to Block ?
            /// </summary>
            this.OnCollisionEnterAsObservable().Where(predicate: x => x.Like(BLOCK_TYPE)).Subscribe(onNext: x => {
                if (!isHitSide(target: x.gameObject)) {
                    _do_update.grounded = true;
                    rb.useGravity = true;
                }
            }).AddTo(this);

            /// <summary>
            /// when leaving blocks.
            /// TODO: to Block ?
            /// </summary>
            this.OnCollisionExitAsObservable().Where(predicate: x => x.Like(BLOCK_TYPE)).Subscribe(onNext: x => {
                rb.useGravity = true;
            }).AddTo(this);

            // LateUpdate is called after all Update functions have been called.
            this.LateUpdateAsObservable().Subscribe(onNext: _ => {
                position = transform.position;
                rotation = transform.rotation;
            }).AddTo(this);

            /// <summary>
            /// when touching grounds.
            /// </summary>
            this.OnCollisionEnterAsObservable().Where(predicate: x => x.Like(GROUND_TYPE)).Subscribe(onNext: x => {
                _do_update.grounded = true;
                rb.useGravity = true;

                // reset rotate.
                Vector3 angle = transform.eulerAngles;
                angle.x = angle.z = 0f;
                transform.eulerAngles = angle;

                OnGrounded?.Invoke(); // call event handler.
            }).AddTo(this);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        /// <summary>
        /// the value until the top of the block.
        /// </summary>
        double getReach() {
            return Round(value: transform.position.y, digits: 2) % 1; // FIXME:
        }

        /// <summary>
        /// move top when the vehicle hits a block.
        /// </summary>
        void moveTop() {
            const float MOVE_VALUE = 6.0f;
            transform.position = new(
                x: transform.position.x,
                y: transform.position.y + MOVE_VALUE * Time.deltaTime,
                z: transform.position.z
            );
        }

        /// <summary>
        /// move aside when the vehicle hits a block.
        /// </summary>
        /// <param name="direction">the vehicle's direction is provided.</param>
        void moveLetfOrRight(Direction direction) {
            const float MOVE_VALUE = 0.3f;
            Vector3 move_position = transform.position;
            // z-axis positive and negative.
            if (direction == Direction.PositiveZ || direction == Direction.NegativeZ) {
                if (transform.forward.x < 0f) {
                    move_position = new(
                        x: transform.position.x - MOVE_VALUE * Time.deltaTime,
                        y: transform.position.y,
                        z: transform.position.z
                    );
                } else if (transform.forward.x >= 0f) {
                    move_position = new(
                        x: transform.position.x + MOVE_VALUE * Time.deltaTime,
                        y: transform.position.y,
                        z: transform.position.z
                    );
                }
            }
            // x-axis positive and negative.
            if (direction == Direction.PositiveX || direction == Direction.NegativeX) {
                if (transform.forward.z < 0f) {
                    move_position = new(
                        x: transform.position.x,
                        y: transform.position.y,
                        z: transform.position.z - MOVE_VALUE * Time.deltaTime
                    );
                } else if (transform.forward.z >= 0f) {
                    move_position = new(
                        x: transform.position.x,
                        y: transform.position.y,
                        z: transform.position.z + MOVE_VALUE * Time.deltaTime
                    );
                }
            }
            // move to a new position.
            transform.position = move_position;
        }

        /// <summary>
        /// returns an enum of the vehicle's direction.
        /// </summary>
        Direction getDirection(Vector3 forward_vector) {
            float forward_x = (float) Round(a: forward_vector.x);
            float forward_y = (float) Round(a: forward_vector.y);
            float forward_z = (float) Round(a: forward_vector.z);
            if (forward_x == 0 && forward_z == 1) { return Direction.PositiveZ; } // z-axis positive.
            if (forward_x == 0 && forward_z == -1) { return Direction.NegativeZ; } // z-axis negative.
            if (forward_x == 1 && forward_z == 0) { return Direction.PositiveX; } // x-axis positive.
            if (forward_x == -1 && forward_z == 0) { return Direction.NegativeX; } // x-axis negative.
            // determine the difference between the two axes.
            float absolute_x = Abs(value: forward_vector.x);
            float absolute_z = Abs(value: forward_vector.z);
            if (absolute_x > absolute_z) {
                if (forward_x == 1) { return Direction.PositiveX; } // x-axis positive.
                if (forward_x == -1) { return Direction.NegativeX; } // x-axis negative.
            } else if (absolute_x < absolute_z) {
                if (forward_z == 1) { return Direction.PositiveZ; } // z-axis positive.
                if (forward_z == -1) { return Direction.NegativeZ; } // z-axis negative.
            }
            return Direction.None; // unknown.
        }

        /// <summary>
        /// whether hits the side of the colliding object.
        /// </summary>
        bool isHitSide(GameObject target) {
            const float ADJUST = 0.1f;
            float target_height = target.Get<Renderer>().bounds.size.y;
            float target_y = target.transform.position.y;
            float target_top = target_height + target_y;
            float position_y = transform.position.y;
            if (position_y < (target_top - ADJUST)) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// changed event handler from energy.
        /// </summary>
        void onChanged(object sender, EvtArgs  e) {
        }
    }
}
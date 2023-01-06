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

        [SerializeField] float _jump_power = 10.0f;

        [SerializeField] float _rotational_speed = 10.0f;

        [SerializeField] float _forward_speed_limit = 1.5f;

        [SerializeField] float _run_speed_limit = 3.25f;

        [SerializeField] float _backward_speed_limit = 1.0f;

        [SerializeField] SimpleAnimation _simple_anime;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [noun, adjectives] 

        DoUpdate _do_update;

        DoFixedUpdate _do_fixed_update;

        Acceleration _acceleration;

        Vector3[] previousPosition = new Vector3[60]; // saves position 30 frames ago.

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
            _acceleration = Acceleration.GetInstance(this);
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
                _simple_anime.Play("Default");
                _do_fixed_update.ApplyIdol();
            }).AddTo(this);

            this.FixedUpdateAsObservable().Where(predicate: _ => _do_fixed_update.idol).Subscribe(onNext: _ => {
                rb.useGravity = true;
            }).AddTo(this);

            /// <summary>
            /// walk.
            /// </summary>
            this.UpdateAsObservable().Where(predicate: _ => _up_button.isPressed && !_y_button.isPressed && !_do_update.virtualControllerMode).Subscribe(onNext: _ => {
                if (_do_update.grounded) { _simple_anime.Play("Walk"); }
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
            this.UpdateAsObservable().Where(predicate: _ => _up_button.isPressed && (_y_button.isPressed || _do_update.virtualControllerMode)).Subscribe(onNext: _ => {
                if (_do_update.grounded) { _simple_anime.Play("Run"); }
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
                _simple_anime.Play("Walk");
                _do_fixed_update.ApplyBackward();
            }).AddTo(this);

            this.FixedUpdateAsObservable().Where(predicate: _ => _do_fixed_update.backward && _acceleration.canBackward).Subscribe(onNext: _ => {
                const float ADJUST_VALUE = 7.5f;
                rb.AddFor​​ce(force: -transform.forward * ADD_FORCE_VALUE * ADJUST_VALUE, mode: ForceMode.Acceleration);
                _do_fixed_update.CancelBackward();
            }).AddTo(this);

            /// <summary>
            /// stop.
            /// </summary>
            this.UpdateAsObservable().Where(predicate: _ => _do_update.grounded && (_up_button.wasReleasedThisFrame || _down_button.wasReleasedThisFrame)).Subscribe(onNext: _ => {
                //_simple_anime.Play("Stop");
                _do_fixed_update.ApplyStop();
            }).AddTo(this);

            this.FixedUpdateAsObservable().Where(predicate: _ => _do_fixed_update.stop).Subscribe(onNext: _ => {
                rb.velocity = new Vector3(0f, 0f, 0f);
                _do_fixed_update.CancelStop();
            }).AddTo(this);

            /// <summary>
            /// virtual controller mode.
            /// </summary>
            this.UpdateAsObservable().Where(predicate: _ => _y_button.wasReleasedThisFrame && useVirtualController).Subscribe(onNext: _ => {
                _do_update.virtualControllerMode = true;
                Observable.TimerFrame(45).Subscribe(onNext: __ => {
                    _do_update.virtualControllerMode = false;
                }).AddTo(this);
            }).AddTo(this);

            /// <summary>
            /// jump.
            /// </summary>
            this.UpdateAsObservable().Where(predicate: _ => _b_button.wasPressedThisFrame && _do_update.grounded).Subscribe(onNext: _ => {
                _do_update.grounded = false;
                _simple_anime.Play("Jump");
                _do_fixed_update.ApplyJump();
            }).AddTo(this);

            this.FixedUpdateAsObservable().Where(predicate: _ => _do_fixed_update.jump).Subscribe(onNext: _ => {
                const float ADJUST_VALUE = 2.0f;
                rb.useGravity = true;
                rb.AddRelativeFor​​ce(force: up * _acceleration.jumpPower * ADD_FORCE_VALUE * ADJUST_VALUE, mode: ForceMode.Acceleration);
                _do_fixed_update.CancelJump();
            }).AddTo(this);

            /// <summary>
            /// stop jump.
            /// </summary>
            this.UpdateAsObservable().Where(predicate: _ => _b_button.wasReleasedThisFrame && !_do_update.grounded).Subscribe(onNext: _ => {
                _do_fixed_update.ApplyStopJump();
            }).AddTo(this);

            this.FixedUpdateAsObservable().Where(predicate: _ => _do_fixed_update.stopJump).Subscribe(onNext: _ => {
                const float ADJUST_VALUE = 0.05f;
                Observable.Timer(TimeSpan.FromSeconds(ADJUST_VALUE)).Subscribe(onNext: _ => {
                    if (!isDown()) {
                        rb.useGravity = true;
                        Vector3 velocity = rb.velocity;
                        rb.velocity = new Vector3(x: velocity.x, y: 0, z: velocity.z);
                    }
                    _do_fixed_update.CancelStopJump();
                }).AddTo(this);
            }).AddTo(this);

            /// <summary>
            /// rotate(yaw).
            /// </summary>
            this.UpdateAsObservable().Subscribe(onNext: _ => {
                int axis = _right_button.isPressed ? 1 : _left_button.isPressed ? -1 : 0;
                transform.Rotate(xAngle: 0, yAngle: axis * (_rotational_speed * Time.deltaTime) * ADD_FORCE_VALUE, zAngle: 0);
            }).AddTo(this);

            /// <summary>
            /// when touching blocks.
            /// TODO: to Block ?
            /// </summary>
            this.OnCollisionEnterAsObservable().Where(predicate: x => x.Like(BLOCK_TYPE)).Subscribe(onNext: x => {
                if (!isHitSide(target: x.gameObject)) {
                    _do_update.grounded = true;
                    rb.useGravity = true;
                    rb.velocity = new Vector3(0f, 0f, 0f);
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
                cashPreviousPosition();
            }).AddTo(this);

            /// <summary>
            /// when touching grounds.
            /// </summary>
            this.OnCollisionEnterAsObservable().Where(predicate: x => x.Like(GROUND_TYPE)).Subscribe(onNext: x => {
                _do_update.grounded = true;
                if (isUpOrDown()) {
                    rb.useGravity = true;
                    rb.velocity = new Vector3(0f, 0f, 0f);

                    // reset rotate.
                    Vector3 angle = transform.eulerAngles;
                    angle.x = angle.z = 0f;
                    transform.eulerAngles = angle;

                    OnGrounded?.Invoke(); // call event handler.
                }
            }).AddTo(this);

            /// <summary>
            /// freeze.
            /// </summary>
            this.OnCollisionStayAsObservable().Where(predicate: x => (x.Like(GROUND_TYPE) || x.Like(BLOCK_TYPE)) && (_up_button.isPressed || _down_button.isPressed) && _acceleration.freeze).Subscribe(onNext: x => {
                if (!isHitSide(target: x.gameObject)) { return; }
                double reach = getReach(target: x.gameObject);
                if (_do_update.grounded && (reach < 0.5d || reach >= 0.99d)) {
                    moveLetfOrRight(direction: getDirection(forward_vector: transform.forward));
                    rb.useGravity = true;
                }
                else if (reach >= 0.5d && reach < 0.99d) {
                    rb.useGravity = false;
                    moveTop();
                    _do_update.grounded = true;
                    rb.useGravity = true;
                }
                else {
                    dropDown();
                    _do_update.grounded = true;
                    rb.useGravity = true;
                }
            }).AddTo(this);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        /// <summary>
        /// saves position value for the previous n frame.
        /// </summary>
        void cashPreviousPosition() {
            for (int i = previousPosition.Length - 1 ; i > -1; i--) {
                if (i > 0) {
                    previousPosition[i] = previousPosition[i - 1];
                } else if (i == 0) {
                    previousPosition[i] = new Vector3(
                        (float) Round(transform.position.x, 3),
                        (float) Round(transform.position.y, 3),
                        (float) Round(transform.position.z, 3)
                    );
                }
            }
        }

        /// <summary>
        /// whether there was an up or down movement.
        /// </summary>
        bool isUpOrDown() {
            int fps = Application.targetFrameRate;
            int ADJUST_VALUE = 9;
            if (fps == 60) ADJUST_VALUE = 9;
            if (fps == 30) ADJUST_VALUE = 20;
            float current_y = (float) Round(transform.position.y, 1, MidpointRounding.AwayFromZero);
            float previous_y = (float) Round(previousPosition[ADJUST_VALUE].y, 1, MidpointRounding.AwayFromZero);
            if (current_y == previous_y) {
                return false;
            } else if (current_y != previous_y) {
                return true;
            } else {
                return true;
            }
        }

        /// <summary>
        /// whether there was a down movement.
        /// </summary>
        bool isDown() {
            int fps = Application.targetFrameRate;
            int ADJUST_VALUE = 9;
            if (fps == 60) ADJUST_VALUE = 9;
            if (fps == 30) ADJUST_VALUE = 20;
            float current_y = (float) Round(transform.position.y, 1, MidpointRounding.AwayFromZero);
            float previous_y = (float) Round(previousPosition[ADJUST_VALUE].y, 1, MidpointRounding.AwayFromZero);
            //Debug.Log($"current_y: {current_y} previous_y: {previous_y}");
            if (current_y > previous_y) {
                return false;
            } else {
                return true;
            }
        }

        /// <summary>
        /// the value until the top of the block.
        /// </summary>
        double getReach(GameObject target) {
            float distance_y = transform.position.y - target.transform.position.y;
            float size_to_one = 1.0f / target.Get<Renderer>().bounds.size.y;
            float rate_for_one = distance_y * size_to_one;
            return Round(value: rate_for_one, digits: 2);
        }

        /// <summary>
        /// move top when the player hits a block.
        /// </summary>
        void moveTop() {
            const float MOVE_VALUE = 12.0f;
            transform.position = new(
                x: transform.position.x,
                y: transform.position.y + MOVE_VALUE * Time.deltaTime,
                z: transform.position.z
            );
        }

        /// <summary>
        /// drop down when the player hits a block.
        /// </summary>
        void dropDown() {
            const float MOVE_VALUE = 6.0f;
            transform.position = new(
                x: transform.position.x,
                y: transform.position.y - MOVE_VALUE * Time.deltaTime,
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
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace TungHT.MyBreakout {
    public class Paddle : MonoBehaviour {
        [Serializable]
        public class Config {
            public float movementSpeed;
            public float size;
            public float collisionOffset = 0.0001f;
            public LayerMask collisionMask;

            public Transform ballHoldingTransform;
            public Transform ballShootingFromTransform;
            public Transform ballPrefab;
        }

        [SerializeField] private Config config = new();

        [Serializable]
        public class State {
            public Vector2 direction;
            public bool isMoving;

            public bool isShootingBall;
            public Ball ball;

            public Vector3 lastContactPoint;

            public bool HasBall => ball != null;
        }

        private State _state = new();

        private void Awake()
        {
            Assert.IsNotNull(config.ballHoldingTransform, "config.ballHoldingTransform != null");
            Assert.IsNotNull(config.ballShootingFromTransform, "config.ballShootingFromTransform != null");
        }

        private void Start()
        {
            SpawnBall();
        }

        private void FixedUpdate()
        {
            if (_state.isMoving)
            {
                // Draw debug ray of config.size at the direction of movement
                Vector3 transformPosition = transform.position;

                RaycastHit2D hit = Physics2D.Raycast(
                    transformPosition,
                    _state.direction,
                    config.size + config.collisionOffset,
                    config.collisionMask
                );

                if (hit.collider == null)
                {
                    Debug.DrawRay(transformPosition, (Vector3)_state.direction * config.size, Color.green, Time.fixedDeltaTime);
                    transform.position = transformPosition + (Vector3)_state.direction * (config.movementSpeed * Time.fixedDeltaTime);
                } else
                {
                    Debug.DrawRay(transformPosition, (Vector3)_state.direction * config.size, Color.red, Time.fixedDeltaTime);
                }
            }

            if (_state.HasBall && _state.isShootingBall)
            {
                LaunchBall();

                // Release the ball
                _state.ball = null;
                _state.isShootingBall = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.TryGetComponent(out Ball ball))
            {
                if (_state.ball != ball)
                {
                    _state.lastContactPoint = other.contacts[0].point;
                    // ball.AttachTo(config.ballHoldingTransform, _state.lastContactPoint);
                    // _state.ball = ball;

                    LaunchBall(ball);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Vector3 ballShootingPosition = config.ballShootingFromTransform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(ballShootingPosition, 0.1f);

            if (_state.HasBall)
            {
                Vector3 ballHoldingPosition = _state.ball.transform.position;
                Vector3 shootingDirection = (ballHoldingPosition - ballShootingPosition).normalized;
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(ballHoldingPosition, 0.1f);

                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(ballShootingPosition, shootingDirection * 3);
            }

            if (_state.lastContactPoint != Vector3.zero)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_state.lastContactPoint, 0.1f);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            var moveInput = context.ReadValue<Vector2>();

            if (moveInput.x != 0)
            {
                _state.direction = moveInput.x > 0 ? Vector2.right : Vector2.left;
                _state.isMoving = context.phase switch
                {
                    InputActionPhase.Disabled => false,
                    InputActionPhase.Waiting => false,
                    InputActionPhase.Started => true,
                    InputActionPhase.Performed => true,
                    InputActionPhase.Canceled => false,
                    _ => throw new ArgumentOutOfRangeException(),
                };
            } else
            {
                _state.isMoving = false;
            }
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            _state.isShootingBall = context.phase switch
            {
                InputActionPhase.Disabled => false,
                InputActionPhase.Waiting => false,
                InputActionPhase.Started => true,
                InputActionPhase.Performed => true,
                InputActionPhase.Canceled => false,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public void SpawnBall()
        {
            if (!_state.HasBall)
            {
                Instantiate(config.ballPrefab, config.ballHoldingTransform).TryGetComponent(out Ball ball);
                _state.ball = ball;
            }
        }

        private void LaunchBall()
        {
            if (_state.HasBall)
            {
                LaunchBall(_state.ball);
                _state.ball = null;
            }
        }

        private void LaunchBall(Ball ball)
        {
            // Shoot the ball in direction from config.ballShootingFromTransform to config.ballHoldingTransform
            Vector3 direction = (ball.transform.position - config.ballShootingFromTransform.position).normalized;
            Debug.DrawRay(config.ballShootingFromTransform.position, direction * 3, Color.blue, 1f);
            ball.RemoveParent();
            ball.Shoot(direction);
        }
    }
}

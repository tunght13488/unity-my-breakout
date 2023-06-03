using System;
using TungHT.Core.Variables;
using UnityEngine;
using UnityEngine.Assertions;

namespace TungHT.MyBreakout {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour {
        [Serializable]
        public class Config {
            public FloatReference movementSpeed = new(10f);
        }

        [SerializeField] private Config config;

        [Serializable]
        public class State {
            public Vector2 direction;
        }

        [SerializeField] private State state;

        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            Assert.IsNotNull(_rigidbody2D, "_rigidbody2D != null");
        }

        public void RemoveParent()
        {
            transform.parent = null;
        }

        public void Shoot(Vector2 direction)
        {
            state.direction = direction.normalized;

            if (state.direction != Vector2.zero)
            {
                _rigidbody2D.velocity = state.direction * config.movementSpeed;
            }
        }

        public void AttachTo(Transform ballHoldingTransform, Vector3 position)
        {
            _rigidbody2D.velocity = Vector2.zero;
            Vector3 localPosition = ballHoldingTransform.InverseTransformPoint(position);
            transform.SetParent(ballHoldingTransform, false);
            transform.SetLocalPositionAndRotation(localPosition, Quaternion.identity);
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}

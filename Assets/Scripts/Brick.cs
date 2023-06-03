using System;
using System.Linq;
using TungHT.Core.Events;
using TungHT.Core.Variables;
using TungHT.MyBreakout.ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace TungHT.MyBreakout {
    [ExecuteInEditMode]
    public class Brick : MonoBehaviour {
        [Serializable]
        public class Event {
            public GameEvent onBrickDestroyed;
        }

        [SerializeField] private Event @event = new();

        [Serializable]
        public class Config {
            public IntReference maxHealth;
            public BrickVisualsSO brickVisualsSO;
            public BrickVisual brickVisual;
        }

        [SerializeField] private Config config = new();

        [Serializable]
        public class State {
            public int health;
        }

        private State _state = new();

        private void Awake()
        {
            Assert.IsNotNull(config.maxHealth, "config.maxHealth != null");
            Assert.IsNotNull(config.brickVisual, "config.brickVisual != null");

            if (config.brickVisualsSO != null)
            {
                config.brickVisualsSO.brickVisuals.Sort((a, b) => a.healthThreshold.Value.CompareTo(b.healthThreshold));
            }
        }

        private void Start()
        {
            _state.health = config.maxHealth;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (config.brickVisualsSO != null)
            {
                BrickSpriteSO brickSpriteSO =
                    config.brickVisualsSO.brickVisuals.LastOrDefault(pair => _state.health >= pair.healthThreshold)?.brickSpriteSO;
                config.brickVisual.SetBrickSpriteSO(brickSpriteSO);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.TryGetComponent(out Ball _))
            {
                _state.health--;
                UpdateVisual();

                if (_state.health <= 0)
                {
                    Destroy(gameObject);
                    @event.onBrickDestroyed.Raise();
                }
            }
        }
    }
}

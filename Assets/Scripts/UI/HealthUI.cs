using System;
using System.Collections.Generic;
using TungHT.Core.Events;
using TungHT.Core.Variables;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace TungHT.MyBreakout.UI {
    public class HealthUI : MonoBehaviour {
        [Serializable]
        public class Event {
            public GameEvent onHealthReachedZero;
        }

        [SerializeField] private Event @event = new();

        [Serializable]
        public class Config {
            public IntReference maxHealth = new(0);
            public Image healthTemplate;
        }

        [SerializeField] private Config config = new();

        [Serializable]
        public class State {
            public int health;
            public List<Image> healthImages = new();
        }

        private State _state = new();

        private void Awake()
        {
            Assert.IsNotNull(config.healthTemplate, "config.healthTemplate != null");
        }

        private void Start()
        {
            config.healthTemplate.gameObject.SetActive(false);
            _state.health = config.maxHealth;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            while (_state.healthImages.Count > config.maxHealth)
            {
                Image lastHealthImage = _state.healthImages[^1];
                _state.healthImages.RemoveAt(_state.healthImages.Count - 1);
                Destroy(lastHealthImage.gameObject);
            }

            while (_state.healthImages.Count < config.maxHealth)
            {
                Image newHealthImage = Instantiate(config.healthTemplate, transform);
                _state.healthImages.Add(newHealthImage);
                newHealthImage.gameObject.SetActive(true);
            }

            for (var i = 0; i < _state.healthImages.Count; i++)
            {
                _state.healthImages[i].gameObject.SetActive(i < _state.health);
            }
        }

        public void DecreaseHealth()
        {
            _state.health--;
            UpdateVisual();

            if (_state.health <= 0)
            {
                @event.onHealthReachedZero.Raise();
            }
        }
    }
}

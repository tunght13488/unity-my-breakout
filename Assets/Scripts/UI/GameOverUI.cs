using System;
using TMPro;
using TungHT.Core.Variables;
using UnityEngine;
using UnityEngine.Assertions;

namespace TungHT.MyBreakout.UI {
    public class GameOverUI : MonoBehaviour {
        [Serializable]
        public class Config {
            public TextMeshProUGUI scoreText;
            public IntReference score;
        }

        [SerializeField] private Config config = new();

        private void Awake()
        {
            Assert.IsNotNull(config.scoreText, "config.scoreText != null");
            Assert.IsNotNull(config.score, "config.score != null");
        }

        public void UpdateVisual()
        {
            config.scoreText.text = config.score.Value.ToString();
        }
    }
}

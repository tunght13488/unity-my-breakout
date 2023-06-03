using System;
using TMPro;
using TungHT.Core.Variables;
using UnityEngine;
using UnityEngine.Assertions;

namespace TungHT.MyBreakout.UI {
    public class ScoreUI : MonoBehaviour {
        [Serializable]
        public class Config {
            public IntReference score;
            public TextMeshProUGUI scoreText;
        }

        [SerializeField] private Config config = new();

        private void Awake()
        {
            Assert.IsNotNull(config.scoreText, "config.scoreText != null");
        }

        private void Start()
        {
            config.score.variable.SetValue(0);
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            config.scoreText.text = config.score.Value.ToString();
        }

        public void AddScore(int score)
        {
            config.score.variable.ApplyChange(score);
            UpdateVisual();
        }
    }
}

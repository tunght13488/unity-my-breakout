using System;
using TungHT.MyBreakout.ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace TungHT.MyBreakout {
    [ExecuteInEditMode]
    public class BrickVisual : MonoBehaviour {
        [Serializable]
        public class Config {
            public BrickSpriteSO brickSpriteSO;
            public SpriteRenderer leftSpriteRenderer;
            public SpriteRenderer rightSpriteRenderer;
        }

        [SerializeField] private Config config = new();

        private void Start()
        {
            Assert.IsNotNull(config.brickSpriteSO, "config.brickSpriteSO != null");
        }

        public void SetBrickSpriteSO(BrickSpriteSO brickSpriteSO)
        {
            config.brickSpriteSO = brickSpriteSO;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (config.brickSpriteSO != null)
            {
                config.leftSpriteRenderer.sprite = config.brickSpriteSO.leftSprite;
                config.rightSpriteRenderer.sprite = config.brickSpriteSO.rightSprite;
            }
        }
    }
}

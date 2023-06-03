using System;
using System.Collections.Generic;
using TungHT.Core.Variables;
using UnityEngine;

namespace TungHT.MyBreakout.ScriptableObjects {
    [CreateAssetMenu]
    public class BrickVisualsSO : ScriptableObject {
        [Serializable]
        public class BrickVisual {
            public IntReference healthThreshold = new(0);
            public BrickSpriteSO brickSpriteSO;
        }

        public List<BrickVisual> brickVisuals = new();
    }
}

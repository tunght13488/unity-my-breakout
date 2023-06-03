using System;
using TungHT.Core.Events;
using UnityEngine;

namespace TungHT.MyBreakout {
    public class BallOutOfPlayZone : MonoBehaviour {
        [Serializable]
        public class Event {
            public GameEvent onBallOutOfPlayZone;
        }

        [SerializeField] private Event @event = new();

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.TryGetComponent(out Ball ball))
            {
                ball.DestroySelf();
                @event.onBallOutOfPlayZone.Raise();
            }
        }
    }
}

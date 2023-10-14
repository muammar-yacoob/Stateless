using UnityEngine;

namespace Stateless.Player.Events
{
    public class BulletFired
    {
        public readonly AudioClip Sfx;
        public readonly Vector3 TransformPosition;
        public BulletFired(AudioClip sfx, Vector3 transformPosition)
        {
            Sfx = sfx;
            TransformPosition = transformPosition;
        }
    }
}
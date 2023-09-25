using System;
using UnityEngine;

namespace Player
{
    public interface IPlayerInput
    {
        Vector2 Move { get; }
        event Action Jump;
    }
}
using UnityEngine.InputSystem;

namespace Stateless.UI
{
    public interface IEntryAnnouncer
    {
        void AnnounceEntry(PlayerInput playerInput);
    }
}
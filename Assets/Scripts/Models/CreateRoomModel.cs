using UniRx;
using UnityEngine;

namespace Models
{
    public static class CreateRoomModel
    {
        private static readonly ReactiveProperty<string> _roomName = new ReactiveProperty<string>("");
        public static IReadOnlyReactiveProperty<string> RoomName => _roomName;

        private static readonly ReactiveProperty<string> _roomPassword = new ReactiveProperty<string>("");
        public static IReadOnlyReactiveProperty<string> RoomPassword => _roomPassword;

        public static void SetRoomName(string roomName)
        {
            _roomName.Value = roomName;
        }

        public static void SetRoomPassword(string roomPassword)
        {
            _roomPassword.Value = roomPassword;
        }
    }
}

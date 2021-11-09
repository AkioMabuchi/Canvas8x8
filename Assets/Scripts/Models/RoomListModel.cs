using System.Collections.Generic;
using Photon.Realtime;
using UniRx;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Models
{
    public enum RoomStatus
    {
        None,
        Private,
        Public
    }

    public static class RoomListModel
    {
        private static readonly Dictionary<string, RoomInfo> _rooms = new Dictionary<string, RoomInfo>();
        public static IReadOnlyDictionary<string, RoomInfo> Rooms => _rooms;

        private static readonly ReactiveProperty<bool>[] _roomsInteractable = new ReactiveProperty<bool>[30];
        public static readonly IReadOnlyReactiveProperty<bool>[] RoomsInteractable =
            new IReadOnlyReactiveProperty<bool>[30];
        
        private static readonly ReactiveProperty<RoomStatus>[] _roomStatuses = new ReactiveProperty<RoomStatus>[30];
        public static readonly IReadOnlyReactiveProperty<RoomStatus>[] RoomStatuses =
            new IReadOnlyReactiveProperty<RoomStatus>[30];
        
        private static readonly ReactiveProperty<string>[] _roomNames = new ReactiveProperty<string>[30];
        public static readonly IReadOnlyReactiveProperty<string>[] RoomNames = 
            new IReadOnlyReactiveProperty<string>[30];
        
        private static readonly ReactiveProperty<string>[] _roomMaximums = new ReactiveProperty<string>[30];
        public static readonly IReadOnlyReactiveProperty<string>[] RoomMaximums =
            new IReadOnlyReactiveProperty<string>[30];
        
        private static readonly ReactiveProperty<string>[] _roomCurrents = new ReactiveProperty<string>[30];
        public static readonly IReadOnlyReactiveProperty<string>[] RoomCurrents =
            new IReadOnlyReactiveProperty<string>[30];

        static RoomListModel()
        {
            for (int i = 0; i < 30; i++)
            {
                _roomsInteractable[i] = new ReactiveProperty<bool>(false);
                RoomsInteractable[i] = _roomsInteractable[i];
                _roomStatuses[i] = new ReactiveProperty<RoomStatus>(RoomStatus.None);
                RoomStatuses[i] = _roomStatuses[i];
                _roomNames[i] = new ReactiveProperty<string>("");
                RoomNames[i] = _roomNames[i];
                _roomMaximums[i] = new ReactiveProperty<string>("0");
                RoomMaximums[i] = _roomMaximums[i];
                _roomCurrents[i] = new ReactiveProperty<string>("0");
                RoomCurrents[i] = _roomCurrents[i];
            }
        }

        public static void Update(IReadOnlyList<RoomInfo> roomList)
        {
            foreach (RoomInfo room in roomList)
            {
                if (room.RemovedFromList)
                {
                    _rooms.Remove(room.Name);
                }
                else
                {
                    _rooms[room.Name] = room;
                }
            }

            List<RoomInfo> rooms = new List<RoomInfo>();
            foreach (RoomInfo room in _rooms.Values) rooms.Add(room);

            for (int i = 0; i < 30; i++)
            {
                if (i < rooms.Count)
                {
                    _roomsInteractable[i].Value = rooms[i].IsOpen && rooms[i].PlayerCount < rooms[i].MaxPlayers;
                    _roomStatuses[i].Value = (string) rooms[i].CustomProperties["Password"] == ""
                        ? RoomStatus.Public
                        : RoomStatus.Private;
                    _roomNames[i].Value = rooms[i].Name;
                    _roomMaximums[i].Value = rooms[i].MaxPlayers.ToString("D1");
                    _roomCurrents[i].Value = rooms[i].PlayerCount.ToString("D1");
                }
                else
                {
                    _roomsInteractable[i].Value = false;
                    _roomStatuses[i].Value = RoomStatus.None;
                    _roomNames[i].Value = "";
                    _roomMaximums[i].Value = "0";
                    _roomCurrents[i].Value = "0";
                }
            }
        }

        public static bool HasRoom(string room)
        {
            return _rooms.ContainsKey(room);
        }
    }
}
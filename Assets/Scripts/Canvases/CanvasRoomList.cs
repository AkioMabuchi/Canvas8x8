using System;
using Canvases.UIObjects;
using Models;
using UniRx;
using UnityEngine;

namespace Canvases
{
    public class CanvasRoomList : SingletonMonoBehaviour<CanvasRoomList>
    {
        [SerializeField] private Sprite spriteButtonRoomNone;
        [SerializeField] private Sprite spriteButtonRoomPrivate;
        [SerializeField] private Sprite spriteButtonRoomPublic;
        
        [SerializeField] private ButtonRoom[] buttonRooms = new ButtonRoom[30];

        private readonly Subject<string> _onClickButtonRoom = new Subject<string>();
        public IObservable<string> OnClickButtonRoom => _onClickButtonRoom;

        protected override void OnAwake()
        {
            for (int i = 0; i < 30; i++)
            {
                int ii = i;

                RoomListModel.RoomsInteractable[i].Subscribe(interactable =>
                {
                    buttonRooms[ii].SetInteractable(interactable);
                }).AddTo(gameObject);


                RoomListModel.RoomStatuses[i].Subscribe(status =>
                {
                    switch (status)
                    {
                        case RoomStatus.Private:
                        {
                            buttonRooms[ii].SetButtonImageSprite(spriteButtonRoomPrivate);
                            buttonRooms[ii].ShowOrHideTexts(true);
                            break;
                        }
                        case RoomStatus.Public:
                        {
                            buttonRooms[ii].SetButtonImageSprite(spriteButtonRoomPublic);
                            buttonRooms[ii].ShowOrHideTexts(true);
                            break;
                        }
                        default:
                        {
                            buttonRooms[ii].SetButtonImageSprite(spriteButtonRoomNone);
                            buttonRooms[ii].ShowOrHideTexts(false);
                            break;
                        }
                    }
                }).AddTo(gameObject);


                RoomListModel.RoomNames[i].Subscribe(roomName =>
                {
                    buttonRooms[ii].SetRoomName(roomName);
                    buttonRooms[ii].SetRoomNameText(roomName);
                }).AddTo(gameObject);


                RoomListModel.RoomMaximums[i].Subscribe(maximum =>
                {
                    buttonRooms[ii].SetMaximumText(maximum);
                }).AddTo(gameObject);


                RoomListModel.RoomCurrents[i].Subscribe(current =>
                {
                    buttonRooms[ii].SetCurrentText(current);
                }).AddTo(gameObject);
            }
        }

        private void Start()
        {
            for (int i = 0; i < 30; i++)
            {
                buttonRooms[i].OnClickButton.Subscribe(roomName =>
                {
                    _onClickButtonRoom.OnNext(roomName);
                }).AddTo(gameObject);
            }
        }
    }
}

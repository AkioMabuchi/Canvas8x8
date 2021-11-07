using System;
using Canvases.UIObjects;
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

        public void SetRoomButtonInteractable(int index, bool interactable)
        {
            buttonRooms[index].SetInteractable(interactable);
        }

        public void SetRoomButtonStatus(int index, RoomStatus status)
        {
            switch (status)
            {
                case RoomStatus.Private:
                {
                    buttonRooms[index].SetButtonImageSprite(spriteButtonRoomPrivate);
                    buttonRooms[index].ShowOrHideTexts(true);
                    break;
                }
                case RoomStatus.Public:
                {
                    buttonRooms[index].SetButtonImageSprite(spriteButtonRoomPublic);
                    buttonRooms[index].ShowOrHideTexts(true);
                    break;
                }
                default:
                {
                    buttonRooms[index].SetButtonImageSprite(spriteButtonRoomNone);
                    buttonRooms[index].ShowOrHideTexts(false);
                    break;
                }
            }
        }

        public void SetRoomButtonRoomName(int index, string roomName)
        {
            buttonRooms[index].SetRoomName(roomName);
        }

        public void SetRoomButtonRoomNameText(int index, string text)
        {
            buttonRooms[index].SetRoomNameText(text);
        }

        public void SetRoomButtonMaximumText(int index, string text)
        {
            buttonRooms[index].SetMaximumText(text);
        }

        public void SetRoomButtonCurrentText(int index, string text)
        {
            buttonRooms[index].SetCurrentText(text);
        }
    }
}

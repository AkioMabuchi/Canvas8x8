using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Models;
using Photon.Pun;
using UnityEngine;

namespace Managers
{
    public class PictureManager : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            var hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
            for (int i = 0; i < 64; i++)
            {
                Color color = Color.white;
                string keyRed = "PicturePixelRed" + i;
                string keyGreen = "PicturePixelGreen" + i;
                string keyBlue = "PicturePixelBlue" + i;
                if (hashtable.ContainsKey(keyRed))
                {
                    if (hashtable[keyRed] is float value)
                    {
                        color.r = value;
                    }
                }

                if (hashtable.ContainsKey(keyGreen))
                {
                    if (hashtable[keyGreen] is float value)
                    {
                        color.g = value;
                    }
                }

                if (hashtable.ContainsKey(keyBlue))
                {
                    if (hashtable[keyBlue] is float value)
                    {
                        color.b = value;
                    }
                }

                PictureModel.DrawPixel(i, color);
            }
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            for (int i = 0; i < 64; i++)
            {
                Color color = PictureModel.PixelsColor[i].Value;
                string keyRed = "PicturePixelRed" + i;
                string keyGreen = "PicturePixelGreen" + i;
                string keyBlue = "PicturePixelBlue" + i;
                if (propertiesThatChanged.ContainsKey(keyRed))
                {
                    color.r = (float) propertiesThatChanged[keyRed];
                }

                if (propertiesThatChanged.ContainsKey(keyGreen))
                {
                    color.g = (float) propertiesThatChanged[keyGreen];
                }

                if (propertiesThatChanged.ContainsKey(keyBlue))
                {
                    color.b = (float) propertiesThatChanged[keyBlue];
                }

                PictureModel.DrawPixel(i, color);
            }
        }
    }
}

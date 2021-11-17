using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace SceneManagers.MainSceneStates
{
    public class MainSceneStateGameInitialize : MainSceneState
    {
        public override void OnChangedRoomState()
        {
            if(true){}

            if (!PhotonNetwork.IsMasterClient) return;

            var intArrayExaminersTurn =
                PhotonNetwork.CurrentRoom.Players.Values.Select(player => player.ActorNumber).ToArray();

            if (intArrayExaminersTurn.Length >= 2)
            {
                var intI = intArrayExaminersTurn.Length - 1;
                while (intI > 0)
                {
                    var intJ = Random.Range(0, intI);
                    var intTmp = intArrayExaminersTurn[intI];
                    intArrayExaminersTurn[intI] = intArrayExaminersTurn[intJ];
                    intArrayExaminersTurn[intJ] = intTmp;
                    intI--;
                }
            }

            var hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
            hashtable["Round"] = 0;
            hashtable["Examiners"] = intArrayExaminersTurn;
            hashtable["Status"] = RoomState.GameStart;

            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }
    }
}

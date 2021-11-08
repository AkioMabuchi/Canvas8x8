using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    public class CanvasPlayerList : SingletonMonoBehaviour<CanvasPlayerList>
    {
        [SerializeField] private Image imageFrame;
        [SerializeField] private TextMeshProUGUI[] textMeshProsPlayerName = new TextMeshProUGUI[5];
        [SerializeField] private TextMeshProUGUI[] textMeshProsMessage = new TextMeshProUGUI[5];

        public void Show()
        {
            imageFrame.gameObject.SetActive(true);
        }

        public void Hide()
        {
            imageFrame.gameObject.SetActive(false);
        }

        public void UpdateMessages(IReadOnlyList<Player> players)
        {

            for (int i = 0; i < 5; i++)
            {
                if (i < players.Count)
                {
                    Player player = players[i];
                    textMeshProsPlayerName[i].text = player.NickName;
                    if (player.CustomProperties.ContainsKey("Status"))
                    {
                        switch ((PlayerStatus) player.CustomProperties["Status"])
                        {
                            case PlayerStatus.NotReady:
                            {
                                textMeshProsMessage[i].text = "準備中";
                                break;
                            }
                            case PlayerStatus.Ready:
                            {
                                textMeshProsMessage[i].text = "準備完了！";
                                break;
                            }
                            case PlayerStatus.Examiner:
                            {
                                textMeshProsMessage[i].text = "出題者";
                                break;
                            }
                            case PlayerStatus.Answerer:
                            {
                                if (player.CustomProperties.ContainsKey("Answer"))
                                {
                                    textMeshProsMessage[i].text = (string) player.CustomProperties["Answer"];
                                }
                                else
                                {
                                    textMeshProsMessage[i].text = "";
                                }

                                break;
                            }
                        }
                    }
                    else
                    {
                        textMeshProsMessage[i].text = "";
                    }
                }
                else
                {
                    textMeshProsPlayerName[i].text = "";
                    textMeshProsMessage[i].text = "";
                }
            }
        }
    }
}

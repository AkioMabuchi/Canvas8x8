using Photon.Pun;

namespace SceneManagers.MainSceneStates
{
    public class MainSceneStateIdle : MainSceneState
    {
        public override void OnClickButtonExit()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnClickButtonReady()
        {
            CanvasTheme.Instance.SetThemeText("変更してみる");
        }
    }
}

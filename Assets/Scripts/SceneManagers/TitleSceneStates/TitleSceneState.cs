namespace SceneManagers.TitleSceneStates
{
    public abstract class TitleSceneState
    {
        public virtual void OnEnter(TitleSceneManager titleSceneManager, TitleSceneState prevState)
        {
        }

        public virtual void OnExit(TitleSceneManager titleSceneManager, TitleSceneState nextState)
        {
            
        }
    }
}

public abstract class UIState
{
    public abstract UIState SetUIMainMenu(UIMainMenu uiMainMenu);
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
}

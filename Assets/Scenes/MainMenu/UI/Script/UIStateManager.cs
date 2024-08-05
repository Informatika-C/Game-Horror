using UnityEngine;

public class UIStateManager : MonoBehaviour
{
    private UIMainMenu uiMainMenu;
    private UIState _currentState;
    private UIState _uiMainMenuState;
    private UIState _uiMultiplayerState;

    public void CreateAllState()
    {
        _uiMainMenuState = new UIMainMenuState().SetUIMainMenu(uiMainMenu);
        _uiMultiplayerState = new UIMultiplayerState().SetUIMainMenu(uiMainMenu);
    }

    public void SetUIMainMenu(UIMainMenu uiMainMenu)
    {
        this.uiMainMenu = uiMainMenu;
    }

    public void SetState(UIState state)
    {
        if (_currentState != null)
        {
            _currentState.OnExit();
        }

        _currentState = state;
        _currentState.OnEnter();
    }

    private void Update()
    {
        if (_currentState != null)
        {
            _currentState.OnUpdate();
        }
    }

    public UIState GetUIMainMenuState()
    {
        return _uiMainMenuState;
    }

    public UIState GetUIMultiplayerState()
    {
        return _uiMultiplayerState;
    }
}

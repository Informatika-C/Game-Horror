using UnityEngine.UIElements;
using UnityEngine;

public class UIMainMenuState : UIState
{
    UIMainMenu uiMainMenu;    
    VisualElement root;
    VisualElement singlePlayerButton;
    VisualElement multiplayerButton;
    VisualElement settingsButton;
    VisualElement quitButton;

    public override UIState SetUIMainMenu(UIMainMenu uiMainMenu)
    {
        this.uiMainMenu = uiMainMenu;
        return this;
    }

    public override void OnEnter()
    {
        root = uiMainMenu.uiDocument.rootVisualElement;
        singlePlayerButton = root.Q<VisualElement>("singleplayer");
        multiplayerButton = root.Q<VisualElement>("multiplayer");
        settingsButton = root.Q<VisualElement>("options");
        quitButton = root.Q<VisualElement>("exit");

        singlePlayerButton.RegisterCallback<ClickEvent>(ev => OnSinglePlayerButtonClicked());
        multiplayerButton.RegisterCallback<ClickEvent>(ev => OnMultiplayerButtonClicked());
        settingsButton.RegisterCallback<ClickEvent>(ev => OnSettingsButtonClicked());
        quitButton.RegisterCallback<ClickEvent>(ev => OnQuitButtonClicked());

        RenderView();
    }

    private void OnSinglePlayerButtonClicked()
    {
        Debug.Log("Single Player Button Clicked");
    }

    private void OnMultiplayerButtonClicked()
    {
        UIState uiState = uiMainMenu.uiStateManager.GetUIMultiplayerState();
        uiMainMenu.uiStateManager.SetState(uiState);
    }

    private void OnSettingsButtonClicked()
    {
        Debug.Log("Settings Button Clicked");
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    private void RenderView()
    {
        root.Q<VisualElement>("main-menu-container").style.display = DisplayStyle.Flex;
    }

    
    public override void OnExit()
    {
        uiMainMenu.ResetUIView();
    }

    public override void OnUpdate()
    {
    }
}

using UnityEngine.UIElements;
using UnityEngine;

public class UIMainMenuState : UIState
{
    readonly UIMainMenu uiMainMenu;    
    VisualElement root;
    VisualElement singlePlayerButton;
    VisualElement multiplayerButton;
    VisualElement settingsButton;
    VisualElement quitButton;

    public UIMainMenuState(UIMainMenu uiMainMenu)
    {
        this.uiMainMenu = uiMainMenu;
        UIInitialization();
        SetEventHandlers();
    }

    private void UIInitialization()
    {
        root = uiMainMenu.uiDocument.rootVisualElement;
        singlePlayerButton = root.Q<VisualElement>("singleplayer");
        multiplayerButton = root.Q<VisualElement>("multiplayer");
        settingsButton = root.Q<VisualElement>("options");
        quitButton = root.Q<VisualElement>("exit");
    }

    private void SetEventHandlers()
    {
        singlePlayerButton.RegisterCallback<ClickEvent>(ev => OnSinglePlayerButtonClicked());
        multiplayerButton.RegisterCallback<ClickEvent>(ev => OnMultiplayerButtonClicked());
        settingsButton.RegisterCallback<ClickEvent>(ev => OnSettingsButtonClicked());
        quitButton.RegisterCallback<ClickEvent>(ev => OnQuitButtonClicked());
    }

    public override void OnEnter()
    {
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

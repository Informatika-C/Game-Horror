using UnityEngine.UIElements;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIMultiplayerState : UIState
{
    UIMainMenu uiMainMenu;    
    VisualElement root;
    VisualTreeAsset lobbyComponent;
    ListView lobbyList;
    VisualElement multiplayerContainer;
    VisualElement refreshButton;
    VisualElement addLobbyButton;
    VisualElement exitButton;
    List<LobbyData> items = new List<LobbyData>();
    private bool isAddLobbyContainerOpen = false;

    public override UIState SetUIMainMenu(UIMainMenu uiMainMenu)
    {
        this.uiMainMenu = uiMainMenu;
        return this;
    }

    private void Initialization()
    {
        root = uiMainMenu.uiDocument.rootVisualElement;
        lobbyComponent = uiMainMenu.lobbyComponent;
        multiplayerContainer = root.Q<VisualElement>("multiplayer-menu-container");
        lobbyList = multiplayerContainer.Q<ListView>("list-loby-container");
        addLobbyButton = multiplayerContainer.Q<VisualElement>("to-add-loby");
        refreshButton = multiplayerContainer.Q<VisualElement>("refresh-button");
        exitButton = multiplayerContainer.Q<VisualElement>("exit-multiplayer-menu");
    }

    private void SetEventHandlers()
    {
        addLobbyButton.RegisterCallback<ClickEvent>(ev => SetAddLobbyContainer(true));
        refreshButton.RegisterCallback<ClickEvent>(ev => OnEnter());
        exitButton.RegisterCallback<ClickEvent>(ev => OnExitButtonClicked());
    }

    public override void OnEnter()
    {
        Initialization();
        SetEventHandlers();
        ResetState();
        uiMainMenu.StartCoroutine(LoadLobbyData());
    }

    private void ResetState()
    {
        items.Clear();
        lobbyList.Clear();
        SetAddLobbyContainer(false);
    }

    private IEnumerator LoadLobbyData()
    {
        RenderView();
        yield return uiMainMenu.StartCoroutine(RenderText("Loading Lobby Data", 2.0f));
        items.Add(new LobbyData { lobbyId = "1", lobbyName = "Lobby 1", currentPlayers = "1", maxPlayers = "4" });
        items.Add(new LobbyData { lobbyId = "2", lobbyName = "Lobby 2", currentPlayers = "2", maxPlayers = "4" });
        items.Add(new LobbyData { lobbyId = "3", lobbyName = "Lobby 3", currentPlayers = "3", maxPlayers = "4" });
        items.Add(new LobbyData { lobbyId = "4", lobbyName = "Lobby 4", currentPlayers = "4", maxPlayers = "4" });
        LoadListView();
    }

    private void LoadListView()
    {
        ResetMultiplayerContainerView();
        lobbyList.Clear();
        
        lobbyList.itemsSource = items;
        lobbyList.makeItem = () => lobbyComponent.CloneTree();
        lobbyList.bindItem = (element, i) =>
        {
            var label = element.Q<Label>();
            var lobbyData = items[i];
            label.text = lobbyData.lobbyName;
        };

        lobbyList.fixedItemHeight = 200;
        ShowLobbyContainer();
    }

    private void OnExitButtonClicked()
    {
        UIState uiState = uiMainMenu.uiStateManager.GetUIMainMenuState();
        uiMainMenu.uiStateManager.SetState(uiState);
    }

    private void SetAddLobbyContainer(bool value)
    {
        isAddLobbyContainerOpen = value;
        VisualElement container = multiplayerContainer.Q<VisualElement>("multiplayer-container");
        VisualElement addLobbyContainer = container.Q<VisualElement>("container-add-lobby");
        VisualElement exitButton = addLobbyContainer.Q<VisualElement>("add-lobby-exit-button");
        exitButton.RegisterCallback<ClickEvent>(ev => SetAddLobbyContainer(false));
        
        if (isAddLobbyContainerOpen)
        {
            addLobbyContainer.style.display = DisplayStyle.Flex;
        }
        else
        {
            addLobbyContainer.style.display = DisplayStyle.None;
        }
    }

    private IEnumerator RenderText(string text, float duration)
    {
        ShowText(text);
        yield return new WaitForSeconds(duration);
    }

    private void ShowLobbyContainer()
    {
        VisualElement container = multiplayerContainer.Q<VisualElement>("multiplayer-container");
        VisualElement lobbyContainer = container.Q<VisualElement>("lobby-container");
        lobbyContainer.style.display = DisplayStyle.Flex;
    }

    private void ShowText(string text)
    {
        ResetMultiplayerContainerView();
        VisualElement container = multiplayerContainer.Q<VisualElement>("multiplayer-container");
        VisualElement placeholderText = container.Q<VisualElement>("text-only");
        placeholderText.style.display = DisplayStyle.Flex;
        Label label = placeholderText.Q<Label>();
        label.text = text;
    }

    private void ResetMultiplayerContainerView()
    {
        VisualElement container = multiplayerContainer.Q<VisualElement>("multiplayer-container");
        foreach (var child in container.Children())
        {
            child.style.display = DisplayStyle.None;
        }
    }

    private void RenderView()
    {
        multiplayerContainer.style.display = DisplayStyle.Flex;
    }

    public override void OnExit()
    {
        uiMainMenu.StopAllCoroutines();
        uiMainMenu.ResetUIView();
    }

    public override void OnUpdate()
    {}
}

public class LobbyData
{
    public string lobbyId;
    public string lobbyName;
    public string currentPlayers;
    public string maxPlayers;
}

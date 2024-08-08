using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Threading.Tasks;
using R3;
using Unity.Services.Lobbies.Models;

public class UIMultiplayerState : UIState
{
    readonly UIMainMenu uiMainMenu;    
    VisualElement root;
    VisualTreeAsset lobbyComponent;
    ListView lobbyList;
    VisualElement multiplayerContainer;
    VisualElement refreshButton;
    VisualElement openAddLobbyButton;
    VisualElement exitButton;
    TextField addLobby;
    Button addLobbyButton;
    TextField findLobbyByCode;
    Button findLobbyButton;
    readonly List<Lobby> items = new();
    private bool isAddLobbyContainerOpen = false;
    LobbyFacade lobbyFacade;

    private async Task InitializationAsync()
    {
        lobbyFacade ??= new LobbyFacade();
        await lobbyFacade.InitializeAsync();
    }

    public UIMultiplayerState(UIMainMenu uiMainMenu)
    {
        this.uiMainMenu = uiMainMenu;
        UIInitialization();
        SetEventHandlers();
    }

    private void UIInitialization()
    {
        root = uiMainMenu.uiDocument.rootVisualElement;
        lobbyComponent = uiMainMenu.lobbyComponent;
        multiplayerContainer = root.Q<VisualElement>("multiplayer-menu-container");
        lobbyList = multiplayerContainer.Q<ListView>("list-loby-container");
        openAddLobbyButton = multiplayerContainer.Q<VisualElement>("to-add-loby");
        refreshButton = multiplayerContainer.Q<VisualElement>("refresh-button");
        addLobby = multiplayerContainer.Q<TextField>("textfield-name-add-loby");
        addLobbyButton = multiplayerContainer.Q<Button>("button-add-loby");
        findLobbyByCode = multiplayerContainer.Q<TextField>("textfield-client-code");
        findLobbyButton = multiplayerContainer.Q<Button>("button-submit-code");
        exitButton = multiplayerContainer.Q<VisualElement>("exit-multiplayer-menu");
    }

    private void SetEventHandlers()
    {
        openAddLobbyButton.RegisterCallback<ClickEvent>(ev => SetOpenAddLobbyContainer(true));
        refreshButton.RegisterCallback<ClickEvent>(ev => OnEnter());
        addLobbyButton.RegisterCallback<ClickEvent>(ev => OnAddLobbyButtonClicked());
        findLobbyButton.RegisterCallback<ClickEvent>(ev => OnFindLobbyButtonClicked());
        exitButton.RegisterCallback<ClickEvent>(ev => OnExitButtonClicked());
    }

    private void OnFindLobbyButtonClicked()
    {
        if (string.IsNullOrEmpty(findLobbyByCode.value)) return;
        JoinToLobbyServerByCode(findLobbyByCode.value);
    }

    private void JoinToLobbyServerByCode(string lobbyCode)
    {
        Observable<Lobby> observable = lobbyFacade.JoinLobbyByCode(lobbyCode);
        ShowText("Bergabung dengan lobby...");
        observable.Subscribe(lobby =>
        {
            OnEnter();
        },
        async error =>
        {
            ShowText("Tidak dapat bergabung dengan lobby: " + error.Message);
            await Task.Delay(2000);
            OnEnter();
        },_ => {});
    }

    private void OnAddLobbyButtonClicked()
    {
        if (string.IsNullOrEmpty(addLobby.value)) return;
        AddLobbyToServer(addLobby.value);
    }

    private void AddLobbyToServer(string lobbyName)
    {
        Observable<Lobby> observable = lobbyFacade.CreateLobbyObservable(lobbyName);
        ShowText("Menambahkan lobby...");
        observable.Subscribe(lobby =>
        {
            OnEnter();
        },
        async error =>
        {
            ShowText("Tidak dapat menambahkan lobby");
            await Task.Delay(2000);
            OnEnter();
        },_ => {});
    }

    public override async void OnEnter()
    {
        await InitializationAsync();
        ResetState();
        RenderView();
        LoadLobbyFromServer();
    }

    private Observable<QueryResponse> LoadLobbyFromServer()
    {
        Observable<QueryResponse> observable = lobbyFacade.QueryLobbiesObservable();
        ShowText("Memuat data...");
        observable.Subscribe(response =>
        {
            items.Clear();
            response.Results.ForEach(lobby =>
            {
                items.Add(lobby);
            });
            LoadListView();
        },
        async error =>
        {
            ShowText("Tidak dapat memuat data");
            await Task.Delay(2000);
            LoadListView();
        },_ => {});

        return observable;
    }

    private void ResetState()
    {
        items.Clear();
        lobbyList.Clear();
        SetOpenAddLobbyContainer(false);
        addLobby.value = "";
        findLobbyByCode.value = "";
    }

    private void LoadListView()
    {
        ResetMultiplayerContainerView();
        lobbyList.Clear();
        
        lobbyList.itemsSource = items;
        lobbyList.makeItem = () => lobbyComponent.CloneTree();
        lobbyList.bindItem = (element, i) =>
        {
            var lobbyData = items[i];

            var name = element.Q<Label>("lobby-name");
            name.text = lobbyData.Name;
            var playerCount = element.Q<Label>("player-count");
            playerCount.text = lobbyData.Players.Count + " / " + lobbyData.MaxPlayers;
            var lobbyCode = element.Q<Label>("lobby-code");
            lobbyCode.text = lobbyData.LobbyCode;
            var joinButton = element.Q<Button>("button-joint-loby");
            joinButton.RegisterCallback<ClickEvent>(ev => JoinToLobbyServer(lobbyData.Id));
        };

        lobbyList.fixedItemHeight = 200;
        ShowLobbyContainer();
    }

    private Observable<Lobby> JoinToLobbyServer(string lobbyId)
    {
        Observable<Lobby> observable = lobbyFacade.JoinLobbyObservable(lobbyId);
        ShowText("Bergabung dengan lobby...");
        observable.Subscribe(lobby =>
        {
            OnEnter();
        },
        async error =>
        {
            ShowText("Tidak dapat bergabung dengan lobby: " + error.Message);
            await Task.Delay(2000);
            OnEnter();
        },_ => {});
    
        return observable;
    }

    private void OnExitButtonClicked()
    {
        UIState uiState = uiMainMenu.uiStateManager.GetUIMainMenuState();
        uiMainMenu.uiStateManager.SetState(uiState);
    }

    private void SetOpenAddLobbyContainer(bool value)
    {
        isAddLobbyContainerOpen = value;
        VisualElement container = multiplayerContainer.Q<VisualElement>("multiplayer-container");
        VisualElement openAddLobbyContainer = container.Q<VisualElement>("container-add-lobby");
        VisualElement exitButton = openAddLobbyContainer.Q<VisualElement>("add-lobby-exit-button");
        exitButton.RegisterCallback<ClickEvent>(ev => SetOpenAddLobbyContainer(false));
        
        if (isAddLobbyContainerOpen)
        {
            openAddLobbyContainer.style.display = DisplayStyle.Flex;
        }
        else
        {
            openAddLobbyContainer.style.display = DisplayStyle.None;
        }
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

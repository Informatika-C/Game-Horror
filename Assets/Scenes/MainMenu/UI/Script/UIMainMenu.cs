using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField]
    public VisualTreeAsset lobbyComponent; 

    [HideInInspector]
    public UIDocument uiDocument;

    [HideInInspector]
    public UIStateManager uiStateManager;

    private void Awake()
    {
        uiStateManager = gameObject.AddComponent<UIStateManager>();
        uiDocument = GetComponent<UIDocument>();
        ResetUIView();

        uiStateManager.SetUIMainMenu(this);
        uiStateManager.CreateAllState();

        UIState uiState = uiStateManager.GetUIMainMenuState();
        uiStateManager.SetState(uiState);
    }

    public void ResetUIView()
    {
        var root = uiDocument.rootVisualElement;
        var parentContainer = root.Q<VisualElement>("parent-container");
        foreach (var child in parentContainer.Children())
        {
            child.style.display = DisplayStyle.None;
        }
    }
}

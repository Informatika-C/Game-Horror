using Unity.Services.Core;
using UnityEngine;
using System;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using System.Threading.Tasks;
using R3;

public class LobbyFacade {

    private const int MaxRetries = 3;
    private const int InitialDelay = 1000;

    public async Task InitializeAsync()
    {
        await InitializationUGS();
        await SignInAnonymously();
    }

    private async Task InitializationUGS()
    {
        try
        {
            var options = new InitializationOptions();
            options.SetProfile("Player" + UnityEngine.Random.Range(0, 1000));
            await UnityServices.InitializeAsync(options);
        }
        catch (Exception e)
        {
            Debug.LogError("Unity Services initialization failed: " + e.Message);
        }
    }

    private async Task SignInAnonymously()
    {
        try
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    public string GetPlayerId()
    {
        return AuthenticationService.Instance.PlayerId;
    }

    private async Task RetryOperationAsync(Func<Task<QueryResponse>> operation, Observer<QueryResponse> observer)
    {
        int retryCount = 0;
        while (retryCount < MaxRetries)
        {
            try
            {
                var response = await operation();
                observer.OnNext(response);
                observer.OnCompleted();
                return;
            }
            catch (LobbyServiceException ex) when (ex.Message.Contains("Rate limit has been exceeded"))
            {
                retryCount++;
                int delay = InitialDelay * (int)Math.Pow(2, retryCount); // Exponential backoff
                Debug.Log($"Rate limit exceeded. Retrying in {delay}ms...");
                await Task.Delay(delay);
            }
        }

        observer.OnCompleted();
    }

    public Observable<QueryResponse> QueryLobbiesObservable()
    {
        return Observable.Create<QueryResponse>(async (observer, cancellationToken) =>
        {
            try
            {
                await RetryOperationAsync(() => LobbyService.Instance.QueryLobbiesAsync(), observer);
            }
            catch (LobbyServiceException e)
            {
                observer.OnErrorResume(e);
            }
        });
    }

    public Observable<Lobby> CreateLobbyObservable(string lobbyName)
    {
        return Observable.Create<Lobby>(async (observer, cancellationToken) =>
        {
            try
            {                
                int maxPlayers = 4;
                CreateLobbyOptions options = new()
                {
                    IsPrivate = false
                };

                Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
                observer.OnNext(lobby);
                observer.OnCompleted();
            }
            catch (LobbyServiceException e)
            {
                observer.OnErrorResume(e);
            }
        });
    }

    public Observable<Lobby> JoinLobbyObservable(string lobbyId)
    {
        return Observable.Create<Lobby>(async (observer, cancellationToken) =>
        {
            try
            {
                Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
                observer.OnNext(lobby);
                observer.OnCompleted();
            }
            catch (LobbyServiceException e)
            {
                observer.OnErrorResume(e);
            }
        });
    }

    public Observable<Lobby> JoinLobbyByCode(string lobbyCode)
    {
        return Observable.Create<Lobby>(async (observer, cancellationToken) =>
        {
            try
            {
                Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
                observer.OnNext(lobby);
                observer.OnCompleted();
            }
            catch (LobbyServiceException e)
            {
                observer.OnErrorResume(e);
            }
        });
    }
}
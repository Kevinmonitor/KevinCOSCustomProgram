using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;

using TMPro;
using Systems.SceneManagement;

using UnityEngine;

public class OfficialLeaderboard : MonoBehaviour
{
    // Create a leaderboard with this ID in the Unity Cloud Dashboard
    const string leaderboardID = "submissionLeaderboard";

    public TMPro.TextMeshProUGUI[] rNames;
    public TMPro.TextMeshProUGUI[] rScores;

    [SerializeField] TMP_Text usernameText; 

    async void Awake()
    {
        // start unity services.
        await UnityServices.InitializeAsync();

    }

    private async void Start() {

        AuthenticationService.Instance.SignedIn += OnSignedIn;
        AuthenticationService.Instance.SignInFailed += OnSignInFailed;

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        for (int i = 0; i < rNames.Length; i++)
        {
            rNames[i].text = i + 1 + ". Unknown";
        }

        StartCoroutine("RefreshHighscores");

    }

    private void OnSignedIn() {
        Debug.Log("Sign in success");
    }

    private void OnSignInFailed(RequestFailedException exception) {
        Debug.Log("Sign in fail");
    }

    public async void SubmitToLeaderboard(){

        if(usernameText.text.Length < 3)
            {return;}

        else
        {
            try{
                await AuthenticationService.Instance.UpdatePlayerNameAsync(usernameText.text);
                await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardID, objGameManager.Instance.GetScore());
            }
            catch
            {
                Debug.Log("Failed to send score.");
            }
            //HighScores.UploadScore(usernameText.text, objGameManager.Instance.GetScore());   
        }

        LoadScoresAsync();

    }

    public async void ToMenu(){
        await SceneLoader.Instance.LoadSceneGroup(0);
    }

    private async void LoadScoresAsync() {
        try {
            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(leaderboardID);

            for (int i = 0; i < rNames.Length;i ++)
            {
                if (scoresResponse.Results.Count > i)
                {
                    rScores[i].text = scoresResponse.Results[i].Score.ToString();
                    rNames[i].text = (scoresResponse.Results[i].Rank+1).ToString() + ". " + scoresResponse.Results[i].PlayerName;
                }
            }

            foreach (var leaderboardEntry in scoresResponse.Results) {

            }

        }
        catch {
            Debug.Log("Failed to retrieve score.");
            throw;
        }
    }

    IEnumerator RefreshHighscores() //Refreshes the scores every 30 seconds
    {
        while(true)
        {
            LoadScoresAsync();
            yield return new WaitForSeconds(30);
        }
    }

}
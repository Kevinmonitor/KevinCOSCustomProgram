using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Systems.SceneManagement;

using Dan.Main;
using Dan.Models;

public class DanqzqLeaderboardManager : MonoBehaviour
{
    // Before using this leaderboard library, familiarise yourself with the steps to create a new leaderboard.
    // In a nutshell, access Danqzq's Leaderboard Creator via itch.io and create a leaderboard with the secret/public keys.
    // Then, install the Leaderboard Creator package for Unity and connect your online leaderboard to the package's editor interface.

    public TMPro.TextMeshProUGUI[] rNames;
    public TMPro.TextMeshProUGUI[] rScores;

    [SerializeField] TMP_Text usernameText; 

    private void Start()
    {

        for (int i = 0; i < rNames.Length; i++)
        {
            rNames[i].text = i + 1 + ". Unknown";
        }
        
        StartCoroutine("RefreshHighscores");

    }

    private void LoadEntries()
    {
        // replace mySubmissionLeaderboard with the name of your leaderboard ID.
        Leaderboards.mySubmissionLeaderboard.GetEntries(OnSuccess, OnFail);
    }

    private void OnSuccess(Entry[] entries)
    {

        for (int i = 0; i < rNames.Length && i < entries.Length; i++){
            rNames[i].text = entries[i].Rank.ToString() + ". " + entries[i].Username;
            rScores[i].text = entries[i].Score.ToString();
        }

    }
    
    private void OnFail(string error){
        Debug.Log($"Failed to get leaderboards due to {error}");
    }
    
    public void UploadEntry()
    {
        Leaderboards.mySubmissionLeaderboard.UploadNewEntry(usernameText.text, objGameManager.Instance.GetScore(), isSuccessful =>
        {
            if (isSuccessful)
                LoadEntries();
        });
    }

    IEnumerator RefreshHighscores() //Refreshes the scores every 30 seconds
    {
        while(true)
        {
            LoadEntries();
            yield return new WaitForSeconds(30);
        }
    }

    public void SubmitToLeaderboard(){

        if(usernameText.text.Length < 3)
            {return;}

        else
        {
            UploadEntry();
        }

    }

    public async void ToMenu(){
        await SceneLoader.Instance.LoadSceneGroup(0);
    }
}

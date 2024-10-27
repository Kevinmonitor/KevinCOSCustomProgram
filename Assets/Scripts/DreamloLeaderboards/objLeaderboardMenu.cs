using System.Collections;
using System.Collections.Generic;
using TMPro;
using Systems.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class objLeaderboardMenu : MonoBehaviour
{

    [SerializeField] TMP_Text usernameText; 

    public void SubmitToLeaderboard(){
        if(usernameText.text.Length < 3){return;}
        else{
            HighScores.UploadScore(usernameText.text, objGameManager.Instance.GetScore());
        }
    }

    public async void ToMenu(){
        await SceneLoader.Instance.LoadSceneGroup(0);
    }

}

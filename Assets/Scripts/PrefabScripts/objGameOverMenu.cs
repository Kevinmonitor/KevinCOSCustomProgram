using System.Collections;
using System.Collections.Generic;
using Systems.SceneManagement;
using UnityEngine;

public class objGameOverMenu : MonoBehaviour
{
    public async void SubmitScore(){
        await SceneLoader.Instance.LoadSceneGroup(3);
    }

    public async void SubmitScoreUnity(){
        await SceneLoader.Instance.LoadSceneGroup(4);
    }

    public async void SubmitScoreAlt(){
        await SceneLoader.Instance.LoadSceneGroup(5);
    }

    public async void RestartGame(){
        await SceneLoader.Instance.LoadSceneGroup(1);
    }

    public async void GoToMenu(){
        await SceneLoader.Instance.LoadSceneGroup(0);
    }

}

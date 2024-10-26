using System.Collections;
using System.Collections.Generic;
using Systems.SceneManagement;
using UnityEngine;

public class objMenu : MonoBehaviour
{
    public async void StartGame(){
        await SceneLoader.Instance.LoadSceneGroup(1);
    }

    public void EndGame(){
        Application.Quit();
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Systems.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class objGameManager : MonoBehaviour
{

	// This singleton script manages game data.
	
	// singleton pattern
	public static objGameManager Instance { get; private set; }

	public objEnemyShot enemyBulletPrefab;
	public objPlayerShot playerBulletPrefab;
	public objEnemy enemyPrefab;

	public GameObject explosionPrefab;

	public SoundManager soundManager;
	
	public List<objEnemyShot> enemyShots;
	public List<objPlayerShot> playerShots;
	public List<objEnemy> enemyObjects;

	// garbage collection (AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA)
	public List<objEnemyShot> enemyShotsToDelete;
	public List<objPlayerShot> playerShotsToDelete;
	public List<objEnemy> enemyObjectsToDelete;

	// scorinf

	public int playerScore = 0;
	public int playerLives = 1;
	public TMP_Text scoreText;
	public TMP_Text livesText;
	public TMP_Text waveText;

	public bool isGameOver = false;

	public float rank = 0f;
	private int waveLevel = 0;

	public void AddScore(int score){
		playerScore += score;
		playerScore = (playerScore/10) * 10;
		scoreText.text = playerScore.ToString();
	}

	public void SetLives(int lives){
		playerLives = lives;
		livesText.text = playerLives.ToString();
	}

	public int CalculateScore(objEnemy target, float maxDist, int minScore, int maxScore)
	{
		Vector2 distance = target.transform.position - objPlayer.Instance.transform.position;
		int score = (int)(minScore + Mathf.Sqrt(maxDist/distance.sqrMagnitude) * (maxScore - minScore)); // linear interpolation
		return score;
	}

	public int GetScore(){
		return (int)((playerScore/10) * 10);
	}

	public int GetLives(){
		return playerLives;
	}
	private void Awake()
	{
		
		if (Instance != null && Instance != this) 
		{ 
			Destroy(this); 
		} 
		else 
		{ 
			Instance = this; 
		} 
		
		Application.targetFrameRate = 60;
		
		SetLives(1);
		scoreText.text = "0";
		SetupPool();
	}

	private void SetupPool()
	{
		objPooler.SetupPool(enemyBulletPrefab, 50, "objEnemyShot");
		objPooler.SetupPool(playerBulletPrefab, 50, "objPlayerShot");
		objPooler.SetupPool(enemyPrefab, 20, "objEnemy");
	}

	public void Initialise(){
		objGameManager.Instance.soundManager.StartMusic();
		isGameOver = false;
		SetLives(3);
		rank = 0;
		waveLevel = 0;
		playerScore = 0;
		waveText.text = "WAVE LEVEL 0";
		scoreText.text = "0";
	}
	public void CreateExplosion(Vector2 spawnPosition){

		objGameManager.Instance.soundManager.PlaySFX(objGameManager.Instance.soundManager.enemyBoom);
		GameObject boom = (GameObject)Instantiate(explosionPrefab);
		boom.transform.position = spawnPosition;

	}

	public void RankUp(){
		rank += 0.1f;
		waveLevel += 1;
		waveText.text = $"WAVE LEVEL {waveLevel}";
	}

	public async void GameOver(){
		objGameManager.Instance.soundManager.StopMusic();
		isGameOver = true;
		await SceneLoader.Instance.LoadSceneGroup(2);
	}
	
	public objEnemyShot CreateEnemyShot(Vector2 spawnPosition, float speed, float angle){

		objEnemyShot shot = objPooler.DequeueObject<objEnemyShot>("objEnemyShot");
		shot.gameObject.SetActive(true);

		shot.transform.position = spawnPosition;
		shot.SetSpeed(speed);
		shot.SetAngle(angle);

		enemyShots.Add(shot);
		// shot.Initialise(spawnPosition, speed, angle);

		return shot;

	}

	public objPlayerShot CreatePlayerShot(Vector2 spawnPosition, float speed, float angle, float damage){

		objPlayerShot shot = objPooler.DequeueObject<objPlayerShot>("objPlayerShot");
		shot.gameObject.SetActive(true);

		shot.transform.position = spawnPosition;
		shot.SetSpeed(speed);
		shot.SetAngle(angle);
		shot.SetBulletDamage(damage);

		playerShots.Add(shot);
		// shot.Initialise(spawnPosition, speed, angle, damage);

		return shot;

	}

	public objEnemy SpawnEnemy(Vector2 startPos, float speed = 5f)
	{

		objEnemy anEnemy = objPooler.DequeueObject<objEnemy>("objEnemy");
		anEnemy.gameObject.SetActive(true);

		anEnemy.transform.position = startPos;
		anEnemy.SetSpeed(speed);

		enemyObjects.Add(anEnemy);
		
		return anEnemy;

	}

	// Update is called once per frame
	void Update()
	{

		if (!isGameOver){

			rank += 0.001f;

			enemyShotsToDelete.Clear();
			playerShotsToDelete.Clear();
			enemyObjectsToDelete.Clear();
			
			// garbage collection
			foreach (objEnemyShot shot in enemyShots){
				if (!shot.gameObject.activeSelf){
					enemyShotsToDelete.Add(shot);
				}
				else{
					continue;
				}
			}

			foreach (objPlayerShot shot in playerShots){
				if (!shot.gameObject.activeSelf){
					playerShotsToDelete.Add(shot);
				}
				else{
					continue;
				}
			}

			foreach (objEnemy enm in enemyObjects){
				if (!enm.gameObject.activeSelf){
					enemyObjectsToDelete.Add(enm);
				}
				else{
					continue;
				}
			}
			// garbage collection! yippee!

			foreach (objEnemyShot shot in enemyShotsToDelete){
				enemyShots.Remove(shot);
			}

			foreach (objPlayerShot shot in playerShotsToDelete){
				playerShots.Remove(shot);
			}

			foreach (objEnemy enm in enemyObjectsToDelete){
				enemyObjects.Remove(enm);
			}

		}
		
	}
}

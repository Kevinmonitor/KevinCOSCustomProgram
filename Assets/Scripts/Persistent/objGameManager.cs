using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Systems.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class objGameManager : MonoBehaviour
{

	// singleton pattern
	public static objGameManager Instance { get; private set; }

	public objEnemyShot enemyBulletPrefab;
	public objPlayerShot playerBulletPrefab;
	public GameObject explosionPrefab;

	public List<objEnemyShot> enemyShots;
	public List<objPlayerShot> playerShots;

	// garbage collection (AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA)
	public List<objEnemyShot> enemyShotsToDelete;
	public List<objPlayerShot> playerShotsToDelete;

	// scorinf

	public int playerScore = 0;
	public int playerLives = 1;
	public Text scoreText;
	public Text livesText;

	public bool isGameOver = false;

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
		
		SetLives(1);
		scoreText.text = "0";
		SetupPool();
	}

	private void SetupPool()
	{
		objPooler.SetupPool(enemyBulletPrefab, 50, "objEnemyShot");
		objPooler.SetupPool(playerBulletPrefab, 50, "objPlayerShot");
	}

	public void Initialise(){
		SetLives(3);
		playerScore = 0;
		scoreText.text = "0";
	}
	public void CreateExplosion(Vector2 spawnPosition){

		GameObject boom = (GameObject)Instantiate(explosionPrefab);
		boom.transform.position = spawnPosition;

	}

	public async void GameOver(){
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

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{

		if (!isGameOver){

			enemyShotsToDelete.Clear();
			playerShotsToDelete.Clear();
			
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

			// garbage collection! yippee!

			foreach (objEnemyShot shot in enemyShotsToDelete){
				enemyShots.Remove(shot);
			}

			foreach (objPlayerShot shot in playerShotsToDelete){
				playerShots.Remove(shot);
			}

		}
		
	}
}

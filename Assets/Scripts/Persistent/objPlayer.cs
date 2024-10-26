using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class objPlayer : MonoBehaviour
{
	// singleton pattern
	public static objPlayer Instance { get; private set; }
	[SerializeField] private GameObject objPlayerHitbox;

	public float bulletDamage = 10f;
	public float bulletSpeed = 36f;

	public float _fireRate = 0.05f;
	private float _nextFire = 0;

	private bool _isFiring = false;
	
	public float playerSpeed;
	public float playerHitbox;
	
	public float playerIframes = 20f;
	private float playerIframeReductionRate = 0.01f;
	private float playerCurrentIframes;

    private FlashScript objFlash;

	// Use this for initialization
	void Start()
	{

		if (Instance != null && Instance != this) 
		{ 
			Destroy(this); 
		} 
		else 
		{ 
			Instance = this; 
			objFlash = GetComponent<FlashScript>();
		} 

	}

	public void OnShoot(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			_isFiring = true;
		}
		else if (context.canceled)
		{
			_isFiring = false;
		}
	}
	// Update is called once per frame
	
	public void TakeDamage(){
		if (playerCurrentIframes <= 0){
			Debug.Log("damage received!");
			objGameManager.Instance.SetLives(objGameManager.Instance.GetLives()-1);
			if(objGameManager.Instance.GetLives() <= 0){
				objGameManager.Instance.isGameOver = true;
				objGameManager.Instance.GameOver();
			}
			playerCurrentIframes = playerIframes;
		}
		else{

		}
	}

	public objPlayerShot fireBullet(Vector2 spawnPosition, float angle){

		objPlayerShot bullet = objGameManager.Instance.CreatePlayerShot(spawnPosition, bulletSpeed, angle, bulletDamage);
		return bullet;

	}

	void Update()
	{
		float x = Input.GetAxisRaw("Horizontal");//the value will be -1, 0 or 1 (for left, no input, and right)
		float y = Input.GetAxisRaw("Vertical");//the value will be -1, 0 or 1 (for down, no input, and up)

		if (playerCurrentIframes > 0){
			playerCurrentIframes -= playerIframeReductionRate;
			playerCurrentIframes = Mathf.Clamp(playerCurrentIframes, 0, playerIframes);
			if (playerCurrentIframes >= 0.5f && (int)(playerCurrentIframes * 60) % 6 == 0){
				objFlash.Flash();
			}
		}

		if (_isFiring && Time.time > _nextFire){
			_nextFire = Time.time + _fireRate;
			
			fireBullet(objPlayerHitbox.transform.position, 0f);
			fireBullet(objPlayerHitbox.transform.position, 5);
			fireBullet(objPlayerHitbox.transform.position, -5f);

			fireBullet(objPlayerHitbox.transform.position, 30f);
			fireBullet(objPlayerHitbox.transform.position, 35);

			fireBullet(objPlayerHitbox.transform.position, -35);
			fireBullet(objPlayerHitbox.transform.position, -30f);
		}
		//now based on the input we compute a direction vector, and we normalize it to get a unit vector

		Vector2 direction = new Vector2(x, y).normalized;

		//now we call the function that computes and sets the player's position

		Move(direction);

	}

	void Move(Vector2 direction)
	{

		//find the screen limits to the player's movement (left, right, top and bottom edges of the screen)

		Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)); //this is the bottom-left point (corner) of the screen
		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)); //this is the top-right point (corner) of the screen

		max.x = max.x - 0.225f; //subtract the player sprite half width
		min.x = min.x + 0.225f; //add the player sprite half width

		max.y = max.y - 0.285f; //subtract the player sprite half height
		min.y = min.y + 0.285f; //add the player sprite half height

		//Get the player's current position

		Vector2 pos = transform.position;

		//Calculate the new position

		pos += direction * playerSpeed * Time.deltaTime;

		//Make sure the new position is outside the screen

		pos.x = Mathf.Clamp(pos.x, min.x, max.x);
		pos.y = Mathf.Clamp(pos.y, min.y, max.y);

		//Update the player's position

		transform.position = pos;

	}

}

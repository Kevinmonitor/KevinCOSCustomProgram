using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class objBulletManager : MonoBehaviour
{

	public static objBulletManager Instance { get; private set; }

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
		
	}


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // enemy bullet colliding with player?
    public bool CheckEnemyShotCollision(objEnemyShot target){

        if (!target.gameObject.activeSelf || objPlayer.Instance == null) { return false; }

        var rad = objPlayer.Instance.GetHitbox() + target.GetHitbox();
        var offset = objPlayer.Instance.transform.position - target.transform.position;
        return offset.sqrMagnitude < (rad * rad); // true = collision

    }

    public bool CheckPlayerShotCollision(objPlayerShot bullet, objEnemy target){

        if (
            !bullet.gameObject.activeSelf || !target.gameObject.activeSelf 
            || objPlayer.Instance == null
        ) { return false; }

        var rad = bullet.GetHitbox() + target.GetHitbox();
        var offset = bullet.transform.position - target.transform.position;
        return offset.sqrMagnitude < (rad * rad); // true = collision

    }

    // Update is called once per frame

    // check bullet collisions
    void Update()
    {
        // check in the pool of bullets

        // if enemy hits player: 

        // player take damage
        // enable player invincibility

        if(!objGameManager.Instance.isGameOver){

            foreach (objEnemyShot shot in objGameManager.Instance.enemyShots)
            {
                if(CheckEnemyShotCollision(shot)){
                    objPlayer.Instance.TakeDamage();
                    shot.DeleteBullet();
                }
            }
        
        // if player hits enemy

        // enemy take damage
        // handle enemy destruction, as well

            foreach (objEnemy enemy in objGameManager.Instance.enemyObjects){
                foreach (objPlayerShot shot in objGameManager.Instance.playerShots){
                    if (CheckPlayerShotCollision(shot, enemy)){
                        enemy.TakeDamage(objPlayer.Instance.GetBulletDamage());
                        shot.DeleteBullet();
                        if(enemy.GetHP() <= 0f){
                            enemy.EnemyDeath();
                        }
                    }
                }
            }

        }

    }
}

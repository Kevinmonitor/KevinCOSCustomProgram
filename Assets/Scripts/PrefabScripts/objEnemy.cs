using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objEnemy : MonoBehaviour
{
    float speed;
    public GameObject objEnemyShot;

    float timer = 0;
    float angle = 90;

    public float enemyHitbox = 20f;
    public float enemyMaxHP = 100f;
    private float enemyCurrentHP = 1f;

    public void TakeDamage(float damage){
        enemyCurrentHP -= damage;
    }

    public void CheckEnemyDeath(){
        if (enemyCurrentHP > 0f) {return;}
        else{

            int score = objGameManager.Instance.CalculateScore(this, objPlayer.Instance.playerHitbox * 100f, 10, 1000);
            objGameManager.Instance.AddScore(score);

            objGameManager.Instance.CreateExplosion(transform.position);
            Destroy(gameObject);
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        speed = 5f;
        timer = 0.5f;
    }

    // Update is called once per frame

    public objEnemyShot fireBullet(Vector2 spawnPosition, float angle){

        objEnemyShot bullet = objGameManager.Instance.CreateEnemyShot(spawnPosition, 8f, angle);
        return bullet;

    }

    void Update()
    {

        Vector2 position = transform.position;
        position = new Vector2(position.x, position.y - speed * Time.deltaTime);
        transform.position = position;

        Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));

        timer -= Time.deltaTime;
        if(timer < 0)
        {
            timer = 0.5f;

            if(objPlayer.Instance != null){
                Vector3 dir = objPlayer.Instance.transform.position - transform.position;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            }

            fireBullet(transform.position, angle);
        }

        if (transform.position.y < min.y * 1.25 || transform.position.x < min.x * 1.25)
        {
            Destroy(gameObject);
        }
        else CheckEnemyDeath();

    }

}


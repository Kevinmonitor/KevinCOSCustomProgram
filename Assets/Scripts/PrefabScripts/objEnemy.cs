using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objEnemy : MonoBehaviour
{

    private float _speed = 3f;
    [SerializeField] private GameObject objEnemyShot;

    private float enemyHitbox = 0.75f;
    [SerializeField] private float enemyMaxHP = 100f;
    private float enemyCurrentHP = 1f;

    private float enemyInv = 0.2f;
    private float timer = 0f;

    public void TakeDamage(float damage){
        if (timer < enemyInv) {damage = 0;}
        enemyCurrentHP -= damage;
    }

    public void SetHP(float value){
        enemyCurrentHP = value;
    }

    public float GetHP(){
        return enemyCurrentHP;
    }

    public void RemoveEnemy()
    {
        objPooler.EnqueueObject(this, "objEnemy");
    }

    public void EnemyDeath(){
        int score = objGameManager.Instance.CalculateScore(this, objPlayer.Instance.GetHitbox() * 100f, 10, 1000);
        objGameManager.Instance.AddScore(score);
        objGameManager.Instance.CreateExplosion(transform.position);
        RemoveEnemy();
    }

    // misc properties

    public void SetSpeed(float value){
        _speed = value;
    }

    public void SetAngle(float value){
        transform.rotation = Quaternion.AngleAxis(value, Vector3.forward);
    }

    public void SetHitbox(float value){
        enemyHitbox = value;
    }

    public float GetSpeed(){
        return _speed;
    }

    public Quaternion GetAngle(){
        return transform.rotation;
    }

    public float GetHitbox(){
        return enemyHitbox;
    }
    

    // Start is called before the first frame update
    void Awake()
    {
        enemyCurrentHP = enemyMaxHP;
        transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
        _speed = 5f;
    }

    // Update is called once per frame

    public objEnemyShot fireBullet(Vector2 spawnPosition, float angle){

        objEnemyShot bullet = objGameManager.Instance.CreateEnemyShot(spawnPosition, 8f, angle);
        return bullet;

    }

    void Update()
    {

        timer += enemyInv/15f;
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        Vector2 currentWorldPosition = Camera.main.WorldToViewportPoint(transform.position);

        if (
            currentWorldPosition.x < -0.5 || currentWorldPosition.x > 1.25 ||
            currentWorldPosition.y < -0.5 || currentWorldPosition.y > 1.25
        )
        {
            RemoveEnemy();
        }

    }

}


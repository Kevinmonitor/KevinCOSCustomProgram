using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class objShot : MonoBehaviour
{

    private float _speed = 6f;
    private float _hitbox = 0.1f;

    public void SetSpeed(float value){
        _speed = value;
    }

    public void SetAngle(float value){
        transform.rotation = Quaternion.AngleAxis(value, Vector3.forward);
    }

    public void SetHitbox(float value){
        _hitbox = value;
    }

    public float GetSpeed(){
        return _speed;
    }

    public Quaternion GetAngle(){
        return transform.rotation;
    }

    public float GetHitbox(){
        return _hitbox;
    }
    
    public void UpdateObjectState(){

        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        if(
            (transform.position.x < min.x) || (transform.position.x > max.x) 
            ||
            (transform.position.y < min.y) || (transform.position.y > max.y))
        {
            DeleteBullet();
        }

        else{
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }

    }

    public virtual void DeleteBullet(){

    }

    // Update is called once per frame
    void Update()
    {
        UpdateObjectState();
    }

}

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

        Vector2 currentWorldPosition = Camera.main.WorldToViewportPoint(transform.position);

        if(
            currentWorldPosition.x < -0.2f || currentWorldPosition.x > 1.2f ||
            currentWorldPosition.y < -0.2f || currentWorldPosition.y > 1.2f
        )
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

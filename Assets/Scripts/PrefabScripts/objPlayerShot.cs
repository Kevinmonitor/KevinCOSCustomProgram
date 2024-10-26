using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class objPlayerShot : objShot
{

    private float _damage = 0;
    public float GetBulletDamage(){
        return _damage;
    }

    public void SetBulletDamage(float value){
        _damage = value;
    }

    public override void DeleteBullet()
    {
        objPooler.EnqueueObject(this, "objPlayerShot");
    }

}

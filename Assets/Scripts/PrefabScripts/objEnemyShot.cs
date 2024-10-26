using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objEnemyShot : objShot
{

    public override void DeleteBullet()
    {
        objPooler.EnqueueObject(this, "objEnemyShot");
    }

}

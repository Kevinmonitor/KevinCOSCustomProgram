using System.Collections;
using System.Collections.Generic;
//using Unity.Mathematics;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Diagnostics.CodeAnalysis;

public class objEnemySpawner : MonoBehaviour
{
	public GameObject objEnemy;
	float spawnRateInSeconds = 2f;
	
	Vector2 min;
	Vector2 max;
	void Start()
	{
		StartCoroutine("Part1");
		StartCoroutine("DetermineSpawnRate");
	}


	IEnumerator Part1(){

		while(objPlayer.Instance != null){

			yield return new WaitForSeconds(2f);

			for (float i = 0.1f; i < 0.9f; i += 0.1f){
				Enemy1(new Vector2(Mathf.Lerp(min.x, max.x, i), max.y), 180, 9f);
				yield return new WaitForSeconds(0.3f);
			}

			yield return new WaitForSeconds(0.1f);

			for (float i = 0.9f; i > 0.1f; i -= 0.1f){
				Enemy1(new Vector2(Mathf.Lerp(min.x, max.x, i), max.y), 180, 9f);
				yield return new WaitForSeconds(0.3f);
			}

			yield return new WaitForSeconds(0.1f);

			for (int i = 0; i < 4; i++){

				for (int f = 0; f < 4; f++){
					Enemy1(new Vector2(Mathf.Lerp(min.x, max.x, 0.2f + (f * 0.1f)), max.y), 180, 6f);
				}

				yield return new WaitForSeconds(0.5f);

			}

			yield return new WaitForSeconds(0.1f);

			for (int i = 0; i < 4; i++){

				for (int f = 0; f < 4; f++){
					Enemy1(new Vector2(Mathf.Lerp(min.x, max.x, 0.8f - (f * 0.1f)), max.y), 180, 6f);
				}

				yield return new WaitForSeconds(0.5f);

			}

			yield return new WaitForSeconds(1.2f);

			for (float i = 1f/7f; i < 0.95f; i += 1f/7f){

				Enemy2(new Vector2(min.x, Mathf.Lerp(min.y, max.y, 0.95f - i)), 270, 6f);
				yield return new WaitForSeconds(0.5f);

			}

			yield return new WaitForSeconds(0.1f);

			for (float i = 1f/7f; i < 0.95f; i += 1f/7f){

				Enemy2(new Vector2(max.x, Mathf.Lerp(min.y, max.y, i)), 90, 6f);
				yield return new WaitForSeconds(0.5f);

			}

			yield return new WaitForSeconds(0.5f);

			for (int f = 0; f < 8; f++){
				Enemy3(new Vector2(min.x + max.x * UnityEngine.Random.Range(0.3f, 0.5f), min.y), 0, 8f);
				yield return new WaitForSeconds(0.2f);
			}

			yield return new WaitForSeconds(0.2f);

			for (int f = 0; f < 8; f++){
				Enemy3(new Vector2(max.x * UnityEngine.Random.Range(0.7f, 0.8f), min.y), 0, 8f);
				yield return new WaitForSeconds(0.2f);
			}

			yield return new WaitForSeconds(0.2f);

			for (int f = 0; f < 6; f++){
				Enemy1(new Vector2(min.x, max.y - max.y * 0.1f), 270, 8f);
				yield return new WaitForSeconds(0.25f);
			}

			for (int f = 0; f < 6; f++){
				Enemy1(new Vector2(max.x, max.y - max.y * 0.25f), 90, 8f);
				yield return new WaitForSeconds(0.25f);
			}

			yield return new WaitForSeconds(0.3f);
			objGameManager.Instance.RankUp();

		}

	}

	public objEnemy Enemy1(Vector2 startPos, float angle = 180, float speed = 6f){

		objEnemy enm = objGameManager.Instance.SpawnEnemy(startPos, speed);
		enm.SetAngle(angle);
		StartCoroutine(EnemyAttack1(enm));

		return enm;

	}
	
	public objEnemy Enemy2(Vector2 startPos, float angle, float speed = 6f){

		objEnemy enm = objGameManager.Instance.SpawnEnemy(startPos, speed);
		enm.SetAngle(angle);
		StartCoroutine(EnemyAttack2(enm));

		return enm;

	}

	
	public objEnemy Enemy3(Vector2 startPos, float angle, float speed = 6f){

		objEnemy enm = objGameManager.Instance.SpawnEnemy(startPos, speed);
		enm.SetAngle(angle);
		StartCoroutine(EnemyAttack3(enm));

		return enm;

	}

	IEnumerator EnemyAttack1(objEnemy enm){

		yield return new WaitForSeconds(0.05f);
		float angle = 90;

		while(enm.gameObject.activeSelf){

			if(objPlayer.Instance != null){
				Vector3 dir = objPlayer.Instance.transform.position - enm.transform.position;
				angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
			}

			for (int i = -2; i < 3; i++){
				objEnemyShot shot = enm.fireBullet(enm.transform.position, angle + (0.5f + 0.3f * objGameManager.Instance.rank) * i);
				shot.SetSpeed(7f * Mathf.Pow(1.01f, objGameManager.Instance.rank));
			}

			yield return new WaitForSeconds(Mathf.Max(0.1f, 0.5f * Mathf.Pow(0.965f, objGameManager.Instance.rank)));
		
		}
	}

	IEnumerator EnemyAttack2(objEnemy enm){

		yield return new WaitForSeconds(0.05f);

		while(enm.gameObject.activeSelf){

			for (int i = 0; i < 5; i++){
				objEnemyShot shot = enm.fireBullet(enm.transform.position, 90 * i);
				shot.SetSpeed(8f * Mathf.Pow(1.01f, objGameManager.Instance.rank));
			}

			yield return new WaitForSeconds(Mathf.Max(0.2f, 0.6f * Mathf.Pow(0.965f, objGameManager.Instance.rank)));
		
		}
	}

	IEnumerator EnemyAttack3(objEnemy enm){

		yield return new WaitForSeconds(0.05f);
		float angle = 90;
		Vector2 position = new Vector2(0, 0);

		while(enm.gameObject.activeSelf){

			if(objPlayer.Instance != null){
				Vector3 dir = objPlayer.Instance.transform.position - enm.transform.position;
				angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
			}

			position = enm.transform.position;

			for (int i = 0; i < 3; i++){
				objEnemyShot shot = enm.fireBullet(position, angle + (0.5f + 0.2f * objGameManager.Instance.rank) * i);
				shot.SetSpeed(8f * Mathf.Pow(1.01f, objGameManager.Instance.rank));
				yield return new WaitForSeconds(0.1f);
			}

			yield return new WaitForSeconds(Mathf.Max(0.3f, 0.9f * Mathf.Pow(0.965f, objGameManager.Instance.rank)));
		
		}
	}

	IEnumerator DetermineSpawnRate(){

		while(objPlayer.Instance != null){
			spawnRateInSeconds = Mathf.Max(0.5f, 2f - objGameManager.Instance.rank);
			yield return new WaitForSeconds(1f);
		}

	}
	void Update(){

		min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

	}

}

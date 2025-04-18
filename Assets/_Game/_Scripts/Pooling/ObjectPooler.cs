using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
    public GameObject[] enemyPrefab;
    public int pool_size = 10;

    public Queue<GameObject> enemy_queue = new Queue<GameObject>();

    private void Start() {
        for (int i = 0; i < pool_size; i++) {
            //GameObject obj = Instantiate(enemyPrefab, transform);
            GameObject obj = RandomTypeEnemy();
            obj.SetActive(false);
            enemy_queue.Enqueue(obj);
        }
    }
    public GameObject GetGameObject() {
        GameObject enemy;
        if (enemy_queue.Count > 0) {
            enemy = enemy_queue.Dequeue();
        }
        else {
            //enemy = Instantiate(enemyPrefab, transform);
            enemy = RandomTypeEnemy();
        }
        enemy.SetActive(true);
        return enemy;
    }
    public void ReturnToPool(GameObject enemy) {
        enemy.SetActive(false);
        enemy_queue.Enqueue(enemy);
    }
    public GameObject RandomTypeEnemy() {
        int num = Random.Range(0, enemyPrefab.Length);
        GameObject enemy = Instantiate(enemyPrefab[num], transform);
        return enemy;
    }
}

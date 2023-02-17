using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public List<GameObject> monsters;
    public float spawnInterval = 10f;
    public float startDelay = 0f;
    public int maxMonsters = 3;
    public Transform player;

    private int currentMonster = 0;

    private void Update()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        currentMonster = 0;
        //monsters = new List<GameObject>();
        StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            monsters[currentMonster].SetActive(true);
            currentMonster++;
            if (currentMonster == monsters.Count) currentMonster = 0;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    //private void SpawnMonster()
    //{
    //    if (monsters.Count <= maxMonsters)
    //    {
    //        GameObject monster = Instantiate(monsterPrefab, transform);
    //        MonsterController controller = monster.GetComponent<MonsterController>();
    //        controller.mySpawner = this;
    //        controller.playerHead = player;
    //        controller.StartCreepin();
    //        monsters.Add(monster);
    //    }
    //}

    public void KillMonster(GameObject monster)
    {
        monsters.Remove(monster);
        monster.SetActive(false);
    }
}

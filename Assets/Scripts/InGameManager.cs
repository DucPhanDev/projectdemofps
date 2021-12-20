using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

public class InGameManager : SingletonMono<InGameManager>
{
    private List<string> lstZombieName = new List<string> 
    {
        "Zombie_Airport",
        "Zombie_Firefighter",
        "Zombie_RoadWorker",
        "Zombie_Santa",
    };

    private IEnumerator ieOnStartWave;

    public bool isFirstTimeStart = true;

    public List<Transform> lstSpawnPos;
    private List<int> lstWaveCount= new List<int>{ 5, 10, 15};
    public int waveIndex = 0;
    public int currentEnemyCount;

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        waveIndex = 0;
        StartWave();
    }

    public void StartWave()
    {
        if(waveIndex == 0)
            AudioManager.Instance.PlayStartWaveAudio();

        currentEnemyCount = lstWaveCount[waveIndex];
        ieOnStartWave = IEOnStartWave();
        StartCoroutine(ieOnStartWave);
        PlayerUIController.Instance.OnUpdateWaveCount(waveIndex);
        PlayerUIController.Instance.OnUpdateEnemyCount(currentEnemyCount, lstWaveCount[waveIndex]);
    }
    public void OnUpdateWaveCount()
    {
        currentEnemyCount -= 1;
        PlayerUIController.Instance.OnUpdateEnemyCount(currentEnemyCount, lstWaveCount[waveIndex]);
        if (currentEnemyCount == 0 && waveIndex < lstWaveCount.Count)
        {
            waveIndex++;
            if (waveIndex == lstWaveCount.Count)
            {
                AudioManager.Instance.PlayWinAudio();
                PlayerUIController.Instance.OnShowEndMissionUI("You Win");
            }
            else
            {
                if(waveIndex == lstWaveCount.Count-1)
                {
                    AudioManager.Instance.PlayLastWaveAudio();
                }
                StartWave();
            }
        }

      
    }

    private IEnumerator IEOnStartWave()
    {
        for(int i = 0;i< lstWaveCount[waveIndex];i++)
        {
            yield return new WaitForSeconds(2f);
            string zombieName = lstZombieName[Random.Range(0, lstZombieName.Count)];
            Transform spawnPos = lstSpawnPos[Random.Range(0, lstSpawnPos.Count)];

            Transform zombieObject = PoolManager.Pools["Zombie_Pool"].Spawn(zombieName, spawnPos.localPosition,Quaternion.identity);
            zombieObject.SetParent(null);
            zombieObject.GetComponent<EnemyZombie>().Setup();
        }
    }
}

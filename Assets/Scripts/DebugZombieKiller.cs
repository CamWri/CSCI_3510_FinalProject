using UnityEngine;

public class DebugZombieKiller : MonoBehaviour
{
    private ZombieRoundManager roundManager;

    private void Awake()
    {
        roundManager = FindFirstObjectByType<ZombieRoundManager>();

        if (roundManager == null)
        {
            Debug.LogError("DebugZombieKiller: No ZombieRoundManager found in scene!");
        }
    }

    private void Update()
    {
        var zombies = GameObject.FindGameObjectsWithTag("Zombie");
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (zombies.Length == 0)
            {
                Debug.Log("No zombies found!");
                return;
            }
            

            if (roundManager == null)
            {
                Debug.LogError("DebugZombieKiller: Cannot kill zombie — no RoundManager!");
                return;
            }
            Destroy(zombies[Random.Range(0, zombies.Length)]);
            Debug.Log("DEBUG: Killed a zombie manually.");
            roundManager.OnZombieKilled();
        }
    }
}
using UnityEngine;

public class DebugSkeletonKiller : MonoBehaviour
{
    private void Update()
    {
        var skeletons = GameObject.FindGameObjectsWithTag("Skeleton");
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (skeletons.Length == 0)
            {
                Debug.Log("No skeletons found!");
                return;
            }

            // Pick a random skeleton
            int randomIndex = Random.Range(0, skeletons.Length);
            skeletons[randomIndex].GetComponent<Skeleton>().TakeDamage(100);
        }
    }
}
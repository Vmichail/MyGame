using UnityEngine;
using System.Collections;

public class GraveScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] fires; // Assign 4 fire child objects
    [SerializeField] private GameObject healthPotionPrefab;
    [SerializeField] private GameObject manaPotionPrefab;
    [SerializeField] private Transform potionSpawnPoint;
    [SerializeField] private bool spawnEnabed = true;

    [Header("Settings")]
    [SerializeField] private float fireActivateInterval = 1f;

    private int currentFireIndex = 0;

    void Start()
    {
        // Start with all fires inactive
        foreach (var fire in fires)
            fire.SetActive(false);

        StartCoroutine(FireSequence());
    }

    private IEnumerator FireSequence()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireActivateInterval);

            // Activate next fire
            if (currentFireIndex < fires.Length)
            {
                GameObject fire = fires[currentFireIndex];
                fire.SetActive(false); // ensure it restarts animation from beginning
                fire.SetActive(true);  // reactivating restarts StartFire → FireLoop
                currentFireIndex++;
            }

            // All fires active
            if (currentFireIndex >= fires.Length)
            {
                if (spawnEnabed)
                    SpawnRandomPotion();

                // Trigger End animation on all fires
                foreach (var fire in fires)
                {
                    if (fire.TryGetComponent<Animator>(out var anim))
                    {
                        anim.ResetTrigger("End"); // reset just in case
                        anim.SetTrigger("End");   // plays extinguish animation
                    }
                }

                currentFireIndex = 0;
            }
        }
    }

    private void SpawnRandomPotion()
    {
        Vector3 spawnPos = potionSpawnPoint != null ? potionSpawnPoint.position : transform.position;

        // 50% chance for either potion
        GameObject potionToSpawn = (Random.value < 0.5f) ? healthPotionPrefab : manaPotionPrefab;
        GameObject spawnedPotion = Instantiate(potionToSpawn, spawnPos, Quaternion.identity);
        if (spawnedPotion.TryGetComponent(out CollectableBaseScript collectableBaseScript))
        {
            collectableBaseScript.Initialize(spawnedPotion.transform.position);
        }
        else
        {
            Debug.LogWarning("Potion prefab missing CollectableBaseScript component.");
        }
    }
}
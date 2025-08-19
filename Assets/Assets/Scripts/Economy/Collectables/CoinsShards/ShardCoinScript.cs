using UnityEngine;
using UnityEngine.EventSystems;

public class ShardScript : MonoBehaviour, ICollectable
{
    private Transform player;
    public float speed = 8f;
    public float stopDistance = 0.1f;
    private float ShardExp { get => GlobalVariables.Instance.shardExp; }
    private bool IsCollected { get; set; }

    void Update()
    {
        if (player == null) return;

        if (IsCollected)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, player.position) <= stopDistance)
            {
                CollectEnds();
            }
        }
    }

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Collect()
    {
        IsCollected = true;
    }

    private void CollectEnds()
    {
        GlobalVariables.Instance.currentExp += ShardExp;
        gameObject.SetActive(false);
        IsCollected = false;
    }
}

//using UnityEngine;
//using UnityEngine.EventSystems;

//public class MysteriousObjectScript : MonoBehaviour, ICollectable
//{
//    private Transform player;
//    [SerializeField] GameObject levelUpPanel;
//    private AudioSource audioSource;
//    public float speed = 8f;
//    public float stopDistance = 0.1f;
//    private bool IsCollected { get; set; }
//    [SerializeField] private EnemySpawningScript enemySpawningScript;
//    void Update()
//    {
//        if (player == null) return;

//        if (IsCollected)
//        {
//            LeanTween.cancel(gameObject);
//            transform.position = Vector3.MoveTowards(
//                transform.position,
//                player.position,
//                speed * Time.deltaTime
//            );

//            if (Vector3.Distance(transform.position, player.position) <= stopDistance)
//            {
//                CollectEnds();
//            }
//        }
//    }

//    private void OnEnable()
//    {
//        LeanTween.cancel(gameObject);
//        //audioSource = AudioManager.Instance.PlaySoundFX("teleportSound", transform.position, 0.8f, 0.75f, 1.25f, loop: false, true);
//        player = GameObject.FindGameObjectWithTag("Player").transform;
//        LeanTween.moveY(gameObject, transform.position.y + 0.3f, 1f)
//        .setEaseInOutSine()
//        .setLoopPingPong();

//        LeanTween.rotateZ(gameObject, 360f, 2f)
//            .setLoopClamp()
//            .setEaseLinear();
//    }

//    public void Collect()
//    {
//        LeanTween.cancel(gameObject);
//        IsCollected = true;
//    }

//    private void CollectEnds()
//    {
//        //AudioManager.Instance.StopSound(audioSource);
//        AudioManager.Instance.PlaySoundFX("Exorcism_cast", transform.position, 0.4f, 0.75f, 1.25f);
//        levelUpPanel.TryGetComponent<LevelUpPanelScript>(out var levelUpPanelScript);
//        levelUpPanelScript.HealthCost = true;
//        levelUpPanel.SetActive(true);
//        IsCollected = false;
//        enemySpawningScript.SpawnMobsOnSpecificPosition(null, GlobalVariables.EnemyTypes.Level1Skeleton);
//        DoorScript.Instance.DoorUp();
//        gameObject.SetActive(false);
//    }
//}

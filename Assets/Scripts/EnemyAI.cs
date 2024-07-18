using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject aSideGeometry;
    public GameObject bSideGeometry;
    public Transform[] waypoints; // 巡逻路线的节点
    public float chaseRange = 10f; // 追击范围
    public float fieldOfViewAngle = 110f; // 视野角度
    public float loseSightDelay = 3f; // 失去视野后的延迟时间
    public LayerMask playerLayer; // 玩家的LayerMask
    public LayerMask obstacleLayer; // 障碍物的LayerMask

    public NavMeshAgent agent;
    public CharacterController controller;
    public Transform player;
    private bool _hasAnimator;
    private Animator _animator;
    public int currentWaypointIndex;
    public float loseSightTimer;
    public bool isPlayerInSight;
    public bool isChasing;
    public bool isASide;

    void Start()
    {
        loseSightTimer = loseSightDelay;
        _hasAnimator = TryGetComponent(out _animator);
        agent.enabled = !isChasing;
        controller.enabled = isChasing;
        player = PlayerManager.Instance.transform;
        currentWaypointIndex = 0;
        obstacleLayer = 1 << LayerMask.NameToLayer("Default");
        MoveToNextWaypoint();
    }

    void Update()
    {
        if (isASide) return;

        bool playerInChaseRange = Vector3.Distance(transform.position, player.position) < chaseRange;
        isPlayerInSight = IsPlayerInSight();

        if (isPlayerInSight && playerInChaseRange)
        {
            // 追击玩家
            isChasing = true;
            loseSightTimer = 0f;
        }
        else
        {
            // 失去玩家视野
            if (loseSightTimer < loseSightDelay)
            {
                isChasing = true;
                loseSightTimer += Time.deltaTime;
            }
            else
            {
                isChasing = false;
            }
        }

        agent.enabled = !isChasing;
        controller.enabled = isChasing;

        if (isChasing)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            // Ensure we don't move up or down
            directionToPlayer.y = 0;

            // Move towards the player
            controller.Move(directionToPlayer * agent.speed * Time.deltaTime);

            // Optionally, make the enemy face the player
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
        else
        {
            // 返回巡逻路线
            if (agent && agent.enabled && !agent.pathPending && agent.remainingDistance < 0.5f)
            {
                MoveToNextWaypoint();
            }
        }
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        agent.SetDestination(waypoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    bool IsPlayerInSight()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(directionToPlayer, transform.forward);

        if (directionToPlayer.magnitude < chaseRange && angle < fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position + Vector3.up, directionToPlayer.normalized, out hit, chaseRange, obstacleLayer))
            {
                if (hit.transform == player)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 玩家死亡逻辑
            Debug.Log("Player has been caught by the enemy!");
            // 可以调用玩家的死亡方法，例如: player.GetComponent<PlayerHealth>().Die();
        }
    }

    void Teleport(Vector3 pos)
    {
        agent.enabled = false;
        controller.enabled = false;

        transform.position = pos;

        agent.enabled = !isChasing;
        controller.enabled = isChasing;

    }

    public void SwitchSide(bool isASide)
    {
        Debug.Log("SwitchSide " + isASide);
        this.isASide = isASide;
        Teleport(transform.position + MapManager.Instance.bSidePos * (isASide ? -1 : 1));
        aSideGeometry.SetActive(isASide);
        bSideGeometry.SetActive(!isASide);
        if (_hasAnimator)
        {
            _animator.enabled = !isASide;
        }
        if (isASide)
        {
            agent.enabled = false;
            controller.enabled = true;
        }
    }
}

using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject aSideGeometry;
    public GameObject bSideGeometry;
    public Transform[] waypoints; // Ѳ��·�ߵĽڵ�
    public float chaseRange = 10f; // ׷����Χ
    public float fieldOfViewAngle = 110f; // ��Ұ�Ƕ�
    public float loseSightDelay = 3f; // ʧȥ��Ұ����ӳ�ʱ��
    public LayerMask playerLayer; // ��ҵ�LayerMask
    public LayerMask obstacleLayer; // �ϰ����LayerMask

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
            // ׷�����
            isChasing = true;
            loseSightTimer = 0f;
        }
        else
        {
            // ʧȥ�����Ұ
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
            // ����Ѳ��·��
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
            // ��������߼�
            Debug.Log("Player has been caught by the enemy!");
            // ���Ե�����ҵ���������������: player.GetComponent<PlayerHealth>().Die();
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

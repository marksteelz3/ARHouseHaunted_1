using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public AudioSource mouthAudio;
    public AudioSource footstepsAudio;
    public float noiseInterval;
    public NavMeshAgent agent;
    public Transform spawnPoint;
    public List<Transform> wanderDestinations;
    public float wanderDestinationThreshold = 0.1f;
    public float playerInfectionDistance = 0.4f;
    public Transform monsterHead;
    public Color infectionColor = Color.green;
    public Renderer[] monsterRenderers;
    public float respawnTime = 60f;
    public AudioClip wanderSound;
    public AudioClip infectionSound;
    public float infectionDuration = 15f;
    public float chasingMultipler = 1f;
    public float torchAngerDistance = 0.3f;
    public Material dissolveMaterial;
    public bool doesAttackPlayer = true;

    private float followInterval = 1.0f;
    private bool wandering;
    private int currentWanderDestination = 0;
    private Vector3 currentPlayerDistnaceVector;
    private float currentPlayerDistance;
    private bool dead = false;
    private float originalAgentSpeed;
    private float originalAgentAngularSpeed;
    private bool attacking = false;
    
    private Coroutine wanderCoroutine;
    private Coroutine noiseCoroutine;

    private void Update()
    {
        CheckPlayerDistance();
        if (!attacking) CheckForEyeContact();
        if (!attacking) CheckTorchDistance();
    }

    private void OnEnable()
    {
        if (agent != null) originalAgentSpeed = agent.speed;
        if (agent != null) originalAgentAngularSpeed = agent.angularSpeed;
        Wander();
    }

    private IEnumerator NoiseRoutine()
    {
        while (wanderSound != null && dead == false)
        {
            PlaySound(wanderSound);

            yield return new WaitForSeconds(noiseInterval);
        }
    }

    private void CheckForEyeContact()
    {
        if (MonsterGameManager.Instance == null || MonsterGameManager.Instance.playerHead == null || monsterHead == null)
            return;

        bool playerLookingAtMonster = IsTargetInView(MonsterGameManager.Instance.playerHead, 8);
        bool monsterLookingAtPlayer = IsTargetInView(monsterHead, 9);

        //if (playerLookingAtMonster)
        //    Debug.Log("Player looking at monster");

        //if (monsterLookingAtPlayer)
        //    Debug.Log("Monster looking at player");
        
        if (monsterLookingAtPlayer && playerLookingAtMonster && wandering)
            AttackPlayer();
    }

    private bool IsTargetInView(Transform source, int targetLayer)
    {
        bool result = false;
        Ray sourceRay = new Ray(source.position, source.forward);
        RaycastHit hit;

        if (Physics.Raycast(sourceRay, out hit, 100f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
        {
            if (hit.transform.gameObject.layer == targetLayer)
                result = true;
        }

        return result;
    }

    public void Wander()
    {
        if (mouthAudio != null) noiseCoroutine = StartCoroutine(NoiseRoutine());

        if (wanderDestinations == null || wanderDestinations.Count == 0) return;

        Debug.Log($"{gameObject.name} is wandering");
        dead = false;

        if (agent == null)
        {
            Debug.LogError("Agent is null");
            return;
        }

        wandering = true;
        agent.updatePosition = true;

        
        if (footstepsAudio != null) footstepsAudio.Play();

        agent.Warp(transform.position);
        wanderCoroutine = StartCoroutine(WanderRoutine());
        agent.speed = originalAgentSpeed;
        agent.angularSpeed = originalAgentAngularSpeed;
        //StartCoroutine(DebugStatePeriodically());
    }

    private IEnumerator WanderRoutine()
    {
        while (wandering)
        {
            if (wanderDestinations[currentWanderDestination] != null) agent.SetDestination(wanderDestinations[currentWanderDestination].position);
            yield return new WaitUntil(() => IsAgentAtWanderDestination());
            IncrementWanderDestination();
        }
    }

    private bool IsAgentAtWanderDestination()
    {
        return Vector3.Distance(wanderDestinations[currentWanderDestination].position, agent.transform.position) < wanderDestinationThreshold;
    }

    private void IncrementWanderDestination()
    {
        if (wanderDestinations == null || wanderDestinations.Count == 0) return;

        if (currentWanderDestination == wanderDestinations.Count - 1)
            currentWanderDestination = 0;
        else
            currentWanderDestination++;
    }

    public void AttackPlayer()
    {
        if (agent == null)
        {
            Debug.LogError("Agent is null");
            return;
        }

        if (!doesAttackPlayer) return;

        if (dead || attacking) return;

        Debug.Log($"{gameObject.name} attacking player!");

        wandering = false;
        attacking = true;
        if (wanderCoroutine != null) StopCoroutine(wanderCoroutine);
        Debug.Log($"Attack warp: {agent.Warp(transform.position)}");
        StartCoroutine(FollowPlayer());
        agent.speed *= chasingMultipler;
        agent.angularSpeed *= chasingMultipler;
    }

    private IEnumerator FollowPlayer()
    {
        if (MonsterGameManager.Instance == null || MonsterGameManager.Instance.playerHead == null) yield break;

        while (!wandering && !dead)
        {
            bool chasingSuccessfully = agent.SetDestination(MonsterGameManager.Instance.playerHead.position);
            Debug.Log($"{gameObject.name} chasing player: {chasingSuccessfully}");
            yield return new WaitForSeconds(followInterval);
        }
    }

    private void CheckPlayerDistance()
    {
        if (MonsterGameManager.Instance == null || MonsterGameManager.Instance.playerHead == null) return;

        currentPlayerDistnaceVector = transform.position - MonsterGameManager.Instance.playerHead.position;
        currentPlayerDistance = Vector3.ProjectOnPlane(currentPlayerDistnaceVector, Vector3.up).magnitude;

        if (currentPlayerDistance < playerInfectionDistance)
        {
            Die();
        }
    }

    private void Die()
    {
        if (!dead) StartCoroutine(DieAndComeBack());
    }

    private IEnumerator DieAndComeBack()
    {
        if (MonsterGameManager.Instance == null) yield break;

        Debug.Log($"{gameObject.name} infected player, goodbye now");

        MonsterGameManager.Instance.InfectPlayer(infectionColor, infectionDuration);
        dead = true;
        //StartCoroutine(DissolveMonster());
        foreach (Renderer renderer in monsterRenderers)
        {
            renderer.enabled = false;
        }
        wandering = false;
        if (wanderCoroutine != null) StopCoroutine(wanderCoroutine);
        currentWanderDestination = 0;
        //if (agent != null) agent.Warp(spawnPoint.position);
        //if (agent != null) agent.ResetPath();
        if (noiseCoroutine != null) StopCoroutine(noiseCoroutine);
        if (infectionSound != null) PlaySound(infectionSound);
        if (footstepsAudio != null) footstepsAudio.Stop();

        float waitTime = infectionSound != null ? infectionSound.length : 0.5f;

        yield return new WaitForSeconds(waitTime);

        //NOTE: decided to get rid of respawning for now, can bring back later
        Destroy(gameObject);

        //yield return new WaitForSeconds(respawnTime);

        //dead = false;
        //monsterRenderer.enabled = true;
        //Wander();
    }

    private void PlaySound(AudioClip sound)
    {
        if (mouthAudio == null) return;

        if (mouthAudio.isPlaying) mouthAudio.Stop();
        mouthAudio.clip = sound;
        mouthAudio.Play();
    }

    private IEnumerator DebugStatePeriodically()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(5.0f);

            Debug.Log($"Wandering: {wandering}");
        }
    }

    public void Flee()
    {
        if (agent == null)
        {
            Debug.LogError("Agent is null");
            return;
        }

        Debug.Log($"{gameObject.name} fleeing!");

        Die();

        //wandering = false;
        //StopCoroutine(wanderCoroutine);
        //Debug.Log($"Flee warp: {agent.Warp(transform.position)}");
        //Debug.Log($"Flee set destination: {agent.SetDestination(spawnPoint.position)}");
        //agent.speed *= 6;
        //agent.angularSpeed *= 6;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerInfectionDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(monsterHead.position, torchAngerDistance);
    }

    private void CheckTorchDistance()
    {
        if (MonsterGameManager.Instance == null || !doesAttackPlayer) return;

        Vector3 rightDistanceVector = monsterHead.position - MonsterGameManager.Instance.rightTorch.position;
        Vector3 leftDistanceVector = monsterHead.position - MonsterGameManager.Instance.leftTorch.position;

        if (rightDistanceVector.magnitude < torchAngerDistance || leftDistanceVector.magnitude < torchAngerDistance)
        {
            AttackPlayer();
        }
    }

    private IEnumerator DissolveMonster()
    {
        foreach (Renderer renderer in monsterRenderers)
        {
            if (dissolveMaterial == null)
            {
                renderer.enabled = false;
            }
            else
            {
                renderer.material = dissolveMaterial;
            }
        }

        float timeLeft = infectionSound != null ? infectionSound.length - 0.1f : 1f;
        float totalTime = timeLeft;

        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timeLeft -= Time.deltaTime;
            //dissolveMaterial.Set

        }
    }
}

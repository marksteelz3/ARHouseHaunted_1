using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGameManager : MonoBehaviour
{
    public static MonsterGameManager Instance;

    public Transform playerHead;
    public PassthroughManager passthroughManager;
    public Transform rightController;
    public Transform leftController;
    public Transform rightTorch;
    public Transform leftTorch;
    public ResetPlayspacePosition positionCalibrationSequence;
    public IntroductionSequence introductionSequence;
    public ResetPlayspaceRotation rotationCalibrationSequence;
    public GameObject explorationSequence;
    public GameObject endSequence;
    public GameObject[] finaleObjects;
    public Animator[] finaleAnimators;
    public List<GameObject> monsters;
    public AudioSource audioSource;
    //public AudioClip firstInfectionAudio;
    public bool skipToExploration = false;

    private bool resetConfirmed = false;
    private int totalCollectibles = 0;
    private int currentCollectiblesCollected = 0;
    private bool hasBeenInfected = false;

    public bool ResetConfirmed => resetConfirmed;
    public int CurrentCollectiblesCollected => currentCollectiblesCollected;
    public int TotalCollectibles => totalCollectibles;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void OnEnable()
    {
        ResetSequences();
        if (!skipToExploration) 
            BeginPositionCalibrationSequence();
        else
            BeginExplorationSequence();
    }

    private void OnDisable()
    {
        ResetSequences();
    }

    public void InfectPlayer(Color infectionColor, float infectionDuration = 15f)
    {
        Debug.Log("Player infected!");
        passthroughManager.TweenEdgeColor(infectionColor, infectionDuration);
        //if (!hasBeenInfected)
        //{
        //    audioSource.clip = firstInfectionAudio;
        //    audioSource.Play();
        //}
    }

    public void CollectCollectible()
    {
        currentCollectiblesCollected++;

        if (endSequence.activeInHierarchy && currentCollectiblesCollected == totalCollectibles)
        {
            BeginFinale();
            return;
        }

        if (currentCollectiblesCollected == totalCollectibles)
        {
            BeginEndSequence();
        }
    }

    public void AddCollectible()
    {
        totalCollectibles++;
    }

    public void BeginPositionCalibrationSequence()
    {
        if (positionCalibrationSequence != null)
        {
            positionCalibrationSequence.gameObject.SetActive(true);
            positionCalibrationSequence.PlayspacePositionReset += OnPlayspacePositionReset;
        }
    }

    private void OnPlayspacePositionReset(ResetPlayspacePosition playspacePositionReset)
    {
        //BeginIntroSequence();
        BeginRotationCalibrationSequence();
    }

    public void BeginIntroSequence()
    {
        if (positionCalibrationSequence != null)
        {
            positionCalibrationSequence.PlayspacePositionReset -= OnPlayspacePositionReset;
            positionCalibrationSequence.gameObject.SetActive(false);
        }

        if (introductionSequence != null)
        {
            introductionSequence.gameObject.SetActive(true);
            introductionSequence.IntroductionSequenceFinished += OnIntroductionSequenceFinished;
        }
    }

    private void OnIntroductionSequenceFinished(IntroductionSequence introSequence)
    {
        BeginRotationCalibrationSequence();
    }

    public void BeginRotationCalibrationSequence()
    {
        if (positionCalibrationSequence != null)
        {
            positionCalibrationSequence.PlayspacePositionReset -= OnPlayspacePositionReset;
            positionCalibrationSequence.gameObject.SetActive(false);
        }

        if (introductionSequence != null)
        {
            introductionSequence.IntroductionSequenceFinished -= OnIntroductionSequenceFinished;
            introductionSequence.gameObject.SetActive(false);
        }

        if (rotationCalibrationSequence != null)
        {
            rotationCalibrationSequence.gameObject.SetActive(true);
            rotationCalibrationSequence.PlayspaceRotationReset += OnPlayspaceRotationReset;
        }
    }

    private void OnPlayspaceRotationReset(ResetPlayspaceRotation playspaceRotationReset)
    {
        BeginExplorationSequence();
    }

    public void BeginExplorationSequence()
    {
        if (explorationSequence != null)
        {
            if (rotationCalibrationSequence != null)
            {
                rotationCalibrationSequence.PlayspaceRotationReset -= OnPlayspaceRotationReset;
                rotationCalibrationSequence.gameObject.SetActive(false);
            }
            explorationSequence.SetActive(true);

            if (monsters != null && monsters.Count > 0) monsters[0].SetActive(true);

            //StartCoroutine(ExplorationSequence());
        }       
    }

    private IEnumerator ExplorationSequence()
    {
        yield return new WaitUntil(() => currentCollectiblesCollected >= 1);

        for (int i = 1; i < monsters.Count; i++)
        {
            monsters[i].SetActive(true);
        }
    }

    private void BeginEndSequence()
    {
        if (endSequence != null) 
        {
            //explorationSequence.SetActive(false);
            endSequence.SetActive(true); 
        }
    }

    public void BeginFinale()
    {
        if (finaleObjects != null && finaleObjects.Length > 0)
        {
            foreach (GameObject obj in finaleObjects)
                obj.SetActive(true);
        }

        if (finaleAnimators != null && finaleAnimators.Length > 0)
        {
            foreach (Animator animator in finaleAnimators)
                animator.enabled = true;
        }
    }

    public void ResetSequences()
    {
        if (positionCalibrationSequence != null)
        {
            positionCalibrationSequence.PlayspacePositionReset -= OnPlayspacePositionReset;
            positionCalibrationSequence.gameObject.SetActive(false);
        }
        if (introductionSequence != null)
        {
            introductionSequence.IntroductionSequenceFinished -= OnIntroductionSequenceFinished;
            introductionSequence.gameObject.SetActive(false);
        }
        if (rotationCalibrationSequence != null)
        {
            rotationCalibrationSequence.PlayspaceRotationReset -= OnPlayspaceRotationReset;
            rotationCalibrationSequence.gameObject.SetActive(false);
        }
        if (explorationSequence != null) explorationSequence.SetActive(false);
        if (endSequence != null) endSequence.SetActive(false);
        if (finaleObjects != null && finaleObjects.Length > 0)
        {
            foreach (GameObject obj in finaleObjects)
                if (obj != null) obj.SetActive(false);
        }
        if (finaleAnimators != null && finaleAnimators.Length > 0)
        {
            foreach (Animator animator in finaleAnimators)
                if (animator != null) animator.enabled = false;
        }
    }

    public void FleeMonsters()
    {
        if (monsters == null) return;

        foreach (GameObject monster in monsters)
        {
            if (monster != null && monster.activeInHierarchy)
            {
                MonsterController controller = monster.GetComponent<MonsterController>();
                if (controller != null) controller.Flee(); 
            }
        }
    }
}

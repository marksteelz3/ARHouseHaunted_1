using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collectible : MonoBehaviour
{
    public float collectionDistance = 0.3f;
    public float totalCollectionTime = 3f;
    public AudioSource audioSource;
    public GameObject collectionAnimation;
    public TextMeshProUGUI collectionText;
    public TimedPopup popup;

    private float currentCollectionTime = 0f;
    private float initialScale;
    private bool collectionStarted = false;

    private void OnEnable()
    {
        initialScale = transform.localScale.x;
        StartCoroutine(RegisterCollectible());
    }

    private void Update()
    {
        if (!collectionStarted) CheckTorchDistance();
    }

    private void CheckTorchDistance()
    {
        Vector3 rightDistanceVector = transform.position - MonsterGameManager.Instance.rightTorch.position;
        Vector3 leftDistanceVector = transform.position - MonsterGameManager.Instance.leftTorch.position;

        if (rightDistanceVector.magnitude < collectionDistance || leftDistanceVector.magnitude < collectionDistance)
        {
            StartCoroutine(StartCollection());
            return;
        }

        //if (distanceVector.magnitude < collectionDistance)
        //{
        //    currentCollectionTime += Time.deltaTime;
        //    float collectionLeft = 1f - Mathf.Clamp01(currentCollectionTime / totalCollectionTime);
        //    float newScale = initialScale * collectionLeft;
        //    transform.localScale = new Vector3(newScale, newScale, newScale);
        //    ResumeAudio();

        //    if (currentCollectionTime > totalCollectionTime)
        //    {
        //        MonsterGameManager.Instance.CollectCollectible();
        //        DestroyCollectible();
        //    }
        //}
        //else
        //{
        //    PauseAudio();
        //}
    }

    private IEnumerator StartCollection()
    {
        audioSource.Play();
        if (collectionAnimation != null) collectionAnimation.SetActive(true);
        if (collectionText != null) collectionText.text = $"{MonsterGameManager.Instance.CurrentCollectiblesCollected + 1}/{MonsterGameManager.Instance.TotalCollectibles} Profit\nSources Burned";

        float collectionTime = audioSource.clip.length;

        if (popup != null) collectionTime = popup.popupDuration + 1f > collectionTime ? popup.popupDuration + 1f : collectionTime;

        if (MonsterGameManager.Instance.CurrentCollectiblesCollected == MonsterGameManager.Instance.TotalCollectibles - 1
            && MonsterGameManager.Instance.endSequence.activeInHierarchy)
        {
            MonsterGameManager.Instance.FleeMonsters();
            StartCoroutine(StartFinaleEarly());
        }         

        yield return new WaitForSeconds(collectionTime);

        MonsterGameManager.Instance.CollectCollectible();
        DestroyCollectible();
    }

    private void ResumeAudio()
    {
        if (audioSource.isPlaying) return;

        if (currentCollectionTime > 0f)
            audioSource.UnPause();
        else
            audioSource.Play();
    }

    private void PauseAudio()
    {
        if (currentCollectionTime > 0f)
            audioSource.Pause();
    }

    private void DestroyCollectible()
    {
        //TODO: add destruction VFX
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collectionDistance);
    }

    private IEnumerator RegisterCollectible()
    {
        if (collectionAnimation != null) collectionAnimation.SetActive(false);
        yield return new WaitForSeconds(1f);
        MonsterGameManager.Instance.AddCollectible();
    }

    private IEnumerator StartFinaleEarly()
    {
        yield return new WaitForSeconds(2.0f);
        MonsterGameManager.Instance.BeginFinale();
    }
}

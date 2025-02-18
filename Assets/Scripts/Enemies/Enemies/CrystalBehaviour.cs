using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBehaviour : MonoBehaviour
{
    private enum AnimationState
    {
        MOVE,
        RAISE,
        FALL
    }

    [SerializeField]
    private float travelDistance = 3.0f;

    [SerializeField]
    private float moveDirection = 1;

    [SerializeField]
    private float moveDelay = 0.5f;
    private float moveCounter = 0;

    [SerializeField]
    private float raiseAnimationLength = 0.25f;
    private float raiseAnimationCounter = 0;

    [SerializeField]
    private float fallAnimationLength = 0.1f;
    private float fallAnimationCounter = 0;

    [SerializeField] 
    private GameObject crystalPrefab;
    private Renderer crystalRenderer;

    [SerializeField]
    private int numberOfCrystals = 8;

    [SerializeField]
    private float spawnOffset = 0.5f;

    private int damage = 3;
    private HashSet<BaseTower> touchedTowers;

    private AnimationState animationState = AnimationState.RAISE;

    private List<GameObject> instantiatedCrystals;
    // Start is called before the first frame update
    void Start()
    {
        touchedTowers = new();
        crystalRenderer = crystalPrefab.GetComponentInChildren<Renderer>();
        
        instantiatedCrystals = new(numberOfCrystals);
        for (int i = 0; i < numberOfCrystals; i++) 
        {
            var rotation = Quaternion.Euler(0, i * 360 / numberOfCrystals, 0);
            var direction =  transform.position + rotation * transform.forward - transform.up * crystalRenderer.bounds.size.y;
            instantiatedCrystals.Add(Instantiate(crystalPrefab, direction, rotation));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (instantiatedCrystals.FindIndex((obj) => obj != null) == -1)
        {
            Destroy(gameObject);
            return;
        }

        switch (animationState)
        {
            case AnimationState.MOVE:
                {
                    HandleMoveState();
                    break;
                }
            case AnimationState.RAISE:
                {
                    HandleRaiseState();
                    break;
                }
            case AnimationState.FALL:
                {
                    HandleFallState();
                    break;
                }
        }
    }
    private void HandleMoveState()
    {
        if (moveCounter >= moveDelay)
        {
            for (int i = 0; i < instantiatedCrystals.Count; i++)
            {
                var crystal = instantiatedCrystals[i];
                if (crystal == null) continue;

                var rotationCorrection = Quaternion.Euler(-90, 0, 0);

                var rayOrigin = crystal.transform.position + (2 + crystalRenderer.bounds.size.y) * crystal.transform.up + crystal.transform.forward * moveDirection;
                var rayDirection = -crystal.transform.up;
                var rayLength = 4f;
                Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.red, 20f);
                if (Physics.Raycast(rayOrigin, rayDirection, out var hit, rayLength))
                {
                    crystal.transform.position = hit.point - hit.transform.up * crystalRenderer.bounds.size.y;
                }
                moveCounter = 0;
                animationState = AnimationState.RAISE;
            }
        } 
        else
        {
            moveCounter += Time.deltaTime;
        }
    }
    private void HandleRaiseState()
    {
        if (raiseAnimationCounter >= raiseAnimationLength)
        {
            raiseAnimationCounter = 0;
            animationState = AnimationState.FALL;
            return;
        }

        raiseAnimationCounter += Time.deltaTime;
        var animationFrameLength = Time.deltaTime;
        if (raiseAnimationCounter >= raiseAnimationLength)
        {
            animationFrameLength -= raiseAnimationCounter - raiseAnimationLength;
        }
        
        var moveAmount = (animationFrameLength / raiseAnimationLength) * crystalRenderer.bounds.size.y;

        for (int i = 0; i < instantiatedCrystals.Count; i++)
        {
            var crystal = instantiatedCrystals[i];
            if (crystal == null) continue;
            if (Vector3.Distance(crystal.transform.position, transform.position) >= travelDistance)
            {
                Destroy(crystal);
                instantiatedCrystals[i] = null;
                continue;
            }
            crystal.transform.position += moveAmount * crystal.transform.up;
        }
    }
    private void HandleFallState()
    {
        if (fallAnimationCounter >= fallAnimationLength)
        {
            fallAnimationCounter = 0;
            animationState = AnimationState.MOVE;
            return;
        }

        fallAnimationCounter += Time.deltaTime;
        var animationFrameLength = Time.deltaTime;
        if (fallAnimationCounter >= fallAnimationLength)
        {
            animationFrameLength -= fallAnimationCounter - fallAnimationLength;
        }

        var moveAmount = (animationFrameLength / fallAnimationLength) * crystalRenderer.bounds.size.y;

        for (int i = 0; i < instantiatedCrystals.Count; i++) 
        {
            var crystal = instantiatedCrystals[i];
            if (crystal == null) continue;
            crystal.transform.position -= moveAmount * crystal.transform.up;
        }
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}

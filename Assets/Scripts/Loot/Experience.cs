using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField]
    private int experience = 1;

    [SerializeField] 
    private float accelerationRate = 1f;

    private LootController lootController;
    private PlayerController playerController;

    [SerializeField]
    private Rigidbody rb;

    private Experience mergeTarget = null;

    // Start is called before the first frame update
    void Start()
    {
        lootController = FindFirstObjectByType<LootController>();
        playerController = FindFirstObjectByType<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (mergeTarget == null) mergeTarget = lootController.GetMergeTarget(this);
        if (mergeTarget != null) MagnetTo(mergeTarget.transform);

        transform.localScale = Vector3.one * Mathf.Sqrt(experience) / 4.0f;
        rb.mass = experience / 4.0f;
    }
    private void MagnetTo(Transform other)
    {
        var direction = (other.position - transform.position).normalized;
        rb.AddForce(direction * accelerationRate);
    }
    private void OnTriggerEnter(Collider other)
    {
        var experience = other.gameObject.GetComponentInChildren<Experience>();
        if (experience != null)
        {
            lootController.UniteExperienceBlobs(experience, this);
        }
    }
    public void AddExperience(int exp)
    {
        experience += exp;
    }
    public int GetExperience()
    {
        return experience;
    }
}

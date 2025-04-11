using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour
{
    private long experience = 0;
    private long experienceBuffer = 0;

    private BuffChoiceHandler buffChoiceHandler;

    private List<int> buffList = new();

    private bool isInChoice = false;

    private Dictionary<Experience, bool> experiences = new();

    [SerializeField]
    private float mergeRadius = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        buffChoiceHandler = FindFirstObjectByType<BuffChoiceHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (experience >= 10 && !isInChoice && buffList.Count == 0)
        {
            isInChoice = true;
            buffChoiceHandler.PresentBuffChoice(3);
        }
        
    }
    public void AddBuff(int buff)
    {
        isInChoice = false;
        buffList.Add(buff);
    }
    public Dictionary<Experience, bool> GetInstantiatedExperienceBlobs()
    {
        return experiences;
    }
    public void TransferExperience()
    {
        experience += experienceBuffer;
        experienceBuffer = 0;
    }
    public void InstantiateExperienceBlob(GameObject experiencePrefab, Vector3 position, Quaternion rotation)
    {
        experiences.Add(Instantiate(experiencePrefab, position, rotation).GetComponentInChildren<Experience>(), false);
    }
    public void PickUpExperienceBlob(Experience experience)
    {
        if (experiences.TryGetValue(experience, out var _))
        {
            experiences.Remove(experience);
            this.experience += experience.GetExperience();
            Destroy(experience.gameObject);
        }
    }
    public void UniteExperienceBlobs(Experience experience1, Experience experience2)
    {
        var does1Exists = experiences.TryGetValue(experience1, out var is1Central);
        var does2Exists = experiences.TryGetValue(experience2, out var is2Central);

        if (!does1Exists || !does2Exists) return;

        if (!is1Central && !is2Central)
        {
            experience1.AddExperience(experience2.GetExperience());
            RemoveExperience(experience2);
        }
        else if (is1Central && !is2Central)
        {
            experience1.AddExperience(experience2.GetExperience());
            RemoveExperience(experience2);
        }
        else if (!is1Central && is2Central)
        {
            experience2.AddExperience(experience1.GetExperience());
            RemoveExperience(experience1);
        }
        else
        {
            var toStay = experience1.GetExperience() >= experience2.GetExperience() ? experience1 : experience2;
            var toDestroy = experience1.GetExperience() < experience2.GetExperience() ? experience1 : experience2;
            toStay.AddExperience(toDestroy.GetExperience());
            RemoveExperience(toDestroy);
        }
    }
    private void RemoveExperience(Experience experience)
    {
        experiences.Remove(experience);
        Destroy(experience.gameObject);
    }
    public Experience GetMergeTarget(Experience experience)
    {
        if (experiences.TryGetValue(experience, out var isCentral) && isCentral)
        {
            return null;
        }

        Experience target = null;
        var closest = float.MaxValue;
        
        foreach (var kvp in experiences)
        {
            if (!kvp.Value) continue;

            var distance = Vector3.Distance(experience.transform.position, kvp.Key.transform.position);
            if (distance < closest)
            {
                target = kvp.Key;
                closest = distance;
            }
        }
        
        if (closest > mergeRadius)
        {
            experiences[experience] = true;
            return null;
        }

        return target;
    }
}

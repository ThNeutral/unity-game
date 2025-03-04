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
    public void TransferExperience()
    {
        experience += experienceBuffer;
        experienceBuffer = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Experience>(out var experience))
        {
            experience.HandleCollect();
        }
    }
    public void AddBufferExperience(int exp)
    {
        experienceBuffer += exp;
    }
}

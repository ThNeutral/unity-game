using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour
{
    public enum ModeOfCollection
    {
        Instant, 
        OnPickUp,
        ViaTower,
    }
    
    private long experience = 0;

    [SerializeField] 
    public ModeOfCollection modeOfCollection;

    private BuffChoiceHandler buffChoiceHandler;

    private List<int> buffList = new();
    // Start is called before the first frame update
    void Start()
    {
        buffChoiceHandler = FindFirstObjectByType<BuffChoiceHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (experience >= 10 && buffList.Count == 0)
        {
            buffChoiceHandler.PresentBuffChoice();
        }
        
    }
    public void AddBuff(int buff)
    {
        buffList.Add(buff);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Experience>(out var experience))
        {
            experience.HandleCollect();
        }
    }
    public void AddExperience(int exp)
    {
        experience += exp;
    }
}

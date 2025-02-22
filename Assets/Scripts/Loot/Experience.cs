using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField]
    private int experience = 1;
    
    private LootController lootController;

    private BaseTower killedBy;
    // Start is called before the first frame update
    void Start()
    {
        lootController = FindFirstObjectByType<LootController>();
        if (lootController.modeOfCollection != LootController.ModeOfCollection.OnPickUp)
        {
            HandleCollect();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void HandleCollect()
    {
        if (lootController.modeOfCollection == LootController.ModeOfCollection.Instant)
        {
            lootController.AddExperience(experience);
        } else if (lootController.modeOfCollection == LootController.ModeOfCollection.ViaTower)
        {
            if (killedBy == null)
            {
                Debug.LogError("Mode of collection was LootController.ModeOfCollection.ViaTower but killedBy is null");
            }
            else
            {
                killedBy.AddExperience(experience);
            }
        }
        Destroy(gameObject);
    }
    public void SetKilledBy(BaseTower tower)
    {
        killedBy = tower;
    }
    public int GetExperience()
    {
        return experience;
    }
}

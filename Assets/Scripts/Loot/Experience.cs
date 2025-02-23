using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField]
    private int experience = 1;
    
    private LootController lootController;

    private BaseTower killedBy;

    [SerializeField]
    private Bounds unionBounds;
    // Start is called before the first frame update
    void Start()
    {
        lootController = FindFirstObjectByType<LootController>();

        if (lootController.modeOfCollection == LootController.ModeOfCollection.OnPickUp)
        {
            return;
        } 
        else
        {
            HandleCollect();
        }
    }
    // Update is called once per frame
    void Update()
    {
        var colliders = Physics.OverlapBox(unionBounds.center, unionBounds.size * experience, Quaternion.identity).ToList();
        foreach (var collider in colliders)
        {
            if ((collider.gameObject.GetInstanceID() != gameObject.GetInstanceID()) 
                && collider.gameObject.TryGetComponent(out Experience other))
            {
                HandleUnite(other);
                return;
            }
        }

        var size = experience / 4.0f;
        transform.localScale = Vector3.one * size;
    }
    public void HandleUnite(Experience target)
    {
        AddExperience(target.GetExperience());
        DestroyImmediate(target.gameObject);
    }
    public void HandleCollect()
    {
        switch (lootController.modeOfCollection)
        {
            case LootController.ModeOfCollection.Instant:
                {
                    lootController.AddExperience(experience);
                    break;
                }
            case LootController.ModeOfCollection.OnPickUp: 
                {
                    lootController.AddExperience(experience);
                    break;
                }
            case LootController.ModeOfCollection.ViaTower:
                {
                    if (killedBy == null)
                    {
                        Debug.LogError("Mode of collection was LootController.ModeOfCollection.ViaTower but killedBy is null");
                        break;
                    }

                    killedBy.AddExperience(experience);
                    break;
                }
        }
        
        Destroy(gameObject);
    }
    public void SetKilledBy(BaseTower tower)
    {
        killedBy = tower;
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

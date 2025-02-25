using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField]
    private int experience = 1;
    
    private LootController lootController;

    [SerializeField]
    private Bounds unionBounds;
    // Start is called before the first frame update
    void Start()
    {
        lootController = FindFirstObjectByType<LootController>();
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

        var size = Mathf.Sqrt(experience) / 4.0f;
        transform.localScale = Vector3.one * size;
    }
    public void HandleUnite(Experience target)
    {
        AddExperience(target.GetExperience());
        DestroyImmediate(target.gameObject);
    }
    public void HandleCollect()
    {
        lootController.AddExperience(experience);
        Destroy(gameObject);
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

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

    [SerializeField]
    private Bounds unionBounds;

    [SerializeField]
    private Vector3 speed = Vector3.zero;

    private bool isDestroyed;
    // Start is called before the first frame update
    void Start()
    {
        lootController = FindFirstObjectByType<LootController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isDestroyed)
        {
            Destroy(gameObject);
            return;
        }

        var colliders = Physics.OverlapBox(unionBounds.center, unionBounds.size * experience, Quaternion.identity).ToList();
        foreach (var collider in colliders)
        {
            if ((collider.gameObject.GetInstanceID() != gameObject.GetInstanceID()) 
                && collider.gameObject.TryGetComponent(out Experience other))
            {
                MagnetTo(other);
                break;
            }
        }

        var size = Mathf.Sqrt(experience) / 4.0f;
        transform.localScale = Vector3.one * size;
    }
    private void MagnetTo(Experience experience)
    {
        var direction = (experience.transform.position - transform.position).normalized;
        speed += direction * accelerationRate * Time.deltaTime;
        transform.position += speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isDestroyed) return;
        if (other.gameObject.TryGetComponent<Experience>(out var experience))
        {
            HandleUnite(experience);
        }
    }
    public void HandleUnite(Experience target)
    {
        if (isDestroyed) return;
        speed = Vector3.zero;
        AddExperience(target.GetExperience());
        target.ScheduleDestroy();
    }
    public void HandleCollect()
    {
        if (isDestroyed) return;
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
    public void ScheduleDestroy()
    {
        isDestroyed = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiseTree : MonoBehaviour
{
    [SerializeField]
    private int health = 10;

    private LootController lootController;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        lootController = FindFirstObjectByType<LootController>();
        playerController = FindFirstObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E) && Vector3.Distance(playerController.transform.position, transform.position) <= 5)
        {
            lootController.TransferExperience();
        }
    }
    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

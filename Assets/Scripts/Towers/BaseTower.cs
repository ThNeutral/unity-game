using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseTower : MonoBehaviour
{ 
    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private float projectileSpeed = 10;

    [SerializeField]
    private float shootSpeed = 0.02f;

    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private float maximumRange = 30f;

    [SerializeField]
    private float dispersionAngle = 10f;

    private float counter;

    private int experience = 0;

    private LootController lootController;
    private EnemyController enemyController;
    private TowerController towerController;

    [SerializeField]
    private TextMeshPro textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        lootController = FindFirstObjectByType<LootController>();
    }
    void Update()
    {
        HandleLivingTower();
        if (Input.GetKey(KeyCode.E))
        {
            HandleGiveExperience();
        }
    }
    private void HandleGiveExperience()
    {
        if (Vector3.Distance(FindFirstObjectByType<PlayerController>().transform.position, transform.position) >= 5) return;
        lootController.AddExperience(experience);
        experience = 0;
        textMeshPro.text = "0";
    }

    private void HandleLivingTower()
    {
        counter += Time.deltaTime;
        while (counter > shootSpeed)
        {
            var target = towerController.GetTarget(this);
            if (target != null)
            {
                ShootAt(target);
                counter -= shootSpeed;
            }
            else
            {
                counter = 0;
            }
        }
    }
    public void SetEnemyController(EnemyController controller) 
    {
        enemyController = controller;
    }
    public void SetTowerController(TowerController controller)
    {
        towerController = controller;
    }
    public void AddExperience(int experience)
    {
        this.experience += experience;
        textMeshPro.text = this.experience.ToString();
    }
    public void ShootAt(BaseEnemy enemyBehaviour)
    {
        var enemyMoveDirection = enemyBehaviour.GetMoveDirection();
        var enemySpeed = enemyBehaviour.GetSpeed();
        var enemyPosition = enemyBehaviour.transform.position;
        var selfPosition = transform.position;

        var estimatedFlightTime = (enemyPosition - selfPosition).magnitude / projectileSpeed;

        var enemyFuturePosition = enemyPosition + enemySpeed * estimatedFlightTime * enemyMoveDirection;

        var xRotation = Random.Range(-dispersionAngle / 2, dispersionAngle / 2);
        var yRotation = Random.Range(-dispersionAngle / 2, dispersionAngle / 2);
        var zRotation = Random.Range(-dispersionAngle / 2, dispersionAngle / 2);

        var direction = Quaternion.Euler(xRotation, yRotation, zRotation) * (enemyFuturePosition - selfPosition).normalized;
        var position = transform.position + direction * 0.75f;
        var rotation = Quaternion.LookRotation(direction);

        var projectileBehaviour = Instantiate(projectile, position, rotation).GetComponent<BaseProjectile>();

        projectileBehaviour.SetDirection(direction);
        projectileBehaviour.SetSpeed(projectileSpeed);
        projectileBehaviour.SetDamage(damage);
        projectileBehaviour.SetEnemyController(enemyController);
        projectileBehaviour.SetMaximumRange(maximumRange);
        projectileBehaviour.SetShotBy(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    public enum MaterialState
    {
        OPAQUE,
        VALID,
        INVALID
    }

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

    private EnemyController enemyController;
    private TowerController towerController;

    private bool isGhost;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Material opaqueMaterial;
    [SerializeField]
    private Material validTransparentMaterial;
    [SerializeField]
    private Material invalidTransparentMaterial;

    private MaterialState materialState = MaterialState.OPAQUE;
    private MaterialState targetMaterialState = MaterialState.OPAQUE;
    // Start is called before the first frame update
    void Start()
    {
        SwitchMaterial(opaqueMaterial);
        if (isGhost)
        {
            materialState = MaterialState.VALID;
            targetMaterialState = MaterialState.VALID;
            SwitchMaterial(validTransparentMaterial);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
    }
    void Update()
    {
        if (isGhost) 
        {
            HandleGhostTower();
        }
        else
        {
            HandleLivingTower();
        }
    }
    private void HandleGhostTower()
    {
        if (targetMaterialState != materialState)
        {
            if (targetMaterialState == MaterialState.VALID)
            {
                SwitchMaterial(validTransparentMaterial);
                materialState = MaterialState.VALID;
            }
            else
            {
                SwitchMaterial(invalidTransparentMaterial);
                materialState = MaterialState.INVALID;
            }
        }
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
    }

    public void SetIsGhost(bool val)
    {
        isGhost = val;
    }
    public void SetTargetMaterial(MaterialState ms)
    {
        targetMaterialState = ms;
    }
    private void SwitchMaterial(Material material)
    {
        var materialsCopy = meshRenderer.materials;
        materialsCopy[0] = material;
        meshRenderer.materials = materialsCopy;
    }
}

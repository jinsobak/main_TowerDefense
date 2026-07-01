using System;
using UnityEngine;

public class Projectile : PoolableObject
{
    private Transform target;
    private int damage;
    private ProjectileData projectileData;

    [SerializeField] private Vector3 rotationOffset;

    #region projectile 생성
    public void Initialize(Transform target, int damage, ProjectileData projectileData)
    {
        this.target = target;
        this.damage = damage;
        this.projectileData = projectileData;
    }
    #endregion

    private void Update()
    {
        if(target == null || projectileData == null)
        {
            DespawnSelf();
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;

        transform.position += direction * projectileData.projectileSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation * Quaternion.Euler(rotationOffset);
        }


        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= 0.2f)
        {
            HitTarget();
        }

    }

    #region 타겟 적중
    private void HitTarget()
    {
        Vector3 hitPoint = GetHitPoint();

        SpawnHitEffect(hitPoint);

        switch (projectileData.attackType)
        {
            case ProjectileAttackType.Single:
                SingleHit();
                break;

            case ProjectileAttackType.Explosion:
                ExplosionHit();
                break;

            case ProjectileAttackType.Area:
                AreaHit();
                break;
        }

        DespawnSelf();
    }
    #endregion

    #region 히트 Trs 가져오기
    private Vector3 GetHitPoint()
    {
        Collider targetCollider = target.GetComponent<Collider>();

        if (targetCollider == null)
            targetCollider = target.GetComponentInChildren<Collider>();

        if (targetCollider == null)
            return transform.position;

        return targetCollider.ClosestPoint(transform.position);
    }
    #endregion
   
    #region 단일 공격
    private void SingleHit()
    {
        Monster monster = target.GetComponent<Monster>();

        if (monster != null)
            monster.TakeDamage(damage);
    }
    #endregion

    #region 광역 공격
    private void ExplosionHit()
    {
        DrawExplosionDebug(transform.position, projectileData.explosionRadius);
        Collider[] hits = Physics.OverlapSphere(transform.position, projectileData.explosionRadius, projectileData.targetLayer);

        foreach (Collider hit in hits)
        {
            Monster monster = hit.GetComponentInParent<Monster>();

            if (monster != null)
                monster.TakeDamage(damage);
        }
    }
    #endregion

    #region 광역 탄
    private void AreaHit()
    {
        ExplosionHit();
    }
    #endregion

    #region 히트 이펙트
    private void SpawnHitEffect(Vector3 hitPoint)
    {
        int effectID = projectileData.hitEffectID;

        if (effectID <= 0)
            return;

        GameObject prefab = ObjectPoolManager.Instance.GetEffect(projectileData.hitEffectID);


        PoolEffect effect = ObjectPoolManager.Instance.Spawn<PoolEffect>(
            prefab,
            hitPoint,
            Quaternion.identity,
            ObjectPoolManager.Instance.GetEffectParent()
        );

        if (effect != null)
            effect.Play();
    }
    #endregion

    #region 디스폰
    private void DespawnSelf()
    {
        ObjectPoolManager.Instance.Despawn(this);
    }
    #endregion

    private void DrawExplosionDebug(Vector3 center, float radius)
    {
        const int segments = 32;

        for (int i = 0; i < segments; i++)
        {
            float angle1 = i * Mathf.PI * 2 / segments;
            float angle2 = (i + 1) * Mathf.PI * 2 / segments;

            Vector3 p1 = center + new Vector3(Mathf.Cos(angle1), 0, Mathf.Sin(angle1)) * radius;
            Vector3 p2 = center + new Vector3(Mathf.Cos(angle2), 0, Mathf.Sin(angle2)) * radius;

            Debug.DrawLine(p1, p2, Color.red, 2f);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (projectileData == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, projectileData.explosionRadius);
    }
}

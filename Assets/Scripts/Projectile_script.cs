//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class Projectile_script : MonoBehaviour
{
    private Tower_script   _parent;
    private Monster_script _target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == "monster_target_tag") && (other.gameObject == _target.gameObject)) {
            Game_script.Instance.ObjectPool.Release(this.gameObject);
            other.GetComponent<Monster_script>().TakeDamage(this._parent.Damage);
        }
    }

    private void Start()
    {
        //
    }

    private void Update()
    {
        this.MoveToTarget();
    }

    public void Initialize(Tower_script parent)
    {
        this._parent = parent;
        this._target = parent.Target;
    }

    private void MoveToTarget()
    {
        if ((this._target != null) && this._target.IsActive)
        {
            this.transform.position = Vector3.MoveTowards(
                this.transform.position, this._target.transform.position, (this._parent.ProjectileSpeed * Time.deltaTime)
            );

            Vector2 direction = (this._target.transform.position - this.transform.position);
            float   angle     = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        } else if (!this._target.IsActive) {
            Game_script.Instance.ObjectPool.Release(this.gameObject);
        }
    }
}

//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_script : MonoBehaviour
{
    [SerializeField]
    private float _projectileSpeed;

    [SerializeField]
    private string _projectileType;

    [SerializeField]
    private float _attackCooldown;

    [SerializeField]
    private int _damage;

    private float                 _attackTimer;
    private bool                  _canAttack;
    private SpriteRenderer        _spriteRenderer;
    private Monster_script        _target;
    private Queue<Monster_script> _targets;

    public int            Damage          { get { return this._damage; } }
    public int            Price           { get; set; }
    public Monster_script Target          { get { return this._target; } }
    public float          ProjectileSpeed { get { return this._projectileSpeed; } }

    private void Start()
    {
        this._canAttack      = true;
        this._spriteRenderer = GetComponent<SpriteRenderer>();
        this._targets        = new Queue<Monster_script>();
    }

    private void Update()
    {
        this.Attack();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "monster_target_tag") {
            this._targets.Enqueue(other.GetComponent<Monster_script>());
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "monster_target_tag") {
            this._target = null;
        } else if (other.tag == "projectile_tag") {
            Game_script.Instance.ObjectPool.Release(other.gameObject);
        }
    }

    private void Attack()
    {
        if (!this._canAttack) {
            this._attackTimer += Time.deltaTime;
        }

        if (this._attackTimer >= this._attackCooldown) {
            this._canAttack   = true;
            this._attackTimer = 0;
        }

        if ((this._target == null) && (this._targets.Count > 0)) {
            this._target = this._targets.Dequeue();
        }

        if ((this._target != null) && this._target.IsActive && this._canAttack) {
            this.Shoot();
            this._canAttack = false;
        }
    }

    public void Select()
    {
        _spriteRenderer.enabled = !_spriteRenderer.enabled;
    }

    private void Shoot()
    {
        Projectile_script projectile  = Game_script.Instance.ObjectPool.GetObject(this._projectileType).GetComponent<Projectile_script>();
        projectile.transform.position = this.transform.position;

        projectile.Initialize(this);
    }
}

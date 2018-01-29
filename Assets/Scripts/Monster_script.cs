using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_script : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private Animator           _animator;
    private Vector3            _destination;
    private int                _health;
    private Stack<Node_script> _path;

    public bool                IsActive { get; private set; }
    public GridPosition_script Position { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "portal_end_tag") {
            StartCoroutine(this.Scale(this.transform.localScale, (this.transform.localScale * 0.1f), true));
            Game_script.Instance.Lives--;
        }
    }

    private void Start()
    {
        this._health  = 100;
        this.IsActive = false;
    }

    private void Update()
    {
        this.Move();
        this.GetComponentInChildren<Text>().text = string.Format("{0} %", this._health);
    }

    public void Animate(GridPosition_script position, GridPosition_script destination)
    {
        if (this._animator != null)
        {
            // DOWN
            if (position.Row < destination.Row) {
                this._animator.SetInteger("Horizontal", 0);
                this._animator.SetInteger("Vertical",   1);
            // UP
            } else if (position.Row > destination.Row) {
                this._animator.SetInteger("Horizontal", 0);
                this._animator.SetInteger("Vertical",   -1);
            // RIGHT
            } else if (position.Column < destination.Column) {
                this._animator.SetInteger("Horizontal", 1);
                this._animator.SetInteger("Vertical",   0);
            // LEFT
            } else if (position.Column > destination.Column) {
                this._animator.SetInteger("Horizontal", -1);
                this._animator.SetInteger("Vertical",   0);
            }
        }
    }

    private void Move()
    {
        if (this.IsActive)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this._destination, (this._speed * Time.deltaTime));

            if ((this.transform.position == this._destination) && (this._path != null) && (this._path.Count > 0)) {
                this.Animate(this.Position, this._path.Peek().Position);

                this.Position     = this._path.Peek().Position;
                this._destination = this._path.Pop().WorldPosition;
            }
        }
    }

    private void Release()
    {
        this._health  = 100;
        this.IsActive = false;
        this.Position = TileMap_script.Instance.PortalStartSpawnPos;
        Game_script.Instance.ObjectPool.Release(this.gameObject);
        Game_script.Instance.RemoveMonster(this);
    }

    private void SetPath(Stack<Node_script> path)
    {
        if (path != null)
        {
            this._path = path;

            this.Animate(this.Position, this._path.Peek().Position);

            this.Position     = this._path.Peek().Position;
            this._destination = this._path.Pop().WorldPosition;
        }
    }

    public void Spawn()
    {
        this.transform.position = TileMap_script.Instance.PortalStart.transform.position;
        this._animator          = GetComponent<Animator>();

        StartCoroutine(this.Scale((this.transform.localScale * 0.1f), this.transform.localScale, false));

        this.SetPath(TileMap_script.Instance.MonsterPath);
    }

    public IEnumerator Scale(Vector3 startSize, Vector3 endSize, bool remove)
    {
        float progress = 0;

        while (progress <= 1.0f) {
            this.transform.localScale = Vector3.Lerp(startSize, endSize, progress);
            progress += Time.deltaTime;
            yield return null;
        }

        this.transform.localScale = endSize;
        this.IsActive             = true;

        if (remove) {
            this.transform.localScale = startSize;
            this.Release();
        }
    }

    public void TakeDamage(int damage)
    {
        if (this.IsActive)
        {
            this._health -= damage;

            if (this._health <= 0) {
                this.Release();
            }
        }
    }
}

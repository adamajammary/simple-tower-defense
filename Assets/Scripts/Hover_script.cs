using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover_script : Singleton<Hover_script>
{
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _spriteRendererRange;

    private void Start()
    {
        this._spriteRenderer      = GetComponent<SpriteRenderer>();
        this._spriteRendererRange = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        this.FollowMouse();
    }

    private void FollowMouse()
    {
        if (this._spriteRenderer.sprite != null) {
            this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
        }
    }

    public void SetSprite(Sprite sprite)
    {
        this._spriteRenderer.sprite = sprite;

        if (sprite == null) {
            Game_script.Instance.ClickedButton = null;
            this._spriteRendererRange.enabled  = false;
        } else {
            this._spriteRendererRange.enabled = true;
        }
    }
}

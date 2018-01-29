using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_script : MonoBehaviour
{
    [SerializeField]
    private float   Speed = 0;
    private Vector2 Limit;

    private void Start()
    {
        //
	}

    private void Update()
    {
        this.GetInput();
	}

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W)) {
            this.transform.Translate(Vector3.up * this.Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * this.Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.down * this.Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * this.Speed * Time.deltaTime);
        }

        this.transform.position = new Vector3(
            Mathf.Clamp(this.transform.position.x, 0, this.Limit.x), Mathf.Clamp(this.transform.position.y, this.Limit.y, 0), -1
        );
    }

    public void SetLimit(Vector3 maxTile)
    {
        Vector3 bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        this.Limit          = new Vector2((maxTile.x - bottomRight.x), (maxTile.y - bottomRight.y));
    }
}

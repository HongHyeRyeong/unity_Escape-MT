﻿using UnityEngine;

public class Item : MonoBehaviour
{
    public SpriteRenderer item;
    public Sprite[] sprites;

    public int itemType;
    private float moveSpeed;

    private void Awake()
    {
        SetData();
    }

    public void SetData()
    {
        this.itemType = Random.Range(1, 3);
        item.sprite = sprites[itemType];

        moveSpeed = Random.Range(2f, 4f);
    }

    private void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EndPos"))
        {
            Destroy(this.gameObject);
        }
    }
}

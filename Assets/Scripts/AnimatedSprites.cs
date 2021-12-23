using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class AnimatedSprites : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer {get; private set;}

    public Sprite[] sprites;
    public float animationTime = 0.25f;
    public int animationFrame { get; private set; }
    public bool loop = true;

    private void  Awake()
    {
        this.SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(Advance), this.animationTime, this.animationTime);
    }

    private void Advance()
    {
        if (!this.SpriteRenderer.enabled)
        {
            return;
        }

        this.animationFrame ++;
        
        if (this.animationFrame >= this.sprites.Length && this.loop)
        {
            this.animationFrame = 0;
        }

        if (this.animationFrame >= 0 && this.animationFrame < this.sprites.Length)
        {
            this.SpriteRenderer.sprite = this.sprites[this.animationFrame];
        }
    }

    public void Restart()
    {
        this.animationFrame = -1;

        Advance();
    }
}
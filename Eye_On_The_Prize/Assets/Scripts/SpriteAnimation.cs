using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField]
    private float fps = 60.0f;

    [SerializeField]
    private SpriteRenderer sRenderer;

    [SerializeField]
    private string folderPath = "SPRITES_FOLDER"; // Relative to the Resources folder
    private List<Sprite> spriteList = new List<Sprite>();
    private bool spritesLoaded = false;

    private int spriteCount;
    private int spriteIndex = 0;

    private float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadSpritesFromFolder(folderPath);
    }

    // Update is called once per frame
    void Update()
    {
        if (spritesLoaded)
        {
            timer += Time.deltaTime;

            float frameDuration = 1f / fps;

            if (timer >= frameDuration)
            {
                timer = 0f;
                if (spriteIndex < spriteCount)
                {
                    sRenderer.sprite = spriteList[spriteIndex];
                    spriteIndex++;
                }
                else
                {
                    spriteIndex = 0;
                }
            }
        }
    }

    void LoadSpritesFromFolder(string path)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);
        if (sprites.Length > 0)
        {
            spriteList.AddRange(sprites);
            Debug.Log($"Loaded {sprites.Length} sprites from {path}");

            spriteCount = sprites.Length;
            spritesLoaded = true;
        }
        else
        {
            Debug.LogWarning($"No sprites found at path: {path}");
        }
    }
}

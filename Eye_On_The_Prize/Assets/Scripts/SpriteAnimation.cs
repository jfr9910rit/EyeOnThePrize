using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField]
    private bool autoPlay = true;
    [SerializeField]
    private bool loop;
    private bool play;

    [SerializeField]
    private float fps = 60.0f;

    [SerializeField]
    private Image imageUI;

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
        play = autoPlay;
    }

    // Update is called once per frame
    void Update()
    {
        if (spritesLoaded && play)
        {
            timer += Time.deltaTime;

            float frameDuration = 1f / fps;

            if (timer >= frameDuration)
            {
                timer = 0f;
                if (spriteIndex < spriteCount)
                {
                    imageUI.sprite = spriteList[spriteIndex];
                    spriteIndex++;
                }
                else if (loop) 
                {
                    spriteIndex = 0;
                }
                else
                {
                    play = false;
                }
            }
        }
    }

    public void playAnimation()
    {
        spriteIndex = 0;
        play = true;
    }

    // loop will begin next time Update() is called
    public void beginLoopingAnimation()
    {
        loop = true;
        play = true;
    }

    // loop will continue until it reaches its final frame
    public void endLoopingAnimation()
    {
        loop = false;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class ExtDebug {
    public static void Log(string color, string core, string message) {
        Debug.Log("<color="+color+">"+core+" | "+message+"</color>");
    }
    public static void Log(string core, string message) {
        Debug.Log("<color=white>"+core+" | "+message+"</color>");
    }
    public static void LogSuccess(string core, string message) {
        Debug.Log("<color=green>"+core+" | "+message+"</color>");
    }
    public static void LogWarning(string core, string message) {
        Debug.LogWarning("<color=yellow> WARN"+core+" | "+message+"</color>");
    }
    public static void LogError(string core, string message) {
        Debug.LogError("<color=red> ERR"+core+" | "+message+"</color>");
    }
}
public static class TextureUtilsExt{
    public static Texture2D spriteToTex(Sprite sprite){
        int width = (int)sprite.rect.width;
        int height = (int)sprite.rect.height;
        int x = (int)sprite.rect.x;
        int y = (int)sprite.rect.y;
        Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);
        for(int xi = 0; xi < width; xi++){
            for(int yi = 0; yi < height; yi++){
                Color color = sprite.texture.GetPixel(x+xi,y+yi,0);
                tex.SetPixel(xi, yi, (color.a==0f) ? new Color(0f,0f,0f,0f) : color);
            }
        }
        tex.filterMode = FilterMode.Point;
        tex.Apply();

        return tex;
    }

    public static List<Color> paletteFromSprite(Sprite sprite) {
        Texture2D tex = TextureUtilsExt.spriteToTex(sprite);
        List<Color> result = new List<Color>();

        for(int x = 0; x < tex.width; x++) {
            Color color = tex.GetPixel(x,0,0);
            result.Add(color);
        }

        return result;
    }
}
[System.Serializable] public class ColorPalettes {
    [Header("Color Palettes")] public List<ColorPalette> palettes;

    public void Init() {
        for(int i = 0; i < palettes.Count; i++) {
            palettes[i].Init();
        }
    }
}
[System.Serializable] public class ColorPalette {
    [Header("Original Color Palette Sprite")] public Sprite oPalette;
    [Header("Target Color Palette Sprite")] public Sprite tPalette;

    [Header("(typeof Color) Source Palette Colors")] public List<Color> oColors;
    [Header("(typeof Color) Target Palette Colors")] public List<Color> tColors;   

    public ColorPalette(Sprite oPalette, Sprite tPalette) {
        this.oPalette=oPalette;
        this.tPalette=tPalette;
        Init();
    }
    public void Init() {
        oColors = TextureUtilsExt.paletteFromSprite(oPalette);
        tColors = TextureUtilsExt.paletteFromSprite(tPalette);
    }

    public Texture2D getTexture(Texture2D sourceTex) {
        Texture2D tex = new Texture2D(sourceTex.width, sourceTex.height, TextureFormat.RGBA32, false);
        for (int x = 0; x < sourceTex.width; x++){
            for (int y = 0; y < sourceTex.height; y++){
                Color color = tex.GetPixel(x,y,0);
                int ogColor = oColors.FindIndex(x => x == color);
                if (ogColor<0) color = tColors[ogColor];
                tex.SetPixel(x,y,color);
            }  
        }
        tex.filterMode = FilterMode.Point;
        tex.Apply();
        return tex;
    }
}


public class PaletteChanger : MonoBehaviour
{
    [Header("Target Renderer To Change Colors")] public SpriteRenderer targetRenderer;
    [Header("An array of generated palettes and their configurations")] public ColorPalettes colorPalettes;

    
    public Texture2D inputtestTexture2D;
    public Texture2D testTexture2D;
    // Start is called before the first frame update
    void Start()
    {
        if (targetRenderer==null) targetRenderer=this.GetComponent<SpriteRenderer>();
        if (targetRenderer==null) {
            ExtDebug.LogError("PaletteChanger", this.gameObject.name + " CAN'T FIND COMPONENT OF TYPE SPRITE RENDERER");
            this.enabled = false;
        }
        colorPalettes.Init();
        testTexture2D=colorPalettes.palettes[0].getTexture(inputtestTexture2D);
    }
    public void ChangeColors(Texture2D colorPalette) {}
    // Update is called once per frame
    void Update()
    {
        
    }
}

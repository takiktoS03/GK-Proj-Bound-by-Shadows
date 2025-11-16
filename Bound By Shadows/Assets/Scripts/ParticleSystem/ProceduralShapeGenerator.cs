//using UnityEngine;

//public static class ProceduralShapeGenerator
//{
//    public static Sprite GenerateCircle(int size, Color color)
//    {
//        Texture2D tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
//        Color[] pixels = new Color[size * size];
//        Vector2 center = new Vector2(size / 2f, size / 2f);
//        float radius = size / 2f;

//        for (int y = 0; y < size; y++)
//        {
//            for (int x = 0; x < size; x++)
//            {
//                float dist = Vector2.Distance(new Vector2(x, y), center);
//                pixels[y * size + x] = dist <= radius ? color : Color.clear;
//            }
//        }
//        tex.SetPixels(pixels);
//        tex.Apply();
//        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
//    }

//    public static Sprite GenerateSquare(int size, Color color)
//    {
//        Texture2D tex = new Texture2D(size, size, TextureFormat .ARGB32, false);
//        Color[] pixels = new Color[size * size];
//        for (int i = 0; i < pixels.Length; i++)
//            pixels[i] = new Color(color.r, color.g, color.b, 1f);

//        tex.SetPixels(pixels);
//        tex.Apply();
//        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
//    }

//    public static Sprite GenerateTriangle(int size, Color color)
//    {
//        Texture2D tex = new Texture2D(size, size, TextureFormat .ARGB32, false);
//        Color[] pixels = new Color[size * size];
//        Vector2 bottom = new Vector2(size / 2f, 0);

//        for (int y = 0; y < size; y++) 
//        {
//            float width = (float)y / size * size;
//            float halfWidth = width / 2f;
//            for (int x = 0; x < size; x++)
//            {
//                if(x > size / 2f - halfWidth && x < size/ 2f + halfWidth)
//                    pixels[y * size + x] = color;
//                else 
//                    pixels[y * size + x] = Color.clear;

//            }
//        }
//        tex.SetPixels(pixels);
//        tex.Apply();
//        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);

//    } 
//}

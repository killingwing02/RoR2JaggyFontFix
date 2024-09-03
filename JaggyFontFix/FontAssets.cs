using UnityEngine;
using System.IO;
using JaggyFontFix;

//Static class for ease of access
public static class FontAssets
{
    //The mod's AssetBundle
    public static AssetBundle mainBundle;
    //A constant of the AssetBundle's name.
    public const string bundleName = "zh_font";
    // Not necesary, but useful if you want to store the bundle on its own folder.
    // public const string assetBundleFolder = "AssetBundles";

    //The direct path to your AssetBundle
    public static string AssetBundlePath
    {
        get
        {
            // This returns the path to your assetbundle assuming said bundle is on the same folder as your DLL.
            // If you have your bundle in a folder, you can uncomment the statement below this one.
            return Path.Combine(Path.GetDirectoryName(JaggyFontFix.JaggyFontFix.PInfo.Location), bundleName);
            //return Path.Combine(Path.GetDirectoryName(MainClass.PInfo.Location), assetBundleFolder, myBundle);
        }
    }

    public static void Init()
    {
        //Loads the assetBundle from the Path, and stores it in the static field.
        mainBundle = AssetBundle.LoadFromFile(AssetBundlePath);
        Log.Info(mainBundle.name + " bundle loaded.");
    }
}
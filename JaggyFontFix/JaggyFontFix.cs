using BepInEx;
using UnityEngine;
using TMPro;
using IO = System.IO;
using System.Text.RegularExpressions;
using System;
using RoR2;
using BepInEx.Configuration;

namespace JaggyFontFix
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    public class JaggyFontFix : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "xykle";
        public const string PluginName = "JaggyFontFix";
        public const string PluginVersion = "1.2.2";

        public static PluginInfo PInfo { get; private set; }

        #region Config
        public static ConfigEntry<UseFontName> UseFontNameConfig { get; set; }
        public static ConfigEntry<string> CustomFontFileNameConfig { get; set; }
        #endregion

        private TMP_FontAsset fontAsset;

        #region Init
        private void Init()
        {
            PInfo = Info;

            // Init our logging class so that we can properly log for debugging
            Log.Init(Logger);
            FontAssets.Init();
            ConfigurationInit();
        }

        private void ConfigurationInit()
        {
            UseFontNameConfig = Config.Bind<UseFontName>(
                "Font Settings",
                "Font Name",
                    UseFontName.NotoSans,
                "Set which font will use in game. If you want use custom font, set this to \"Custom\"."
                );
            CustomFontFileNameConfig = Config.Bind<string>(
                "Font Settings",
                "Custom Font File Name",
                    "font.ttf",
                "Set custom font file name that you wanna use in game. Only work when \"Font Name\" set to \"Custom\".\n" +
                "You NEED to put your font file under this plugin folder, path to font file is not supported.\n" +
                "File extension is optional."
                );
        }
        #endregion

        #region Unity Loop
        public void Awake()
        {
            Init();

            GenerateFontAsset();

            SubcribeEvents();
        }

        private void OnDestory()
        {
            UnSubcribeEvents();
        }
        #endregion

        #region Event Subscriber
        private void SubcribeEvents()
        {
            On.RoR2.UI.HGTextMeshProUGUI.Awake += ReplaceHGTextFont;
            On.RoR2.UI.LanguageTextMeshController.CacheComponents += ReplaceTmpControllerFont;
            On.RoR2.Run.InstantiateUi += ReplaceFontOnStageBegin;
        }

        private void UnSubcribeEvents()
        {
            On.RoR2.UI.HGTextMeshProUGUI.Awake -= ReplaceHGTextFont;
            On.RoR2.UI.LanguageTextMeshController.CacheComponents -= ReplaceTmpControllerFont;
            On.RoR2.Run.InstantiateUi -= ReplaceFontOnStageBegin;
        }
        #endregion

        #region Load Font File
        private Font LoadFont(UseFontName fontName)
        {
            Font font = null;

            #region Old Load Font Function
            /*
            // Try load font from disk
            var fontFilePath = IO.Path.Combine(IO.Path.GetDirectoryName(PInfo.Location), fontFileName);

            if (IO.File.Exists(fontFilePath + ".ttf"))
            {
                font = FontFromFile(fontFilePath + ".ttf");
            }
            else if (IO.File.Exists(fontFilePath + ".otf"))
            {
                font = FontFromFile(fontFilePath + ".otf");
            }

            // If font doesn't exist, try load font from bundle asset.
            if (font == null)
            {
                Log.Info("Font in plugin folder not found, loading from zh_font bundle asset.");

                string fontName = defultFontName;
                string path = IO.Path.Combine(IO.Path.GetDirectoryName(PInfo.Location), fontConfigFile);

                if (IO.File.Exists(path))
                {
                    IO.StreamReader reader = new IO.StreamReader(path);
                    string _font = reader.ReadLine();
                    reader.Close();
                    reader.Dispose();
                    if (_font != null) fontName = Regex.Replace(_font, @"\r\n?|\n", "");
                    else Log.Warning("UseFont.txt found, but it's empty. Default font NotoSansCJKsc-Regular will be load.");
                }
                else
                {
                    Log.Info(path + " not found. Default font NotoSansCJKsc-Regular will be load.");
                }

                font = FontAssets.mainBundle.LoadAsset<Font>(fontName);
            }

            // If font name in UseFont.txt is not exist, load "NotoSansCJKsc-Regular".
            if (font == null)
            {
                Log.Warning("Font " + fontFileName + " is not in bundle. Change font to NotoSansCJKsc-Regular.");
                font = FontAssets.mainBundle.LoadAsset<Font>(defultFontName);
            }
            */
            #endregion

            switch (fontName)
            {
                case UseFontName.TaipeiSans:
                    font = FontAssets.mainBundle.LoadAsset<Font>("TaipeiSansTCBeta-Regular");
                    break;
                case UseFontName.Cubic11:
                    font = FontAssets.mainBundle.LoadAsset<Font>("Cubic_11");
                    break;
                case UseFontName.Custom:
                    font = FontFromFile(CustomFontFileNameConfig.Value);
                    break;
                default:
                    font = FontAssets.mainBundle.LoadAsset<Font>("NotoSansCJKsc-Regular");
                    break;
            }

            return font;
        }

        private Font FontFromFile(string fileName)
        {
            var fontFilePath = IO.Path.Combine(IO.Path.GetDirectoryName(PInfo.Location), fileName);

            // Check if user add file extension or not.
            if (!fileName.Contains(".ttf") && !fileName.Contains(".otf"))
            { 
                if (IO.File.Exists(fontFilePath + ".ttf")) fontFilePath += ".ttf";
                else if (IO.File.Exists(fontFilePath + ".otf")) fontFilePath += ".otf";
                else
                {
                    Log.Warning("Custom font file not found! Is there any typo or not support format? (Only .ttf and .otf are supported.) Input name: " + fileName);
                    return FontAssets.mainBundle.LoadAsset<Font>("NotoSansCJKsc-Regular");
                }
            }

            if (!IO.File.Exists(fontFilePath))
            {
                    Log.Warning("Custom font file not found! Is there any typo? Input name: " + fileName);
                    return FontAssets.mainBundle.LoadAsset<Font>("NotoSansCJKsc-Regular");
            }

            Font font = new Font(fontFilePath);

            Log.Info("Loading \"" + fontFilePath + "\".");
            return font;
        }
        #endregion

        #region Replace Fonts
        private GameObject ReplaceFontOnStageBegin(On.RoR2.Run.orig_InstantiateUi orig, RoR2.Run self, Transform uiRoot)
        {
            var obj = orig(self, uiRoot);
            var count = ReplaceAllFont();

            Log.Info(string.Format("New stage begun, replace {0} font(s) on scene.", count));

            return obj;
        }

        private void ReplaceTmpControllerFont(On.RoR2.UI.LanguageTextMeshController.orig_CacheComponents orig, RoR2.UI.LanguageTextMeshController self)
        {
            orig(self);

            if (self.textMeshPro == null) return;

            self.textMeshPro.font = fontAsset;
        }

        private void ReplaceHGTextFont(On.RoR2.UI.HGTextMeshProUGUI.orig_Awake orig, RoR2.UI.HGTextMeshProUGUI self)
        {
            orig(self);

            self.font = fontAsset;
            self.UpdateFontAsset();
        }

        private int ReplaceAllFont()
        {
            var tmps = FindObjectsByType<TMP_Text>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            int counter = 0;
            foreach (var tmp in tmps)
            {
                if (tmp.font == fontAsset) continue;
                tmp.font = fontAsset;
                counter++;
            }

            return counter;
        }
        #endregion

        private void GenerateFontAsset()
        {
            fontAsset = null;

            Font fontFile = LoadFont(UseFontNameConfig.Value);
            Log.Info(fontFile.name + " is loaded.");

            // Create font asset with multi atlas texture feature
            fontAsset = TMP_FontAsset.CreateFontAsset(
                fontFile,
                90,
                9,
                UnityEngine.TextCore.LowLevel.GlyphRenderMode.SDFAA,
                4096,
                2048,
                AtlasPopulationMode.Dynamic,
                true
                );

            // Adding shadow to font asset to make font closer to original game
            var mat = fontAsset.material;
            mat.SetFloat("_UnderlayOffsetX", .3f);
            mat.SetFloat("_UnderlayOffsetY", -.3f);
            mat.SetFloat("_UnderlayDilate", .5f);
            mat.SetFloat("_UnderlaySoftness", .1f);
            mat.EnableKeyword("UNDERLAY_ON");
            fontAsset.material = mat;
        }
    }

    public enum UseFontName
    {
        NotoSans = 0,
        TaipeiSans = 1,
        Cubic11 = 2,
        Custom = 3
    }
}

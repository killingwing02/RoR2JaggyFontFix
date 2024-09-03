# Jaggy Font Fix
No more jaggy font for Chinese, Japanese and Korean! And now with customize font feature!

# Why?
After Seekers of the Storm update, Chinese, Japanese and Korean font becomes jaggy.

![image](https://imgur.com/pCt6Q9j.png)

It's very annoying to read like this, so I made this mod to deal with it.\
And this is my first time to make a mod. (My first time is for this, such a meme lol)

# OK, so how this mod work?
~~This mod will scan for all TMP_Text compoments available in scene when specific events is triggered, and then replace them with newly generated TMP_FontAsset.~~\
But noted that due to dynamic font that generate characters at runtime, you might notice some FPS spike.

Now I found out there's a function that can just replace about 90% font assets in game. Sometimes this mod still search for all TMP_Text in the scene (At every stage begin), but (hopelly) this mod won't cause too much performance issue now.

# And what else?
This mod also provide alt fonts cuz I don't like default font very much. :\\

~~To change font, just modify `UseFont.txt` and change text to font name and you are done.\
Fonts name is also include in this mod as a text file, make sure type name right to change font.~~

Now I change configuration method to BepInEx build-in one, if you use [r2modman](https://thunderstore.io/package/ebkr/r2modman/) to manage mod, you can just edit config file in r2modman. If you're not use r2modman, config file is locate at `\BepInEx\config\xykle.JaggyFontFix.cfg`.\
Note that config file only generated after game launched once.

## Available fonts list: 
* Noto Sans (Default game font)
* [台北黑體／Taipei Sans TC](https://sites.google.com/view/jtfoundry/zh-tw?authuser=0)
* [俐方體11號／Cubic 11](https://github.com/ACh-K/Cubic-11)

> All fonts are licensed under SIL Open Font License 1.1.

# But wait, here's more!
Now we have **CUSTOMIZE FONT** feature! You can just put your font file (in `.ttf` or `.otf` format) under this mod folder (`\BepInEx\plugins\xykle-JaggyFontFix`, same folder as `JaggyFontFix.dll`)~~, then rename your font file to `font.ttf` or `font.otf` and your are good to go.~~

Since I'm using build-in configuration method now, you can just type in your font file name in config file, and enjoy your game. :)

# FAQ
**Q: Will this mod keep update?**\
A: Probably not. Cuz jaggy font maight patch out any time, and basiclly I made this mod just for me and my friends.

**Q: YOU LIAR YOU UPDATE THIS MOD THREE DAY IN A ROW!**\
A: Not that often anymore. lol

**Q: There's sill some font not been change.**\
A: ~~I didn't put a lot of time to make this mod, basiclly this mod is just useable. And also I'm lazy.~~\
Now it about 95%(I think) font got replace now, it's too much work for 5% plus I'm still lazy, so I'll call it a day.

**Q: Can you add xxx font to the mod?**\
A: ~~Short answer, no. But maybe someday I will add load font from file function, just maybe.~~\
Now it's YES. You can customize font by put `.ttf` or `.oft` file under the plugin folder(`\BepInEx\plugins\xykle-JaggyFontFix`) and change config file to match the font file.

**Q: Taipei Sans TC and Cubic 11 not work with Korean characters.**\
A: Taipei Sans TC and Cubic 11 does NOT support Korean characters, use Noto Sans or custom font instead.

**Q: THIS MOD DOES NOT WORK!**\
A: Please update to v1.1.2, I forget to dependency [HookGenPatcher](https://thunderstore.io/package/RiskofThunder/HookGenPatcher/). \:facepalm\:
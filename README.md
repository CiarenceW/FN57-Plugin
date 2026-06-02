# FN57-Plugin
A Five-seveN gun mod for Receiver 2

Also check out: [MagLoaderThing](https://thunderstore.io/c/receiver-2/p/CiarenceW/MagLoaderThing/), this un-cuts a thing in the Compound that makes reloading big magazines a breeze  

And [this](https://thunderstore.io/c/receiver-2/p/CiarenceW/CustomCompoundAmmoBoxes/) as well, it adds ammo boxes for modded cartridges in the Compound, with possibilities of adding custom models for them too (like this gun has)

## Changelog  
`1.0.0`  
Initial release  
  
`1.1.0`
 - (*By Szikaka*) Added **low capacity** mags to the Dreaming, and **standard capacity** mags to the spawn menu.
 - Removed **ugly gloss texture** map (as suggested by Szikaka)
 - Made the **sights texture emissive**, you can now aim in the dark, yay
 - **Mac** and **Linux** versions now available
 - Also made the unity source downloadable, if you want a model to compare or whatever  
  
`1.2.0`
 - Made the materials look better, I didn't notice that removing the roughness map would revert the metal and smoothness values, oops.
 - Fixed pegboard hanging positioning for the Five-seveN
 - Made mags hang-able as well. 
 - Fixed minor issue that probably nobody noticed with the cam thingy rotating forever when the slide was pulled. no idea what it's really called.
 - Scaled down collision on the ammo box to make it possible to collect a tape if they had the misfortune of spawning together.
 - Changed the materializing image sprite, looks a bit better, will improve on it later on.  
  
`2.0.0`  
 - Updated the gun to use the new modding kit, which got rid of those pesky error messages when using the custom sounds.
 - Changed the materials, the gun no longer looks washed out when in darkness and underneath lights
 - Added a description in the Help menu, it took me like 20 minutes to make in between Valorant games
 - Added a spring to the firing pin, looks cool idk

`2.0.1`  
 - Fixed the gun not chambering another round at high (>122) fps, or with slow-mo activated
 - Changed the X-Ray materials to be more in line with the vanilla guns (enable inspect mode in debug menu, and hold "i")
 - Corrected the magazine_catch_pressed amount  
  
`2.1.0`  
 - Made magazines double stacked
 - Made magazines spawn on their side in the dreaming
 - Made the loaded chamber indicator work (it's useless and you probably won't notice it, but it's there!)
 - Changed magazine pose when held
 - Fixed for the second time the weapon frame's materials  
  
`2.2.0`
 - Added custom ammo box model, to go along with my [new thing](https://thunderstore.io/c/receiver-2/p/CiarenceW/CustomCompoundAmmoBoxes/) :)
 - Also fixed some bugs  
  
`2.2.1`  
 - Fixed cartridge glint being scaled x100 for no fucking reason
 - Kind of fixed placement of rounds in ammo boxes  
  
`3.0.0`  
 - Moved this shit to my new cool unity project :)  
 - Fixed bullets not fucking penetrating stuff
 - Changed cartridge models and textures for new ones from H3VR
 - Slightly moved back the position of the cartridge when it's chambered properly

## DISCLAIMER
If you run into an issue, create an issue in the Issues tab on GitHub, with a screenshot of your problem and your player.log file, located in your  %UserProfile%\AppData\LocalLow\Wolfire Games\Receiver2\ folder or equivalent for your OS.

## Requirements
 - [BepinEx 5](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.21)
 - Receiver 2 (duh) 
 - The latest version of the [modding kit](https://github.com/Szikaka-97/Receiver2ModdingKit/releases/download/ver1%2C0%2C0/Receiver2ModdingKit.-.Release.zip)
 
## Download & Installation
If you're on WINDOW$ just download it from [there](https://thunderstore.io/c/receiver-2/p/CiarenceW/FN57/)  
  
Otherwise just do this idk:
(You can watch [this](https://www.youtube.com/watch?v=xe5f_CwQQVo) video guide in case something isn't clear)  							
 - Download the files in the releases section.<br />
 - Copy the file called _"FN57"_ in **FN-57_R2_(OS).zip** to <br />
 %UserProfile%\AppData\LocalLow\Wolfire Games\Receiver2\Guns - Windows <br />
 ~/Library/Application Support/Wolfire Games/Receiver2/Guns – Mac OS<br />
 ~/.config_unity3d/Wolfire Games/Receiver2/Guns – Linux <br />
 - Copy the file called _"FN57Patch"_ in **FN-57_R2_(OS).zip** to (where you have R2 installed) **Receiver 2\BepinEx\plugins**
 
 ## Usage
Go to the debug menu, and start the custom campaign, or go to the debug menu and spawn the gun + mag in.

## Credits
 Szikaka for all their precious help, and also for the modding kit  
 Wolfire for this masterpiece of a game  
 me (Ciarence#6364)  
 BSG for the models and sounds and textures and not being able to spell Reciever correctly  
 RustLTD for the new cartridge model and texture :)
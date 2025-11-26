# Milk outside a bag of Milk: Wii U Edition

**A homebrew recreation of Milk outside a bag of Milk for the Wii U.** <br>

## About
Milk outside a bag of Milk: Wii U Edition is a fan-made homebrew port of *Milk outside a bag of Milk*, developed from scratch for the Wii U. This project aims to faithfully recreate the experience of the original game while optimizing it for Nintendo's console. This remake also make serval improvement that the Switch version implemented badly.
## How to Get the Source Code

You can download the source code of *Milk outside a bag of Milk: Wii U Edition* directly from this repository. However, all original game assets have been **censored** to protect the work of **Nikita Kryukov**, the original creator.

To restore the missing assets, you’ll need a copy of the original game and follow the steps below:

### Requirements

* The original game files (Steam or Itch.io)
* [MOBMAssetRecover](https://github.com/Source-Macchiato/MOBM-WiiU-SC/releases/tag/release)
* [RPA Archive Extractor](https://iwanplays.itch.io/rpaex) by IwanPlays (`RPAExtract.exe`)

### Steps

1. **Download** both the source code of *Milk outside a bag of Milk: Wii U Edition* and the **MOBMAssetRecover** tool.
2. **Extract the original game assets:**

   * Locate the original game’s install folder.
   * Go to the `game` subfolder and copy `archive.rpa` to a new empty folder (i recommend to put it in a brand new folder, as the whole game will be decompiled in it).
   * Place `RPAExtract.exe` in the same folder and drag `archive.rpa` onto it to extract the contents.
3. **Recover the assets:**

   * Launch `MOBMAssetRecover.exe`.
   * When prompted in the command line, enter the following paths:

     ```
     [*] Enter Unity Assets folder path:         <- e.g. C:/Users/SourceMacchiato/MOBM-WIIU-SC/Assets
     [*] Enter original assets folder path:      <- Path to the extracted game files
     [*] Enter AssetConfig.json path:            <- Just type AssetConfig.json if it's in the same folder
     ```
4. **Wait** for the tool to finish restoring the assets.
5. **Done!** You can now open the project in Unity with all required assets properly restored.

## Features
- Brand new main menu
- Layout system
- New Translations

### Controllers supported :
- GamePad
- Pro
- Classic
- Classic Pro
- Wiimote

## Installation
### Requirements
- A homebrewed Wii U (Haxchi, Mocha, Tiramisu or Aroma)
- NUSspli or WUP Installer GX2
- SD card with sufficient space

### Steps
1. Download the latest release from the [itch.io official page](https://source-macchiato.itch.io/milk-collection-wiiu-edition).
2. Put the unzipped folder in the "install" folder of your SD card
3. Start the installation with WUP Installer GX2 or NUSspli (don't forget to put the [Sigpatches module](https://github.com/marco-calautti/SigpatchesModuleWiiU) in SD:/wiiu/environments/your-environment/modules/setup).
4. Go back to the Wii U menu, the game should appear. if you have any issue [join our discord server](https://sourcemacchiato.com/discord)


## Images of the game
<img src="./Medias/cover1.jpg" width="300"> <img src="./Medias/cover2.jpg" width="300"> <img src="./Medias/cover3.jpg" width="300"> <img src="./Medias/cover4.jpg" width="300">

## Contributing
If you’d like to contribute to this project, feel free to submit pull requests. Any bug reports or feature suggestions are welcome in the [Issues](https://github.com/Source-Macchiato/MOBM-WiiU-SC/issues) section.

## Disclaimer
This is a **fan project** and is in no way affiliated with Nikita Kryukov. This project is made for educational and entertainment purposes only.
© 2022, Original version developed by Nikita Kryukov

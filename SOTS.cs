using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using SOTS.Void;

namespace SOTS
{
	public class SOTS : Mod
	{	
	
		internal static SOTS Instance;

		public SOTS()
		{
			Instance = this;
			
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}
	
	
	
	
		private UserInterface _VoidUserInterface;
		internal VoidUI VoidUI;
		
		public override void Load()
        {
            if (!Main.dedServ)
            {
                VoidUI = new VoidUI();
                VoidUI.Activate();
                _VoidUserInterface = new UserInterface();
                _VoidUserInterface.SetState(VoidUI);

            }
        }
		public override void Unload() 
		{
			Instance = null;
		}
		public override void UpdateUI(GameTime gameTime) 
		{
			if (_VoidUserInterface != null)
			{
						_VoidUserInterface.Update(gameTime);
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (mouseTextIndex != -1) {
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"SOTS: Void Meter",
					delegate {
						if (VoidUI.visible) {
							_VoidUserInterface.Draw(Main.spriteBatch, new GameTime());
						}
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
		
		
		
		
	public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "ObsidianScale", 12);
			recipe.AddIngredient(ItemID.HellstoneBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(ItemID.HotlineFishingHook, 1);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "CrusherEmblem", 1);
			recipe.AddIngredient(null, "DemonBlood", 20);
			recipe.AddIngredient(ItemID.Obsidian, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(null, "ObsidianScale", 12);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "DemonBlood", 24);
			recipe.AddIngredient(null, "SteelBar", 1);
			recipe.AddIngredient(ItemID.Obsidian, 1);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(ItemID.HellstoneBar, 1);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "FatBass", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ItemID.Sashimi, 15);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "SandFish", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ItemID.SandBlock, 25);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "AngelCarp", 1);
			recipe.AddIngredient(null, "EmptyPlanetariumOrb", 1);
			recipe.AddIngredient(ItemID.TitaniumBar, 20);
			recipe.AddIngredient(ItemID.AdamantiteBar, 20);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(null, "PlanetariumOrb", 1);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "GelBar", 20);
			recipe.AddIngredient(null, "SlimeyFeather", 16);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.SlimeStaff, 1);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "HiveFish", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ItemID.Hive, 50);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.Gel, 5);
			recipe.AddTile(TileID.Solidifier);
			recipe.SetResult(null, "GelBar", 2);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "JewelFish", 1);
			recipe.SetResult(ItemID.Amethyst, 10);
			recipe.AddRecipe();
			recipe.needLava = true;
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "JewelFish", 1);
			recipe.SetResult(ItemID.Topaz, 10);
			recipe.AddRecipe();
			recipe.needLava = true;
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "JewelFish", 1);
			recipe.SetResult(ItemID.Sapphire, 10);
			recipe.AddRecipe();
			recipe.needLava = true;
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "JewelFish", 1);
			recipe.SetResult(ItemID.Emerald, 10);
			recipe.AddRecipe();
			recipe.needLava = true;
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "JewelFish", 1);
			recipe.SetResult(ItemID.Ruby, 10);
			recipe.AddRecipe();
			recipe.needLava = true;
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "JewelFish", 1);
			recipe.SetResult(ItemID.Diamond, 10);
			recipe.AddRecipe();
			recipe.needLava = true;
			
			//just in case temple gets cucked
			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.TempleKey, 3);
			recipe.AddIngredient(1293, 1); //power cell
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(1292, 1); //altar
			recipe.AddRecipe();
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.TempleKey, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(1293, 1); //power cell
			recipe.AddRecipe();
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(1101, 40); //lizahrd brick
			recipe.AddIngredient(ItemID.FallenStar, 5); //lizahrd brick
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(1293, 1);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.TempleKey, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(1101, 50);
			recipe.AddRecipe();
			
			
		}
		public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer != -1 && !Main.gameMenu)
            {
                if(Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].GetModPlayer<SOTSPlayer>(this).PlanetariumBiome) //this makes the music play only in Custom Biome
                {
                    music = this.GetSoundSlot(SoundType.Music, "Sounds/Music/JourneyFromJar");  //add where is the custom music is located
					priority = MusicPriority.BossLow;
				
                } 
            }
			if (Main.myPlayer != -1 && !Main.gameMenu)
            {
                if(Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].GetModPlayer<SOTSPlayer>(this).GeodeBiome) 
                {
                    music = this.GetSoundSlot(SoundType.Music, "Sounds/Music/GeodeMusic");  //add where is the custom music is located
					priority = MusicPriority.BossLow;
				
                } 
            }
			if (Main.myPlayer != -1 && !Main.gameMenu)
            {
                if(Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].GetModPlayer<SOTSPlayer>(this).PyramidBiome)
                {
                    music = MusicID.Desert;
					priority = MusicPriority.BossLow;
                } 
            }
        }
		public override void PostSetupContent()
        {
            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if(bossChecklist != null)
            {
                // AddBoss, bossname, order or value in terms of vanilla bosses, inline method for retrieving downed value.
                //bossChecklist.Call(....
                // To include a description:
                //bossChecklist.Call("AddBoss", "Putrid Pinky", 4.2f, (Func<bool>)(() => SOTSWorld.downedPinky));
                bossChecklist.Call("AddBossWithInfo", "Putrid Pinky", 4.2f, (Func<bool>)(() => SOTSWorld.downedPinky), "Use [i:" + ItemType("JarOfPeanuts") + "]");
                bossChecklist.Call("AddBossWithInfo", "Pharaoh's Curse", 4.3f, (Func<bool>)(() => SOTSWorld.downedCurse), "Find the [i:" + ItemType("Sarcophagus") + "] in the pyramid");
                bossChecklist.Call("AddBossWithInfo", "Cryptic Carver", 5.2f, (Func<bool>)(() => SOTSWorld.downedCarver), "Use [i:" + ItemType("MargritArk") + "]");
                bossChecklist.Call("AddBossWithInfo", "Ethereal Entity", 6.5f, (Func<bool>)(() => SOTSWorld.downedEntity), "Use [i:" + ItemType("PlanetariumDiamond") + "] in a planetarium biome");
				
                bossChecklist.Call("AddBossWithInfo", "Antimaterial Antlion", 7.21f, (Func<bool>)(() => SOTSWorld.downedAntilion), "Use [i:" + ItemType("ForbiddenPyramid") + "] in a desert biome");
                bossChecklist.Call("AddBossWithInfo", "Icy Amalgamation", 8.21f, (Func<bool>)(() => SOTSWorld.downedAmalgamation), "Use [i:" + ItemType("FrostedKey") + "] on a [i:" + ItemType("FrostArtifact") + "] in a snow biome");
                bossChecklist.Call("AddBossWithInfo", "The Queen and King", 11.51f, (Func<bool>)(() => SOTSWorld.downedChess), "Use [i:" + ItemType("CheckeredBall") + "] in an open area");
            }
        }
	}
}

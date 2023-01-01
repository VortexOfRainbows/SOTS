using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using SOTS.WorldgenHelpers;
using SOTS.Items.Earth.Glowmoth;

namespace SOTS.Items.Tools
{
	public class PyramidGenerator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyramid Generation Demonstrator");
			Tooltip.SetDefault("Development tool, NOT MEANT FOR GAMEPLAY\nUsing this may generate a whole pyramid on your cursor, step-by-step\nRight clicking will literally make your world vanish\nWhatever the case, it's probably best not to use it");
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 16;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.value = 0;
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item1;
		}
		public override void HoldItem(Player player)
		{
			player.rulerGrid = true;
		}
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        int StepNumber = 0;
		int GenSeed = 0;
		int warning = 0;
		public override bool? UseItem(Player player)
		{
			if(player.altFunctionUse == 2)
            {
				DeleteEverythingInWorld();
				return true;
            }
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			if (StepNumber % 13 == 0)
			{
				GenSeed = Main.rand.Next(1000000); //kind of ironic. I'm seeding the random generation using a number fetched by random generation :)
			}
			Main.NewText("Generating with seed: " + GenSeed);
			Main.NewText("Generated Pyramid Step " + StepNumber % 13);
			PyramidWorldgenHelper.GenerateSOTSPyramid(Mod, false, StepNumber % 13, (int)tileLocation.X, (int)tileLocation.Y, GenSeed);
			player.position.X += 11600; //move 16 pix * 800 tiles 
			StepNumber++;
			return true;
		}
		public void DeleteEverythingInWorld()
        {
			if(warning == 3)
            {
				Main.NewText("Here comes NOTHING!!!", Color.Red);
				for (int x = 0; x < Main.maxTilesX; x++)
				{
					for (int y = 0; y < Main.maxTilesY; y++)
					{
						Tile tile = Main.tile[x, y];
						tile.ClearEverything();
					}
				}
			}
			if(warning == 2)
			{
				Main.NewText("If anything goes wrong, it's because YOU asked for it", Color.Red);
			}
			if (warning == 1)
			{
				Main.NewText("Seriously. I'm warning you", Color.Red);
			}
			if (warning == 0)
			{
				Main.NewText("Don't right click this. Your world will be gone", Color.Red);
			}
			warning++;
        }
	}
}
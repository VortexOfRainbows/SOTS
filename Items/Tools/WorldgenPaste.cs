using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace SOTS.Items.Tools
{
	public class WorldgenPaste : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worldgen Paste");
			Tooltip.SetDefault("Development tool, NOT MEANT FOR GAMEPLAY");
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 34;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = ItemUseStyleID.Stabbing;
			item.value = 0;
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item1;
		}
		public override void HoldItem(Player player)
		{
			player.rulerGrid = true;
		}
		public void PasteStructure()
		{
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			SOTSWorldgenHelper.GenerateSkyArtifact((int)tileLocation.X, (int)tileLocation.Y, mod);
			for(int i = 0; i < 30; i ++)
			{
				tileLocation = mousePos / 16f;
				if (Main.rand.Next(2) == 0)
				{
					tileLocation.X += Main.rand.Next(300);
				}
				else
				{
					tileLocation.X -= Main.rand.Next(300);
				}

				if (Main.rand.Next(2) == 0)
				{
					tileLocation.Y += Main.rand.Next(50);
				}
				else
				{
					tileLocation.Y -= Main.rand.Next(36) + 50;
				}

				int extend = 0;
				while (!SOTSWorldgenHelper.GenerateArtifactIslands((int)tileLocation.X, (int)tileLocation.Y, i % 10, mod))
				{
					tileLocation = mousePos / 16f;
					if (Main.rand.Next(2) == 0)
					{
						tileLocation.X += Main.rand.Next(300 + extend);
					}
					else
					{
						tileLocation.X -= Main.rand.Next(300 + extend);
					}

					if (Main.rand.Next(2) == 0)
					{
						tileLocation.Y += Main.rand.Next(50);
					}
					else
					{
						tileLocation.Y -= Main.rand.Next(36) + 50;
					}

					extend++;
				}
			}
			SOTSWorldgenHelper.DistributeSkyThings(mod, 24, 7, 7, 4, 5, 45);
		}
		public void PasteCoconut()
		{
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			SOTSWorldgenHelper.GenerateCoconutIsland(mod, (int)tileLocation.X, (int)tileLocation.Y, Main.rand.NextBool(2) ? 1 : -1);
		}
		public void PasteStarter()
		{
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			SOTSWorldgenHelper.GenerateStarterHouse(mod, (int)tileLocation.X + 0, (int)tileLocation.Y, 9);
		}
		public void PasteCrystal()
		{
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			int xOff = Main.rand.Next(20) + 30;
			PyramidWorldgenHelper.GeneratePyramidPath(mod, (int)tileLocation.X, (int)tileLocation.Y, (int)tileLocation.X + xOff, (int)tileLocation.Y + Main.rand.Next(-30, 31));
			//SOTSWorldgenHelper.GeneratePyramidCrystalRoom(mod, (int)tileLocation.X, (int)tileLocation.Y);
		}
		public override bool UseItem(Player player)
		{
			//PasteCrystal();
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			int amount = Main.rand.Next(8, 32);
			float depthMult = amount / 16f;
			SOTSWorldgenHelper.GenerateVibrantGeode((int)tileLocation.X, (int)tileLocation.Y, amount, (int)(amount * Main.rand.NextFloat(0.9f, 1.1f)), depthMult, (float)Math.Sqrt(depthMult));
			return true;
		}
	}
}
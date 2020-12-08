using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class InfectionTester : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infection Tester");
			Tooltip.SetDefault("Creates a clump of infected pyramid brick\nCode within is also used for the Pyramid Worldgen (static method)");
		}
		public override void SetDefaults()
		{
            item.useTime = 15; 
            item.useAnimation = 15;
            item.useStyle = 5;    
            item.knockBack = 0;
            item.rare = 12;
            item.UseSound = SoundID.Item8;
            item.autoReuse = false;
            item.shoot =  10; 
            item.shootSpeed = 24;
			item.expert = true;

		}
		public static void Generate(Vector2 pos, Mod mod, bool item = false)
		{
			int[] sumRolls = new int[11];
			int[] sumRolls2 = new int[9];
			int i = (int)pos.X / 16;
			int j = (int)pos.Y / 16;
			int i2 = i;
			int amt = 1;
			if (item)
				amt = Main.rand.Next(2) + 2;
			for (int k = 0; k < amt; k ++)
			{
				sumRolls = new int[11];
				sumRolls2 = new int[9];
				Tile tileCenter = Framing.GetTileSafely(i, j - 3);
				if(tileCenter.type == mod.TileType("PyramidSlabTile") || item)
				{
					for (int k2 = 0; k2 < 30; k2++)
					{
						int rand = Main.rand.Next(5);
						int rand2 = Main.rand.Next(4);
						int rand3 = Main.rand.Next(4);
						int sum = rand + rand2 + rand3;
						if (sumRolls[sum] < 9)
						{
							sumRolls[sum]++;
						}
						else
							k2--;
					}
					for (int k2 = 0; k2 < sumRolls.Length; k2++)
					{
						for (int k3 = 0; k3 < sumRolls[k2] + 1; k3++)
						{
							Tile tile = Framing.GetTileSafely(i + k2 - 5, j - k3 - 3);
							if (Main.rand.Next(40) != 0 && (tile.type == mod.TileType("PyramidSlabTile") || !tile.active()))
							{
								if (item)
									WorldGen.PlaceTile(i + k2 - 5, j - k3 - 3, (ushort)mod.TileType("CursedHive"));
								tile.type = (ushort)mod.TileType("CursedHive");
								tile.active(true);
							}
						}
					}
					for (int k2 = 0; k2 < 20; k2++)
					{
						int rand = Main.rand.Next(5);
						int rand2 = Main.rand.Next(5);
						int sum = rand + rand2;
						if (sumRolls2[sum] < 3)
						{
							sumRolls2[sum]++;
						}
						else
							k2--;
					}
					for (int k2 = 0; k2 < sumRolls2.Length; k2++)
					{
						for (int k3 = 0; k3 < sumRolls2[k2]; k3++)
						{
							Tile tile = Framing.GetTileSafely(i + k2 - 4, j + k3 - 2);
							if (Main.rand.Next(20) != 0 && (tile.type == mod.TileType("PyramidSlabTile") || !tile.active()))
							{
								if (item)
									WorldGen.PlaceTile(i + k2 - 4, j + k3 - 2, (ushort)mod.TileType("CursedHive"));
								tile.type = (ushort)mod.TileType("CursedHive");
								tile.active(true);
							}
						}
					}
				}
				sumRolls = new int[11];
				sumRolls2 = new int[9];
				tileCenter = Framing.GetTileSafely(i, j + 3);
				if (tileCenter.type == mod.TileType("PyramidSlabTile") || item)
				{
					for (int k2 = 0; k2 < 30; k2++)
					{
						int rand = Main.rand.Next(5);
						int rand2 = Main.rand.Next(4);
						int rand3 = Main.rand.Next(4);
						int sum = rand + rand2 + rand3;
						if (sumRolls[sum] < 9)
						{
							sumRolls[sum]++;
						}
						else
							k2--;
					}
					for (int k2 = 0; k2 < sumRolls.Length; k2++)
					{
						for (int k3 = 0; k3 < sumRolls[k2] + 1; k3++)
						{
							Tile tile = Framing.GetTileSafely(i + k2 - 5, j + k3 + 3);
							if (Main.rand.Next(40) != 0 && (tile.type == mod.TileType("PyramidSlabTile") || !tile.active()))
							{
								if (item)
									WorldGen.PlaceTile(i + k2 - 5, j + k3 + 3, (ushort)mod.TileType("CursedHive"));
								tile.type = (ushort)mod.TileType("CursedHive");
								tile.active(true);
							}
						}
					}
					for (int k2 = 0; k2 < 20; k2++)
					{
						int rand = Main.rand.Next(5);
						int rand2 = Main.rand.Next(5);
						int sum = rand + rand2;
						if (sumRolls2[sum] < 3)
						{
							sumRolls2[sum]++;
						}
						else
							k2--;
					}
					for (int k2 = 0; k2 < sumRolls2.Length; k2++)
					{
						for (int k3 = 0; k3 < sumRolls2[k2]; k3++)
						{
							Tile tile = Framing.GetTileSafely(i + k2 - 4, j - k3 + 2);
							if (Main.rand.Next(20) != 0 && (tile.type == mod.TileType("PyramidSlabTile") || !tile.active()))
							{
								if (item)
									WorldGen.PlaceTile(i + k2 - 4, j - k3 + 2, (ushort)mod.TileType("CursedHive"));
								tile.type = (ushort)mod.TileType("CursedHive");
								tile.active(true);
							}
						}
					}
				}
				i = Main.rand.Next(-30, 31) + i2;
			}
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			position = Main.MouseWorld;
			if (player.altFunctionUse == 2)
			{
				SOTSWorldgenHelper.GenerateAcediaRoom((int)position.X /16, (int)position.Y / 16, mod, Main.rand.Next(-1, 2));
				return false;
			}
			if (player.altFunctionUse == 2)
			{
				//Generate(position, mod, false);
				return false;
			}
			Generate(position, mod, true);
			return false;
		}
	}
}

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using SOTS.WorldgenHelpers;
using SOTS.Items.Earth.Glowmoth;
using Terraria.GameContent.Bestiary;
using Humanizer;
using SOTS.Items.Furniture;
using SOTS.Items.Planetarium.Furniture;
using SOTS.Items.Pyramid;
using SOTS.Items.Furniture.Goopwood;

namespace SOTS.Items.Tools
{
	public class LockingMechanism : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 30;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 0;
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}
		public override void HoldItem(Player player)
		{
			player.rulerGrid = true;
		}
		public override bool? UseItem(Player player)
		{
			int i = (int)Main.MouseWorld.X / 16;
            int j = (int)Main.MouseWorld.Y / 16;
			Tile tile = Main.tile[i, j];
			if(tile != null )
            {
                if (tile.TileFrameX % 36 != 0)
                {
                    i--;
                }
                if (tile.TileFrameY != 0)
                {
                    j--;
                }
                tile = Main.tile[i, j];
				if (tile != null)
				{
					bool isCorrectChest = tile.TileType == TileID.Containers && (tile.TileFrameX == 13 * 36 || tile.TileFrameX == 49 * 36); //Meteority and Skyware chests
					if(ModContent.GetModTile(tile.TileType) is ContainerType ct && tile.TileType != ModContent.TileType<PyramidChestTile>() && tile.TileType != ModContent.TileType<GoopwoodBarrelTile>())
					{
						if(!ct.IsLockedChest(i, j))
						{
							LockChest(i, j);
						}
					}
					else if(isCorrectChest)
                    {
						if(tile.TileFrameX == 13 * 36)
                            SwapChest(i, j, (ushort)ModContent.TileType<LockedSkywareChest>());
						if (tile.TileFrameX == 49 * 36)
							SwapChest(i, j, (ushort)ModContent.TileType<LockedMeteoriteChest>());
                        LockChest(i, j);
                    }
				}
            }
            return true;
		}
		public void SwapChest(int i, int j, ushort type)
        {
            for (int i2 = i; i2 <= i + 1; i2++)
            {
                for (int j2 = j; j2 <= j + 1; j2++)
                {
                    Tile residual = Main.tile[i2, j2];
					residual.TileType = type;
					residual.TileFrameX %= 36;
					residual.TileFrameY %= 36;
                }
            }
        }
		public void LockChest(int i, int j)
        {
            for (int i2 = i; i2 <= i + 1; i2++)
            {
                for (int j2 = j; j2 <= j + 1; j2++)
                {
                    Tile residual = Main.tile[i2, j2];
                    residual.TileFrameX += 36;
                }
            }
        }
	}
}
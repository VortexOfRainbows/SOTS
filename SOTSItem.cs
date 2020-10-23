using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using SOTS.Items.Otherworld.FromChests;
using System.Linq;
using System.Collections.Generic;

namespace SOTS
{
	public class SOTSItem : GlobalItem
	{
		public static int[] rarities1;
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			rarities1 = new int[] { mod.ItemType("StarlightAlloy"), mod.ItemType("HardlightAlloy"), mod.ItemType("OtherworldlyAlloy"), mod.ItemType("PotGenerator"), mod.ItemType("PrecariousCluster") };
			if (rarities1.Contains(item.type))
			{
				foreach (TooltipLine line2 in tooltips)
				{
					if (line2.mod == "Terraria" && line2.Name == "ItemName")
					{
						line2.overrideColor = new Color(0, 130, 235, 255);
					}
				}
			}
		}
		public Tile FindTATile(Player player)
        {
			int x = (int)player.Center.X / 16;
			int y = (int)player.Center.Y / 16;
			Vector2 between = new Vector2(100000, 0);
			Tile bestTile = (Tile)null;
			for (int i = -9; i < 10; i++)
            {
				for(int j = -9; j < 10; j++)
                {
					Tile tile = Framing.GetTileSafely(x + i, y + j);
					int type = 0;
					if (Main.tile[x + i, y + j].frameX >= 18 && Main.tile[x + i, y + j].frameX < 36 && Main.tile[x + i, y + j].frameY % 36 >= 18)
						type = 1;
					if (tile.type == mod.TileType("TransmutationAltarTile") && type == 1)
					{
						Vector2 newBetween = new Vector2(i * 16, j * 16);
						if (newBetween.Length() < between.Length())
						{
							between = newBetween;
							bestTile = tile;
						}
					}
				}
            }
			return bestTile;
		}
		public Point16 FindTATileIJ(Player player)
		{
			int x = (int)player.Center.X / 16;
			int y = (int)player.Center.Y / 16;
			Vector2 between = new Vector2(100000, 0);
			Point16 bestTile = new Point16(x, y);
			for (int i = -9; i < 10; i++)
			{
				for (int j = -9; j < 10; j++)
				{
					Tile tile = Framing.GetTileSafely(x + i, y + j);
					int type = 0;
					if (Main.tile[x + i, y + j].frameX >= 18 && Main.tile[x + i, y + j].frameX < 36 && Main.tile[x + i, y + j].frameY % 36 >= 18)
						type = 1;
					if (tile.type == mod.TileType("TransmutationAltarTile") && type == 1)
					{
						Vector2 newBetween = new Vector2(i * 16, j * 16);
						if (newBetween.Length() < between.Length())
						{
							between = newBetween;
							bestTile = new Point16(x + i, y + j);
						}
					}
				}
			}
			return bestTile;
		}
		public override void OnCraft(Item item, Recipe recipe)
		{
			Player player = Main.player[item.owner];
			if (recipe.requiredTile[0] == TileID.DemonAltar || recipe.requiredTile[0] == mod.ItemType("TransmutationAltarTile"))
            {
				Tile tile = FindTATile(player);
				if(tile != null)
				{
					Point16 ij = FindTATileIJ(player);
					int left = ij.X - tile.frameX / 18;
					int top = ij.Y - tile.frameY / 18;
					int index = GetInstance<TransmutationAltarStorage>().Find(left, top);
					if (index == -1)
					{
						return;
					}
					TransmutationAltarStorage entity = (TransmutationAltarStorage)TileEntity.ByID[index];
					entity.itemsArray[0] = item.type;
					entity.itemAmountsArray[0] = item.stack;
					entity.itemFrames[0] = Main.itemTexture[item.type].Height / recipe.requiredItem[0].height;
					int amountOfUniqueItems = 0;
					for (int l = 0; l < recipe.requiredItem.Length; l++)
					{
						if (recipe.requiredItem[l].type != 0)
						{
							amountOfUniqueItems++;
						}
						else
							break;
					}
					for (int i = 0; i < (amountOfUniqueItems < 20 ? amountOfUniqueItems : 19); i++)
                    {
						int itemType = recipe.requiredItem[i].type;
						int itemStack = recipe.requiredItem[i].stack;
						int itemFrames = Main.itemTexture[itemType].Height / recipe.requiredItem[i].height;
						entity.itemsArray[i + 1] = itemType;
						entity.itemAmountsArray[i + 1] = itemStack;
						entity.itemFrames[i + 1] = itemFrames;
					}
					entity.netUpdate = true;
				}
			}
		}
    }
}
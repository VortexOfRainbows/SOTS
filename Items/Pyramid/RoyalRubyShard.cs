using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.Pyramid
{
	public class RoyalRubyShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Royal Ruby Shard");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 26;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = ItemRarityID.LightRed;
			item.consumable = true;
			item.createTile = mod.TileType("RoyalRubyShardTile");
		}
	}
	public class RoyalRubyShardTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			drop = mod.ItemType("RoyalRubyShard");
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Royal Ruby Shard");
			AddMapEntry(new Color(211, 69, 74), name);
			soundType = 2;
			soundStyle = 27;
			dustType = 12;
		}
        public override bool CanExplode(int i, int j)
		{
			return true;
		}
		public override bool CanPlace(int i, int j)
		{
			if (Main.tile[i, j + 1].slope() == 0 && !Main.tile[i, j + 1].halfBrick())
			{
				return true;
			}
			if (Main.tile[i, j - 1].slope() == 0 && !Main.tile[i, j - 1].halfBrick())
			{
				return true;
			}
			if (Main.tile[i + 1, j].slope() == 0 && !Main.tile[i + 1, j].halfBrick())
			{
				return true;
			}
			if (Main.tile[i - 1, j].slope() == 0 && !Main.tile[i - 1, j].halfBrick())
			{
				return true;
			}
			return false;
		}
		public override void PlaceInWorld(int i, int j, Item item)
		{
			if (Main.tile[i, j + 1].active() && Main.tileSolid[Main.tile[i, j + 1].type] && Main.tile[i, j + 1].slope() == 0 && !Main.tile[i, j + 1].halfBrick())
			{
				Main.tile[i, j].frameY = 0;
			}
			else if (Main.tile[i, j - 1].active() && Main.tileSolid[Main.tile[i, j - 1].type] && Main.tile[i, j - 1].slope() == 0 && !Main.tile[i, j - 1].halfBrick())
			{
				Main.tile[i, j].frameY = 18;
			}
			else if (Main.tile[i + 1, j].active() && Main.tileSolid[Main.tile[i + 1, j].type] && Main.tile[i + 1, j].slope() == 0 && !Main.tile[i + 1, j].halfBrick())
			{
				Main.tile[i, j].frameY = 36;
			}
			else if (Main.tile[i - 1, j].active() && Main.tileSolid[Main.tile[i - 1, j].type] && Main.tile[i - 1, j].slope() == 0 && !Main.tile[i - 1, j].halfBrick())
			{
				Main.tile[i, j].frameY = 54;
			}
			Main.tile[i, j].frameX = (short)(WorldGen.genRand.Next(18) * 18);
		}
	}
}
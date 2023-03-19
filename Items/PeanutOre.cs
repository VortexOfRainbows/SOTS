using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Invidia;
using SOTS.Items.Permafrost;
using SOTS.Items.Slime;
using SOTS.Items.Void;
using SOTS.Projectiles.Permafrost;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class PeanutOreTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileSpelunker[Type] = true;
			Main.tileMerge[TileID.Dirt][Type] = true;
			Main.tileMerge[Type][TileID.Dirt] = true;
			MineResist = 1f;
			DustType = DustID.Dirt;
			ItemDrop = ModContent.ItemType<Peanut>();
			ModTranslation name = CreateMapEntryName();
			AddMapEntry(new Color(154, 78, 15), name);
			HitSound = SoundID.Dig;
		}
		public override bool CreateDust(int i, int j, ref int type)
		{
			if (Main.rand.NextBool(3))
				type = DustID.Grass;
			else if (Main.rand.NextBool(3))
				type = DustID.Obsidian;
			return true;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 11;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
			SOTSTile.DrawSlopedGlowMask(i, j, Type, Terraria.GameContent.TextureAssets.Tile[TileID.Dirt].Value, Lighting.GetColor(i, j), Vector2.Zero, false);
            return true;
        }
        public override bool Drop(int i, int j)
		{
			int peanutDropAmt = 1;
			int evostoneDropAmt = 1;
			if (Main.rand.NextBool(6))
				evostoneDropAmt++;
			else if (Main.rand.NextBool(6))
				peanutDropAmt++;
			if(!Main.rand.NextBool(200))
			{
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Peanut>(), peanutDropAmt);
			}
			else
			{
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<FoulConcoction>(), 1);
			}
			if (Main.rand.NextBool(100))
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Fragments.FragmentOfNature>(), 1);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Evostone>(), evostoneDropAmt);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.DirtBlock, 1);
			return false;
        }
    }
	/*public class PeanutOre : ModItem
	{
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
        public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 16;
			Item.height = 16;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 1, 0);
			Item.createTile = ModContent.TileType<PeanutOreTile>();
		}
	}*/
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.NPCs.Boss;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Earth
{
	public class BigCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Big");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 26;
			Item.height = 32;
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<BigCrystalTile>();
		}
	}	
	public class BigCrystalTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
			TileObjectData.newTile.Width = 13;
			TileObjectData.newTile.Height = 14;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 10, 2);
			TileObjectData.newTile.Origin = new Point16(7, 13);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Giant Crystal");
			AddMapEntry(new Color(237, 255, 193), name);
			MinPick = 250;
			DustType = ModContent.DustType<VibrantDust>();
			HitSound = SoundID.Item27;
			MineResist = 0.1f;
		}
        public override bool CanExplode(int i, int j)
        {
            return true;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return true;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 2;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int width = 6 * 16;
			int height = 7 * 16;
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16 + width, j * 16 + height, 16, 16, ModContent.ItemType<BigCrystal>());
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			/*Tile tile = Main.tile[i, j];
			int left = i;
			int top = j;
			if (tile.TileFrameX % 36 != 0)
			{
				left--;
			}
			if (tile.TileFrameY % 36 != 0)
			{
				top--;
			}*/
			r = 0.27f;
			g = 0.33f;
			b = 0.15f;
		}
	}
}
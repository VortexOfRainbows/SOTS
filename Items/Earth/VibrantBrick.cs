using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Invidia;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Otherworld.Furniture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using SOTS.Items.Pyramid;

namespace SOTS.Items.Earth
{
	public class VibrantBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<VibrantBrickTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(ModContent.ItemType<VibrantOre>(), 1).AddIngredient(ItemID.StoneBlock, 1).AddTile(TileID.Furnaces).Register();
		}
	}
	public class VibrantBrickTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileShine[Type] = 1200;
			Main.tileShine2[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<VibrantBrick>();
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(181, 220, 97), name);
			MineResist = 1.0f;
			HitSound = SoundID.Tink;
			DustType = ModContent.DustType<VibrantDust>();
		}
		public bool canGlow(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int frameX = tile.TileFrameX / 18;
			int frameY = tile.TileFrameY / 18;
			if (frameX >= 1 && frameX <= 3 && (frameY == 1))
				return true;
			if (frameX >= 6 && frameX <= 8 && (frameY == 1 || frameY == 2 || frameY == 11))
				return true;
			if ((frameX == 10 || frameX == 11) && frameY >= 0 && frameY <= 2)
				return true;
			if (frameY >= 5 && frameY <= 10 && (frameX <= 3 || (frameX >= 8 && frameX <= 12)))
				return true;
			return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (canGlow(i, j))
			{
				r = 0.27f;
				g = 0.33f;
				b = 0.15f;
			}
			else
			{
				r = 0;
				g = 0;
				b = 0;
			}
		}
		public override bool KillSound(int i, int j, bool fail)
		{
			Vector2 pos = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
			int type = Main.rand.Next(3) + 1;
			SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/VibrantOre" + type), (int)pos.X, (int)pos.Y, 1.8f, Main.rand.NextFloat(0.3f, 0.4f));
			return false;
		}
	}
}
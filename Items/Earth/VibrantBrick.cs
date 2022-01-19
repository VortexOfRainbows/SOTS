using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Nvidia;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Otherworld.Furniture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Items.Pyramid;

namespace SOTS.Items.Earth
{
	public class VibrantBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Brick");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<VibrantBrickTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantOre>(), 1);
			recipe.AddIngredient(ItemID.StoneBlock, 1);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
	public class VibrantBrickTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileShine[Type] = 1200;
			Main.tileShine2[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			drop = ModContent.ItemType<VibrantBrick>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Vibrant Brick");
			AddMapEntry(new Color(181, 220, 97), name);
			mineResist = 1.0f;
			soundType = SoundID.Tink;
			soundStyle = 2;
			dustType = ModContent.DustType<VibrantDust>();
		}
		public override bool KillSound(int i, int j)
		{
			Vector2 pos = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
			int type = Main.rand.Next(3) + 1;
			Main.PlaySound(SoundLoader.customSoundType, (int)pos.X, (int)pos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/VibrantOre" + type), 1.8f, Main.rand.NextFloat(0.3f, 0.4f));
			return false;
		}
	}
}
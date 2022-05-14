using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Otherworld.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.Blocks
{
	public class DullPlating : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<DullPlatingTile>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DullPlatingWall>(), 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AvaritianPlating>(), 1);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TwilightGel>(), 5);
			recipe.AddIngredient(ModContent.ItemType<TwilightShard>(), 1);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this, 10);
			recipe.AddRecipe();
		}
	}
	public class DullPlatingTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<DullPlating>();
			AddMapEntry(new Color(30, 30, 30));
			MineResist = 2f;
			MinPick = 60;
			soundType = 21;
			soundStyle = 2;
			DustType = ModContent.DustType<AvaritianDust>();
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override bool Slope(int i, int j)
		{
			return true;
		}
	}
}
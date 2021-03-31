using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld
{
	public class AvaritianPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avaritian Plating");
			Tooltip.SetDefault("'It bares striking resemblance to luminite'");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = ItemRarityID.Cyan;
			item.consumable = true;
			item.createTile = mod.TileType("AvaritianPlatingTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DullPlatingWall", 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DullPlating", 1);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TwilightGel", 5);
			recipe.AddIngredient(null, "TwilightShard", 1);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}
	public class AvaritianPlatingTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("AvaritianPlating");
			AddMapEntry(new Color(32, 90, 85));
			mineResist = 2f;
			minPick = 80;
			soundType = 21;
			soundStyle = 2;
			dustType = mod.DustType("AvaritianDust");
		}
		public override bool CanExplode(int i, int j)
		{
			if (Main.tile[i, j].type == mod.TileType("AvaritianPlatingTile"))
			{
				return false;
			}
			return false;
		}
		public override bool Slope(int i, int j)
		{
			return true;
		}
	}
}
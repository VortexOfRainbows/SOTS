using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Otherworld
{
	public class DullPlatingWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dull Plating Wall");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 7;
			item.useStyle = 1;
			item.rare = 9;
			item.consumable = true;
			item.createWall = mod.WallType("SafeDullPlatingWallWall");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DullPlating", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
		}
	}
	public class SafeDullPlatingWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = mod.DustType("AvaritianDust");
			drop = mod.ItemType("DullPlatingWall");
			AddMapEntry(new Color(50, 50, 50));
		}
	}
	public class DullPlatingWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = false;
			dustType = mod.DustType("AvaritianDust");
			drop = mod.ItemType("DullPlatingWall");
			AddMapEntry(new Color(50, 50, 50));
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}
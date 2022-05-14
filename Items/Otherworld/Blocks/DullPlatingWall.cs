using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Dusts;

namespace SOTS.Items.Otherworld.Blocks
{
	public class DullPlatingWall : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.rare = ItemRarityID.LightRed;
			Item.createWall = ModContent.WallType<SafeDullPlatingWallWall>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DullPlating>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
		}
	}
	public class SafeDullPlatingWallWall : ModWall
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SOTS/Items/Otherworld/Blocks/DullPlatingWallWall";
			return base.Autoload(ref name, ref texture);
		}
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = ModContent.DustType<AvaritianDust>();
			drop = ModContent.ItemType<DullPlatingWall>();
			AddMapEntry(new Color(44, 44, 44));
		}
	}
	public class DullPlatingWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = false;
			DustType = ModContent.DustType<AvaritianDust>();
			drop = ModContent.ItemType<DullPlatingWall>();
			AddMapEntry(new Color(44, 44, 44));
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}
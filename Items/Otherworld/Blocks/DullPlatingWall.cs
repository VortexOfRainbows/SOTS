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
			CreateRecipe(4).AddIngredient(ModContent.ItemType<DullPlating>(), 1).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class SafeDullPlatingWallWall : ModWall
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SOTS/Items/Otherworld/Blocks/DullPlatingWallWall";
			return base.Autoload(ref name, ref texture);
		}
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = ModContent.DustType<AvaritianDust>();
			drop = ModContent.ItemType<DullPlatingWall>();
			AddMapEntry(new Color(44, 44, 44));
		}
	}
	public class DullPlatingWallWall : ModWall
	{
		public override void SetStaticDefaults()
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
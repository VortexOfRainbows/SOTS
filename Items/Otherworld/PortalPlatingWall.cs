using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Otherworld
{
	public class PortalPlatingWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avaritian Plating Wall");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 14;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 9;
			item.consumable = true;
			item.createWall = mod.WallType("AvaritianPlatingWallWall");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AvaritianPlating", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
		}
	}
	public class AvaritianPlatingWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallLargeFrames[Type] = (byte)2;
			Main.wallHouse[Type] = true;
			dustType = mod.DustType("AvaritianDust");
			drop = mod.ItemType("PortalPlatingWall");
			AddMapEntry(new Color(52, 150, 140));
		}
	}
	public class PortalPlatingWallWall : ModWall
	{

		public override void SetDefaults()
		{
			Main.wallLargeFrames[Type] = (byte)2;
			Main.wallHouse[Type] = false;
			dustType = mod.DustType("AvaritianDust");
			drop = mod.ItemType("PortalPlatingWall");
			AddMapEntry(new Color(52, 150, 140));
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void KillWall(int i, int j, ref bool fail)
		{
			fail = true;

			//if (SOTSWorld.downedEntity)
				fail = false;
		}
	}
}
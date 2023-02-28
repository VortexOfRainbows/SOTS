using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid.PyramidWalls
{
	public class OvergrownPyramidWall : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(400);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<OvergrownPyramidWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<OvergrownPyramidBlock>(), 1).AddTile(TileID.WorkBenches).Register();
			Recipe.Create(ModContent.ItemType<OvergrownPyramidBlock>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class UnsafeOvergrownPyramidWall : ModItem
	{
		public override string Texture => "SOTS/Items/Pyramid/PyramidWalls/OvergrownPyramidWall";
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(400);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Red;
			Item.createWall = ModContent.WallType<UnsafeOvergrownPyramidWallWall>();
		}
	}
	public class OvergrownPyramidWallWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = DustID.Grass;
			ItemDrop = ModContent.ItemType<OvergrownPyramidWall>();
			AddMapEntry(new Color(18, 82, 36));
		}
	}
	public class UnsafeOvergrownPyramidWallWall : ModWall
	{
        public override string Texture => "SOTS/Items/Pyramid/PyramidWalls/OvergrownPyramidWallWall";
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			DustType = DustID.Grass;
			ItemDrop = ModContent.ItemType<OvergrownPyramidWall>();
			AddMapEntry(new Color(18, 82, 36));
		}
		public override void KillWall(int i, int j, ref bool fail)
		{
			fail = true;
			if (SOTSWorld.downedCurse && (NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3))
				fail = false;
		}
	}
}
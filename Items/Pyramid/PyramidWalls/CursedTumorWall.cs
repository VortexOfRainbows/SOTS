using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Dusts;

namespace SOTS.Items.Pyramid.PyramidWalls
{
	public class CursedTumorWall : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(400);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<CursedTumorWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<CursedTumor>(), 1).AddTile(TileID.WorkBenches).Register();
			Recipe.Create(ModContent.ItemType<CursedTumor>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class UnsafeCursedTumorWall : ModItem
	{
        public override string Texture => "SOTS/Items/Pyramid/PyramidWalls/CursedTumorWall";
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
			Item.createWall = ModContent.WallType<UnsafeCursedTumorWallWall>();
		}
	}
	public class CursedTumorWallWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = ModContent.DustType<CurseDust3>();
			////ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<CursedTumorWall>();
			HitSound = SoundID.NPCHit1;
			AddMapEntry(new Color(49, 33, 75));
		}
	}
	public class UnsafeCursedTumorWallWall : ModWall
	{
        public override string Texture => "SOTS/Items/Pyramid/PyramidWalls/CursedTumorWallWall"; 
        public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			DustType = ModContent.DustType<CurseDust3>();
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<CursedTumorWall>();
			HitSound = SoundID.NPCHit1;
			AddMapEntry(new Color(49, 33, 75));
		}
	}
}
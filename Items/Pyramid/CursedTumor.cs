using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.Pyramid
{
	public class CursedTumor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Tumor Block");
			Tooltip.SetDefault("'Baked beans'");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = ItemRarityID.Pink;
			item.consumable = true;
			item.createTile = mod.TileType("CursedTumorTile");
		}
	}
	public class CursedTumorTile : ModTile
	{
        public override void SetDefaults()
		{
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("CursedTumor");
			AddMapEntry(new Color(70, 60, 110));
			mineResist = 1.5f;
			soundType = SoundID.NPCHit;
			soundStyle = 1;
			dustType = mod.DustType("CurseDust3");
		}
        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
			dustType = this.dustType;
			makeDust = true;
            base.WalkDust(ref dustType, ref makeDust, ref color);
        }
        public override bool HasWalkDust()
        {
            return true;
        }
        public override bool CanExplode(int i, int j)
		{
			return true;
		}
		public override bool Slope(int i, int j)
		{
			return true;
		}
	}
	public class CursedTumorWallTile : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = false;
			dustType = mod.DustType("CurseDust3");
			drop = mod.ItemType("CursedTumorWall");
			soundType = SoundID.NPCHit;
			soundStyle = 1;
			AddMapEntry(new Color(50, 45, 90));
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
	public class CursedTumorWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unsafe Cursed Tumor Wall");
			Tooltip.SetDefault("Changes the biome to pyramid when in front of\nAlso envokes the Pharaoh's Curse");
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 7;
			item.useStyle = 1;
			item.rare = 5;
			item.consumable = true;
			item.createWall = mod.WallType("CursedTumorWallTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CursedTumor>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ModContent.ItemType<CursedTumor>(), 1);
			recipe.AddRecipe();
		}
	}
}
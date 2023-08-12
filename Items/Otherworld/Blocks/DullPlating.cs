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
		public override void SetStaticDefaults() => this.SetResearchCost(100);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<DullPlatingTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DullPlatingWall>(), 4).AddTile(TileID.WorkBenches).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<AvaritianPlating>(), 1).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
			CreateRecipe(10).AddIngredient(ModContent.ItemType<TwilightGel>(), 5).AddIngredient(ModContent.ItemType<TwilightShard>(), 1).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
	public class DullPlatingTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<DullPlating>();
			AddMapEntry(new Color(30, 30, 30));
			MineResist = 2f;
			MinPick = 60;
			HitSound = SoundID.Tink;
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
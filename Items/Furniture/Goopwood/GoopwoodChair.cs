using Microsoft.Xna.Framework;
using SOTS.Items.Slime;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.Goopwood
{
	public class GoopwoodChair : ModItem
	{
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.Size = new Vector2(16, 32);
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<GoopwoodChairTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Wormwood>(), 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}	
	public class GoopwoodChairTile : Chair<GoopwoodChair>
	{
		protected override void SetDefaults(TileObjectData t)
		{
			Main.tileLighted[Type] = true;
			base.SetDefaults(t);
		}
	}
}
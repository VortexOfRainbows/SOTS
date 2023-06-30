using Microsoft.Xna.Framework;
using SOTS.Items.Earth.Glowmoth;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class GlowJelly : ModItem
	{
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 30;
			Item.height = 46;   
            Item.value = Item.sellPrice(0, 2, 10, 0);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.additionalHeal += 40;
			modPlayer.additionalPotionMana += 40;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<RoyalJelly>(1).AddIngredient<GlowSpores>(1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}
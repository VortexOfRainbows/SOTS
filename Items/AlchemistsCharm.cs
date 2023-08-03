using Microsoft.Xna.Framework;
using SOTS.Items.Earth.Glowmoth;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class AlchemistsCharm : ModItem
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
            Item.width = 38;
			Item.height = 60;   
            Item.value = Item.sellPrice(0, 2, 10, 0);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.additionalHeal += 100;
			modPlayer.additionalPotionMana += 100;
			modPlayer.PotionBuffDegradeRate -= 0.4f;
			player.pStone = true;
			player.lifeRegen += 2;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.CharmofMyths).AddIngredient<Temple.LihzahrdTail>(1).AddIngredient<GlowJelly>(1).AddIngredient(ItemID.Ectoplasm, 10).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}
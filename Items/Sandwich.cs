using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{	
	public class Sandwich : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandwich");
			Tooltip.SetDefault("Increases healing recieved from potions by 40\nKilling enemies will drop baguette crumbs\nSummons a pet Putrid Pinky to assist in combat\nLatches onto enemies, slowing them down and draining life\nIncreases life regeneration by 1");
		}
		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.DamageType = DamageClass.Summon;
            Item.width = 40;     
            Item.height = 34;   
            Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.lifeRegen += 1;
			modPlayer.petPinky += SOTSPlayer.ApplyDamageClassModWithGeneric(player, Item.DamageType, Item.damage);
			modPlayer.baguetteDrops = true;
			modPlayer.additionalHeal += 40;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<Baguette>(1).AddIngredient<RoyalJelly>(1).AddIngredient<PeanutButter>(1).AddTile(TileID.CookingPots).Register();
		}
	}
}
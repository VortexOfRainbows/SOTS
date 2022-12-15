using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class RockCandy : ModItem
	{	
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases pickaxe power by 5\nIncreases mining speed, melee speed, and movement speed by 5%");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 40;     
            Item.height = 38;   
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
			player.moveSpeed += 0.05f;
			player.pickSpeed += 0.05f;
			modPlayer.bonusPickaxePower += 5;
		}
	}
}
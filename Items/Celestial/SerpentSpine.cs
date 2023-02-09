using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using SOTS.Buffs;

namespace SOTS.Items.Celestial
{
	public class SerpentSpine : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serpent Spine");
			Tooltip.SetDefault("Increases melee weapon size by 40% and whip length by 40%\nIncreases melee speed by 10%\nMelee and whip attacks may scorch enemies, increasing damage dealt by 3% permanently");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 44;     
            Item.height = 48;   
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
			Item.expert = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer.ModPlayer(player).meleeItemScale += 0.40f;
			SOTSPlayer.ModPlayer(player).SerpentSpine = true;
			player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
			player.whipRangeMultiplier += 0.4f;
		}
	}
}
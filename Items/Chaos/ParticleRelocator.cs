using SOTS.Items.Fragments;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chaos
{
	public class ParticleRelocator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Particle Relocator");
			Tooltip.SetDefault("When an enemy hits you, you will be teleported behind them, avoiding damage and stunning the enemy temporarily\n" +
				"20 second cooldown, but increases melee damage by 50% for 7 seconds after teleporting");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 4));
		}
		public override void SetDefaults()
		{
            item.width = 28;     
            item.height = 28;   
            item.value = Item.sellPrice(gold: 7);
            item.rare = ItemRarityID.Pink;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{

		}
	}
}
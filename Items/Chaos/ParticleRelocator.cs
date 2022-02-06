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
			Tooltip.SetDefault("When an enemy hits you, you will be teleported behind them, avoiding damage and stunning them temporarily\n" +
				"16 second cooldown, but increases melee damage by 50% for 7 seconds after teleporting\nDoes damage equal to your defense");
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
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.ParticleRelocator = true;
		}
	}
}
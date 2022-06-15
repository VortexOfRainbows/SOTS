using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Items.Fragments;

namespace SOTS.Items.OreItems
{
	public class GoldBattery : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Battery");
			Tooltip.SetDefault("Increases void gain by 1 and max void by 20\nRegenerate void when hit, but also have a 10% chance to recieve ichor when near max void");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 18;     
            Item.height = 34;   
            Item.value = Item.sellPrice(0, 0, 45, 0);
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.GoldBar, 10).AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 1).AddTile(TileID.Anvils).Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.bonusVoidGain += 1;
			voidPlayer.voidMeterMax2 += 20;

			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (modPlayer.onhit == 1)
			{
				voidPlayer.voidMeter += 2 + (modPlayer.onhitdamage / 11);
				VoidPlayer.VoidEffect(player, 2 + (modPlayer.onhitdamage / 11));
				if(voidPlayer.voidMeter >= voidPlayer.voidMeterMax2 - 5 && Main.rand.Next(10) == 0)
				{
					int minute = 3600;
					player.AddBuff(BuffID.Ichor, (int)(minute * 2.5f), false);
				}
			}
		}
	}
}
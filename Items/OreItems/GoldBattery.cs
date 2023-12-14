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
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.bonusVoidGain += 1;
			voidPlayer.voidMeterMax2 += 20;
			voidPlayer.GainVoidOnHurt += 0.1f;
			if (modPlayer.onhit == 1)
			{
				if(voidPlayer.voidMeter >= voidPlayer.voidMeterMax2 && Main.rand.NextBool(10))
				{
					int minute = 3600;
					player.AddBuff(BuffID.Ichor, (int)(minute * 2.5f), false);
				}
			}
		}
	}
}
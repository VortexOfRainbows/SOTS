using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium.EpicWings
{
	public class TwilightGyroscope : ModItem	
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 34;     
            Item.height = 34;   
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
			Item.expert = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			TestWingsPlayer testWingsPlayer = player.GetModPlayer<TestWingsPlayer>();
			testWingsPlayer.gyro = true;
			player.jumpSpeedBoost += 3f;
			player.noFallDmg = true;
			player.moveSpeed += 0.2f;
		}
	}
}
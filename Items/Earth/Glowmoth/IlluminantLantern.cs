using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Earth.Glowmoth
{
	public class IlluminantLantern : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 42;     
            Item.height = 46;   
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
			Item.expert = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if(player.whoAmI == Main.myPlayer)
            {
				SOTSWorld.lightingChange = 1.0525f;
				if (player.HasBuff(BuffID.NightOwl))
					SOTSWorld.lightingChange = 1.038f;
            }
			player.GetDamage<VoidGeneric>() += 0.05f;
		}
	}
}
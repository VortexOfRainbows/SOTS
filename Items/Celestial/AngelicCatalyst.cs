using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Celestial
{
	public class AngelicCatalyst : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Angelic Catalyst");
			Tooltip.SetDefault("Provides access to an infinite supply of celestial arrows\nDecreases void regen by 2.25 while in the inventory");
		}public override void SetDefaults()
		{
			item.damage = 3;
			item.ranged = true;
			item.width = 34;
			item.height = 40;
			item.maxStack = 1;
			item.consumable = false; 
			item.knockBack = 0.2f;
            item.value = Item.sellPrice(0, 2, 25, 0);
			item.rare = 8;
			item.shoot = mod.ProjectileType("CelestialArrow");  
			item.shootSpeed = 0.3f;                 
			item.ammo = AmmoID.Arrow;   
			item.expert = true;
		}
		public override void UpdateInventory(Player player)
		{
				VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
				SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
				
				voidPlayer.voidRegen -= 0.225f;
		}
		public override bool ConsumeAmmo(Player p)
		{
				return false;
		}
	}
}
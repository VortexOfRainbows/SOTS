using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Celestial
{
	public class AngelicCatalyst : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Angelic Catalyst");
			Tooltip.SetDefault("Provides access to an infinite supply of celestial arrows");
		}public override void SafeSetDefaults()
		{
			item.damage = 1;
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
		public override bool BeforeConsumeAmmo(Player p)
		{
			return false;
		}
	}
}
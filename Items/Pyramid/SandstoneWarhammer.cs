using Terraria;
using Terraria.ID;
using SOTS.Void;

namespace SOTS.Items.Pyramid
{
	public class SandstoneWarhammer : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandstone Warhammer");
			Tooltip.SetDefault("Launches homing hammers");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 23;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 18;
			item.useAnimation = 18;
			item.useStyle = 1;
			item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.noMelee = false;
			item.shoot = mod.ProjectileType("Bloodaxe");  
            item.shootSpeed = 13.5f;
		}
		public override int GetVoid(Player player)
		{
			return  6;
		}
	}
}
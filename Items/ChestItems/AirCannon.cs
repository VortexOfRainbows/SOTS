using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{
	public class AirCannon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Air Cannon");
			Tooltip.SetDefault("Fires an extremely strong gush of wind");
		}
		public override void SetDefaults()
		{
            item.damage = 5;  	
			item.ranged = true;
            item.width = 48;    	
            item.height = 32;  	
            item.useTime = 35; 	
            item.useAnimation = 35;
            item.useStyle = 5;    
            item.noMelee = false;
			item.knockBack = 1f;  
            item.value = 52500;
            item.rare = 3;
            item.UseSound = SoundID.Item65;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("CarryAir"); 
            item.shootSpeed = 3.75f;
		}
	}
}

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{
	public class LavaPelter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Pelter");
			Tooltip.SetDefault("Fires out pelts of defense ingnoring balls of lava");
		}
		public override void SetDefaults()
		{
            item.damage = 21;	
            item.width = 34; 	
            item.height = 32; 	
            item.useTime = 13; 	
            item.useAnimation = 39;
            item.useStyle = 5;    
            item.noMelee = false;
			item.knockBack = 1f;  
            item.value = 72500;
            item.rare = 5;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("LavaBall"); 
            item.shootSpeed = 12.65f;
			item.reuseDelay = 15;
		}
	}
}

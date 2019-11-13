using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{
	public class Grenadier : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grenadier");
			Tooltip.SetDefault("'The ultimate weapon'");
		}
		public override void SetDefaults()
		{
            item.damage = 8;
            item.ranged = true;  
            item.width = 40;    
            item.height = 22; 
            item.useTime = 72;
            item.useAnimation = 72;
            item.useStyle = 5;    
            item.noMelee = true;
			item.knockBack = 1f;  
            item.value = 62500;
            item.rare = 6;
            item.UseSound = SoundID.Item61;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("GrenadierBolt"); 
            item.shootSpeed = 11.5f;
		}
	}
}

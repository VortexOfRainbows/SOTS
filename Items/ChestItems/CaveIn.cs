using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{
	public class CaveIn : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cave In");
			Tooltip.SetDefault("Collapses the ceilings of caves");
		}
		public override void SetDefaults()
		{
            item.damage = 0;  //gun damag
            item.width = 40;     //gun image width
            item.height = 30;   //gun image  height
            item.useTime = 25;  //how fast 
            item.useAnimation = 25;
            item.useStyle = 5;    
            item.noMelee = false;
			item.knockBack = 1f;  
            item.value = 52500;
            item.rare = 5;
            item.UseSound = SoundID.Item61;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("CaveIn"); 
            item.shootSpeed = 13.75f;
		}
	}
}

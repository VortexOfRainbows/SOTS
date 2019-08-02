using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SoldStuff
{
	public class DartBlaster : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nerf Blaster");
			Tooltip.SetDefault("Nerfs your enemies hp");
		}
		public override void SetDefaults()
		{
            item.damage = 9;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 36;     //gun image width
            item.height = 24;   //gun image  height
            item.useTime = 4;  //how fast 
            item.useAnimation = 20;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 25000;
            item.rare = 2;
            item.UseSound = SoundID.Item5;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("Dart"); 
            item.shootSpeed = 18;
			item.reuseDelay = 24;

		}

	}
}

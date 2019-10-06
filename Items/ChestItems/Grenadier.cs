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
			Tooltip.SetDefault("'Born in trashfire; made of crap; developed by an idiot'\n'The ultimate weapon'");
		}
		public override void SetDefaults()
		{
            item.damage = 9;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 40;     //gun image width
            item.height = 22;   //gun image  height
            item.useTime = 64;  //how fast 
            item.useAnimation = 64;
            item.useStyle = 5;    
            item.noMelee = false;
			item.knockBack = 1f;  
            item.value = 62500;
            item.rare = 6;
            item.UseSound = SoundID.Item61;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("GrenadierBolt"); 
            item.shootSpeed = 9;
		}
	}
}

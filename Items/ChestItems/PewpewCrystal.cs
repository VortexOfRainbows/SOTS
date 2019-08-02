using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.ChestItems
{
	public class PewpewCrystal : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pewpew Crystal");
			Tooltip.SetDefault("A weirdly shaped life crystal\nYields 8 health upon killing an enemy");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 9;  	
            item.magic = true;  	
            item.width = 34;	
            item.height = 24;  	
            item.useTime = 17; 	 
            item.useAnimation = 17;
            item.useStyle = 5;    
            item.noMelee = false;
			item.knockBack = 1f;  
            item.value = 52500;
            item.rare = 3;
            item.UseSound = SoundID.Item72;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("Heartdrop"); 
            item.shootSpeed = 4.65f;
		}
		public override void GetVoid(Player player)
		{
				voidMana = 2;
		}
	}
}

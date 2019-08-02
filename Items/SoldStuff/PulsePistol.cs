using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SoldStuff
{
	public class PulsePistol : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pulse Pistol");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.damage = 27;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 36;     //gun image width
            item.height = 26;   //gun image  height
            item.useTime = 16;  //how fast 
            item.useAnimation = 16;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 3f;
            item.value = 150000;
            item.rare = 3;
            item.UseSound = SoundID.Item9;
            item.autoReuse = true;
            item.shoot = 357; 
            item.shootSpeed = 18;
			item.reuseDelay = 24;

		}

	}
}

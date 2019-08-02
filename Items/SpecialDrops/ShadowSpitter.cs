using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class ShadowSpitter: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Splicer");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.damage = 32;  //gun damage
            item.melee = true;   //its a gun so set this to true
            item.width = 54;     //gun image width
            item.height = 54;   //gun image  height
            item.useTime = 33;  //how fast 
            item.useAnimation = 12;
            item.useStyle = 1;    
            item.knockBack = 4;
            item.value = 10000;
            item.rare = 11;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = 585; 
            item.shootSpeed = 27;

		}
	}
}

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class MinersSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Miner's Sword");
		}
		public override void SetDefaults()
		{
            item.damage = 20;  //gun damage
            item.melee = true;   //its a gun so set this to true
            item.width = 32;     //gun image width
            item.height = 32;   //gun image  height
            item.useTime = 16;  //how fast 
            item.useAnimation = 16;
            item.useStyle = 1;    
            item.knockBack = 2;
            item.value = 10000;
            item.rare = 3;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("PikeProj"); 
            item.shootSpeed = 5;

		}
	}
}

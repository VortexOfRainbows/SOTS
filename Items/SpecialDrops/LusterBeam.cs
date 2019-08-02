using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class LusterBeam : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luster Purge");
			Tooltip.SetDefault("Fire a meteor into the skies");
		}
		public override void SetDefaults()
		{
            item.damage = 56;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 38;     //gun image width
            item.height = 38;   //gun image  height
            item.useTime = 68;  //how fast 
            item.useAnimation = 68;
            item.useStyle = 1;    
            item.knockBack = 8;
            item.value = 10000;
            item.rare = 9;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("DracoOrb"); 
            item.shootSpeed = 27;
			item.mana = 150;

		}
	}
}

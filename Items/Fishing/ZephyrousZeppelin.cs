using SOTS.Projectiles;
using Terraria;                  
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Fishing
{
    public class ZephyrousZeppelin : ModItem   
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zephyrous Zeppelin");
			Tooltip.SetDefault("Surrounded by a ring of razorwater that deals 75% damage");
		}
        public override void SetDefaults()
        {
            item.damage = 22;
            item.melee = true; 
            item.useTime = 25;  
            item.useAnimation = 25;   
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.channel = true;
            item.knockBack = 2f;
            item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.LightRed;
            item.autoReuse = false; 
            item.shoot = ModContent.ProjectileType<Zeppelin>(); 
            item.noUseGraphic = true; 
            item.noMelee = true;
            item.UseSound = SoundID.Item1; 
		}
    }
}
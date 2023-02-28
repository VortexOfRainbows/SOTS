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
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.DamageType = DamageClass.Melee; 
            Item.useTime = 25;  
            Item.useAnimation = 25;   
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.LightRed;
            Item.autoReuse = false; 
            Item.shoot = ModContent.ProjectileType<Zeppelin>(); 
            Item.noUseGraphic = true; 
            Item.noMelee = true;
            Item.UseSound = SoundID.Item1; 
		}
    }
}
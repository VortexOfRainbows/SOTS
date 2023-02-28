using SOTS.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tools
{
	public class MinersPickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
            this.SetResearchCost(99);
        }
		public override void SetDefaults()
		{
            Item.damage = 12; 
            Item.DamageType = DamageClass.Melee;  
            Item.width = 28;    
            Item.height = 28;   
            Item.useTime = 16; 
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;    
            Item.noMelee = true;
            Item.knockBack = 2.4f;
            Item.value = Item.sellPrice(0, 0, 2, 75);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot =  ModContent.ProjectileType<Projectiles.Earth.MinersPickaxe>(); 
            Item.shootSpeed = 13f;
			Item.consumable = true;
			Item.noUseGraphic = true;
			Item.maxStack = 999;
		}
	}
}

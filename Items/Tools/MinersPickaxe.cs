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
			DisplayName.SetDefault("Miner's Pickaxe");
			Tooltip.SetDefault("'Finally, throwing pickaxe'");
		}
		public override void SetDefaults()
		{
            item.damage = 12; 
            item.melee = true;  
            item.width = 28;    
            item.height = 28;   
            item.useTime = 16; 
            item.useAnimation = 16;
            item.useStyle = ItemUseStyleID.SwingThrow;    
            item.noMelee = true;
            item.knockBack = 2.4f;
            item.value = Item.sellPrice(0, 0, 2, 75);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot =  ModContent.ProjectileType<Projectiles.Earth.MinersPickaxe>(); 
            item.shootSpeed = 13f;
			item.consumable = true;
			item.noUseGraphic = true;
			item.maxStack = 999;
		}
	}
}

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
			Tooltip.SetDefault("Throw a wall breaking pickaxe");
		}
		public override void SetDefaults()
		{
            item.damage = 12; 
            item.melee = true;  
            item.width = 28;    
            item.height = 28;   
            item.useTime = 38; 
            item.useAnimation = 38;
            item.useStyle = ItemUseStyleID.SwingThrow;    
            item.noMelee = true;
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 0, 2, 75);
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.shoot =  ModContent.ProjectileType<Pick>(); 
            item.shootSpeed = 3.75f;
			item.consumable = true;
			item.noUseGraphic = true;
			item.maxStack = 99;
			item.crit = 6;
		}
	}
}

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.SpecialDrops
{
	public class SpiderCrusher : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spider Crusher");
			Tooltip.SetDefault("Charge to increase damage up to 500%");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 40;
            item.melee = true;  
            item.width = 44;
            item.height = 44;  
            item.useTime = 60; 
            item.useAnimation = 60;
            item.useStyle = 5;    
            item.knockBack = 0f;
            item.value = Item.sellPrice(0, 1, 80, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("SpiderCrusher"); 
            item.shootSpeed = 18f;
			item.channel = true;
            item.noUseGraphic = true; 
            item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			return player.ownedProjectileCounts[type] <= 0; 
		}
		public override void GetVoid(Player player)
		{
			voidMana = 4;
		}
	}
}

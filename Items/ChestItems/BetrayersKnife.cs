using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;
using System;
using SOTS.Projectiles.Blades;

namespace SOTS.Items.ChestItems
{
	public class BetrayersKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Betrayer's Knife");
            Tooltip.SetDefault("Bleeds and deals double damage on the first hit");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 21;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 14;
            Item.height = 54;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;		
            Item.knockBack = 3.6f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BetrayersSlash>(); 
            Item.shootSpeed = 16f;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 2 * Math.Sign(velocity.X), 1);
			return false;
		}
    }
}

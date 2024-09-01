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
	public class StarshardSaber : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 22;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 52;
            Item.height = 50;  
            Item.useTime = 24; 
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;		
            Item.knockBack = 1.5f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<StarshardSlash>(); 
            Item.shootSpeed = 16f;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int count = 4;
            if(player.altFunctionUse == 2)
            {
                count = -5;
            }
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, count * SOTSUtils.SignNoZero(velocity.X) * -player.gravDir, 1);
			return false;
		}
    }
}

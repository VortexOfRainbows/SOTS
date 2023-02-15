using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;
using System;
using SOTS.Projectiles.Blades;

namespace SOTS.Items.Evil
{
	public class Vertebraeker : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vertebraeker");
            Tooltip.SetDefault("Strike with a flurry of slashes, then toss a spinning blade for 120% damage");
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 21;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 60;
            Item.height = 64;  
            Item.useTime = 20; 
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;		
            Item.knockBack = 1.5f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<VertebraekerSlash>(); 
            Item.shootSpeed = 16f;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 20 * Math.Sign(velocity.X) * player.gravDir, Main.rand.NextFloat(0.98f, 1.02f));
			return false;
		}
        public override int GetVoid(Player player)
        {
            return 9;
        }
    }
}

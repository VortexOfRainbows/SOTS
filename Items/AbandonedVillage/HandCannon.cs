using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Items.Fragments;
using SOTS.Projectiles;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class HandCannon : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
        }
		public override void SetDefaults()
		{
            Item.damage = 14; 
            Item.DamageType = DamageClass.Ranged;  
            Item.width = 40;   
            Item.height = 28;
            Item.useTime = 20; 
            Item.useAnimation = 20;
            Item.reuseDelay = 20;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item36;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ProjectileID.ExplosiveBullet;
            Item.shootSpeed = 5f;
            Item.useAmmo = AmmoID.Bullet;
            Item.channel = true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-0.5f, -4);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position += HoldoutOffset().Value.RotatedBy(velocity.ToRotation()) * Math.Sign(velocity.X);
            if(type == ProjectileID.Bullet)
            {
                type = ProjectileID.ExplosiveBullet;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for(int i = -1; i <= 1; i++)
            {
                float damageMult = 0.5f;
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(15 * i));
                if (i == 0)
                {
                    perturbedSpeed *= 1.5f;
                    damageMult = 1;
                }
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, (int)(damage * damageMult + 0.5f), knockback, player.whoAmI);
            }
            return false;
        }
    }
}

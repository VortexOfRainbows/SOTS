using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Evil;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Evil
{
	public class DeathSpiral : ModItem //I must credit pyroknight for creating examplesolareruption. 
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.width = 54;
			Item.height = 54;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 36;
			Item.useTime = 36;
			Item.shootSpeed = 27f;
			Item.knockBack = 3.5f;
			Item.UseSound = SoundID.Item116;
			Item.shoot = ModContent.ProjectileType<DeathSpiralProj>();
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.LightPurple;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.autoReuse = true;
			Item.DamageType = DamageClass.Melee;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for(int direction = -1; direction <= 1; direction += 2)
				for (int i = 0; i < 4; i++)
				{
					Vector2 velocity2 = velocity * (0.9f - 0.125f * i);
					Vector2 velocity3 = velocity.RotatedBy(MathHelper.ToRadians(40 * i + 35) * direction) * 1.25f;
					velocity2 = velocity2 + velocity3;
					velocity2 *= 0.5f;
					Projectile.NewProjectileDirect(source, player.RotatedRelativePoint(player.MountedCenter), velocity2, type, damage, knockback, player.whoAmI, -1f, MathHelper.ToRadians(25 + 15 * i) * -direction);
				}
			//speedX *= 1.0f;
			//speedY *= 1.0f;
			Projectile.NewProjectileDirect(source, player.RotatedRelativePoint(player.MountedCenter), velocity * 1.1f, type, damage, knockback, player.whoAmI, -0, 0);
			return false;
		}
	}
}
		
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Terra;
using SOTS.Projectiles.Tide;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class Earthshaker : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Launch a long ranged pickaxe which is capable of breaking many blocks at a time");
		}
		public override void SetDefaults()
		{
			item.damage = 11;
			item.melee = true;
			item.width = 30;
			item.height = 30;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 7.5f;
            item.value = Item.sellPrice(0, 2, 25, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item61;
			item.shoot = ModContent.ProjectileType<Projectiles.Terra.Earthshaker>(); 
            item.shootSpeed = 16f;
			item.noUseGraphic = true;
			item.channel = true;
			item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 velocity = new Vector2(speedX, speedY) * 0.25f;
			Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<EarthshakerPickaxe>(), damage, knockBack, player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y);
			return true; 
		}
    }
}
	

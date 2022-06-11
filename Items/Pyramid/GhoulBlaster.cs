using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Projectiles.Pyramid;

namespace SOTS.Items.Pyramid
{
	public class GhoulBlaster : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghoul Blaster");
			Tooltip.SetDefault("Unloads almost as fast as the trigger is pulled\nTransforms bullets into cursed singularities every 6th shot");
		}
		public override void SetDefaults()
		{
            Item.damage = 28;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 42; 
            Item.height = 26;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 5, 25, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = false;
            Item.shoot = 10; //not really important 
            Item.shootSpeed = 13.5f;
			Item.useAmmo = AmmoID.Bullet;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.PhoenixBlaster, 1).AddIngredient<RoyalMagnum>(1).AddIngredient<CursedMatter>(4).AddIngredient<SoulResidue>(12).AddIngredient(ItemID.SoulofNight, 15).AddIngredient(ItemID.Ruby, 1).AddTile(TileID.MythrilAnvil).Register();
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-0.25f, 0);
		}
		int shotNum = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Item.reuseDelay = 0;
			shotNum++;
			if(shotNum == 5)
			{
				Item.reuseDelay = 12;
			}
			if(shotNum >= 6)
			{
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item38, position);
				shotNum = 0;
				Projectile.NewProjectile(source, position, velocity * 0.9f, ModContent.ProjectileType<CurseSingularity>(), damage, knockback, player.whoAmI);
				return false;
			}
			return true; 
		}
	}
}

using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	public class FlareDetonator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flare Detonator");
			Tooltip.SetDefault("Fire an explosive flare that detonates upon releasing the trigger");
		}
		public override void SetDefaults()
		{
			item.noMelee = true;
			item.damage = 23;  
            item.ranged = true;    
            item.width = 44;  
            item.height = 26;   
            item.useTime = 10;  
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.knockBack = 5f;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.Item11;
            item.autoReuse = false;
            item.shoot = 10; 
            item.shootSpeed = 8f;
			item.reuseDelay = 8;
			item.useAmmo = AmmoID.Flare;
			item.noUseGraphic = true;
			item.channel = true;
		}
        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			item.useTime = 10;
			item.useAnimation = 10;
			base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int ai = 2;
			if (type == ProjectileID.BlueFlare)
			{
				ai = 1;
				speedX *= 0.65f;
				speedY *= 0.65f;
			}
			if (type == ProjectileID.Flare)
			{
				ai = 0;
				speedX *= 1.7f;
				speedY *= 1.7f;

			}
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), mod.ProjectileType("BombFlare"), damage, knockBack, player.whoAmI, ai);
			speedX *= 0f;
			speedY *= 0f;
			type = mod.ProjectileType("FlareDetonatorHold");
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FlareGun, 1);
			recipe.AddIngredient(null, "OtherworldlyAlloy", 12);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

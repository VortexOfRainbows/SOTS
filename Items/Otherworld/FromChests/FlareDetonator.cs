using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Otherworld;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	public class FlareDetonator : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.noMelee = true;
			Item.damage = 23;  
            Item.DamageType = DamageClass.Ranged;    
            Item.width = 44;  
            Item.height = 26;   
            Item.useTime = 10;  
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 5f;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item11;
            Item.autoReuse = false;
            Item.shoot = 10; 
            Item.shootSpeed = 8f;
			Item.reuseDelay = 8;
			Item.useAmmo = AmmoID.Flare;
			Item.noUseGraphic = true;
			Item.channel = true;
		}
        /*public override void ModifyWeaponDamage(Player player, ref StatModifier damage) //This is somewhat unneeded?
		{
			Item.useTime = 10;
			Item.useAnimation = 10;
			base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
        }*/
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int ai = 2;
			if (type == ProjectileID.BlueFlare)
			{
				ai = 1;
				velocity *= 0.65f;
			}
			if (type == ProjectileID.Flare)
			{
				ai = 0;
				velocity *= 1.7f;
			}
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<BombFlare>(), damage, knockback, player.whoAmI, ai);
			type = ModContent.ProjectileType<FlareDetonatorHold>();
			Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI, ai);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.FlareGun, 1).AddIngredient<OtherworldlyAlloy>(12).AddTile(ModContent.TileType<Furniture.HardlightFabricatorTile>()).Register();
		}
	}
}

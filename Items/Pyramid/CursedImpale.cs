using Microsoft.Xna.Framework;
using SOTS.Projectiles.Pyramid;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class CursedImpale : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 30;
			Item.DamageType = DamageClass.Melee;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;            
			Item.shoot = ModContent.ProjectileType<CurseSpear>(); 
            Item.shootSpeed = 5;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<CursedMatter>(7).AddIngredient<RoyalRubyShard>(10).AddTile(TileID.Anvils).Register();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int numberProjectiles = 1;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
				//Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("RubyBurst"), damage, knockBack, player.whoAmI);
			}
			return false; 
		}
	}
}
	

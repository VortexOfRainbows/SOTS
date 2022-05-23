using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Celestial;
using SOTS.Items.Evil;
using SOTS.Items.Pyramid;
using Terraria.DataStructures;

namespace SOTS.Items.Celestial
{
	public class DanceOfDeath : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dance of Death");
			Tooltip.SetDefault("Cast many demon scythes around you");
		}
		public override void SetDefaults()
		{
            Item.damage = 110; 
            Item.DamageType = DamageClass.Magic; 
            Item.width = 42;   
            Item.height = 44;   
            Item.useTime = 30;   
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;  
            Item.knockBack = 6.5f;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DeathBlade>(); 
            Item.shootSpeed = 9.5f;
			Item.mana = 12;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<SanguiteBar>(15).AddIngredient<TomeOfTheReaper>(1).AddIngredient<ShiftingSands>(1).AddTile(TileID.MythrilAnvil).Register();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for(int i = 0; i < 4; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(90 * i)); 
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			}
			return false; 
		}
	}
}

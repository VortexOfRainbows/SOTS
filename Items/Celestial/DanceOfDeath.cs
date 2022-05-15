using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Celestial;

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
			CreateRecipe(1).AddIngredient(null, "SanguiteBar", 15).AddIngredient(null, "TomeOfTheReaper", 1).AddIngredient(null, "ShiftingSands", 1).AddTile(TileID.MythrilAnvil).Register();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			for(int i = 0; i < 4; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(90 * i)); 
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false; 
		}
	}
}

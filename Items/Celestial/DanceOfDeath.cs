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
            item.damage = 110; 
            item.magic = true; 
            item.width = 42;   
            item.height = 44;   
            item.useTime = 30;   
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.noMelee = true;  
            item.knockBack = 6.5f;
            item.value = Item.sellPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item71;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<DeathBlade>(); 
            item.shootSpeed = 9.5f;
			item.mana = 12;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SanguiteBar", 15);
			recipe.AddIngredient(null, "TomeOfTheReaper", 1);
			recipe.AddIngredient(null, "ShiftingSands", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
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

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.IceStuff
{
	public class HypericeClusterCannon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hyperice Cluster Cannon");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.damage = 27;
            item.ranged = true;
            item.width = 64;
            item.height = 24;
            item.useTime = 29; 
            item.useAnimation = 29;
            item.useStyle = 5;    
            item.noMelee = true;
			item.knockBack = 1f;  
            item.value = Item.sellPrice(0, 8, 0, 0);
            item.rare = 7;
            item.UseSound = SoundID.Item61;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("HypericeRocket"); 
            item.shootSpeed = 8;
			item.useAmmo = ItemID.Snowball;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, -1);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("HypericeRocket"), damage, knockBack, player.whoAmI);
			return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AbsoluteBar", 16);
			recipe.AddIngredient(null, "CryoCannon", 1);
			recipe.AddIngredient(ItemID.SnowballCannon, 1);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
	}
}

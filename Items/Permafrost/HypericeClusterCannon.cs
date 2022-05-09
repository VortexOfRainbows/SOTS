using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Permafrost
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
            Item.damage = 30;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 64;
            Item.height = 24;
            Item.useTime = 26; 
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
			Item.knockBack = 1f;  
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = 7;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = mod.ProjectileType("HypericeRocket"); 
            Item.shootSpeed = 8;
			Item.useAmmo = ItemID.Snowball;
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

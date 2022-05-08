using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Projectiles.Earth;

namespace SOTS.Items.Earth
{
	public class VibrantCannon : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Cannon");
			Tooltip.SetDefault("");
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 18;
            Item.ranged = true;
            Item.width = 38;
            Item.height = 28;
            Item.useTime = 50; 
            Item.useAnimation = 50;
            Item.useStyle = 5;    
            Item.noMelee = true;
			Item.knockBack = 3f;  
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<VibrantBall>(); 
            Item.shootSpeed = 8;
		}
		public override int GetVoid(Player player)
		{
			return 22;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-0.25f, -0.25f);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			return true; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantBar>(), 10);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}

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
            item.damage = 18;
            item.ranged = true;
            item.width = 38;
            item.height = 28;
            item.useTime = 50; 
            item.useAnimation = 50;
            item.useStyle = 5;    
            item.noMelee = true;
			item.knockBack = 3f;  
            item.value = Item.sellPrice(0, 0, 80, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item61;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<VibrantBall>(); 
            item.shootSpeed = 8;
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

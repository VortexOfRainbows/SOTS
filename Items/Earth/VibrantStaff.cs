using SOTS.Projectiles.Earth;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth
{
	public class VibrantStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Staff");
			Tooltip.SetDefault("Fires a homing bolt");
		}
		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.magic = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 23;
			Item.useAnimation = 23;
			Item.useStyle = 5;
			Item.knockBack = 2.5f;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<VibrantArc>(); 
            Item.shootSpeed = 12f;
			Item.noMelee = true;
			Item.staff[Item.type] = true; //this makes the useStyle animate as a staff
			Item.mana = 8;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantBar>(), 6);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}
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
			item.damage = 16;
			item.magic = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 23;
			item.useAnimation = 23;
			item.useStyle = 5;
			item.knockBack = 2.5f;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;            
			item.shoot = ModContent.ProjectileType<VibrantArc>(); 
            item.shootSpeed = 12f;
			item.noMelee = true;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff
			item.mana = 8;
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
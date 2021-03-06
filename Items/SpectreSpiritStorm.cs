using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace SOTS.Items
{
	public class SpectreSpiritStorm : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Spirit Storm");
			Tooltip.SetDefault("Fires phantom arrows\nCan hit up to 4 enemies at a time");
		}
		public override void SafeSetDefaults()
		{

			item.damage = 58;
			item.ranged = true;
			item.width = 36;
			item.height = 74;
			item.useTime = 16;
			item.useAnimation = 16;
			item.useStyle = 5;
			item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 7, 25, 0);
			item.rare = 5;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;            
			item.shoot = 1; 
            item.shootSpeed = 21.5f;
			item.useAmmo = AmmoID.Arrow;
			item.noMelee = true;
			item.expert = true;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 6;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(4, 0);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("StormArrow"), damage, knockBack, player.whoAmI, 0, type);
			return false; 
		}
		public override void AddRecipes()	
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SpiritTracer", 1);
			recipe.AddIngredient(ItemID.DaedalusStormbow, 1);
			recipe.AddIngredient(ItemID.SpectreBar, 10);
			recipe.AddIngredient(null, "FragmentOfTide", 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
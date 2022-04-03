using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Items.Pyramid;
using SOTS.Items.Fragments;

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
			item.damage = 70;
			item.ranged = true;
			item.width = 36;
			item.height = 74;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;            
			item.shoot = 10; 
            item.shootSpeed = 21.5f;
			item.useAmmo = AmmoID.Arrow;
			item.noMelee = true;
		}
		public override int GetVoid(Player player)
		{
			return  6;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 0);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("StormArrow"), damage, knockBack, player.whoAmI, 0, type);
			return false; 
		}
		public override void AddRecipes()	
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SpiritTracer>(), 1);
			recipe.AddIngredient(ItemID.DaedalusStormbow, 1);
			recipe.AddIngredient(ItemID.SpectreBar, 10);
			recipe.AddIngredient(ModContent.ItemType<DissolvingDeluge>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
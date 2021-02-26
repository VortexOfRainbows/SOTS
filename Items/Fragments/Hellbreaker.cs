using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Crushers;

namespace SOTS.Items.Fragments
{
	public class Hellbreaker : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellbreaker");
			Tooltip.SetDefault("Charge to increase damage up to 600%\nTakes 4.5 seconds to reach max charge");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 48;
            item.melee = true;  
            item.width = 46;
            item.height = 46;  
            item.useTime = 30; 
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.knockBack = 8f;
            item.value = Item.sellPrice(0, 1, 55, 0);
            item.rare = ItemRarityID.Orange;
            item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<HellbreakerCrusher>(); 
            item.shootSpeed = 20f;
			item.channel = true;
            item.noUseGraphic = true; 
            item.noMelee = true;
			Item.staff[item.type] = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.ownedProjectileCounts[type] <= 0;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 6;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellstoneBar, 12);
			recipe.AddIngredient(null, "FragmentOfInferno", 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
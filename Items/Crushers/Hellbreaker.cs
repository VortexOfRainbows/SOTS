using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Crushers;
using SOTS.Items.Fragments;

namespace SOTS.Items.Crushers
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
            Item.damage = 48;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 46;
            Item.height = 46;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 8f;
            Item.value = Item.sellPrice(0, 1, 55, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item22;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<HellbreakerCrusher>(); 
            Item.shootSpeed = 20f;
			Item.channel = true;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
			Item.staff[Item.type] = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.ownedProjectileCounts[type] <= 0;
		}
		public override int GetVoid(Player player)
		{
			return 6;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellstoneBar, 12);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
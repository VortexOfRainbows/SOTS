using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Crushers;
using SOTS.Items.Slime;
using SOTS.Items.Pyramid;

namespace SOTS.Items.Crushers
{
	public class AquaticEclipse : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eclipse");
			Tooltip.SetDefault("Charge to increase damage up to 700%\nTakes 3 seconds to reach max charge\nCan strike through walls\nReleases bubbles that do 10% damage");
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 92;
            Item.melee = true;  
            Item.width = 56;
            Item.height = 56;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldingOut;    
            Item.knockBack = 10f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item22;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EclipseCrusher>(); 
            Item.shootSpeed = 20f;
			Item.channel = true;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.ownedProjectileCounts[type] <= 0;
		}
		public override int GetVoid(Player player)
		{
			return 7;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Hellbreaker>(), 1);
			recipe.AddIngredient(ModContent.ItemType<WormWoodCollapse>(), 1);
			recipe.AddIngredient(ModContent.ItemType<CrabClaw>(), 1);
			recipe.AddIngredient(ModContent.ItemType<CursedMatter>(), 5);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddIngredient(ItemID.Amethyst, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Pyramid
{
	public class AquaticEclipse : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aquatic Eclipse");
			Tooltip.SetDefault("Charge to increase damage up to 600%\nTakes 3 seconds to reach max charge\nCan strike through walls\nReleases bubbles that do 10% damage");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 60;
            item.melee = true;  
            item.width = 60;
            item.height = 60;  
            item.useTime = 30; 
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.knockBack = 10f;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.LightPurple;
            item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("EclipseCrusher"); 
            item.shootSpeed = 20f;
			item.channel = true;
            item.noUseGraphic = true; 
            item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.ownedProjectileCounts[type] <= 0;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 7;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Hellbreaker", 1);
			recipe.AddIngredient(null, "WormWoodCollapse", 1);
			recipe.AddIngredient(null, "CrabClaw", 1);
			recipe.AddIngredient(null, "CursedMatter", 5);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddIngredient(ItemID.Amethyst, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

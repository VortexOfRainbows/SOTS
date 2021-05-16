using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Crushers;

namespace SOTS.Items.GelGear
{
	public class WormWoodCollapse : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goopwood Collapse");
			Tooltip.SetDefault("Charge to increase damage up to 500%\nTakes 3 seconds to reach max charge");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 34;
            item.melee = true;  
            item.width = 44;
            item.height = 44;  
            item.useTime = 30; 
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.knockBack = 7f;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<WormWoodCrusher>(); 
            item.shootSpeed = 18f;
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
			voidMana = 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CorrosiveGel>(), 20);
			recipe.AddIngredient(null, "Wormwood", 32);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

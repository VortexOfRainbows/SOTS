using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Chaos;

namespace SOTS.Items.Chaos
{
	public class StellarSerpentLauncher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Serpent Launcher");
			Tooltip.SetDefault("Launches a Starlight Serpent which homes on enemys and attacks them repeatedly");
		}
		public override void SetDefaults()
		{
            item.damage = 60;   
            item.ranged = true;   
            item.width = 78;    
            item.height = 26;  
            item.useTime = 20;  
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.noMelee = true; 
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 20, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item61;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<ChaosSnake>(); 
            item.shootSpeed = 16.0f;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Chaos/StellarSerpentLauncherGlow");
				item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -2;
			}
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Vector2 speed = new Vector2(speedX, speedY);
			Projectile.NewProjectile(position + speed.SafeNormalize(Vector2.Zero) * 40, speed, ModContent.ProjectileType<ChaosSnake>(), damage, knockBack, player.whoAmI, Main.rand.Next(360));
			return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PhaseBar>(), 24);
			recipe.AddIngredient(ItemID.PiranhaGun, 1);
			recipe.AddIngredient(ModContent.ItemType<SnakeBow>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

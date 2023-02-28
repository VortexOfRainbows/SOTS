using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Chaos;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Chaos
{
	public class StellarSerpentLauncher : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 55;   
            Item.DamageType = DamageClass.Ranged;   
            Item.width = 78;    
            Item.height = 26;  
            Item.useTime = 24;  
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true; 
            Item.knockBack = 1;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ChaosSnake>(); 
            Item.shootSpeed = 16.0f;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Chaos/StellarSerpentLauncherGlow").Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -2;
			}
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position + velocity.SafeNormalize(Vector2.Zero) * 40, velocity, ModContent.ProjectileType<ChaosSnake>(), damage, knockback, player.whoAmI, Main.rand.Next(360));
			return false; 
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<PhaseBar>(24).AddIngredient(ItemID.PiranhaGun, 1).AddIngredient<SnakeBow>(1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}

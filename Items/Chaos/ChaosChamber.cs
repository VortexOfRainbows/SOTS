using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Chaos;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Chaos
{
	public class ChaosChamber : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Chamber");
			Tooltip.SetDefault("Unloads almost as fast as the trigger is pulled\nBullets gain homing capabilities\nTransforms bullets into laser balls every 6th shot");
		}
		public override void SetDefaults()
		{
            Item.damage = 100;   
            Item.DamageType = DamageClass.Ranged;   
            Item.width = 48;    
            Item.height = 26;  
            Item.useTime = 5;  
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true; 
            Item.knockBack = 1;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item36;
            Item.autoReuse = false;
            Item.shoot = ProjectileID.PurificationPowder; 
            Item.shootSpeed = 15f;
			Item.useAmmo = AmmoID.Bullet;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Chaos/ChaosChamberGlow").Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -2;
			}
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
		int counter = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Item.reuseDelay = 0;
			counter++;
			if (counter == 5)
			{
				Item.reuseDelay = 4;
			}
			if (counter >= 6)
			{
				counter = 0;
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item38, position);
				Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ChaosBallFriendly>(), damage, knockback, player.whoAmI);
			}
			else
			{
				Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
				proj.GetGlobalProjectile<SOTSProjectile>().affixID = -3; //this sould sync automatically on the SOTSProjectile end
			}
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PhaseBar>(), 15).AddIngredient(ItemID.VenusMagnum, 1).AddIngredient(ModContent.ItemType<GhoulBlaster>(), 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}

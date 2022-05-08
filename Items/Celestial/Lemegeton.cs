using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;
using SOTS.Items.Inferno;
using SOTS.Projectiles.Inferno;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Celestial
{
	public class Lemegeton : VoidItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Lemegeton");
			Tooltip.SetDefault("Summons a horde of Wisps to defend you from nearby enemies\nRed wisps will attack closer enemies and steal life for 100% damage\nGreen wisps will fire short-ranged blasts for 200% damage\nPurple wisps launch a cluster of homing bolts for 75% damage each");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 62;
			Item.knockBack = 2.5f;
			Item.width = 24;
			Item.height = 34;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<InfernalDefense>();
			Item.shoot = ModContent.ProjectileType<LemegetonWispRed>();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<LemegetonWispRed>(), damage, knockBack, player.whoAmI);
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<LemegetonWispGreen>(), (int)(damage * 2.00f), knockBack, player.whoAmI);
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<LemegetonWispPurple>(), (int)(damage * 0.75f), knockBack, player.whoAmI);
			player.AddBuff(Item.buffType, 2);
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SanguiteBar>(), 10);
			recipe.AddIngredient(ModContent.ItemType<BookOfVirtues>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
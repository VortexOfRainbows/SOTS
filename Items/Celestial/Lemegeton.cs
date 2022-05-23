using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;
using SOTS.Items.Inferno;
using SOTS.Projectiles.Inferno;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
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
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<LemegetonWispRed>(), damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<LemegetonWispGreen>(), (int)(damage * 2.00f), knockback, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<LemegetonWispPurple>(), (int)(damage * 0.75f), knockback, player.whoAmI);
			player.AddBuff(Item.buffType, 2);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<SanguiteBar>(), 10).AddIngredient(ModContent.ItemType<BookOfVirtues>(), 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
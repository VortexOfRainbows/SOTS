using Microsoft.Xna.Framework;
using SOTS.Buffs;
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
			Tooltip.SetDefault("Summons a horde of Wisps to defend you from nearby enemies\nRed wisps will attack particularly close enemies and steal life\nGreen wisps will fire short-ranged blasts at farther away enemies\nPurple wisps launch a cluster of homing bolts at mid-range enemies");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}
		public override void SafeSetDefaults()
		{
			item.damage = 25;
			item.knockBack = 3f;
			item.width = 24;
			item.height = 34;
			item.useTime = 16;
			item.useAnimation = 16;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.value = Item.sellPrice(0, 15, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item44;
			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<InfernalDefense>();
			item.shoot = ModContent.ProjectileType<LemegetonWispRed>();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<LemegetonWispRed>(), damage, knockBack, player.whoAmI);
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<LemegetonWispGreen>(), damage, knockBack, player.whoAmI);
			//Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<LemegetonWispRed>(), damage, knockBack, player.whoAmI);
			player.AddBuff(item.buffType, 2);
			return false;
		}
	}
}
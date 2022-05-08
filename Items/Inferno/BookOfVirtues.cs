using Microsoft.Xna.Framework;
using SOTS.Buffs.MinionBuffs;
using SOTS.Projectiles.Inferno;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Inferno
{
	public class BookOfVirtues : VoidItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Book of Virtues");
			Tooltip.SetDefault("Summons a Spectral Wisp that defends you from nearby enemies\nThe wisp does 75% damage on contact with enemies, and 100% with its laser");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}
		public override void SafeSetDefaults() 
		{
			Item.damage = 21;
			Item.knockBack = 3f;
			Item.width = 24;
			Item.height = 34;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.HoldingUp;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.summon = true;
			Item.buffType = ModContent.BuffType<Virtuous>();
			Item.shoot = ModContent.ProjectileType<SpectralWisp>();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) 
		{
			player.AddBuff(Item.buffType, 2);
			return true;
		}
	}
}
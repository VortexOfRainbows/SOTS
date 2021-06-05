using Microsoft.Xna.Framework;
using SOTS.Void;
using Terraria;
using Terraria.ID;

namespace SOTS.Items.Inferno
{
	public class BookOfVirtues : VoidItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Book of Virtues");
			Tooltip.SetDefault("Summons a Spectral Wisp to fight for you\nThe wisp does 75% damage on contanct with enemies, and 100% with its laser");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}
		public override void SafeSetDefaults() 
		{
			item.damage = 15;
			item.knockBack = 4f;
			item.width = 24;
			item.height = 34;
			item.useTime = 18;
			item.useAnimation = 18;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.value = Item.sellPrice(0, 7, 50, 0);
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item44;
			item.noMelee = true;
			item.summon = true;
			item.buffType = mod.BuffType("Virtuous");
			item.shoot = mod.ProjectileType("SpectralWisp");
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) 
		{
			player.AddBuff(item.buffType, 2);
			return true;
		}
	}
}
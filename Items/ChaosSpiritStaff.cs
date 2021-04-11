using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class ChaosSpiritStaff : VoidItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Chaos Spirit Staff");
			Tooltip.SetDefault("Summons an Chaos Spirit to fight for you\nLocks onto one enemy at a time");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}
		public override void SafeSetDefaults() 
		{
			item.damage = 48;
			item.knockBack = 4f;
			item.mana = 14;
			item.width = 38;
			item.height = 38;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 1;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item44;
			item.noMelee = true;
			item.summon = true;
			item.buffType = mod.BuffType("ChaosSpiritAid");
			item.shoot = mod.ProjectileType("ChaosSpirit");
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) 
		{
			player.AddBuff(item.buffType, 2);
			position = Main.MouseWorld;
			return true;
		}
	}
}
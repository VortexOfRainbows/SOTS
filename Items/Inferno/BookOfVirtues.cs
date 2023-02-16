using Microsoft.Xna.Framework;
using SOTS.Buffs.MinionBuffs;
using SOTS.Projectiles.Inferno;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Inferno
{
	public class BookOfVirtues : VoidItem
	{
		public override void SetStaticDefaults() 
		{
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults() 
		{
			Item.damage = 21;
			Item.knockBack = 3f;
			Item.width = 24;
			Item.height = 34;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<Virtuous>();
			Item.shoot = ModContent.ProjectileType<SpectralWisp>();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			player.AddBuff(Item.buffType, 2);
			player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
			return false;
		}
	}
}
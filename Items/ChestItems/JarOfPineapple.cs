using Microsoft.Xna.Framework;
using SOTS.Buffs.MinionBuffs;
using SOTS.Projectiles.BiomeChest;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{
	public class JarOfPineapple : ModItem
	{
		public override void SetStaticDefaults() 
		{
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
			this.SetResearchCost(1);
		}
		public override void SetDefaults() 
		{
			Item.damage = 20;
			Item.knockBack = 1f;
			Item.width = 18;
			Item.height = 24;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<FreshGreenyBuff>();
			Item.shoot = ModContent.ProjectileType<FreshGreeny>();
            Item.mana = 10;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			player.AddBuff(Item.buffType, 2);
			player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
			return false;
		}
	}
}
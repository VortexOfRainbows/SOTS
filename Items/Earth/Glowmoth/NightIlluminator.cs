using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using SOTS.Buffs.MinionBuffs;
using SOTS.Projectiles.Earth.Glowmoth;

namespace SOTS.Items.Earth.Glowmoth
{
    public class NightIlluminator : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Night Illuminator");
			Tooltip.SetDefault("Summons luminous moths to swarm your enemies\nThree moths take up one minion slot");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.knockBack = 3f;
			Item.mana = 10; // mana cost
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.Swing; // how the player's arm moves when using the item
			Item.value = Item.sellPrice(gold: 30);
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item44; // What sound should play when using the item

			// These below are needed for a minion weapon
			Item.noMelee = true; // this item doesn't do any melee damage
			Item.DamageType = DamageClass.Summon; // Makes the damage register as summon. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type
			Item.buffType = ModContent.BuffType<NightIlluminatorBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<MothMinion>(); // This item creates the minion projectile
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			// Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position
			position = Main.MouseWorld;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.AddBuff(Item.buffType, 2);

			var m1 = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
			var m2 = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
			var m3 = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
			m1.originalDamage = m2.originalDamage = m3.originalDamage = Item.damage;

			return false;
		}
	}
}
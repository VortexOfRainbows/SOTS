using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;
using SOTS.Projectiles.BiomeChest;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{
	public class StarcallerStaff : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Starcaller Staff");
			Tooltip.SetDefault("Summons a Crystal Serpent to fight for you\nThe Crystal Serpent circles around enemies and fires stars at them");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
			ItemID.Sets.StaffMinionSlotsRequired[Item.type] = 1;
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.mana = 10;
			Item.damage = 64;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 10f;
			Item.width = 54;
			Item.height = 48;
			Item.UseSound = SoundID.Item44;
			Item.useAnimation = 36;
			Item.useTime = 36;
			Item.rare = ItemRarityID.Yellow;
			Item.noMelee = true;
			Item.knockBack = 1.3f;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<StarlightSerpent>();
            Item.shoot = ModContent.ProjectileType<CrystalSerpentBody>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			player.AddBuff(Item.buffType, 2);
			player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
			return false;
		}
    }
}
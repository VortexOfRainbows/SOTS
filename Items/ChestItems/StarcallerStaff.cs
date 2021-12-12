using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Projectiles.BiomeChest;
using System.Collections.Generic;
using Terraria;
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
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
			ItemID.Sets.StaffMinionSlotsRequired[item.type] = 1;
		}
		public override void SetDefaults()
		{
			item.mana = 10;
			item.damage = 64;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.shootSpeed = 10f;
			item.width = 54;
			item.height = 48;
			item.UseSound = SoundID.Item44;
			item.useAnimation = 36;
			item.useTime = 36;
			item.rare = ItemRarityID.Yellow;
			item.noMelee = true;
			item.knockBack = 1.3f;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.summon = true;
			item.buffType = ModContent.BuffType<StarlightSerpent>();
            item.shoot = ModContent.ProjectileType<CrystalSerpentBody>();
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.AddBuff(item.buffType, 2);
			position = Main.MouseWorld;
			return true;
		}
    }
}
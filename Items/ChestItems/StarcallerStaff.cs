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
			Tooltip.SetDefault("Summons an Starlight Serpent to fight for you\nRight click to add onto the most recently summoned serpent, left click to summon a new serpent\nThis item was made for testing purposes");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
			ItemID.Sets.StaffMinionSlotsRequired[item.type] = 1;
		}
		public override void SetDefaults()
		{
			item.mana = 10;
			item.damage = 40;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.shootSpeed = 10f;
			item.width = 54;
			item.height = 48;
			item.UseSound = SoundID.Item44;
			item.useAnimation = 36;
			item.useTime = 36;
			item.rare = ItemRarityID.Yellow;
			item.noMelee = true;
			item.knockBack = 2f;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.summon = true;
			item.buffType = ModContent.BuffType<StarlightSerpent>();
            item.shoot = ModContent.ProjectileType<StarlightSerpentHead>();
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.AddBuff(item.buffType, 2);
			position = Main.MouseWorld;
			if (modPlayer.lightDragon == -1 || player.altFunctionUse != 2)
			{
				player.MinionNPCTargetAim();
				return true;
			}
			else
            {
				if(modPlayer.lightDragon != -1)
                {
					Projectile proj = Main.projectile[modPlayer.lightDragon];
					if(proj.active && proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<StarlightSerpentHead>())
                    {
						proj.ai[0] = 2;
						return false;
					}
					else
                    {
						modPlayer.lightDragon = -1;
						return true;
					}
				}
				return true;
			}
		}
        public override bool AltFunctionUse(Player player)
        {
			return true;
        }
        public override bool CanUseItem(Player player)
		{
			return true;
        }
    }
}
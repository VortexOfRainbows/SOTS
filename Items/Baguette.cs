using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Tide;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class Baguette : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crusty Baguette");
			Tooltip.SetDefault("Killing enemies will drop baguette crumbs\nPickup baguette crumbs to increase the range and damage of your baguette, and heal lost life\n'Surrender is not an option'");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.DamageType = DamageClass.Melee;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 7.5f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			//Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<ExtendoBaguette>(); 
            Item.shootSpeed = 4f;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.noMelee = true;
		}
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			damage.Base += modPlayer.baguetteLength;
        }
        public override void HoldItem(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.baguetteDrops = true;
		}
		public override bool CanUseItem(Player player)
		{
			int num = 0;
			for(int i = 0; i < Main.maxProjectiles; i++)
            {
				Projectile proj = Main.projectile[i];
				if(proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<ExtendoBaguette>() && proj.active && proj.alpha != 255)
                {
					num++;
					break;
                }
            }
			return num < 1;
		}
    }
}
	

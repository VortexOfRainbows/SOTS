using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;
using System;

namespace SOTS.Items.Celestial
{
	public class ContinuumCollapse : VoidItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Continuum Collapse");
			Tooltip.SetDefault("'Devour all that is infinite, including your system's memory'\nCan hit up to 15 enemies at a time\nWill not hurt players");
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 120;
			Item.DamageType = DamageClass.Magic;
			Item.width = 26;
			Item.height = 32;
            Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.rare = 12;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;            
			Item.shoot = mod.ProjectileType("ContinuumSphere"); 
			Item.shootSpeed = 1;
			Item.knockBack = 3;
			Item.channel = true;
			Item.UseSound = SoundID.Item15; //phaseblade
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
		public override bool BeforeDrainMana(Player player)
		{
			return false;
		}
		public override float UseTimeMultiplier(Player player)
		{
			return 1f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			bool summon = true;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if(proj.active && proj.type == Item.shoot && Main.player[proj.owner] == player)
				{
					summon = false;
				}
			}
			if(player.altFunctionUse != 2)
			{
				//Item.UseSound = SoundID.Item22;
				if(summon)
				{
					return true; 
				}
			}
            return false; 
		}
		public override int GetVoid(Player player)
		{
			return 8;
		}
	}
}
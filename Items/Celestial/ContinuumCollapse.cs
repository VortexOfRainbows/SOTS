using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;
using System;
using SOTS.Projectiles.Laser;

namespace SOTS.Items.Celestial
{
	public class ContinuumCollapse : VoidItem
	{	
		public override void SetStaticDefaults()//TODO 文本无法正常显示
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 120;
			Item.DamageType = DamageClass.Magic;
			Item.width = 26;
			Item.height = 32;
            Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.rare = ItemRarityID.Purple;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<ContinuumSphere>(); 
			Item.shootSpeed = 1;
			Item.knockBack = 3;
			Item.channel = true;
			Item.UseSound = SoundID.Item15; //phaseblade
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
		public override bool BeforeDrainVoid(Player player)
		{
			return false;
		}
		public override float UseTimeMultiplier(Player player)
		{
			return 1f;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
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
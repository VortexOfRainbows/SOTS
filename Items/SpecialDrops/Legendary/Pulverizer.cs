using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;


namespace SOTS.Items.SpecialDrops.Legendary
{
	public class Pulverizer : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pulverizer");
			Tooltip.SetDefault("Legendary drop\nLevels up as you progress\nConsume your enemies to turn them into blood essence\nCharging the crusher could increase damage by up to 2000%\nRight click to charge a blood railgun\nCharging the railgun could increase damage by up to 500%\nThe railgun will not hurt players");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 20;
            item.melee = true;  
            item.width = 44;
            item.height = 44;  
            item.useTime = 6; 
            item.useAnimation = 6;
            item.useStyle = 5;    
            item.knockBack = 0f;
            item.value = Item.sellPrice(1, 25, 0, 0);
            item.rare = 9;
            item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("PulverizerArm"); 
            item.shootSpeed = 0f;
			item.channel = true;
            item.noUseGraphic = true; 
            item.noMelee = true;
			Item.staff[item.type] = true; 
		}
		public override bool AltFunctionUse(Player player)
        {
            return true;
        } 
		public override void BeforeUseItem(Player player)
        {
				if(player.altFunctionUse == 2)
				{
					item.useTime = 2;
					item.useAnimation = 2;
					item.noUseGraphic = true; 
				}
				if(player.altFunctionUse != 2)
				{
					item.useTime = 6;
					item.useAnimation = 6;
					item.noUseGraphic = true; 
				}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
				VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			
				bool summon = true;
				for (int l = 0; l < Main.projectile.Length; l++)
				{
					Projectile proj = Main.projectile[l];
					if(proj.active && proj.type == item.shoot && Main.player[proj.owner] == player)
					{
						summon = false;
					}
				}
			if(player.altFunctionUse != 2)
			{
				item.UseSound = SoundID.Item22;
				if(summon)
				{
					Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, 0, player.whoAmI);
					Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, 1, player.whoAmI);
				}
			}
			if(player.altFunctionUse == 2)
			{
				player.AddBuff(mod.BuffType("PulverizerCharge"), 2);	
				for (int l = 0; l < Main.projectile.Length; l++)
				{
					Projectile proj = Main.projectile[l];
					if(proj.active && proj.type == mod.ProjectileType("BloodBeam") && Main.player[proj.owner] == player)
					{
						summon = false;
					}
				}
				if(summon)
				{
					
					Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("BloodBeam"), damage, 0, player.whoAmI);
				}
			}
              return false; 
		}
		public override void GetVoid(Player player)
		{
				voidMana = 2;
		}
		public override void UpdateInventory(Player player)
		{
			item.damage = 20;
						for(int repeatLevel = SOTSWorld.legendLevel; 0 < repeatLevel; repeatLevel--)
							{
								item.damage++;
							}
		}
	}
}

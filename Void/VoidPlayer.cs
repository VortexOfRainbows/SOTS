using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;
using SOTS.Projectiles.Base;

namespace SOTS.Void
{
	public class VoidPlayer : ModPlayer
	{
		public int voidMeterMax = 100;
		public int voidAnkh = 0;
		public override TagCompound Save() {
				
			return new TagCompound {
				
				{"voidMeterMax", voidMeterMax},
				{"voidAnkh", voidAnkh},
				};
		}

		public override void Load(TagCompound tag) 
		{
			voidMeterMax = tag.GetInt("voidMeterMax");
			voidAnkh = tag.GetInt("voidAnkh");
		}
		
		public float voidMeter = 100; 
		public float voidRegen = 0.0035f; 
		public float voidCost = 1f; 
		public float voidSpeed = 1f;
		public int voidMeterMax2 = 0;
		public bool voidShock = false;
		public bool voidRecovery = false;

		public static VoidPlayer ModPlayer(Player player) {
			return player.GetModPlayer<VoidPlayer>();
		}

		public float voidDamage = 1f;
		public float voidKnockback;
		public int voidCrit;

		public override void ResetEffects() {
			ResetVariables();
		}

		public override void UpdateDead() {
			voidMeter = voidMeterMax2/2;
			ResetVariables();
		}
		public static void VoidEffect(Player player, int voidAmount)
		{
			//CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(100, 80, 115, 255), string.Concat(voidAmount), false, false);
			if(player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ProjectileType<VoidHealEffect>(), 0, 0, player.whoAmI, Main.rand.Next(360), voidAmount);
				//NetMessage.SendData(43, -1, -1, "", player.whoAmI, (float)voidAmount, 0f, 0f, 0);
			}
		}
		public static string[] voidDeathMessages = {
			" was extremely careless.",
			" was devoured by the void.",
			" was consumed by the void.",
			" was taken by the void.",
			" was devoured by the darkness.",
			" was consumed by the darkness.",
			" was taken by the darkness.",
			" doesn't understand void mechanics.",
			" couldn't handle their own power.",
			" didn't manage their void well.",
			" died."
		};

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (voidShock || voidRecovery)
			{
				//damageSource = PlayerDeathReason.ByCustomReason(Main.LocalPlayer.name + " was consumed by the void.");
			}
			if (damage == 10.0 && voidShock)
			{
				genGore = false; //apparently, genGore false doesn't remove almost anygore what-so-ever
				damageSource = PlayerDeathReason.ByCustomReason(Main.LocalPlayer.name + voidDeathMessages[0]);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ProjectileType<VoidDeath>(), 0, 0, player.whoAmI);
				return true;
			}
			if (damage == 10.0 && voidRecovery)
			{
				genGore = false;
				damageSource = PlayerDeathReason.ByCustomReason(Main.LocalPlayer.name + voidDeathMessages[Main.rand.Next(voidDeathMessages.Length)]);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ProjectileType<VoidDeath>(), 0, 0, player.whoAmI);
				return true;
			}
			return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
		}
		public override void UpdateBadLifeRegen()
		{
			if(voidShock)
			{
				if (player.lifeRegen > 0)
				{
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				player.lifeRegen -= 7;
				if (player.statLife <= 0 && player.whoAmI == Main.myPlayer)
				{
					player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " was devoured by the void."), 10.0, 0, false);
				}
			}
			if (voidRecovery)
			{
				if (player.lifeRegen > 0)
				{
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				player.lifeRegen -= 10;
				player.lifeRegen -= player.statLifeMax2 / 20;
				if (player.statLife <= 0 && player.whoAmI == Main.myPlayer)
				{
					player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " was consumed by the void."), 10.0, 0, false);
				}
			}
		}

		private void ResetVariables() {
			voidShock = false;
			voidRecovery = false;

			//make sure damage is 100% before making modifications
			voidDamage = 1f;
			
			//percent damage grows as health lowers
			//voidDamage += 1f - (float)((float)player.statLife / (float)player.statLifeMax2);
			
			voidSpeed = 1f; 
			voidCost = 1f; 
			voidMeter += (float)(voidRegen / 60);
			
			if(voidMeter > voidMeterMax2)
			{
				//make sure meter doesn't go above max
				voidMeter = voidMeterMax2;
			}
			
			voidMeterMax2 = voidMeterMax;
			
			voidKnockback = 0f;
			voidCrit = 0;
			
			
			
			
			voidRegen = 0.175f; 
			
			voidRegen += 0.05f * (float)voidAnkh;
			
			
			
			if(voidMeter != 0)
			{
				VoidUI.visible = true;
			}
		}
		public override void PostUpdateBuffs()
		{
			if(voidMeter < 0)
			{
				if(!voidShock && !voidRecovery)
				{
					int time = 900 - voidMeterMax2;
					if(time < 120) time = 120;
					player.AddBuff(mod.BuffType("VoidShock"), time);
				}
				player.lifeRegen += (int)(voidMeter * 0.2f);
				if(voidMeter <= -150)
				{
					voidMeter = -150;
				}
			}
		}
	}
}
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using SOTS.Projectiles.Base;
using System;
using Microsoft.Xna.Framework;
using static SOTS.SOTS;
using System.Collections.Generic;
using SOTS.Projectiles.Minions;
using SOTS.Buffs;
using Terraria.ID;
using SOTS.Projectiles.Inferno;

namespace SOTS.Void
{
	public class VoidPlayer : ModPlayer
	{
		public static Vector2 voidBarOffset = new Vector2(810, 30);
		public static Color soulLootingColor = new Color(66, 56, 111);
		public static Color destabilizeColor = new Color(80, 190, 80);
		public static Color pastelRainbow = Color.White;
		public static Color natureColor = new Color(180, 240, 180);
		public static Color EarthColor = new Color(230, 220, 145);
		public static Color OtherworldColor = new Color(167, 45, 225, 0);
		public static Color VibrantColor = new Color(85, 125, 215, 0);
		public static Color LemegetonColor = new Color(255, 82, 97, 0);
		public static int soulColorCounter = 0;
		public int voidMeterMax = 100;
		public int voidAnkh = 0;
		public int voidStar = 0;
		public int lootingSouls = 0;
		public int soulsOnKill = 0;
		public bool frozenVoid = false;
		public int frozenCounter = 0;
		public int frozenDuration = 0;
		public int frozenMaxDuration = 0;
		public int frozenMinTimer = 3600;
		public float frozenVoidCount = 0;
		public override TagCompound Save() {

			return new TagCompound {

				{"voidMeterMax", voidMeterMax},
				{"voidMeterMax2", voidMeterMax2},
				{"voidAnkh", voidAnkh},
				{"voidStar", voidStar},
				{"voidMeter", voidMeter},
				{"lootingSouls", lootingSouls},
				{"voidBarOffsetX", voidBarOffset.X},
				{"voidBarOffsetY", voidBarOffset.Y},
				};
		}
		public override void Load(TagCompound tag)
		{
			voidMeterMax = tag.GetInt("voidMeterMax");
			voidMeterMax2 = tag.GetInt("voidMeterMax2");
			voidAnkh = tag.GetInt("voidAnkh");
			voidStar = tag.GetInt("voidStar");
			if (tag.ContainsKey("voidMeter"))
				voidMeter = tag.GetFloat("voidMeter");
			lootingSouls = tag.GetInt("lootingSouls");
			if (tag.ContainsKey("voidBarOffsetX"))
				voidBarOffset.X = tag.GetFloat("voidBarOffsetX");
			if (tag.ContainsKey("voidBarOffsetY"))
				voidBarOffset.Y = tag.GetFloat("voidBarOffsetY");
		}
		public bool netUpdate = false;
		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			// Here we would sync something like an RPG stat whenever the player changes it.
			VoidPlayer cloneV = clientPlayer as VoidPlayer;
			if (netUpdate)
			{
				if (cloneV.lootingSouls != lootingSouls || cloneV.voidMeterMax2 != voidMeterMax2)
				{
					// Send a Mod Packet with the changes.
					var packet = mod.GetPacket();
					packet.Write((byte)SOTSMessageType.SyncLootingSoulsAndVoidMax);
					packet.Write((byte)player.whoAmI);
					packet.Write(lootingSouls);
					packet.Write(voidMeterMax);
					packet.Write(voidMeterMax2);
					packet.Write(voidMeter);
					packet.Send();
				}
				netUpdate = false;
			}
		}
		public float voidMeter = 100;
		public float voidRegen = 0.0035f;
		public float voidCost = 1f;
		public float voidSpeed = 1f;
		public int voidMeterMax2 = 0;
		public bool voidShock = false;
		public bool voidRecovery = false;
		public float voidMultiplier = 1f;
		public float voidRegenMultiplier = 1f;
		public static VoidPlayer ModPlayer(Player player) {
			return player.GetModPlayer<VoidPlayer>();
		}
		public float voidDamage = 1f;
		public float voidKnockback;
		public int voidCrit;
		public override void ResetEffects()
		{
			ResetVariables();
		}
		public override void UpdateDead() {
			voidMeter = voidMeterMax2 / 2;
			ResetVariables();
		}
		public static void VoidEffect(Player player, int voidAmount)
		{
			//CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(100, 80, 115, 255), string.Concat(voidAmount), false, false);
			if (player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ProjectileType<VoidHealEffect>(), 0, 0, player.whoAmI, Main.rand.Next(360), voidAmount);
				//NetMessage.SendData(43, -1, -1, "", player.whoAmI, (float)voidAmount, 0f, 0f, 0);
			}
		}
		public static string[] voidDeathMessages = {
			" was extremely careless.",
			//" was devoured by the void.",
			" was consumed by the void.",
			//" was taken by the void.",
			" was devoured by the darkness.",
			//" was consumed by the darkness.",
			//" was taken by the darkness.",
			" doesn't understand void mechanics.",
			//" couldn't handle their own power.",
			//" didn't manage their void well.",
			" died."
		};
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (voidShock || voidRecovery)
			{
				//damageSource = PlayerDeathReason.ByCustomReason(Main.LocalPlayer.name + " was consumed by the void.");
			}
			if (voidShock)
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
		public List<int> VoidMinions = new List<int>();
		public override void UpdateBadLifeRegen()
		{
			if (voidShock)
			{
				player.lifeRegen = 0;
				if (player.lifeRegen > 0)
				{
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				player.lifeRegen -= 16;
				if (player.statLife <= 0 && player.whoAmI == Main.myPlayer)
				{
					player.KillMe(PlayerDeathReason.ByCustomReason(player.name + voidDeathMessages[Main.rand.Next(voidDeathMessages.Length)]), 10.0, 0, false);
				}
			}
			if (voidRecovery)
			{
				if (player.lifeRegen > 0)
				{
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				player.lifeRegen -= 20;
				player.lifeRegen -= player.statLifeMax2 / 20;
				if (player.statLife <= 0 && player.whoAmI == Main.myPlayer)
				{
					player.KillMe(PlayerDeathReason.ByCustomReason(player.name + voidDeathMessages[Main.rand.Next(voidDeathMessages.Length)]), 10.0, 0, false);
				}
			}
		}
		float standingTimer = 1;
		public float maxStandingTimer = 2;
		public void ApplyDynamicMultiplier()
		{
			voidMultiplier = 0.85f;
			float hpMult = (float)player.statLife / player.statLifeMax2;
			hpMult *= 0.2f;
			float voidMult = (float)voidMeter / voidMeterMax2;
			voidMult *= 0.2f;
			if (Math.Abs(player.velocity.X) <= 0.1f && Math.Abs(player.velocity.Y) <= 0.1f)
			{
				standingTimer += 0.005f;
				if (standingTimer > maxStandingTimer)
					standingTimer = maxStandingTimer;
			}
			else
			{
				if (standingTimer > 1)
					standingTimer -= 0.05f;
				if (standingTimer < 1)
					standingTimer = 1;
			}
			voidMultiplier += hpMult + voidMult;
			voidMultiplier *= standingTimer;
		}
		public void UseSouls()
		{
			if (Main.mouseRight && Main.mouseRightRelease && player.ownedProjectileCounts[mod.ProjectileType("HarvestingStrike")] < 1)
			{
				if (Main.myPlayer == player.whoAmI)
					Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, mod.ProjectileType("HarvestingStrike"), 1, 0, player.whoAmI);
			}
		}
		public void ColorUpdate()
		{
			soulColorCounter++;
			destabilizeColor = Color.Lerp(new Color(80, 190, 80), new Color(64, 178, 172), 0.5f + new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(soulColorCounter * 1.25f)).X);
			soulLootingColor = Color.Lerp(new Color(66, 56, 111), new Color(171, 3, 35), 0.5f + new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(soulColorCounter * 1.5f)).X);
			float newAi = soulColorCounter * 3 / 13f;
			double frequency = 0.3;
			double center = 200;
			double width = 55;
			double red = Math.Sin(frequency * newAi) * width + center;
			double grn = Math.Sin(frequency * newAi + 2.0) * width + center;
			double blu = Math.Sin(frequency * newAi + 4.0) * width + center;
			pastelRainbow = new Color((int)red, (int)grn, (int)blu);
			natureColor = Color.Lerp(new Color(65, 180, 80), new Color(180, 240, 180), 0.5f + new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(soulColorCounter * 1.0f)).X);
			EarthColor = Color.Lerp(new Color(230, 220, 145), new Color(255, 190, 0), 0.5f + new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(soulColorCounter * 1.2f)).X);
			Color color = new Color(167, 45, 225, 0);
			Color color2 = new Color(64, 178, 172, 0);
			OtherworldColor = Color.Lerp(color, color2, 0.5f + new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(soulColorCounter * 1.0f)).X);
			VibrantColor = Color.Lerp(new Color(80, 120, 220, 0), new Color(180, 230, 100, 0), 0.5f + new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(soulColorCounter * 2.5f)).X);

			Color LemegetonRed = new Color(255, 82, 97);
			Color LemegetonGreen = new Color(104, 229, 101);
			Color LemegetonPurple = new Color(200, 119, 247);
			float lerpAmt = 1.5f + 3 * (new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(soulColorCounter * 0.33f)).X);
			if(lerpAmt < 1)
            {
				LemegetonColor = Color.Lerp(LemegetonRed, LemegetonGreen, lerpAmt);
			}
			else if (lerpAmt < 2)
			{
				lerpAmt -= 1;
				LemegetonColor = Color.Lerp(LemegetonGreen, LemegetonPurple, lerpAmt);
			}
			else
			{
				lerpAmt -= 2;
				LemegetonColor = Color.Lerp(LemegetonPurple, LemegetonRed, lerpAmt);
			}
		}
		public static Color VibrantColorAttempt(float degrees)
        {
			return Color.Lerp(new Color(80, 120, 220, 0), new Color(180, 230, 100, 0), 0.5f + new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(soulColorCounter * 2.5f + degrees)).X);
		}
		public static Color pastelAttempt(float radians)
		{
			float newAi = radians;
			double center = 190;
			Vector2 circlePalette = new Vector2(1, 0).RotatedBy(newAi);
			double width = 65 * circlePalette.Y;
			int red = (int)(center + width);
			circlePalette = new Vector2(1, 0).RotatedBy(newAi + MathHelper.ToRadians(120));
			width = 65 * circlePalette.Y;
			int grn = (int)(center + width);
			circlePalette = new Vector2(1, 0).RotatedBy(newAi + MathHelper.ToRadians(240));
			width = 65 * circlePalette.Y;
			int blu = (int)(center + width);
			return new Color(red, grn, blu);
		}
		public int RegisterVoidMinions()
		{
			VoidMinions = new List<int>();
			List<int> whoAmI = new List<int>();
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.owner == player.whoAmI && projectile.active && isVoidMinion(projectile))
				{
					VoidMinions.Add(voidMinion(projectile));
					whoAmI.Add(projectile.whoAmI);
				}
			}
			int total = 0;
			for (int i = 0; i < VoidMinions.Count; i++)
			{
				int type = VoidMinions[i];
				total += minionVoidCost(type);
			}
			bool flag = false;
			for (int i = VoidMinions.Count - 1; i >= 0; i--)
			{
				if(total > voidMeterMax2)
				{
					int type = VoidMinions[i];
					Projectile projectile = Main.projectile[whoAmI[i]];
					projectile.active = false;
					projectile.Kill();
					total -= minionVoidCost(type);
					VoidMinions.RemoveAt(i);
					flag = true;
				}
			}
			if (flag) netUpdate = true;
			return total;
		}
		public int VoidMinionConsumption = 0;
		public static int minionVoidCost(int type)
		{
			if (type == (int)VoidMinionID.NatureSpirit)
				return 40;
			if (type == (int)VoidMinionID.ChaosSpirit)
				return 150;
			if (type == (int)VoidMinionID.EarthenSpirit)
				return 20;
			if (type == (int)VoidMinionID.OtherworldSpirit)
				return 65;
			if (type == (int)VoidMinionID.BethanySpirit)
				return 15;
			if (type == (int)VoidMinionID.TBethanySpirit)
				return 20;
			if (type == (int)VoidMinionID.CursedBlade)
				return 100;
			return 1;
		}
		public static Color minionVoidColor(int type)
		{
			if (type == (int)VoidMinionID.NatureSpirit)
				return VoidPlayer.natureColor;
			if (type == (int)VoidMinionID.ChaosSpirit)
				return VoidPlayer.pastelRainbow;
			if (type == (int)VoidMinionID.EarthenSpirit)
				return VoidPlayer.EarthColor;
			if (type == (int)VoidMinionID.OtherworldSpirit)
				return new Color(OtherworldColor.R, OtherworldColor.G, OtherworldColor.B);
			if (type == (int)VoidMinionID.BethanySpirit)
				return new Color(170, 220, 255);
			if (type == (int)VoidMinionID.TBethanySpirit)
				return LemegetonColor;
			if (type == (int)VoidMinionID.CursedBlade)
				return new Color(76, 58, 101);
			return Color.White;
		}
		public static bool isVoidMinion(Projectile projectile)
        {
			return voidMinion(projectile) > -1;
        }
		public static int voidMinion(Projectile projectile)
        {
			return voidMinion(projectile.type);
		}
		public static bool isVoidMinion(int type)
		{
			return voidMinion(type) > -1;
		}
		public static int voidMinion(int type)
		{
			if (type == ProjectileType<NatureSpirit>())
				return (int)VoidMinionID.NatureSpirit;
			if (type == ProjectileType<ChaosSpirit>())
				return (int)VoidMinionID.ChaosSpirit;
			if (type == ProjectileType<EarthenSpirit>())
				return (int)VoidMinionID.EarthenSpirit;
			if (type == ProjectileType<OtherworldlySpirit>())
				return (int)VoidMinionID.OtherworldSpirit;
			if (type == ProjectileType<SpectralWisp>())
				return (int)VoidMinionID.BethanySpirit;
			if (type == ProjectileType<LemegetonWispGreen>() || type == ProjectileType<LemegetonWispPurple>() || type == ProjectileType<LemegetonWispRed>())
				return (int)VoidMinionID.TBethanySpirit;
			if (type == ProjectileType<Projectiles.Minions.CursedBlade>())
				return (int)VoidMinionID.CursedBlade;
			return -1;
		}
		public enum VoidMinionID
        {
			NatureSpirit,
			ChaosSpirit,
			EarthenSpirit,
			OtherworldSpirit,
			BethanySpirit,
			TBethanySpirit,
			CursedBlade
		}
        public override void PostUpdateEquips()
        {
            base.PostUpdateEquips();
        }
        private void ResetVariables() 
		{
			ColorUpdate();
			if (soulsOnKill > 0)
				UseSouls();
			if(soulColorCounter % 40 == 0)
            {
				if(soulsOnKill <= 0)
                {
					if (lootingSouls > 0)
						lootingSouls--;
                }
				if(soulColorCounter % 360 == 0)
                {
					netUpdate = true;
				}
            }
			voidShock = false;
			voidRecovery = false;
			voidDamage = 1f;
			soulsOnKill = 0;
			//percent damage grows as health lowers
			//voidDamage += 1f - (float)((float)player.statLife / (float)player.statLifeMax2);

			voidSpeed = 1f; 
			voidCost = 1f;
			ApplyDynamicMultiplier();
			if (voidRegen > 0)
				voidMeter += (float)(voidRegen / 60f) * (voidRegenMultiplier + voidMultiplier - 1);
			else
				voidMeter += (float)(voidRegen / 60f);

			VoidMinionConsumption = RegisterVoidMinions();
			voidMeterMax2 -= VoidMinionConsumption;

			if (lootingSouls > voidMeterMax2 && lootingSouls > 0)
				lootingSouls = voidMeterMax2;
			if (lootingSouls < 0)
				lootingSouls = 0;
			voidMeterMax2 -= lootingSouls;

			if (frozenMaxDuration > 0)
			{
				if (frozenDuration > 0)
                {
					frozenDuration--;
					frozenVoid = true;
                }
				else
				{
					frozenVoid = false;
					frozenCounter++;
					if (frozenCounter >= frozenMinTimer)
					{
						//Main.NewText("Frozen");
						frozenCounter = 0;
						frozenDuration = frozenMaxDuration;
						frozenVoidCount = voidMeter;
					}
					//Main.NewText(frozenCounter);
				}
            }
			else
			{
				frozenVoid = false;
				frozenCounter = 0;
				frozenDuration = 0;
			}
			if (this.frozenCounter == this.frozenMinTimer - 30 && Main.myPlayer == player.whoAmI)
			{
				Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 29, 1.1f, -0.1f);
			}
			if (this.frozenDuration == 30 && Main.myPlayer == player.whoAmI)
			{
				Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 29, 1.1f, 0.3f);
			}

			frozenMaxDuration = 0;
			frozenMinTimer = 3600;
			if (voidMeter > voidMeterMax2)
			{
				//make sure meter doesn't go above max
				voidMeter = voidMeterMax2; 
				frozenVoidCount = voidMeter;
			}
			else
            {
				if(frozenVoid)
                {
					voidMeter = frozenVoidCount;
                }
			}
			voidMeterMax2 = voidMeterMax;
			voidKnockback = 0f;
			voidCrit = 0;
			voidRegen = 0.25f; 
			voidRegen += 0.05f * (float)voidAnkh;
			voidRegen += 0.1f * (float)voidStar;
			maxStandingTimer = 2;
			if (voidMeter != 0)
			{
				VoidUI.visible = true;
			}
		}
		public override void PostUpdateBuffs()
		{
			if (voidMeter < 0)
			{
				if(!voidShock && !voidRecovery)
				{
					int time = 600;
					player.AddBuff(mod.BuffType("VoidShock"), time);
					//if(time < 120) time = 120;
				}
				player.lifeRegen += (int)(voidMeter * 0.2f);
				if(voidMeter <= -150)
				{
					voidMeter = -150;
				}
			}
			if (player.HasBuff(ModContent.BuffType<SulfurBurn>()))
			{
				if(voidRegen > 0)
					voidRegen *= 0.5f;
				voidRegen -= 100f;
			}
		}
		public override float UseTimeMultiplier(Item item)
		{
			float standard = voidSpeed;
			int time = item.useAnimation;
			int cannotPass = 2;
			float current = time / standard;
			if (current < cannotPass)
			{
				standard = time / 2f;
			}
			if (item.modItem is VoidItem isVoid)
				if (item.channel == false)
					return standard;
			return base.UseTimeMultiplier(item);
		}
	}
}
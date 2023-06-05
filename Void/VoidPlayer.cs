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
using SOTS.Items.Void;
using SOTS.Projectiles.Pyramid;
using SOTS.Buffs.DilationSickness;
using Terraria.Localization;
using System.Linq;
using SOTS.Projectiles.Pyramid.GhostPepper;

namespace SOTS.Void
{
	public class VoidPlayer : ModPlayer
	{
		public Vector2 voidBarOffset = new Vector2(810, 30);
		public float VoidFoodGainMultiplier = 1f;
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
		public bool safetySwitch = false;
		public bool safetySwitchVisual = false;
		public bool CrushCapacitor = false;
		public bool CrushResistor = false;
		public float CrushTransformer = 1f;
		public int BonusCrushRangeMin = 0;
		public int BonusCrushRangeMax = 0;
		public float VoidGenerateMoney = 0;

		public static string GetVoidDeathMessage(int num)
        {
			return Language.GetTextValue(("Mods.SOTS.DeathMessage.VD" + num));
		}
        public override void SaveData(TagCompound tag)/* Edit tag parameter rather than returning new TagCompound */
		{
			tag["voidMeterMax"] = voidMeterMax;
			tag["voidMeterMax2"] = voidMeterMax2;
			tag["voidAnkh"] = voidAnkh;
			tag["voidStar"] = voidStar;
			tag["voidMeter"] = voidMeter;
			tag["lootingSouls"] = lootingSouls;
			tag["voidBarPosX3"] = voidBarOffset.X;
			tag["voidBarPosY3"] = voidBarOffset.Y;
		}
		public override void LoadData(TagCompound tag)
		{
			voidMeterMax = tag.GetInt("voidMeterMax");
			voidMeterMax2 = tag.GetInt("voidMeterMax2");
			voidAnkh = tag.GetInt("voidAnkh");
			voidStar = tag.GetInt("voidStar");
			//if (tag.ContainsKey("voidMeter"))
			voidMeter = tag.GetFloat("voidMeter");
			lootingSouls = tag.GetInt("lootingSouls");
			if (tag.ContainsKey("voidBarPosX3"))
				voidBarOffset.X = tag.GetFloat("voidBarPosX3");
			if (tag.ContainsKey("voidBarPosY3"))
				voidBarOffset.Y = tag.GetFloat("voidBarPosY3");
		}
        public bool netUpdate = false;
        public override void clientClone(ModPlayer clientClone)
		{
			//will need to fix this later...
		}
		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			// Here we would sync something like an RPG stat whenever the player changes it.
			VoidPlayer cloneV = clientPlayer as VoidPlayer;
			if (netUpdate)
			{
				if (cloneV.lootingSouls != lootingSouls || cloneV.voidMeterMax != voidMeterMax || cloneV.voidMeterMax2 != voidMeterMax2)
				{
					// Send a Mod Packet with the changes.
					var packet = Mod.GetPacket();
					packet.Write((byte)SOTSMessageType.SyncLootingSoulsAndVoidMax);
					packet.Write((byte)Player.whoAmI);
					packet.Write(lootingSouls);
					packet.Write(voidMeterMax);
					packet.Write(voidMeterMax2);
					packet.Write(voidMeter);
					packet.Write(VoidMinionConsumption);
					packet.Send();
				}
				netUpdate = false;
			}
		}
		public float voidMeter = 100;
		public float voidCost = 1f;
		public int voidMeterMax2 = 0;
		public bool hasEnteredVoidShock = false;
		public int voidShockAnimationCounter = 0;
		public bool voidShock = false;
		public bool voidRecovery = false;
		public float voidMultiplier = 1f;
		public static VoidPlayer ModPlayer(Player player) {
			return player.GetModPlayer<VoidPlayer>();
		}
		public override void ResetEffects()
		{
			if (Player.isDisplayDollOrInanimate || Player.isHatRackDoll || Player.isFirstFractalAfterImage)
			{
				return;
			}
			ResetVariables();
		}
		public override void UpdateDead()
		{
			if (Player.isDisplayDollOrInanimate || Player.isHatRackDoll || Player.isFirstFractalAfterImage)
			{
				return;
			}
			voidMeter = voidMeterMax2 / 2;
			ResetVariables();
		}
		public int TrueMax()
        {
			return voidMeterMax2 - VoidMinionConsumption - lootingSouls;
		}
        public static void VoidEffect(Player player, int voidAmount, bool damageOverTime = false, bool resolve = true)
		{
			if (Main.gameMenu || Main.gameInactive) //hopefully prevent projectile entity issues
			{
				return;
            }
			//CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(100, 80, 115, 255), string.Concat(voidAmount), false, false);
			if (player.whoAmI == Main.myPlayer)
			{
				if(resolve && voidAmount > 0)
				{
					VoidPlayer vPlayer = ModPlayer(player);
					int amt = voidAmount;
					if(vPlayer.lastVoidMeter + amt > vPlayer.TrueMax())
                    {
						amt = vPlayer.TrueMax() - (int)vPlayer.lastVoidMeter;
					}
					if(amt > 0)
					{
						vPlayer.resolveVoidCounter = 15;
						vPlayer.resolveVoidAmount += amt;
					}
				}
				Projectile.NewProjectile(player.GetSource_Misc("SOTS:VoidHeal"), player.Center.X, player.Center.Y, 0, 0, ProjectileType<VoidHealEffect>(), 0, 0, player.whoAmI, damageOverTime ? -1 : 0, voidAmount);
				//NetMessage.SendData(43, -1, -1, "", player.whoAmI, (float)voidAmount, 0f, 0f, 0);
			}
		}
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (voidShock || voidRecovery)
			{
				//damageSource = PlayerDeathReason.ByCustomReason(Main.LocalPlayer.name + " was consumed by the void.");
			}
			if (voidShock)
			{
				genGore = false; //apparently, genGore false doesn't remove almost anygore what-so-ever
				damageSource = PlayerDeathReason.ByCustomReason(Player.name + GetVoidDeathMessage(1));
				Projectile.NewProjectile(Player.GetSource_Death(), Player.Center.X, Player.Center.Y, 0, 0, ProjectileType<VoidDeath>(), 0, 0, Player.whoAmI);
				return true;
			}
			if (damage == 10.0 && voidRecovery)
			{
				genGore = false;
				damageSource = PlayerDeathReason.ByCustomReason(Player.name + GetVoidDeathMessage(1 + Main.rand.Next(5)));
				Projectile.NewProjectile(Player.GetSource_Death(), Player.Center.X, Player.Center.Y, 0, 0, ProjectileType<VoidDeath>(), 0, 0, Player.whoAmI);
				return true;
			}
			return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
		}
		public List<int> VoidMinions = new List<int>();
		public override void UpdateBadLifeRegen()
		{
			if (Player.isDisplayDollOrInanimate || Player.isHatRackDoll || Player.isFirstFractalAfterImage)
			{
				return;
			}
			SOTSPlayer sPlayer = SOTSPlayer.ModPlayer(Player);
			if (voidShock)
			{
				Player.lifeRegen = 0;
				if (Player.lifeRegen > 0)
				{
					Player.lifeRegen = 0;
				}
				Player.lifeRegenTime = 0;
				if (sPlayer.VMincubator || sPlayer.VoidAnomaly) //also lengthens time of curse
					Player.lifeRegen -= 20; // -10 hp * 20 seconds = -200 hp
				else
					Player.lifeRegen -= 16; // -8 hp * 10 seconds = -80 hp
				if (Player.statLife <= 0 && Player.whoAmI == Main.myPlayer)
				{
					Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + GetVoidDeathMessage(1 + Main.rand.Next(5))), 10.0, 0, false);
				}
			}
			if (voidRecovery)
			{
				if (Player.lifeRegen > 0)
				{
					Player.lifeRegen = 0;
				}
				Player.lifeRegenTime = 0;
				Player.lifeRegen -= 20;
				Player.lifeRegen -= Player.statLifeMax2 / 20;
				if (sPlayer.VMincubator || sPlayer.VoidAnomaly)
                {
					if(Player.lifeRegen < 0)
                    {
						Player.lifeRegen = (int)(Player.lifeRegen * 1.25f); //simply 1.25x the hp loss as normal
                    }
                }
				if (Player.statLife <= 0 && Player.whoAmI == Main.myPlayer)
				{
					Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + GetVoidDeathMessage(1 + Main.rand.Next(5))), 10.0, 0, false);
				}
			}
		}
		//float standingTimer = 1;
		//public float maxStandingTimer = 2;
		/*public void ApplyDynamicMultiplier()
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
		}*/
		public void UseSouls()
		{
			if (Player.isDisplayDollOrInanimate || Player.isHatRackDoll || Player.isFirstFractalAfterImage)
			{
				return;
			}
			if (Main.mouseRight && Main.mouseRightRelease && Player.ownedProjectileCounts[ProjectileType<HarvestingStrike>()] < 1)
			{
				if (Main.myPlayer == Player.whoAmI)
					Projectile.NewProjectile(Player.GetSource_Misc("SOTS:RightClickWithSoulsOfLooting"), Main.MouseWorld, Vector2.Zero, ProjectileType<HarvestingStrike>(), 1, 0, Player.whoAmI);
			}
		}
		public int VoidMinionConsumption = 0;
		public int RegisterVoidMinions()
		{
			if (Player.isDisplayDollOrInanimate || Player.isHatRackDoll || Player.isFirstFractalAfterImage)
			{
				return 0;
			}
			VoidMinions = new List<int>();
			List<int> whoAmI = new List<int>();
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.owner == Player.whoAmI && projectile.active && isVoidMinion(projectile))
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
					if (projectile.owner == Player.whoAmI)
					{
						projectile.active = false;
						projectile.Kill();
					}
					total -= minionVoidCost(type);
					if (projectile.owner == Player.whoAmI)
						VoidMinions.RemoveAt(i);
					flag = true;
				}
			}
			if (flag) netUpdate = true;
			return total;
		}
		public static int minionVoidCost(int type)
		{
			if (type == (int)VoidMinionID.NatureSpirit || type == (int)VoidMinionID.TidalSpirit)
				return 40;
			if (type == (int)VoidMinionID.ChaosSpirit)
				return 150;
			if (type == (int)VoidMinionID.EarthenSpirit)
				return 25;
			if (type == (int)VoidMinionID.OtherworldSpirit)
				return 75;
			if (type == (int)VoidMinionID.BethanySpirit)
				return 15;
			if (type == (int)VoidMinionID.TBethanySpirit)
				return 20;
			if (type == (int)VoidMinionID.CursedBlade || type == (int)VoidMinionID.EvilSpirit)
				return 100;
			if (type == (int)VoidMinionID.PermafrostSpirit)
				return 54;
			if(type == (int)VoidMinionID.InfernoSpirit)
				return 120;
			return 1;
		}
		public static Color minionVoidColor(int type)
		{
			if (type == (int)VoidMinionID.NatureSpirit)
				return ColorHelpers.natureColor;
			if (type == (int)VoidMinionID.ChaosSpirit)
				return ColorHelpers.pastelRainbow;
			if (type == (int)VoidMinionID.EarthenSpirit)
				return ColorHelpers.EarthColor;
			if (type == (int)VoidMinionID.OtherworldSpirit)
				return new Color(ColorHelpers.OtherworldColor.R, ColorHelpers.OtherworldColor.G, ColorHelpers.OtherworldColor.B);
			if (type == (int)VoidMinionID.BethanySpirit)
				return new Color(170, 220, 255);
			if (type == (int)VoidMinionID.TBethanySpirit)
				return ColorHelpers.LemegetonColor;
			if (type == (int)VoidMinionID.CursedBlade)
				return new Color(76, 58, 101);
			if (type == (int)VoidMinionID.TidalSpirit)
				return ColorHelpers.TideColor;
			if (type == (int)VoidMinionID.PermafrostSpirit)
				return ColorHelpers.PermafrostColor;
			if (type == (int)VoidMinionID.InfernoSpirit)
				return ColorHelpers.InfernoColorAttemptDegrees(ColorHelpers.soulColorCounter);
			if (type == (int)VoidMinionID.EvilSpirit)
			{
				Color color = Color.Lerp(new Color(165, 21, 0), new Color(114, 54, 183), 0.5f + (float)Math.Sin(MathHelper.ToRadians(ColorHelpers.soulColorCounter * 1.25f)) * 0.5f);
				return color;
			}
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
			if (type == ProjectileType<TidalSpirit>())
				return (int)VoidMinionID.TidalSpirit;
			if (type == ProjectileType<PermafrostSpirit>())
				return (int)VoidMinionID.PermafrostSpirit;
			if (type == ProjectileType<InfernoSpirit>())
				return (int)VoidMinionID.InfernoSpirit;
			if (type == ProjectileType<EvilSpirit>())
				return (int)VoidMinionID.EvilSpirit;
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
			CursedBlade,
			TidalSpirit,
			PermafrostSpirit,
			InfernoSpirit,
			EvilSpirit
		}
		public static void VoidBurn(Mod mod, Player player, int damage, int duration)
		{
			VoidDamage(mod, player, damage);
			player.AddBuff(BuffType<VoidBurn>(), duration, false);
		}
		public float voidDamageTimer = 0;
		public static void VoidDamage(Mod mod, Player player, int damage)
		{
			VoidPlayer local = ModPlayer(player);
			if (local.voidDamageTimer > 0)
            {
				return;
            }
			damage = (int)(damage * Main.rand.NextFloat(0.9f, 1.1f));
			if (player.whoAmI == Main.LocalPlayer.whoAmI)
				SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Void/Void_Damage"), (int)player.Center.X, (int)player.Center.Y, 1.1f);
			for (int i = 0; i < (int)(4 + 0.5f * Math.Sqrt(damage)); i++)
			{
				Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 198);
				dust.noGravity = true;
				dust.velocity *= 4;
				dust.scale = 2.2f;
				dust = Dust.NewDustDirect(player.position, player.width, player.height, 198);
				dust.noGravity = true;
				dust.velocity *= 3.5f;
				dust.scale = 2.7f;
				dust = Dust.NewDustDirect(player.position, player.width, player.height, 198);
				dust.noGravity = true;
				dust.velocity *= 3;
				dust.scale = 3.2f;
			}
			local.voidMeter -= damage;
			VoidEffect(player, -damage, false);
			if(Main.myPlayer == player.whoAmI)
				local.netUpdate = true;
			local.voidDamageTimer = 60;
		}
        public override void PostUpdateEquips()
		{
			if (Player.isDisplayDollOrInanimate || Player.isHatRackDoll || Player.isFirstFractalAfterImage)
			{
				return;
			}
			for (int i = 0; i < Player.inventory.Length; i++)
            {
				Item item = Player.inventory[i];
				if(item.ModItem as VoidConsumable != null)
                {
					VoidConsumable vCon = item.ModItem as VoidConsumable;
					vCon.SealedUpdateInventory(Player);
				}
			}
			if (voidMeter < 0)
			{
				if (!voidShock && !voidRecovery)
				{
					SOTSPlayer sPlayer = SOTSPlayer.ModPlayer(Player);
					int time = 600;
					if (sPlayer.VMincubator || sPlayer.VoidAnomaly)
						time = 1200;
					Player.AddBuff(ModContent.BuffType<VoidShock>(), time);
					if (Player.whoAmI == Main.LocalPlayer.whoAmI)
						SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Void/Void_Shock"), (int)Player.Center.X, (int)Player.Center.Y, 0.9f);
					//if(time < 120) time = 120;
				}
				if(flatVoidRegen > -1 && Player.HasBuff(BuffType<VoidShock>()))
                {
					flatVoidRegen = -1;
                }
				Player.lifeRegen += (int)(voidMeter * 0.2f);
				if (voidMeter <= -150)
				{
					voidMeter = -150;
				}
			}
			if (Player.HasBuff(BuffType<VoidShock>()) || Player.HasBuff(BuffType<VoidRecovery>()))
			{
				int chance = 25;
				if (Player.HasBuff(BuffType<VoidRecovery>()))
					chance = 10;
				if (Main.rand.NextBool(chance))
				{
					Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 198);
					dust.noGravity = true;
					dust.velocity *= 4;
					dust.scale = 2.2f;
				}
				if (Main.rand.NextBool(chance))
				{
					Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 198);
					dust.noGravity = true;
					dust.velocity *= 3.5f;
					dust.scale = 2.7f;
				}
				if (Main.rand.NextBool(chance))
				{
					Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 198);
					dust.noGravity = true;
					dust.velocity *= 3f;
					dust.scale = 3.2f;
				}
			}
			if (Player.HasBuff(BuffType<SulfurBurn>()))
			{
				if (flatVoidRegen > 0)
					flatVoidRegen *= 0.5f;
				flatVoidRegen -= 100f;
			}
			if (Player.HasBuff(BuffType<VoidBurn>()))
			{
				if (flatVoidRegen > 0)
					flatVoidRegen *= 0.2f;
				flatVoidRegen -= 2f;
			}
			if(Player.HasBuff(BuffType<VoidMetamorphosis>()))
			{
				if (flatVoidRegen > 0)
					flatVoidRegen *= 0.5f;
				flatVoidRegen -= 6f;
			}
			base.PostUpdateEquips();
        }
		bool isFull = false;
        private void ResetVariables()
		{
			if (Player.isDisplayDollOrInanimate || Player.isHatRackDoll || Player.isFirstFractalAfterImage)
			{
				return;
			}
			//Main.NewText("I think " + player.whoAmI + " has " + voidMeterMax2 + " meter max 2 and " + voidMeter + " void and " + voidMeterMax + " void meter max 1 and " + VoidMinionConsumption + " void minion consumption");
			lastVoidMeter = voidMeter;
			ColorHelpers.ColorUpdate();
			if (soulsOnKill > 0)
				UseSouls();
			if (ColorHelpers.soulColorCounter % 40 == 0)
            {
				if(soulsOnKill <= 0)
                {
					if (lootingSouls > 0)
						lootingSouls--;
                }
				if (ColorHelpers.soulColorCounter % 360 == 0)
                {
					netUpdate = true;
				}
			}
			if (voidShock)
			{
				if (!hasEnteredVoidShock)
				{
					voidShockAnimationCounter = 1;
					//play voidshock animation
				}
				hasEnteredVoidShock = true;
			}
			else
            {
				voidShockAnimationCounter = -1;
				hasEnteredVoidShock = false;
			}
			soulsOnKill = 0;
			//percent damage grows as health lowers
			//voidDamage += 1f - (float)((float)player.statLife / (float)player.statLifeMax2);

			voidCost = 1f;
			if(Main.myPlayer == Player.whoAmI)
			{
				int newVoidConsumption = RegisterVoidMinions();
				if (VoidMinionConsumption != newVoidConsumption)
					netUpdate = true;
				VoidMinionConsumption = newVoidConsumption;
			}
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
			if (this.frozenCounter == this.frozenMinTimer - 30 && Main.myPlayer == Player.whoAmI)
			{
				SOTSUtils.PlaySound(SoundID.Item29, (int)Player.Center.X, (int)Player.Center.Y, 1.1f, -0.1f);
			}
			if (this.frozenDuration == 30 && Main.myPlayer == Player.whoAmI)
			{
				SOTSUtils.PlaySound(SoundID.Item29, (int)Player.Center.X, (int)Player.Center.Y, 1.1f, 0.3f);
			}

			frozenMaxDuration = 0;
			frozenMinTimer = 3600;
			if (voidMeter >= voidMeterMax2)
			{
				//make sure meter doesn't go above max
				voidMeter = voidMeterMax2; 
				frozenVoidCount = voidMeter;
				if(!isFull)
				{
					if (Player.whoAmI == Main.LocalPlayer.whoAmI && !frozenVoid)
						SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Void/Void_Full"), (int)Player.Center.X, (int)Player.Center.Y, 1.4f);
					isFull = true;
				}
			}
			else
            {
				if(frozenVoid)
                {
					voidMeter = frozenVoidCount;
				}
				isFull = false;
			}
			UpdateVoidRegen();
			voidMeterMax2 = voidMeterMax;
			//maxStandingTimer = 2;
			if (voidMeter != 0)
			{
				VoidUI.visible = true;
			}
			VoidFoodGainMultiplier = 1f;
			safetySwitch = false;
			safetySwitchVisual = false;
			CrushCapacitor = false;
			CrushResistor = false;
			CrushTransformer = 1f;
			BonusCrushRangeMax = 0;
			BonusCrushRangeMin = 0;
		}
		public float resolveVoidCounter = 0;
		public float resolveVoidAmount = 0;
		public float voidRegenTimer = 0;
		public float lerpingVoidMeter = 0;
		public const float voidRegenTimerMax = 900;
		public const float baseVoidGain = 5f;
		public float bonusVoidGain = 0f;
		public float voidGainMultiplier = 1f;
		public float voidRegenSpeed = 1f;
		public float flatVoidRegen = 0f;
		public int GreenBarCounter = 0;
		public float lastVoidMeter = 0;
		public float negativeVoidRegenCounter = 0;
		public float positiveVoidRegenCounter = 0;
		public int negativeVoidRegenPopupNumber = 1;
		public void UpdateVoidRegen()
		{
			if (Player.isDisplayDollOrInanimate || Player.isHatRackDoll || Player.isFirstFractalAfterImage)
			{
				return;
			}
			if (Player.HasBuff(BuffType<DilationSickness>()))
			{
				voidRegenSpeed -= 0.9f;
			}
			if (voidRegenSpeed < 0.01f)
				voidRegenSpeed = 0.01f;
			float increaseAmount = voidRegenSpeed;
			if (voidRecovery)
			{
				if (voidMeter < 0)
                {
					voidRegenTimer = voidRegenTimerMax;
					voidMeter = 0;
				}
				if (voidMeter < voidMeterMax2 / 2)
				{
					flatVoidRegen = voidMeterMax2 / 12f;
					increaseAmount = 90;
				}
				else
				{
					voidRegenTimer = 0;
					increaseAmount = 0;
				}
			}
			else if(voidShock || voidMeter >= voidMeterMax2 || Player.dead)
            {
				increaseAmount = -4;
            }
			if(!frozenVoid && !SOTSWorld.GlobalFrozen)
				voidRegenTimer += increaseAmount;

			voidRegenTimer = MathHelper.Clamp(voidRegenTimer, 0, voidRegenTimerMax);
			if(voidRegenTimer >= voidRegenTimerMax) //this is for Void GAIN
			{
				if(!voidRecovery)
				{
					float voidGain = (baseVoidGain + bonusVoidGain) * voidGainMultiplier;
					VoidEffect(Player, (int)voidGain);
					voidMeter += voidGain;
					if(VoidGenerateMoney > 0 && Main.myPlayer == Player.whoAmI)
                    {
						float generateMoneyValue = (Main.rand.NextFloat(50, 70) + 4 * voidGain) * (VoidGenerateMoney + (float)Math.Sqrt(16* VoidGenerateMoney + voidGain)) * voidGainMultiplier;
						while ((int)generateMoneyValue > 0)
						{
							if (generateMoneyValue > 1000000f)
							{
								int num4 = (int)(generateMoneyValue / 1000000f);
								generateMoneyValue -= (1000000 * num4);
								Player.QuickSpawnItem(Player.GetSource_Misc("SOTS:VoidCoins"), ItemID.PlatinumCoin, num4);
							}
							else if (generateMoneyValue > 10000f)
							{
								int num6 = (int)(generateMoneyValue / 10000f);
								generateMoneyValue -= (10000 * num6);
								Player.QuickSpawnItem(Player.GetSource_Misc("SOTS:VoidCoins"), ItemID.GoldCoin, num6);
							}
							else if (generateMoneyValue > 100f)
							{
								int num7 = (int)(generateMoneyValue / 100f);
								generateMoneyValue -= (100 * num7);
								Player.QuickSpawnItem(Player.GetSource_Misc("SOTS:VoidCoins"), ItemID.SilverCoin, num7);
							}
							else
							{
								int num8 = (int)generateMoneyValue;
								if (num8 < 1)
								{
									num8 = 1;
								}
								generateMoneyValue -= num8;
								Player.QuickSpawnItem(Player.GetSource_Misc("SOTS:VoidCoins"), ItemID.CopperCoin, num8);
							}
						}
					}						
				}
				else
					resolveVoidCounter = 15;
				voidRegenTimer = 0; 
				GreenBarCounter = 20;
			}
			float voidRegen = flatVoidRegen / 60f;
			if(voidRegen < 0) //this is negative void regen
			{
				int numberScaling = (int)(flatVoidRegen * -0.25f);
				negativeVoidRegenPopupNumber = numberScaling + 1;
				negativeVoidRegenCounter -= voidRegen;
				if (negativeVoidRegenCounter >= negativeVoidRegenPopupNumber)
				{
					negativeVoidRegenCounter -= negativeVoidRegenPopupNumber;
					if (voidMeter >= 0)
					{
						VoidEffect(Player, -negativeVoidRegenPopupNumber, true);
						if (Player.HasBuff(BuffType<VoidMetamorphosis>()))
						{
							int healAmt = (int)(1.6 * negativeVoidRegenPopupNumber);
							if (Player.whoAmI == Main.myPlayer)
								Player.HealEffect(healAmt);
							Player.statLife += healAmt;
						}
					}
					voidMeter -= negativeVoidRegenPopupNumber;
				}
			}
			else if(voidMeter >= 0) //this is positive void regen
			{
				positiveVoidRegenCounter += voidRegen;
				if(positiveVoidRegenCounter > 1)
                {
					positiveVoidRegenCounter -= 1;
					voidMeter += 1;
				}					
			}

            #region reset variables
            bonusVoidGain = 0f;
			voidGainMultiplier = 1f;
			voidRegenSpeed = 1f;
			flatVoidRegen = 0f;
			voidShock = false;
			voidRecovery = false;
			VoidGenerateMoney = 0;
			#endregion

			#region apply upgrade effects
			voidRegenSpeed += 0.02f * voidAnkh;
			voidRegenSpeed += 0.05f * voidStar;
			#endregion

			lerpingVoidMeter = MathHelper.Lerp(lerpingVoidMeter, voidMeter, 0.08f);
			if (lerpingVoidMeter - voidMeter < 0.25f)
				lerpingVoidMeter = voidMeter;
			if (lerpingVoidMeter > voidMeterMax2)
				lerpingVoidMeter = voidMeterMax2;
			if (resolveVoidCounter < 0)
			{
				resolveVoidAmount = 0;
				resolveVoidCounter = 0;
			}
			if(resolveVoidAmount > 0)
            {
				resolveVoidAmount--;
            }
			if (voidDamageTimer > 0)
				voidDamageTimer--;
			else
				voidDamageTimer = 0;
		}
    }
}

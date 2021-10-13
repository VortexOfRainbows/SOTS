using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Items;
using SOTS.Items.IceStuff;
using SOTS.Items.Otherworld;
using SOTS.Items.Otherworld.EpicWings;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Pyramid;
using SOTS.Items.SpecialDrops;
using SOTS.Items.Vibrant;
using SOTS.NPCs.Boss;
using SOTS.Projectiles.BiomeChest;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Base;
using SOTS.Projectiles.Otherworld;
using SOTS.Void;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static SOTS.SOTS;
using SOTS.Items.Pyramid.AncientGold;
using SOTS.NPCs.Boss.Curse;
using SOTS.Projectiles.Pyramid;
using SOTS.Projectiles.Minions;
using SOTS.Projectiles.Permafrost;
using SOTS.Items.Celestial;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Inferno;
using SOTS.Projectiles.Nature;
using SOTS.Items.Crushers;

namespace SOTS
{
	public class SOTSPlayer : ModPlayer
	{
		public static int[] locketBlacklist;
		public static int[] typhonBlacklist;
		public static int[] typhonWhitelist;
		public static int[] symbioteBlacklist;
		public static void LoadArrays()
		{
			locketBlacklist = new int[] { ItemID.BookStaff, ModContent.ItemType<LashesOfLightning>(), ModContent.ItemType<SkywardBlades>(), ItemID.GolemFist, ItemID.Flairon,
				ModContent.ItemType<PhaseCannon>(), ModContent.ItemType<Items.Otherworld.FromChests.HardlightGlaive>(), ModContent.ItemType<StarcoreAssaultRifle>(), ModContent.ItemType<VibrantPistol>(),
				ModContent.ItemType<Items.Otherworld.FromChests.SupernovaHammer>(), ItemID.MonkStaffT1, ModContent.ItemType<Items.IceStuff.FrigidJavelin>(), ModContent.ItemType<Items.DigitalDaito>() };
			typhonBlacklist = new int[] { ModContent.ProjectileType<ArcColumn>(), ModContent.ProjectileType<PhaseColumn>(), ModContent.ProjectileType<MacaroniBeam>(), ModContent.ProjectileType<GenesisArc>(), ModContent.ProjectileType<GenesisCore>() };
			symbioteBlacklist = new int[] { ModContent.ProjectileType<BloomingHook>(), ModContent.ProjectileType<BloomingHookMinion>() };
			typhonWhitelist = new int[] { ModContent.ProjectileType<HardlightArrow>() };
		}
		/*
		public override TagCompound Save() {
			return new TagCompound {
				
				{"soulAmount", soulAmount},
				};
		}
		public override void Load(TagCompound tag) 
		{
			soulAmount = tag.GetInt("soulAmount");
		}
		*/
		public static SOTSPlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<SOTSPlayer>();
		}
		public void TrailStuff()
		{
			FluidCurse = false;
			if (player.HasBuff(ModContent.BuffType<FluidCurse>()))
            {
				PetFluidCurse();
				FluidCurse = true;
			}
			float mult = player.statLife / (float)player.statLifeMax2;
			if (mult < 0) mult = 0;
			mult = (float)Math.Sqrt(mult);
			if (mult > 1) mult = 1;
			FluidCurseMult = 4 + (int)(60 * (1 - mult));
			if (FluidCurseMult > 60)
				FluidCurseMult = 60;
		}
		public bool CanKillNPC = false;
		public bool CreativeFlightButtonPressed = false;
		public bool CurseAura = false;
		public bool FluidCurse = false;
		public float FluidCurseMult = 120;
		public bool petPepper = false;
		public bool petAdvisor = false;
		public int petPinky = -1;
		public int symbioteDamage = -1;
		public bool rippleEffect = false;
		public int rippleTimer = 0;
		public int rippleBonusDamage = 0;
		public bool doomDrops = false;
		public bool baguetteDrops = false;
		public int baguetteLength = 0;
		public int baguetteLengthCounter = 0;
		Vector2 playerMouseWorld;
		public int lightDragon = -1;
		public int halfLifeRegen = 0;
		public int additionalHeal = 0;
		public int darkEyeShader = 0;
		public int HoloEyeDamage = 0;
		public bool HoloEye = false;
		public bool HoloEyeAttack = false;
		public bool HoloEyeAutoAttack = false;
		public float blinkPackMult = 1f;
		public bool rainbowGlowmasks = false;
		public int skywardBlades = 0;
		public float cursorRadians = 0;
		public float BlinkedAmount = 0;
		public int BlinkType = 0;
		public int BlinkDamage = 0;
		public int typhonRange = 0;
		public bool weakerCurse = false;
		public bool vibrantArmor = false;
		public int brokenFrigidSword = 0;
		public int shardSpellExtra = 0;
		public int frigidJavelinBoost = 0;
		public bool frigidJavelinNoCost = false;
		public int orbitalCounter = 0;
		public int shardOnHit = 0;
		public int bonusShardDamage = 0;
		public int phaseCannonIndex = -1;
		public float assassinateNum = 1;
		public int assassinateFlat = 0;
		public bool assassinate = false;
		public int polarCannons = 0;

		public Vector2 starCen;
		private const int saveVersion = 0;

		public int mourningStarFire = 0;

		public bool deoxysPet = false;

		public bool DapperChu = false;

		public bool TurtleTem = false;

		public bool PlanetariumBiome = false;
		//public bool GeodeBiome = false;
		public bool PyramidBiome = false;
		public bool chessSkip = false;
		public bool backUpBow = false;
		public int doubledActive = 0;
		public int doubledAmount = 0;
		public bool ceres = false;
		public bool megHat = false;
		public bool megShirt = false;
		public bool megSet = false;
		public int megSetDamage = 0;
		public bool orion = false;
		public bool lostSoul = false;
		public int onhit = 0;
		public int onhitdamage = 0;
		public float attackSpeedMod = 0;
		//some important variables 2

		public bool PurpleBalloon = false;
		public int StartingDamage = 0;
		public bool ItemDivision = false;
		public bool PushBack = false; // marble protecter effect

		public bool pearlescentMagic = false; //pearlescent core effect
		public bool bloodstainedJewel = false; //bloodstained jewel effect
		public bool snakeSling = false; //snakeskin sling effect

		public int CritLifesteal = 0; //crit clover
		public float maxCritLifestealPerSecond = 0;
		public float maxCritLifestealPerSecondTimer = 0;
		public float CritManasteal = 0f; //starbelt
		public float maxCritManastealPerSecond = 0;
		public float maxCritManastealPerSecondTimer = 0;
		public float CritVoidsteal = 0f; //crit void charm
		public float maxCritVoidStealPerSecond = 0;
		public float maxCritVoidStealPerSecondTimer = 0;
		public int CritBonusDamage = 0; //crit coin + amplfiier
		public bool CritFire = false; //hellfire icosahedron
		public bool CritFrost = false; //borealis icosahedron
		public bool CritCurseFire = false; //cursed icosahedron
		public bool netUpdate = false;
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			TestWingsPlayer testPlayer = player.GetModPlayer<TestWingsPlayer>();
			VoidPlayer voidPlayer = player.GetModPlayer<VoidPlayer>();
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)SOTSMessageType.SOTSSyncPlayer);
			packet.Write((byte)player.whoAmI);
			packet.Write(orbitalCounter);
			packet.Write(testPlayer.creativeFlight);
			packet.Write(voidPlayer.lootingSouls);
			packet.Send(toWho, fromWho);
		}
		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			// Here we would sync something like an RPG stat whenever the player changes it.
			SOTSPlayer clone = clientPlayer as SOTSPlayer;
			if(netUpdate)
			{
				if (clone.orbitalCounter != orbitalCounter)
				{
					// Send a Mod Packet with the changes.
					var packet = mod.GetPacket();
					packet.Write((byte)SOTSMessageType.OrbitalCounterChanged);
					packet.Write((byte)player.whoAmI);
					packet.Write(orbitalCounter);
					packet.Send();
				}
				if (clone.skywardBlades != skywardBlades)
				{
					// Send a Mod Packet with the changes.
					var packet = mod.GetPacket();
					packet.Write((byte)SOTSMessageType.SyncPlayerKnives);
					packet.Write((byte)player.whoAmI);
					packet.Write(skywardBlades);
					packet.Write(cursorRadians);
					packet.Send();
				}
				netUpdate = false;
			}
		}
		int foamParticleCounter = 0;
		public List<CurseFoam> foamParticleList1 = new List<CurseFoam>();
		public void FoamStuff()
		{
			for (int i = 0; i < foamParticleList1.Count; i++)
			{
				CurseFoam particle = foamParticleList1[i];
				particle.Update();
				if (!particle.active)
				{
					particle = null;
					foamParticleList1.RemoveAt(i);
					i--;
				}
				else
				{
					particle.Update();
					if (!particle.active)
					{
						particle = null;
						foamParticleList1.RemoveAt(i);
						i--;
					}
					else if (!particle.noMovement)
						particle.position += player.velocity * 0.85f;
				}
			}
			foamParticleCounter++;
			if (foamParticleCounter >= 1200)
			{
				foamParticleCounter = 0;
				ResetFoamLists();
			}
		}
		public void ResetFoamLists()
		{
			List<CurseFoam> temp = new List<CurseFoam>();
			for (int i = 0; i < foamParticleList1.Count; i++)
			{
				if (foamParticleList1[i].active && foamParticleList1[i] != null)
					temp.Add(foamParticleList1[i]);
			}
			foamParticleList1 = new List<CurseFoam>();
			for (int i = 0; i < temp.Count; i++)
			{
				foamParticleList1.Add(temp[i]);
			}
		}
		public int bladeAlpha = 0;
		public static readonly PlayerLayer BladeEffectBack = new PlayerLayer("SOTS", "BladeEffectBack", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo) 
		{
			Mod mod = ModLoader.GetMod("SOTS");	
			Player drawPlayer = drawInfo.drawPlayer;
			SOTSPlayer modPlayer = drawPlayer.GetModPlayer<SOTSPlayer>();
			if (drawInfo.shadow != 0)
				return;
			if (modPlayer.skywardBlades > 0 && !drawPlayer.dead)
			{
				float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;
				float drawY = (int)drawInfo.position.Y + drawPlayer.height / 2;
				int amt = modPlayer.skywardBlades;
				float total = amt * 8;
				Color color2 = Color.White.MultiplyRGBA(Lighting.GetColor((int)drawX / 16, (int)drawY / 16));
				drawX -= Main.screenPosition.X;
				drawY -= Main.screenPosition.Y;
				for (int i = 0; i < amt; i++)
				{
					Color color = color2;
					float number = 0;
					if(i == 0)
						number = 0;
					if (i == 1)
						number = -7.5f;
					if (i == 2)
						number = 7.5f;
					if (i == 3)
						number = -15;
					if (i == 4)
						number = 15;
					Vector2 moveDraw = new Vector2(64, 0).RotatedBy(modPlayer.cursorRadians + MathHelper.ToRadians(number));
					Texture2D texture = mod.GetTexture("Projectiles/Otherworld/SkywardBladeBeam");
					DrawData data = new DrawData(texture, new Vector2(drawX, drawY) + moveDraw, null, color * ((255 - modPlayer.bladeAlpha)/255f), modPlayer.cursorRadians - 0.5f * MathHelper.ToRadians(number) + MathHelper.ToRadians(90), new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
					Main.playerDrawData.Add(data);

					int recurse = 1;
					if (modPlayer.rainbowGlowmasks)
					{
						color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
						recurse = 2;
					}
					for (int j = 0; j < recurse; j++)
					{
						texture = mod.GetTexture("Projectiles/Otherworld/SkywardBladeGlowmask");
						data = new DrawData(texture, new Vector2(drawX, drawY) + moveDraw, null, color * ((255 - modPlayer.bladeAlpha) / 255f), modPlayer.cursorRadians - 0.5f * MathHelper.ToRadians(number) + MathHelper.ToRadians(90), new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
						Main.playerDrawData.Add(data);
					}
				}
			}
		});
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			BladeEffectBack.visible = true;
			layers.Insert(0, BladeEffectBack);
		}
		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (SOTS.BlinkHotKey.JustPressed)
			{
				if (BlinkType == 1 && !player.HasBuff(BuffID.ChaosState) && !player.mount.Active && !(player.grappling[0] >= 0) && !player.frozen)
				{
					Vector2 toCursor = Main.MouseWorld - player.Center;
					Projectile.NewProjectile(player.Center, toCursor.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<Blink1>(), 0, 0, player.whoAmI);
				}
			}
			if (SOTS.ArmorSetHotKey.JustPressed)
			{
				HoloEyeAttack = true;
			}
			else
			{
				HoloEyeAttack = false;
			}
			if (SOTS.MachinaBoosterHotKey.JustPressed)
			{
				CreativeFlightButtonPressed = true;
			}
			else
			{
				CreativeFlightButtonPressed = false;
			}
		}
		int Probe = -1;
		int Probe2 = -1;
		int Probe3 = -1;
		int Probe4 = -1;
		int Probe5 = -1;
		int Probe6 = -1;
		public void PetAdvisor()
        {
			if (Main.myPlayer == player.whoAmI)
			{
				if (Probe == -1)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("AdvisorPet"), 0, 0, player.whoAmI, 0);
				}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("AdvisorPet") || Main.projectile[Probe].owner != player.whoAmI)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("AdvisorPet"), 0, 0, player.whoAmI, 0);
				}
				Main.projectile[Probe].timeLeft = 6;
			}
		}
		public void PetPepper()
		{
			if (Main.myPlayer == player.whoAmI)
			{
				if (Probe2 == -1)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<GhostPepper>(), 0, 0, player.whoAmI, 0);
				}
				if (!Main.projectile[Probe2].active || Main.projectile[Probe2].type != ModContent.ProjectileType<GhostPepper>() || Main.projectile[Probe2].owner != player.whoAmI)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<GhostPepper>(), 0, 0, player.whoAmI, 0);
				}
				Main.projectile[Probe2].timeLeft = 6;
			}
		}
		public void PetHoloEye()
		{
			if (Main.myPlayer == player.whoAmI)
			{
				if (Probe3 == -1)
				{
					Probe3 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<HoloEye>(), 1 + HoloEyeDamage, 0, player.whoAmI, 0);
				}
				if (!Main.projectile[Probe3].active || Main.projectile[Probe3].type != ModContent.ProjectileType<HoloEye>() || Main.projectile[Probe3].owner != player.whoAmI)
				{
					Probe3 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<HoloEye>(), 1 + HoloEyeDamage, 0, player.whoAmI, 0);
				}
				Main.projectile[Probe3].timeLeft = 6;
			}
		}
		public void PetPinky()
		{
			if (Main.myPlayer == player.whoAmI)
			{
				if (Probe4 == -1)
				{
					Probe4 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<PetPutridPinkyCrystal>(), petPinky, 0, player.whoAmI, 0);
				}
				if (!Main.projectile[Probe4].active || Main.projectile[Probe4].type != ModContent.ProjectileType<PetPutridPinkyCrystal>() || Main.projectile[Probe4].owner != player.whoAmI)
				{
					Probe4 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<PetPutridPinkyCrystal>(), petPinky, 0, player.whoAmI, 0);
				}
				Main.projectile[Probe4].timeLeft = 6;
			}
		}
		public void PetFluidCurse()
		{
			if (Main.myPlayer == player.whoAmI)
			{
				if (Probe5 == -1)
				{
					Probe5 = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("FluidFollower"), 0, 0, player.whoAmI);
				}
				if (!Main.projectile[Probe5].active || Main.projectile[Probe5].type != mod.ProjectileType("FluidFollower") || Main.projectile[Probe5].owner != player.whoAmI)
				{
					Probe5 = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("FluidFollower"), 0, 0, player.whoAmI);
				}
				if (Probe6 == -1)
				{
					Probe6 = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ModContent.ProjectileType<ClairvoyanceShade>(), 0, 0, player.whoAmI);
				}
				if (!Main.projectile[Probe6].active || Main.projectile[Probe6].type != ModContent.ProjectileType<ClairvoyanceShade>() || Main.projectile[Probe6].owner != player.whoAmI)
				{
					Probe6 = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ModContent.ProjectileType<ClairvoyanceShade>(), 0, 0, player.whoAmI);
				}
			}
		}
		public void doCurseAura()
        {
			if(CurseAura)
				for(int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if(npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage && Vector2.Distance(npc.Center, player.Center) <= 270 && npc.realLife == -1)
					{
						npc.AddBuff(ModContent.BuffType<Buffs.PharaohsCurse>(), 120);
					}
				}
        }
        public override void PostUpdateMiscEffects()
		{
			Vector2 detect = AncientGoldSpikeTile.HurtTiles(player.position, player.width, player.height);
			if(detect.Y != 0f)
			{
				int damage3 = Main.DamageVar(50);
				player.Hurt(PlayerDeathReason.ByOther(3), damage3, 0, false, false, false, 0);
			}
			base.PostUpdateMiscEffects();
        }
        public override void PostUpdate()
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			maxCritVoidStealPerSecond = voidPlayer.voidRegen * 20; //max stored voidsteal is 20x the voidRegen speed
			maxCritVoidStealPerSecondTimer += (voidPlayer.voidRegen + CritVoidsteal / 10f) / 30f; //max stored voidsteal regenerates at the twice rate as normal voidRegen (basically, stores 20 seconds of regen) 
			//Add critvoidsteal to the timer in some way to make it scale well with multiple voidsteal accessories. Same logic applies to other stat steals
			if (maxCritVoidStealPerSecondTimer > maxCritVoidStealPerSecond)
			{
				maxCritVoidStealPerSecondTimer = maxCritVoidStealPerSecond;
			}

			maxCritLifestealPerSecond = (player.lifeRegen * 3) + 6; //max stored lifesteal is 3x lifeRegen (1 lifeRegen = 0.5 life per second) speed + 6 
			maxCritLifestealPerSecondTimer += (player.lifeRegen + 3 + CritLifesteal) / 60f; //max stored lifesteal regenerates at the twice rate as normal regen (excluding movement based factors) (basically regenerates 3 life to the pool per second, faster with increased regen)
			if (maxCritLifestealPerSecondTimer > maxCritLifestealPerSecond)
			{
				maxCritLifestealPerSecondTimer = maxCritLifestealPerSecond;
			}

			maxCritManastealPerSecond = 30 + player.statManaMax2 / 6; //max stored manasteal is 30 + 1/6th of the mana max
			maxCritManastealPerSecondTimer += (6f + CritManasteal / 1.5f) / 60f; //max stored voidsteal regenerates at the twice rate as normal voidRegen (basically regenerates 6 mana to the pool per second, the pool grows with larger max mana)
			if (maxCritManastealPerSecondTimer > maxCritManastealPerSecond)
			{
				maxCritManastealPerSecondTimer = maxCritManastealPerSecond;
			}
			base.PostUpdate();
        }
        public override bool? CanHitNPC(Item item, NPC target)
        {
			if(CanKillNPC && item.melee && target.townNPC)
            {
				return true;
            }
            return base.CanHitNPC(item, target);
        }
        public override void PreUpdate()
		{
			FoamStuff();
			base.PreUpdate();
        }
        public override void ResetEffects()
		{
			TrailStuff();
			doCurseAura(); 
			CanKillNPC = false;
			baguetteDrops = false;
			if (baguetteLengthCounter >= 180)
			{
				if(baguetteLength > 0)
					baguetteLength--;
				baguetteLengthCounter = baguetteLength * 3;
			}
			if (baguetteLength > 0)
				baguetteLengthCounter++;
			else
            {
				baguetteLengthCounter = 0;
            }
			doomDrops = false;
			player.lifeRegen += halfLifeRegen / 2;
			halfLifeRegen = 0;
			if (player.HasBuff(BuffID.ChaosState))
            {
				BlinkedAmount = 0;
			}
			if(BlinkedAmount > 0 && BlinkedAmount < 2)
            {
				BlinkedAmount -= 0.002f;
				if (BlinkedAmount < 0) BlinkedAmount = 0;
			}
			if(player.whoAmI == Main.myPlayer)
			{
				cursorRadians = (Main.MouseWorld - player.Center).ToRotation();
				if(skywardBlades >= 0)
				{
					netUpdate = true;
				}
				if(skywardBlades == 0)
                {
					skywardBlades = -1;
					netUpdate = true;
				}
			}
			if (skywardBlades >= 0)
			{
				if (player.HeldItem.type == mod.ItemType("SkywardBlades"))
				{
					if (bladeAlpha > 0)
						bladeAlpha -= 5;
					else
						bladeAlpha = 0;
				}
				else
				{
					if (bladeAlpha < 255)
						bladeAlpha += 5;
					else
						bladeAlpha = 255;
				}
			}
			additionalHeal = 0;
			HoloEyeAutoAttack = false;
			blinkPackMult = 1f;
			BlinkDamage = 0;
			BlinkType = 0;
			if (petAdvisor)
				PetAdvisor();
			if (petPepper)
				PetPepper();
			if (HoloEye)
				PetHoloEye();
			if (petPinky >= 0)
				PetPinky();
			if(rippleEffect)
			{
				float healthPercent = (float)player.statLife / (float)player.statLifeMax2;
				int timerMax = (int)(70 * healthPercent) + 20;
				if(rippleTimer > timerMax)
				{
					if (Main.myPlayer == player.whoAmI)
						Projectile.NewProjectile(player.Center, new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))), ModContent.ProjectileType<Projectiles.Tide.RippleWave>(), 20 + rippleBonusDamage, 0f, player.whoAmI, 1, 0);
					rippleTimer -= timerMax;
                }
			}
			else
            {
				rippleTimer = 0;
            }
			rippleEffect = false;
			rippleBonusDamage = 0;
			symbioteDamage = -1;
			petPinky = -1;
			petPepper = false;
			petAdvisor = false; 
			rainbowGlowmasks = false; 
			HoloEye = false;
			HoloEyeDamage = 0;
			darkEyeShader = 0;
			for (int i = 9 + player.extraAccessorySlots; i < player.armor.Length; i++) //checking vanity slots
            {
				Item item = player.armor[i];
				if(item.type == ModContent.ItemType<CursedApple>())
				{
					petPepper = true;
				}
				if (item.type == ModContent.ItemType<Calculator>())
				{
					petAdvisor = true;
				}
				if (item.type == ModContent.ItemType<PeanutButter>())
				{
					petPinky = 0;
				}
				if (item.type == ModContent.ItemType<SkywareBattery>())
				{
					rainbowGlowmasks = true;
				}
				if (item.type == ModContent.ItemType<TwilightAssassinsCirclet>())
				{
					if (!HoloEye)
						HoloEyeDamage += (int)(33 * (1f + (player.minionDamage - 1f) + (player.allDamage - 1f)));
					HoloEye = true;
				}
				if (item.type == ModContent.ItemType<TestWings>())
				{
					TestWingsPlayer testWingsPlayer = (TestWingsPlayer)player.GetModPlayer(mod, "TestWingsPlayer");
					if(!testWingsPlayer.canCreativeFlight)
                    {
						testWingsPlayer.HaloDust();
					}
				}
				/*if (item.type == ModContent.ItemType<SubspaceLocket>())
				{
					SubspacePlayer.ModPlayer(player).subspaceServantShader = GameShaders.Armor.GetShaderIdFromItemId(player.dye[i].type);
				}*/
			}
			for (int i = 0; i < 10; i++) //iterating through armor + accessories
			{
				Item item = player.armor[i];
				if (item.type == ModContent.ItemType<TheDarkEye>())
				{
					darkEyeShader = GameShaders.Armor.GetShaderIdFromItemId(player.dye[i].type);
				}
				/*if (item.type == ModContent.ItemType<SubspaceLocket>())
				{
					SubspacePlayer.ModPlayer(player).subspaceServantShader = GameShaders.Armor.GetShaderIdFromItemId(player.dye[i].type);
				}*/
			}
			for (int i = 0; i < player.inventory.Length; i++)
			{
				Item item = player.inventory[i];
				if (item.type == ModContent.ItemType<TwilightAssassinsCirclet>() && item.favorited)
				{
					if (!HoloEye)
						HoloEyeDamage += (int)(33 * (1f + (player.minionDamage - 1f) + (player.allDamage - 1f)));
					HoloEye = true;
					break;
				}
			}
			typhonRange = 0;
			assassinateFlat = 0;
			assassinateNum = 1;
			assassinate = false;
			vibrantArmor = false;
			shardSpellExtra = 0;
			frigidJavelinBoost = 0;
			frigidJavelinNoCost = false;
			brokenFrigidSword = brokenFrigidSword > 0 ? brokenFrigidSword - 1 : brokenFrigidSword;
			orbitalCounter++;
			if (orbitalCounter % 360 == 0)
			{
				netUpdate = true;
			}
			shardOnHit = 0;
			bonusShardDamage = 0;
			playerMouseWorld = Main.MouseWorld;
			if (onhit > 0)
			{
				onhit--;
			}
			attackSpeedMod = 0;
			//Some important variables 1
			lostSoul = false;
			orion = false;
			megSet = false;
			megShirt = false;
			megHat = false;
			ceres = false;
			doubledActive = 0;
			backUpBow = false;
			deoxysPet = false;
			DapperChu = false;
			TurtleTem = false;
			chessSkip = false;
			//DevilSpawn = false;	
			PurpleBalloon = false;
			ItemDivision = false;
			//projectileSize = 1;
			PushBack = false;
			pearlescentMagic = false;
			bloodstainedJewel = false;
			snakeSling = false;
			CritLifesteal = 0;
			CritVoidsteal = 0f;
			CritManasteal = 0f;
			CritBonusDamage = 0;
			CritFire = false;
			CritFrost = false;
			CritCurseFire = false;
			CurseAura = false;
			if (PyramidBiome)
				player.AddBuff(ModContent.BuffType<Buffs.PharaohsCurse>(), 16, false); 
			polarCannons = 0;
		}
		public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
		{
			//Fish Set 1

			if (Main.rand.NextBool(24) && player.ZoneSkyHeight) 
				caughtType = ModContent.ItemType<TinyPlanetFish>(); 

			//if (Main.rand.Next(200) == 0 && ZeplineBiome) {
			//caughtType = mod.ItemType("ZephyriousZepline"); }
			//if (Main.rand.Next(330) == 1 && liquidType == 2 && poolSize >= 500)   {
			//caughtType = mod.ItemType("ScaledFish");}

			//Fish Set 2

			if (player.ZoneBeach && liquidType == 0 && Main.rand.NextBool(175)) 
				caughtType = ModContent.ItemType<SpikyPufferfish>(); 
			if (player.ZoneBeach && liquidType == 0 && Main.rand.NextBool(225)) 
				caughtType = ModContent.ItemType<CrabClaw>(); 


			if (ScaleCatch2(power, 0, 90, 150, 750) && player.ZoneBeach && liquidType == 0) 
				caughtType = ModContent.ItemType<PinkJellyfishStaff>(); 
			else if (ScaleCatch2(power, 0, 70, 30, 150) && player.ZoneBeach && liquidType == 0 && bait.type == ItemID.PinkJellyfish) //Checks for pink jellyfish bait
				caughtType = ModContent.ItemType<PinkJellyfishStaff>();

			if (ScaleCatch2(power, 0, 90, 150, 750) && player.ZoneRockLayerHeight && liquidType == 0)
				caughtType = ModContent.ItemType<BlueJellyfishStaff>();
			else if (ScaleCatch2(power, 0, 70, 30, 150) && player.ZoneRockLayerHeight && liquidType == 0 && bait.type == ItemID.BlueJellyfish) //Checks blue jellyfish bait
				caughtType = ModContent.ItemType<BlueJellyfishStaff>();
			else if (ScaleCatch2(power, 0, 70, 30, 150) && player.ZoneRockLayerHeight && liquidType == 0 && bait.type == ItemID.GreenJellyfish) //Checks green jellyfish bait
				caughtType = ModContent.ItemType<BlueJellyfishStaff>();

			if (ScaleCatch2(power, 0, 30, 5, 10) && PyramidBiome && liquidType == 0) 
				caughtType = ModContent.ItemType<SeaSnake>(); 
			else if (ScaleCatch2(power, 0, 40, 7, 11) && PyramidBiome && liquidType == 0) 
				caughtType = ModContent.ItemType<PhantomFish>(); 
			else if (ScaleCatch2(power, 20, 80, 7, 20) && PyramidBiome && liquidType == 0) //gains the same rarity as Phantom Fish when at 80, fails to catch below 20 power
				caughtType = ModContent.ItemType<Curgeon>(); 
			else if (ScaleCatch2(power, 0, 200, 100, 300) && PyramidBiome && liquidType == 0) //1/300 at 0, 1/200 at 100, 1/100 at 200, etc
				caughtType = ModContent.ItemType<ZephyrousZeppelin>(); 
			else if (ScaleCatch2(power, 0, 200, 100, 300) && PyramidBiome && liquidType == 0) //1/300 at 0, 1/200 at 100, 1/100 at 200, etc
				caughtType = ItemID.ZephyrFish; 
			else if (!player.HasBuff(BuffID.Crate))
			{
				if (ScaleCatch2(power, 0, 200, 20, 200) && PyramidBiome && liquidType == 0) 
					caughtType = ModContent.ItemType<PyramidCrate>(); 
			}
			else
			{
				if (ScaleCatch2(power, 0, 200, 10, 100) && PyramidBiome && liquidType == 0) 
					caughtType = ModContent.ItemType<PyramidCrate>(); 
			}

		}
		/** minPower is the minimum power required, and yields a 1/maxRate chance of catching
		*	maxPower is the maximum power required, and yields a 1/minRate chance of catching
		*	rates are overall rounded down
		*	anything below minPower will fail to catch
		*	pre condition: minPower < maxPower, minRate < maxRate
		*	post condition: returns true at a specific chance.
		*/
		public static bool ScaleCatch2(int power, int minPower, int maxPower, int minRate, int maxRate)
		{
			if (power < minPower)
			{
				return false;
			}
			int fixRate = maxRate - minRate;
			power -= minPower;
			maxPower -= minPower;
			float powerRate = (float)power / maxPower;
			int rate = maxRate - (int)(fixRate * powerRate);
			if (rate < minRate)
			{
				rate = minRate;
			}
			return Main.rand.Next(rate) == 0;
		}
		/*
		public int scaleCatch(int power, int minPower, int maxPower, int minRate, int maxRate)
		{
			int fixRate = maxRate - minRate;
			power -= minPower;
			maxPower -= minPower;
			float powerRate = (float)power / maxPower;
			int rate = maxRate - (int)(fixRate * powerRate);
			if(rate > maxRate)
			{
				rate = maxRate;
			}
			if(rate < minRate)
			{
				rate = minRate;
			}
			return rate;
		}
		*/
		public override void UpdateBiomes()
		{
			PlanetariumBiome = (SOTSWorld.planetarium > 100) && player.Center.Y < Main.worldSurface * 16 * 0.6f;
			//GeodeBiome = (SOTSWorld.geodeBiome > 300);

			//checking for background walls
			int tileBehindX = (int)(player.Center.X / 16);
			int tileBehindY = (int)(player.Center.Y / 16);
			Tile tile = Framing.GetTileSafely(tileBehindX, tileBehindY);
			if (SOTSWall.unsafePyramidWall.Contains(tile.wall) || tile.wall == (ushort)ModContent.WallType<TrueSandstoneWallWall>())
			{
				PyramidBiome = true;
			}
			else
			{
				PyramidBiome = SOTSWorld.pyramidBiome > 0; //if there is a sarcophagus or zepline block on screen
			}
		}
		public override bool CustomBiomesMatch(Player other)
		{
			var modOther = other.GetModPlayer<SOTSPlayer>();
			return PyramidBiome == modOther.PyramidBiome && PlanetariumBiome == modOther.PlanetariumBiome;
		}
		public override void CopyCustomBiomesTo(Player other)
		{
			var modOther = other.GetModPlayer<SOTSPlayer>();
			modOther.PyramidBiome = PyramidBiome;
			modOther.PlanetariumBiome = PlanetariumBiome;
		}
		public override void SendCustomBiomes(BinaryWriter writer)
		{
			BitsByte flags = new BitsByte();
			flags[0] = PyramidBiome;
			flags[1] = PlanetariumBiome;
			writer.Write(flags);
		}
		public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
		{
			base.ModifyHitByNPC(npc, ref damage, ref crit);
		}
		public override void ReceiveCustomBiomes(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			PyramidBiome = flags[0];
			PlanetariumBiome = flags[1];
		}
		public override void OnHitByNPC(NPC npc, int damage, bool crit)
		{
			onhitdamage = damage;
			onhit = 2;
			if (PushBack)
			{
				float dX = npc.Center.X - player.Center.X;
				float dY = npc.Center.Y - player.Center.Y;
				float distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
				float speed = 16.0f / distance;
				dX *= speed;
				dY *= speed;
				if(Main.myPlayer == player.whoAmI)
				{
					int Proj = Projectile.NewProjectile(npc.Center.X - dX * 5, npc.Center.Y - dY * 5, dX, dY, ProjectileID.JavelinFriendly, 12, 25f, player.whoAmI);
					Main.projectile[Proj].timeLeft = 15;
					Main.projectile[Proj].netUpdate = true;
				}
			}
		}
		public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
		{
			onhitdamage = damage;
			onhit = 2;
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			if (orion == true)
			{
				float amount = 0;
				for (int j = Main.rand.Next(2); j == 0; j = Main.rand.Next((int)(1 + amount)))
				{
					amount++;
				}
				for (int i = Main.rand.Next(200); amount > 0; i = Main.rand.Next(200))
				{
					NPC target2 = Main.npc[i];

					float shootFromX = target.Center.X;
					float shootFromY = target.Center.Y;

					if (target2.Center.X >= target.Center.X)
						shootFromX += target.width;

					if (target2.Center.X <= target.Center.X)
						shootFromX -= target.width;

					if (target2.Center.Y >= target.Center.Y)
						shootFromY += target.height;

					if (target2.Center.Y <= target.Center.Y)
						shootFromY -= target.height;

					float shootToX = target2.position.X + target2.width * 0.5f - shootFromX;
					float shootToY = target2.position.Y + target2.height * 0.5f - shootFromY;
					float distance = (float)Math.Sqrt((shootToX * shootToX + shootToY * shootToY));

					if (distance < 320f && !target2.friendly && target2.active)
					{
						if (amount > 0)
						{
							amount--;

							distance = 0.2f / distance;

							shootToX *= distance * 5;
							shootToY *= distance * 5;

							Projectile.NewProjectile(shootFromX, shootFromY, shootToX, shootToY, mod.ProjectileType("OrionChain"), (int)(proj.damage * 0.75f), 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
						}
					}
					else
					{
						amount -= 0.01f;
					}
				}
			}
			if (megShirt == true)
			{
				if (Main.rand.Next(35) == 0)
				{
					Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, ItemID.Heart);
				}
				if (Main.rand.Next(35) == 0)
				{
					Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, ItemID.Star);
				}
			}
		}
		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			Vector2 vector14;

			if (player.gravDir == 1f)
			{
				vector14.Y = (float)Main.mouseY + Main.screenPosition.Y;
			}
			else
			{
				vector14.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
			}
			vector14.X = (float)Main.mouseX + Main.screenPosition.X;
			if (megShirt == true)
			{
				if (Main.rand.Next(35) == 0)
				{
					Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, ItemID.Heart);
				}
				if (Main.rand.Next(35) == 0)
				{
					Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, ItemID.Star);
				}
			}
		}
		public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
		{
			/*
			Item item = new Item();
			item.SetDefaults(mod.ItemType("ImitationCrate"));
			item.stack = 1;        
			items.Add(item); */
		}
		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			float downgrade = 0.5f;
			if(shardOnHit > 0 && damage > 5)
			{
				for(int i = 0; i < shardOnHit; i++)
				{
					Vector2 circularSpeed = new Vector2(0, -12).RotatedBy(MathHelper.ToRadians(i * (360f/shardOnHit)));
					if(Main.myPlayer == player.whoAmI)
						Projectile.NewProjectile(player.Center.X, player.Center.Y, circularSpeed.X, circularSpeed.Y, mod.ProjectileType("ShatterShard"), 10 + bonusShardDamage, 3f, player.whoAmI);
				}
			}
			if(Main.expertMode == true)
			{
				downgrade = 0.75f;
			}
			if(megSet == true)
			{
				
				if(player.statLife < damage - (player.statDefense * downgrade) && player.statMana > 0 && player.statManaMax > 0)
				{
					player.AddBuff(mod.BuffType("ManaCut"), 18000, false);
					megSetDamage += -(int)(player.statLife - (damage - (player.statDefense * downgrade)));
					damage = 0;
					player.statLife = player.statMana;
				}
			}
			return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
		}
		int shotCounter = 0;
		public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			shotCounter++;
			
			Vector2 cursorPos = playerMouseWorld;
			
			float shootCursorX = player.Center.X - cursorPos.X;
			float shootCursorY = player.Center.Y - cursorPos.Y;
			Vector2 toCursor = new Vector2(-1, 0).RotatedBy(Math.Atan2(shootCursorY, shootCursorX));
			
			if(PurpleBalloon && item.fishingPole > 0)
			{
				  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(50));
				  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("PurpleBobber"), damage, type, player.whoAmI);
				  //return false;
			}
			if(snakeSling && item.ranged && item.damage > 3 && shotCounter % 5 == 0)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8));
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X * 1.1f, perturbedSpeed.Y * 1.1f, mod.ProjectileType("Pebble"), damage, knockBack, player.whoAmI);
			}
			if(backUpBow && item.ranged)
			{
				Vector2 perturbedSpeed = -new Vector2(speedX, speedY);
				Projectile.NewProjectile(position, perturbedSpeed, ModContent.ProjectileType<BackupArrow>(), (int)(damage * 0.45f) + 1, knockBack, player.whoAmI);
			}
			if(doubledActive == 1 && item.fishingPole > 0)
			{
				for(int i = doubledAmount; i > 0; i--)
				{
					Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(i % 2 == 0 ? i * 6 : i * -6));
					Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				}
			}
			return true;
		}
		public override void OnRespawn(Player player)
		{
			megSet = false;
			megSetDamage = 0;
		}
		public override float UseTimeMultiplier(Item item)
		{
			float standard = 1 + attackSpeedMod;
			int time = item.useAnimation;
			int cannotPass = 2;
			float current = time / standard;
			if (current < cannotPass)
			{
				standard = time / 2f;
			}
			if (item.channel == false || item.type == ModContent.ItemType<OlympianAxe>())
				return standard;
			return base.UseTimeMultiplier(item);
		}
		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			ModifyHitNPCGeneral(target, proj, null, ref damage, ref knockback, ref crit);
		}
		public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit) 
		{
			ModifyHitNPCGeneral(target, null, item, ref damage, ref knockback, ref crit);
		}
		public void ModifyHitNPCGeneral(NPC target, Projectile projectile, Item item, ref int damage, ref float knockback, ref bool crit)
        {
			if (crit)
			{
				if (CritManasteal > 0 && maxCritManastealPerSecondTimer > 0)
				{
					maxCritManastealPerSecondTimer -= CritManasteal;
					if (Main.myPlayer == player.whoAmI)
						Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<HealProj>(), 1, 0, player.whoAmI, CritManasteal, 3);
				}
				if (CritLifesteal > 0 && maxCritLifestealPerSecondTimer > 0)
				{
					maxCritLifestealPerSecondTimer -= CritLifesteal;
					if (Main.myPlayer == player.whoAmI)
						Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<HealProj>(), 0, 0, player.whoAmI, CritLifesteal, 6);
				}
				if (CritVoidsteal > 0 && maxCritVoidStealPerSecondTimer > 0)
				{
					maxCritVoidStealPerSecondTimer -= CritVoidsteal;
					if (Main.myPlayer == player.whoAmI)
						Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<HealProj>(), 2, 0, player.whoAmI, CritVoidsteal, 5);
				}
				damage += CritBonusDamage;
				int randBuff = Main.rand.Next(3);
				if (randBuff == 2 && CritCurseFire)
				{
					Main.PlaySound(SoundID.Item, (int)target.Center.X, (int)target.Center.Y, 93, 0.9f);
					target.AddBuff(BuffID.CursedInferno, 900, false);
					int numberProjectiles = 4;
					int rand = Main.rand.Next(360);
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(i * 90 + rand));
						if (Main.myPlayer == player.whoAmI)
							Projectile.NewProjectile(target.Center.X, target.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<CursedThunder>(), damage, 1f, player.whoAmI, 2);
					}
				}
				else if (randBuff == 1 && (CritFrost || CritCurseFire))
				{
					target.AddBuff(BuffID.Frostburn, 900, false);
					if (Main.myPlayer == player.whoAmI)
					{
						if (CritFrost)
							Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<IcePulseSummon>(), damage * 2, 1f, player.whoAmI, 3);
						else
							Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<IcePulseSummon>(), damage, 1f, player.whoAmI, 3);
					}
				}
				else if (randBuff == 0 && (CritFire || CritCurseFire))
				{
					target.AddBuff(BuffID.OnFire, 900, false);
					if (Main.myPlayer == player.whoAmI)
					{
						if (CritCurseFire && CritFire)
						{
							Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<SharangaBlastSummon>(), damage * 2, 1f, player.whoAmI, 3);
						}
						else
							Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<SharangaBlastSummon>(), damage, 1f, player.whoAmI, 3);
					}
				}
			}
		}
		public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
			healValue += additionalHeal;
            base.GetHealLife(item, quickHeal, ref healValue);
        }
        public override bool PreItemCheck()
		{
			return base.PreItemCheck();
        }
		public float screenShakeMultiplier = 0f;
        public override void ModifyScreenPosition()
        {
			Vector2 screenDimensions = new Vector2(Main.screenWidth, Main.screenHeight);
			bool seenSubspace = false;
			for(int i = 0; i < 1000; i++)
            {
				Projectile projectile = Main.projectile[i];
				if(projectile.type == ModContent.ProjectileType<Projectiles.Celestial.SubspaceEye>() && projectile.active)
                {
					seenSubspace = true;
					int current = projectile.alpha;
					current -= 50;
					if (current < 0)
						current = 0;
					float percent = (float)current / 205f;
					if((int)projectile.ai[1] == -1)
					{
						percent *= 0.5f;
						Vector2 toSubEye = projectile.Center - player.Center;
						if (toSubEye.Length() < 4000f)
							Main.screenPosition.X = (Main.screenPosition.X * (1f - percent)) + ((projectile.Center.X - (screenDimensions.X / 2)) * percent);
					}
					else
					{
						Vector2 toSubEye = projectile.Center - player.Center;
						if (toSubEye.Length() < 4000f)
							Main.screenPosition = (Main.screenPosition * (1f - percent)) + ((new Vector2(projectile.Center.X, projectile.Center.Y) - (screenDimensions / 2)) * percent);
					}
					break;
                }
				if(!seenSubspace)
				{
					if (projectile.type == ModContent.ProjectileType<Projectiles.Celestial.FluidFollower>() && projectile.active && projectile.owner == Main.myPlayer)
					{
						Vector2 toSubEye = projectile.Center - player.Center;
						if (toSubEye.Length() < 4000f)
							Main.screenPosition = new Vector2(projectile.Center.X, projectile.Center.Y) - (screenDimensions / 2);
					}
				}
            }
			if(screenShakeMultiplier > 0)
			{
				Vector2 offset = new Vector2(0, Main.rand.NextFloat(1f) * screenShakeMultiplier).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
				Main.screenPosition += offset;
				screenShakeMultiplier -= 0.75f;
				screenShakeMultiplier *= 0.95f;
			}
			else
            {
				screenShakeMultiplier = 0;
            }
			base.ModifyScreenPosition();
        }
        public override void UpdateBadLifeRegen()
		{
			if (player.HasBuff(ModContent.BuffType<AbyssalInferno>()))
            {
				if(player.lifeRegen > 0)
					player.lifeRegen = 0;
				player.lifeRegenTime = 0;
				player.lifeRegen -= 60;
            }
			base.UpdateBadLifeRegen();
        }
    }
}




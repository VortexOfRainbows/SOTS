using Microsoft.Xna.Framework;
using SOTS.Items.Otherworld.EpicWings;
using SOTS.Void;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static SOTS.SOTS;

namespace SOTS
{
	public class SOTSPlayer : ModPlayer
	{
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
		Vector2 playerMouseWorld;
		public bool weakerCurse = false;
		public bool vibrantArmor = false;
		public int brokenFrigidSword = 0;
		public int shardSpellExtra = 0;
		public int frigidJavelinBoost = 0;
		public int orbitalCounter = 0;
		public int shardOnHit = 0;
		public int bonusShardDamage = 0;
		public int phaseCannonIndex = -1;
		public float assassinateNum = 1;
		public int assassinateFlat = 0;
		public bool assassinate = false;

		public Vector2 starCen;
		private const int saveVersion = 0;

		public int mourningStarFire = 0;

		public bool deoxysPet = false;

		public bool DapperChu = false;

		public bool TurtleTem = false;

		public bool PlanetariumBiome = false;
		public bool ZeplineBiome = false;
		//public bool GeodeBiome = false;
		public bool PyramidBiome = false;
		public bool HeartSwapDelay = false;
		public int BloodTapping = 0;
		public int HeartSwapBonus = 0;
		public bool chessSkip = false;
		public int libraActive = 0;
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
									  //public float projectileSize = 1;

		public bool pearlescentMagic = false; //pearlescent core effect
		public bool bloodstainedJewel = false; //bloodstained jewel effect
		public bool snakeSling = false; //snakeskin sling effect

		public int CritLifesteal = 0; //crit clover
		public float CritVoidsteal = 0f; //crit void charm
		public float maxCritVoidStealPerSecond = 0;
		public float maxCritVoidStealPerSecondTimer = 0;
		public int CritBonusDamage = 0; //crit coin + amplfiier
		public bool CritFire = false; //hellfire icosahedron
		public bool CritFrost = false; //borealis icosahedron
		public bool CritCurseFire = false; //cursed icosahedron
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			TestWingsPlayer testPlayer = player.GetModPlayer<TestWingsPlayer>();
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)SOTSMessageType.SOTSSyncPlayer);
			packet.Write((byte)player.whoAmI);
			packet.Write(orbitalCounter);
			packet.Write(testPlayer.creativeFlight);
			packet.Send(toWho, fromWho);
		}
		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			// Here we would sync something like an RPG stat whenever the player changes it.
			SOTSPlayer clone = clientPlayer as SOTSPlayer;
			if (clone.orbitalCounter != orbitalCounter)
			{
				// Send a Mod Packet with the changes.
				var packet = mod.GetPacket();
				packet.Write((byte)SOTSMessageType.OrbitalCounterChanged);
				packet.Write((byte)player.whoAmI);
				packet.Write(orbitalCounter);
				packet.Send();
			}
		}
		public void DespawnSkyEnemies()
		{
			int[] unableEnemies = { NPCID.Harpy , NPCID.WyvernHead , NPCID.BlueSlime , NPCID.GreenSlime, NPCID.Pinky, NPCID.YellowSlime, NPCID.PurpleSlime};
			for(int i = 0; i < Main.npc.Length; i++)
			{
				NPC npc = Main.npc[i];
				if((unableEnemies.Contains(npc.type)) && npc.target == player.whoAmI && npc.active)
				{
					bool withinBounds = false;
					for (int j = 0; j < Main.player.Length; j++)
					{
						Player other = Main.player[j];
						if (other.active)
						{
							float distanceX = other.Center.X - npc.Center.X;
							float distanceY = other.Center.Y - npc.Center.Y;
							distanceX = Math.Abs(distanceX);
							distanceY = Math.Abs(distanceY);
							if (distanceX < 992 && distanceY < 560)
							{
								withinBounds = true;
							}
						}
					}
					if(!withinBounds && npc.life >= npc.lifeMax)
						npc.active = false;
				}
			}
		}
		public override void ResetEffects()
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			assassinateFlat = 0;
			assassinateNum = 1;
			assassinate = false;
			if(PlanetariumBiome)
			{
				//DespawnSkyEnemies();
			}
			vibrantArmor = false;
			shardSpellExtra = 0;
			frigidJavelinBoost = 0;
			brokenFrigidSword = brokenFrigidSword > 0 ? brokenFrigidSword - 1 : brokenFrigidSword;
			orbitalCounter++;
			if (orbitalCounter % 180 == 0)
			{
				SendClientChanges(this);
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
			libraActive = 0;
			deoxysPet = false;
			BloodTapping = 0;
			HeartSwapDelay = false;
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
			CritBonusDamage = 0;
			CritFire = false;
			CritFrost = false;
			CritCurseFire = false;

			maxCritVoidStealPerSecond = voidPlayer.voidRegen * 20; //max stored voidsteal is 20x the voidRegen speed
			maxCritVoidStealPerSecondTimer += voidPlayer.voidRegen / 30f; //max stored voidsteal regenerates at the twice rate as normal voidRegen (basically, stores 20 seconds of regen)
			if (maxCritVoidStealPerSecondTimer > maxCritVoidStealPerSecond)
			{
				maxCritVoidStealPerSecondTimer = maxCritVoidStealPerSecond;
			}
			if (PyramidBiome)
				player.AddBuff(mod.BuffType("PharaohsCurse"), 16, false);
		}
		public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
		{
			//Fish Set 1

			if (Main.rand.Next(24) == 1 && player.ZoneSkyHeight) {
				caughtType = mod.ItemType("TinyPlanetFish"); }

			//if (Main.rand.Next(200) == 0 && ZeplineBiome) {
			//caughtType = mod.ItemType("ZephyriousZepline"); }
			//if (Main.rand.Next(330) == 1 && liquidType == 2 && poolSize >= 500)   {
			//caughtType = mod.ItemType("ScaledFish");}

			//Fish Set 2

			if (player.ZoneBeach && liquidType == 0 && Main.rand.Next(175) == 1) {
				caughtType = mod.ItemType("SpikyPufferfish"); }
			if (player.ZoneBeach && liquidType == 0 && Main.rand.Next(225) == 0) {
				caughtType = mod.ItemType("CrabClaw"); }


			if (ScaleCatch2(power, 0, 90, 150, 750) && player.ZoneBeach && liquidType == 0) {
				caughtType = mod.ItemType("PinkJellyfishStaff"); }
			else if (ScaleCatch2(power, 0, 70, 30, 150) && player.ZoneBeach && liquidType == 0 && bait.type == 2438) { //Checks for pink jellyfish bait
				caughtType = mod.ItemType("PinkJellyfishStaff"); }

			if (ScaleCatch2(power, 0, 90, 150, 750) && player.ZoneRockLayerHeight && liquidType == 0) {
				caughtType = mod.ItemType("BlueJellyfishStaff"); }
			else if (ScaleCatch2(power, 0, 70, 30, 150) && player.ZoneRockLayerHeight && liquidType == 0 && bait.type == 2436) { //Checks blue jellyfish bait
				caughtType = mod.ItemType("BlueJellyfishStaff"); }
			else if (ScaleCatch2(power, 0, 70, 30, 150) && player.ZoneRockLayerHeight && liquidType == 0 && bait.type == 2437) { //Checks green jellyfish bait
				caughtType = mod.ItemType("BlueJellyfishStaff"); }

			if (ScaleCatch2(power, 0, 30, 5, 10) && ZeplineBiome && liquidType == 0) {
				caughtType = mod.ItemType("SeaSnake"); }
			else if (ScaleCatch2(power, 0, 60, 6, 12) && PyramidBiome && !ZeplineBiome && liquidType == 0) {
				caughtType = mod.ItemType("SeaSnake"); }
			else if (ScaleCatch2(power, 0, 40, 7, 11) && ZeplineBiome && liquidType == 0) {
				caughtType = mod.ItemType("PhantomFish"); }
			else if (ScaleCatch2(power, 0, 80, 8, 14) && PyramidBiome && !ZeplineBiome && liquidType == 0) {
				caughtType = mod.ItemType("PhantomFish"); }
			else if (ScaleCatch2(power, 20, 80, 7, 20) && ZeplineBiome && liquidType == 0) { //gains the same rarity as Phantom Fish when at 80, fails to catch below 20 power
				caughtType = mod.ItemType("Curgeon"); }
			else if (ScaleCatch2(power, 30, 150, 12, 25) && PyramidBiome && !ZeplineBiome && liquidType == 0) {
				caughtType = mod.ItemType("Curgeon"); }
			else if (ScaleCatch2(power, 0, 200, 100, 300) && ZeplineBiome && liquidType == 0) { //1/300 at 0, 1/200 at 100, 1/100 at 200, etc
				caughtType = mod.ItemType("ZephyrousZeppelin"); }
			else if (ScaleCatch2(power, 0, 200, 100, 300) && ZeplineBiome && liquidType == 0) { //1/300 at 0, 1/200 at 100, 1/100 at 200, etc
				caughtType = ItemID.ZephyrFish; }
			else if (!player.HasBuff(BuffID.Crate))
			{
				if (ScaleCatch2(power, 0, 200, 20, 200) && (PyramidBiome || ZeplineBiome) && liquidType == 0) {
					caughtType = mod.ItemType("PyramidCrate"); }
			}
			else
			{
				if (ScaleCatch2(power, 0, 200, 10, 100) && (PyramidBiome || ZeplineBiome) && liquidType == 0) {
					caughtType = mod.ItemType("PyramidCrate"); }
			}

		}
		/** minPower is the minimum power required, and yields a 1/maxRate chance of catching
		*	maxPower is the maximum power required, and yields a 1/minRate chance of catching
		*	rates are overall rounded down
		*	anything below minPower will fail to catch
		*	pre condition: minPower < maxPower, minRate < maxRate
		*	post condition: returns true at a specific chance.
		*/
		public bool ScaleCatch2(int power, int minPower, int maxPower, int minRate, int maxRate)
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
			PlanetariumBiome = (SOTSWorld.planetarium > 250) && player.Center.Y < Main.worldSurface * 16 * 0.6f;
			//GeodeBiome = (SOTSWorld.geodeBiome > 300);
			ZeplineBiome = (SOTSWorld.zeplineBiome > 0);

			//checking for background walls
			int tileBehindX = (int)(player.Center.X / 16);
			int tileBehindY = (int)(player.Center.Y / 16);
			Tile tile = Framing.GetTileSafely(tileBehindX, tileBehindY);
			if (tile.wall == (ushort)mod.WallType("PyramidWallTile"))
			{
				PyramidBiome = true;
			}
			else
			{
				PyramidBiome = (SOTSWorld.pyramidBiome > 0);
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
			HeartSwapBonus -= damage;
			if (HeartSwapBonus < 0)
			{
				HeartSwapBonus = 0;
			}


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
					int Proj = Projectile.NewProjectile(npc.Center.X - dX * 5, npc.Center.Y - dY * 5, dX, dY, 507, 12, 25f, player.whoAmI);
					Main.projectile[Proj].timeLeft = 15;
					Main.projectile[Proj].netUpdate = true;
				}
			}
		}
		public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
		{
			onhitdamage = damage;
			onhit = 2;
			HeartSwapBonus -= damage;
			if (HeartSwapBonus < 0)
			{
				HeartSwapBonus = 0;
			}

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

			if (BloodTapping == 1)
			{
				if (Main.rand.Next(10) == 0 && BloodTapping * damage > 20)
				{
					player.statLife += (int)(BloodTapping * damage / 20);
					player.HealEffect((int)(BloodTapping * damage / 20));
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

			if (BloodTapping * damage > 10)
			{
				player.statLife += (int)(BloodTapping * damage / 10);
				player.HealEffect((int)(BloodTapping * damage / 10));
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
			if(pearlescentMagic && item.magic && item.damage > 3 && shotCounter % 6 == 0)
			{
				for(int i = 0; i < 1000; i++)
				{
					Projectile proj = Main.projectile[i];
					if(proj.owner == player.whoAmI && proj.type == mod.ProjectileType("PearlescentCore"))
					{
						float shootCursorX2 = proj.Center.X - cursorPos.X;
						float shootCursorY2 = proj.Center.Y - cursorPos.Y;
						Vector2 toCursor2 = new Vector2(-1, 0).RotatedBy(Math.Atan2(shootCursorY2, shootCursorX2));
						Projectile.NewProjectile(proj.Center.X, proj.Center.Y, toCursor2.X * 9.25f, toCursor2.Y * 9.25f, mod.ProjectileType("PearlescentShot"), (int)(damage * 1.2f) + 3, knockBack, player.whoAmI);
						break;
					}
				}
			}
			if(snakeSling && item.ranged && item.damage > 3 && shotCounter % 5 == 0)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8));
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X * 1.1f, perturbedSpeed.Y * 1.1f, mod.ProjectileType("Pebble"), damage, knockBack, player.whoAmI);
			}
			if(libraActive == 1 && item.ranged == true && item.type != mod.ItemType("Vulcan"))
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8));
				Projectile.NewProjectile(position.X, position.Y, -(perturbedSpeed.X/4f), -(perturbedSpeed.Y/4f), mod.ProjectileType("BackupArrow"), (int)(damage/2f) + 1, knockBack, player.whoAmI);
			}
			if(doubledActive == 1 && item.fishingPole > 0)
			{
				for(int i = doubledAmount; i > 0; i--)
				{
				  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(i % 2 == 0 ? i * 6 : i * -6));
				  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				}
			}
			
			if(megHat == true && item.magic == true && item.type != mod.ItemType("TheMelter"))
			{
				float numberProjectiles = 1;
				for (int i = 0; i < numberProjectiles; i++)
				{
				  Projectile.NewProjectile(position.X, position.Y, speedX * 0.5f, speedY * 0.5f, type, damage, knockBack, player.whoAmI);
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
			float mod = attackSpeedMod;
			if(mod > (item.useAnimation / 2f) -1f)
			{
				mod = ((item.useAnimation / 2f) -1f);
			}
			/*
			if(ceres == true)
			{
				item.useTurn = true;
				item.autoReuse = true;
			}
			if(megHat == true && item.useAnimation > 4 && item.magic == true)
			{
				return 1.25f;
			}
			else if(ceres == true && item.useAnimation > 4)
			{
				return 1.2f;
			}
			*/
			if(mod != 0)
			{
				return 1 + mod;
			}
			return 1f;
		}
		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if(crit)
			{
				if(CritLifesteal > 0)
				{
					if(Main.myPlayer == player.whoAmI)
						Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("HealProj"), 0, 0, player.whoAmI, CritLifesteal, 6);
				}
				if(CritVoidsteal > 0 && maxCritVoidStealPerSecondTimer > 0)
				{
					maxCritVoidStealPerSecondTimer -= CritVoidsteal;
					if (Main.myPlayer == player.whoAmI)
						Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("HealProj"), 2, 0, player.whoAmI, CritVoidsteal, 5);
				}
				damage += CritBonusDamage;
				int randBuff = Main.rand.Next(3);
				if (randBuff == 2 && CritCurseFire)
				{
					Main.PlaySound(2, (int)target.Center.X, (int)target.Center.Y, 93, 0.9f);
					target.AddBuff(BuffID.CursedInferno, 900, false);
					int numberProjectiles = 4;
					int rand = Main.rand.Next(360);
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(i * 90 + rand));
						if (Main.myPlayer == player.whoAmI)
							Projectile.NewProjectile(target.Center.X, target.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("CursedThunder"), damage, 1f, player.whoAmI, 2);
					}
				}
				else if (randBuff == 1 && (CritFrost || CritCurseFire))
				{
					target.AddBuff(BuffID.Frostburn, 900, false);
					if (Main.myPlayer == player.whoAmI)
					{
						if (CritFrost)
							Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("IcePulseSummon"), damage * 2, 1f, player.whoAmI, 3);
						else
							Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("IcePulseSummon"), damage, 1f, player.whoAmI, 3);
					}
				}
				else if (randBuff == 0 && (CritFire || CritCurseFire))
				{
					target.AddBuff(BuffID.OnFire, 900, false);
					if (Main.myPlayer == player.whoAmI)
					{
						if (CritCurseFire && CritFire)
						{
							Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("SharangaBlastSummon"), damage * 2, 1f, player.whoAmI, 3);
						}
						else
							Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("SharangaBlastSummon"), damage, 1f, player.whoAmI, 3);
					}
				}
			}
		}
		public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit) 
		{
			if(crit)
			{
				if(CritLifesteal > 0)
				{
					if (Main.myPlayer == player.whoAmI)
						Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("HealProj"), 0, 0, player.whoAmI, CritLifesteal, 6);
				}
				if (CritVoidsteal > 0 && maxCritVoidStealPerSecondTimer > 0)
				{
					maxCritVoidStealPerSecondTimer -= CritVoidsteal;
					if (Main.myPlayer == player.whoAmI)
						Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("HealProj"), 2, 0, player.whoAmI, CritVoidsteal, 5);
				}
				damage += CritBonusDamage;
				int randBuff = Main.rand.Next(3);
				if (randBuff == 2 && CritCurseFire)
				{
					Main.PlaySound(2, (int)target.Center.X, (int)target.Center.Y, 93, 0.9f);
					target.AddBuff(BuffID.CursedInferno, 900, false);
					int numberProjectiles = 4;
					int rand = Main.rand.Next(360);
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(i * 90 + rand));
						if (Main.myPlayer == player.whoAmI)
							Projectile.NewProjectile(target.Center.X, target.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("CursedThunder"), damage, 1f, player.whoAmI, 2);
					}
				}
				else if (randBuff == 1 && (CritFrost || CritCurseFire))
				{
					target.AddBuff(BuffID.Frostburn, 900, false);
					if (Main.myPlayer == player.whoAmI)
					{
						if(CritFrost)
							Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("IcePulseSummon"), damage * 2, 1f, player.whoAmI, 3);
						else
							Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("IcePulseSummon"), damage, 1f, player.whoAmI, 3);
					}
				}
				else if(randBuff == 0 && (CritFire || CritCurseFire))
				{
					target.AddBuff(BuffID.OnFire, 900, false);
					if (Main.myPlayer == player.whoAmI)
                    {
						if (CritCurseFire && CritFire)
						{
							Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("SharangaBlastSummon"), damage * 2, 1f, player.whoAmI, 3);
						}
						else
							Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("SharangaBlastSummon"), damage, 1f, player.whoAmI, 3);
					}
				}
			}
		}
	}
}




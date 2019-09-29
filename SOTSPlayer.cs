using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Shaders;
using Terraria.Localization;

namespace SOTS
{
    public class SOTSPlayer : ModPlayer
    {
		public int soulAmount;
		public override TagCompound Save() {
				
			return new TagCompound {
				
				{"soulAmount", soulAmount},
				};
		}

		public override void Load(TagCompound tag) 
		{
			soulAmount = tag.GetInt("soulAmount");
		
		}
		//public static int FusionFuel = 0;
		//public static int itemProduct = 0;
		//public static int replicateCost = 0;
		
		 //Some Important variables 1
		 
		public bool weakerCurse = false;
		
		public bool bloodDecay = true;
		
		public bool Eclipse = false;
        private const int saveVersion = 0;
        public bool Phankin = false;
		public Vector2 starCen;
		public int mourningStarFire = 0;
		public static bool hasProjectile;
		public bool SpectreCool = false;
		public bool SpectreUnicorn = false;
		public bool deoxysPet = false;
		public bool DapperChu = false;
		public bool TurtleTem = false;
		public bool LavaStar = false;
		public int sunSoulActivate = 0;
		public bool PlanetariumBiome = false;
		public bool ZeplineBiome = false;
		public bool GeodeBiome = false;
		public bool PyramidBiome = false;
		public bool HeartSwapDelay = false;
		public bool needle = false;
		public int BloodTapping = 0;
		public int EndreEdit = 0;
		public int HeartSwapBonus = 0;
		public float needleSpeed = 0.1f;
		public bool chessSkip = false;
		public int plutoActive = 0;
		public int corruptSmell = 0;
		public int cosmicPlague = 0;
		public int libraActive = 0;
		public int dead = 0;
		public int doubledActive = 0;
		public int doubledAmount = 0;
		public int neptune = 0;
		public int subFish = 0;
		public bool rapidity = false;
		public bool rapidity2 = false;
		public bool ceres = false;
		public bool vulcanBoost = false;
		public float reloadBoost = 1;
		public int heartActive = 0;
		public int mercuryActive = 0;
		public int jupiter = 0;
		public bool ouranus = false;
		public bool megHat = false;
		public bool musketHat = false;
		public bool megShirt = false;
		public bool megSet = false;
		public int megSetDamage = 0;
		public bool ammoRecover = false;
		public bool orion = false;
		public bool Andromeda = false;
		public bool devilHat = false;
		public bool recycleOn = false;
		public float recycleDamageBoost = 0;
		public bool starBurst = false;
		public bool Oblivion = false;
		public bool Tesseract = false;
		public bool Catalyst = false;
		public bool lostSoul = false;
		public int onhit = 0;
		public int onhitdamage = 0;
		public bool Prism = false;
		public int EnchantedBoomerangs = 0;
		//some important variables 2
     
		public bool PurpleBalloon = false;
		public int StartingDamage = 0;
		public bool ItemDivision = false;
		public bool PushBack = false;
		//public float projectileSize = 1;
		
		
		public override void ResetEffects()
        { 
			
		/* //probably code in a more efficient and-non-bug inducing way
		for(int i = 0; i < 200; i++)
		{
			NPC target = Main.npc[i];
			if(target.life < 5000000)
			{
				for(int life = target.life; life > (target.lifeMax * 0.01 * (100 - StartingDamage)); life--)
				{
					target.life--;
				}
			}
		}
		StartingDamage = 0;
		*/
		
			
			
			
		if(recycleDamageBoost > 0)
		{
			recycleDamageBoost -= 0.001f;
		}
		if(onhit > 0)
		{
			onhit--;
		}
		 //Some important variables 1
			bloodDecay = true;
			EnchantedBoomerangs = 0;
			Prism = false;
			lostSoul = false;
			Catalyst = false;
			Tesseract = false;
			Oblivion = false;
			starBurst = false;
			recycleOn = true;
			devilHat = false;
			Andromeda = false;
			orion = false;
			ammoRecover = false;
			musketHat = false;
			megSet = false;
			megShirt = false;
			megHat = false;
			ouranus = false;
			jupiter = 0;
			heartActive = 0;
			reloadBoost = 1;
			mercuryActive = 0;
			vulcanBoost = false;
			neptune = 0;
			ceres = false;
			subFish = 0;
			rapidity = false;
			rapidity2 = false;
			doubledActive = 0;
			libraActive = 0;
			cosmicPlague = 0;
			corruptSmell = 0;
			deoxysPet = false;
			plutoActive = 0;
            Phankin = false;
			LavaStar = false;
			SpectreCool = false;
			SpectreUnicorn = false;
			sunSoulActivate = 0;
			BloodTapping = 0;
			HeartSwapDelay = false;
			EndreEdit = 0;
			needle = false;
			DapperChu = false;
			TurtleTem = false;
			chessSkip = false;
			//DevilSpawn = false;	
			PurpleBalloon = false;
			ItemDivision = false;
			Eclipse = false;
			//projectileSize = 1;
			PushBack = false;
			
			
				if(PyramidBiome)
				player.AddBuff(mod.BuffType("PharaohsCurse"), 16, false);
        } 
		public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
        {
			{ //Fish Set 1
				
				if (liquidType == 2 && Main.rand.Next(60) == 1)   {
				caughtType = mod.ItemType("HoneyBlade"); }
				if (Main.rand.Next(400) == 1) {
				caughtType = mod.ItemType("SnappedLine");}
				
				
				if (Main.rand.Next(24) == 1 && player.ZoneSkyHeight)   {
					caughtType = mod.ItemType("TinyPlanetFish"); 
				}
				if (Main.rand.Next(24) == 1 && player.ZoneCorrupt)
				{
				caughtType = mod.ItemType("RottingBloodKoi"); 
				}
				if (Main.rand.Next(24) == 1 && player.ZoneCrimson) {
				caughtType = mod.ItemType("RottingBloodKoi"); }
				
				if (Main.rand.Next(70) == 1 && player.ZoneSnow) {
				caughtType = mod.ItemType("FrozenJavelin");}
				if (Main.rand.Next(7) == 1 && player.ZoneSnow && bait.bait >= 10 && bait.bait <= 20)  {
				caughtType = mod.ItemType("FrozenJavelin"); }
				if (Main.rand.Next(125) == 1 && player.ZoneDesert && Main.hardMode && !player.ZoneBeach)  {
				caughtType = mod.ItemType("ForbiddenKnife"); }
				
				if (Main.rand.Next(7) == 1 && player.ZoneDesert && bait.bait >= 25 && !player.ZoneBeach){
				caughtType = mod.ItemType("SandFish");}
				if (Main.rand.Next(30) == 1 && player.ZoneDesert && bait.bait <= 15 && !player.ZoneBeach) {
				caughtType = mod.ItemType("SandFish"); }
				if (Main.rand.Next(125) == 1 && player.ZoneSnow && Main.hardMode)  {
				caughtType = mod.ItemType("FrostKnife"); }
				
				if (Main.rand.Next(1000) == 1 && ZeplineBiome) {
				caughtType = mod.ItemType("ZephyriousZepline"); }
            //if (Main.rand.Next(330) == 1 && liquidType == 2 && poolSize >= 500)   {
			//caughtType = mod.ItemType("ScaledFish");}
			}
			{ //Fish Set 2
			   
				if (player.ZoneBeach && liquidType == 0 && Main.rand.Next(175) == 1) 
				{
				caughtType = mod.ItemType("SpikyPufferfish"); }
				if (liquidType == 2 && Main.rand.Next(50) == 1) 
				{
				caughtType = mod.ItemType("HiveFish"); }
				if (liquidType == 1 && Main.rand.Next(130) == 1) 
				{
				caughtType = mod.ItemType("IceCreamOre"); }
				if (liquidType == 1 && Main.rand.Next(70) == 1) 
				{
				caughtType = mod.ItemType("IceCream"); }
				if (liquidType == 1 && Main.rand.Next(70) == 1) 
				{
				caughtType = mod.ItemType("IceCreamOre"); }
				if (liquidType == 1 && Main.rand.Next(50) == 1)
				{
				caughtType = mod.ItemType("IceCream"); }
				if (liquidType == 1 && Main.rand.Next(200) == 1)
				{
				caughtType = mod.ItemType("GeodeCrate"); }
				if (liquidType == 1 && Main.rand.Next(35) == 0 && player.FindBuffIndex(BuffID.Crate) > -1)
				{
				caughtType = mod.ItemType("GeodeCrate"); }
				if (liquidType == 1 && Main.rand.Next(60) == 0)
				{
				caughtType = mod.ItemType("JewelFish"); }
				if (liquidType == 1 && player.FindBuffIndex(BuffID.Spelunker) > -1 && GeodeBiome && Main.rand.Next(55) == 0)
				{
				caughtType = mod.ItemType("JewelFish"); }
			
			}
			
            if (Main.rand.Next(1000) == 0 && player.ZoneBeach && liquidType == 0){
			caughtType = mod.ItemType("PinkJellyfishStaff"); }
            else if (Main.rand.Next(100) == 0 && player.ZoneBeach && liquidType == 0 && bait.type == 2438){ //Checks for pink jellyfish bait
			caughtType = mod.ItemType("PinkJellyfishStaff"); }
            if (Main.rand.Next(1000) == 0 && player.ZoneRockLayerHeight && liquidType == 0){
			caughtType = mod.ItemType("BlueJellyfishStaff"); }
            else if (Main.rand.Next(100) == 0 && player.ZoneRockLayerHeight && liquidType == 0 && bait.type == 2436){ //Checks blue for jellyfish bait
			caughtType = mod.ItemType("BlueJellyfishStaff"); }
            else if (Main.rand.Next(100) == 0 && player.ZoneRockLayerHeight && liquidType == 0 && bait.type == 2437){ //Checks blue for jellyfish bait
			caughtType = mod.ItemType("BlueJellyfishStaff"); }
			
		}
		public override void UpdateBiomes()
        {
            PlanetariumBiome = (SOTSWorld.planetarium > 0);
            GeodeBiome = (SOTSWorld.geodeBiome > 300);
            ZeplineBiome = (SOTSWorld.zeplineBiome > 0);
			
			//checking for background walls
			int tileBehindX = (int)(player.Center.X / 16);
			int tileBehindY = (int)(player.Center.Y / 16);
			Tile tile = Framing.GetTileSafely(tileBehindX, tileBehindY);
			if(tile.wall == (ushort)mod.WallType("PyramidWallTile"))
			{
            PyramidBiome = true;
			}
			else
			{
            PyramidBiome = false;
			}
		}
		public override bool CustomBiomesMatch(Player other) 
		{
			var modOther = other.GetModPlayer<SOTSPlayer>();
			return PyramidBiome == modOther.PyramidBiome;
		}
		public override void CopyCustomBiomesTo(Player other) 
		{
			var modOther = other.GetModPlayer<SOTSPlayer>();
			modOther.PyramidBiome = PyramidBiome;
		}
		public override void SendCustomBiomes(BinaryWriter writer)
		{
			BitsByte flags = new BitsByte();
			flags[0] = PyramidBiome;
			writer.Write(flags);
		}

		public override void ReceiveCustomBiomes(BinaryReader reader) 
		{
			BitsByte flags = reader.ReadByte();
			PyramidBiome = flags[0];
		}
		public override void OnHitByNPC(NPC npc, int damage, bool crit)
		{
			onhitdamage = damage;
			onhit = 2;
			HeartSwapBonus -= damage;
			if(HeartSwapBonus < 0)
			{
			HeartSwapBonus = 0;
			}
			if(npc.type == NPCID.BloodCrawler || npc.type == NPCID.FaceMonster || npc.type == NPCID.Crimera || npc.type == NPCID.Herpling || npc.type == NPCID.Crimslime || npc.type == NPCID.BloodJelly || npc.type == NPCID.BloodFeeder || npc.type == NPCID.DarkMummy || npc.type == NPCID.BrainofCthulhu || npc.type == NPCID.Creeper || npc.type == NPCID.EaterofSouls || npc.type == NPCID.DevourerHead || npc.type == NPCID.DevourerBody || npc.type == NPCID.DevourerTail || npc.type == NPCID.Corruptor || npc.type == NPCID.CorruptSlime || npc.type == NPCID.Slimeling || npc.type == NPCID.Slimer || npc.type == 98 || npc.type == 99 || npc.type == 100 || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.CursedHammer || npc.type == NPCID.Clinger || npc.type == NPCID.CrimsonAxe || npc.type == NPCID.IchorSticker || npc.type == NPCID.FloatyGross)
			{
			player.statLife += damage * corruptSmell;
			}
			
			if(PushBack)
			{
				float dX = npc.Center.X - player.Center.X;
				float dY = npc.Center.Y - player.Center.Y;
				float distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
				float speed = 16.0f / distance;
				dX *= speed;
				dY *= speed;
				
				int Proj = Projectile.NewProjectile(npc.Center.X - dX * 5, npc.Center.Y - dY * 5, dX, dY, 507, 12, 25f, player.whoAmI);
				Main.projectile[Proj].timeLeft = 15;
				Main.projectile[Proj].alpha = 125;
				Main.projectile[Proj].tileCollide = false;
			}
			
			
			
	
		}
		public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
		{
			onhitdamage = damage;
			onhit = 2;
			HeartSwapBonus -= damage;
			if(HeartSwapBonus < 0)
			{
			HeartSwapBonus = 0;
			}
			
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			if(proj.type == ProjectileID.EnchantedBoomerang && EnchantedBoomerangs > 1)
			{
				target.immune[proj.owner] = 0;
				//proj.Kill();
				//damage += (int)(target.defense *	0.5f);
			}
			if(recycleOn)
			{
			recycleDamageBoost += ((float)damage * 0.001f);
			
			}
			if(orion == true)
			{
				float amount = 0;
				for(int j = Main.rand.Next(2); j == 0; j = Main.rand.Next((int)(1 + amount)))
				{
					amount++;
				}
				
				 for(int i = Main.rand.Next(200); amount > 0; i = Main.rand.Next(200))
				{
				   NPC target2 = Main.npc[i];
					
				float shootFromX = target.Center.X;
				float shootFromY = target.Center.Y;
				
					if(target2.Center.X >= target.Center.X)
					shootFromX += target.width;
						
					if(target2.Center.X <= target.Center.X)
					shootFromX -= target.width;
						
					if(target2.Center.Y >= target.Center.Y)
					shootFromY += target.height;
						
					if(target2.Center.Y <= target.Center.Y)
					shootFromY -= target.height;
						
					
				   
				   float shootToX = target2.position.X + (float)target2.width * 0.5f - shootFromX;
				   float shootToY = target2.position.Y + (float)target2.height * 0.5f - shootFromY;
				   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

				   //If the distance between the projectile and the live target is active
				   if(distance < 320f && !target2.friendly && target2.active)
				   {
					   if(amount > 0)
					   {
						amount--;
					   
						   //Dividing the factor of 3f which is the desired velocity by distance
						   distance = 0.2f / distance;
			   
						   //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
						   shootToX *= distance * 5;
						   shootToY *= distance * 5;
			   
						   //Shoot projectile and set ai back to 0
						   Projectile.NewProjectile(shootFromX, shootFromY, shootToX, shootToY, mod.ProjectileType("OrionChain"), (int)(proj.damage * 0.75f), 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
					   }
				   }
				   else
				   {
					amount -= 0.01f;  
				   }
				}
				
				
				
			}
			
			
			
			
			
			
			
			
			
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
			if(BloodTapping == 1)
			{
				if(Main.rand.Next(10) == 0 && BloodTapping * damage > 20)
				{
					player.statLife += (int)(BloodTapping * damage/20);
					player.HealEffect((int)(BloodTapping * damage/20));
				}
			}
			target.scale += (float)((Main.rand.Next(-100, 101) * EndreEdit)/100f);
			target.rotation += (float)((Main.rand.Next(-100, 101) * EndreEdit)/100f);
			target.defense -= (int)(Main.rand.Next(48) * EndreEdit);
			target.lifeMax += (int)(Main.rand.Next(-4000,5000) * EndreEdit);
			target.width += 1 * EndreEdit;
			target.height += 1 * EndreEdit;
			
			if(cosmicPlague * target.type == target.type )
			{
				
				if(target.defense == -92)
				{
				Projectile.NewProjectile(target.Center.X, target.position.Y - 20, 0, -5, proj.type, (int)(proj.damage), knockback, player.whoAmI);
				Projectile.NewProjectile(target.position.X - 20, target.Center.Y, -5, 0, proj.type, (int)(proj.damage), knockback, player.whoAmI);
				Projectile.NewProjectile(target.position.X + -(2 * (target.position.X - target.Center.X)) + 20, target.Center.Y, +5, 0, proj.type, (int)(proj.damage), knockback, player.whoAmI);
				Projectile.NewProjectile(target.Center.X, target.position.Y + -(2 * (target.position.Y - target.Center.Y)) + 20, 0, +5, proj.type, (int)(proj.damage), knockback, player.whoAmI);
				Projectile.NewProjectile(target.position.X + -(2 * (target.position.X - target.Center.X)) + 20, target.position.Y - 20, +5, -5, proj.type, (int)(proj.damage), knockback, player.whoAmI);
				Projectile.NewProjectile(target.position.X - 20, target.position.Y - 20, -5, -5, proj.type, (int)(proj.damage), knockback, player.whoAmI);
				Projectile.NewProjectile(target.position.X + -(2 * (target.position.X - target.Center.X)) + 20, target.position.Y + -(2 * (target.position.Y - target.Center.Y)) + 20, +5, +5, proj.type, (int)(proj.damage), knockback, player.whoAmI);
				Projectile.NewProjectile(target.position.X - 20, target.position.Y + -(2 * (target.position.Y - target.Center.Y)) + 20, -5, +5, proj.type, (int)(proj.damage), knockback, player.whoAmI);
				}
			}
			if(heartActive == 1)
			{
				target.AddBuff(mod.BuffType("OverhealHeart"), 900, false);
			}
			if(mercuryActive == 1)
			{
				int addbuff = 0;
				int randomBuff = 0;
				for(randomBuff = 0; Main.debuff[randomBuff] == false; randomBuff = Main.rand.Next(203))
				{
				}
				addbuff = randomBuff;
				target.AddBuff(randomBuff, 420, false);
			}
			if(proj.melee == true && ouranus == true)
			{
				if(proj.type != 612)
				{
				Projectile.NewProjectile(vector14.X, vector14.Y, 0, 0, 612, (int)(proj.damage), knockback, player.whoAmI);
				}
				else if(proj.type == 612 && Main.rand.Next(10) == 0)
				{
				Projectile.NewProjectile(vector14.X, vector14.Y, 0, 0, 612, (int)(proj.damage), knockback, player.whoAmI);	
				}
			}
			if(megShirt == true)
			{
				if(Main.rand.Next(35) == 0)
				{
					Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height,  ItemID.Heart);	
				}
				if(Main.rand.Next(35) == 0)
				{
					Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height,  ItemID.Star);
				}
			}
			
			if(ammoRecover == true)
			{
				target.AddBuff(mod.BuffType("DropAmmo"), 900, false);
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
						
			if(BloodTapping * damage > 10)
			{
					player.statLife += (int)(BloodTapping * damage/10);
					player.HealEffect((int)(BloodTapping * damage/10));
			}
			if(heartActive * target.type == target.type)
			{
				target.AddBuff(mod.BuffType("OverhealHeart"), 900, false);
			}
			if(mercuryActive * target.type == target.type)
			{
				int randomBuff = Main.rand.Next(203);
				if(Main.debuff[randomBuff] == true)
				{
				target.AddBuff(randomBuff, 900, false);
				}
			}
			target.scale += (float)((Main.rand.Next(-100, 101) * EndreEdit)/100f);
			target.rotation += (float)((Main.rand.Next(-100, 101) * EndreEdit)/100f);
			target.defense -= (int)(Main.rand.Next(48) * EndreEdit);
			target.lifeMax += (int)(Main.rand.Next(-4000,5000) * EndreEdit);
			target.width += 1 * EndreEdit;
			target.height += 1 * EndreEdit;
			
			if(item.melee == true && ouranus == true)
			{
				Projectile.NewProjectile(vector14.X, vector14.Y, 0, 0, 612, (int)(item.damage), knockback, player.whoAmI);
			}
			if(megShirt == true)
			{
				if(Main.rand.Next(35) == 0)
				{
					Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height,  ItemID.Heart);	
				}
				if(Main.rand.Next(35) == 0)
				{
					Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height,  ItemID.Star);
				}
			}
			if(ammoRecover == true)
			{
				target.AddBuff(mod.BuffType("DropAmmo"), 900, false);
			}
			
			
			
		}
		public override void SetupStartInventory(IList<Item> items)
		{
				Item item = new Item();
				item.SetDefaults(mod.ItemType("ImitationCrate"));   //this is an example of how to add your moded item
				item.stack = 1;         //this is the stack of the item
				items.Add(item);
		}
		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			float downgrade = 0.5f;
			if(Main.expertMode == true)
			{
				downgrade = 0.75f;
			}
			if((float)(plutoActive * ((player.statLife/10f) + 35)) > damage - (player.statDefense * downgrade))
			{
				damage = 0;
				player.statLife +=1;
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
			
			
			
			
			return true;
		}
		public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			
			
			Vector2 cursorPos;
					
						if (player.gravDir == 1f)
					{
					cursorPos.Y = (float)Main.mouseY + Main.screenPosition.Y;
					}
					else
					{
					cursorPos.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
						cursorPos.X = (float)Main.mouseX + Main.screenPosition.X;
			
			if(PurpleBalloon && item.fishingPole > 0)
			{
				  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(40));
				  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("PurpleBobber"), damage, type, player.whoAmI);
				  //return false;
			}
			if(item.type == ItemID.EnchantedBoomerang)
			{
				float numberProjectiles = EnchantedBoomerangs;
				for (int i = 1; i < numberProjectiles; i++)
				{
                 
				  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(EnchantedBoomerangs * 2));
				  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				}
			}
			int checker = 12;
			if(libraActive == 1 && item.ranged == true && item.type != mod.ItemType("Vulcan"))
			{
				  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8));
                  Projectile.NewProjectile(position.X, position.Y, -(perturbedSpeed.X/4f), -(perturbedSpeed.Y/4f), mod.ProjectileType("BackupArrow"), (int)(damage/2f) + 1, knockBack, player.whoAmI);
				
			}
			if(doubledActive == 1 && item.fishingPole > 0)
			{
				for(int i = doubledAmount; i > 0; i--)
				{
				  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(40));
				  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				}
			}
			if(item.thrown == true && neptune * checker == 12 && item.type != mod.ItemType("MightyDart") && item.type != mod.ItemType("Circuit"))
			{
				
              float numberProjectiles = 3;
              for (int i = 0; i < numberProjectiles; i++)
              {
                 
				  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(33));
				  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				}
              return true;
			  
			}
			if(jupiter == 1 && item.type != mod.ItemType("MightyDart") && item.type != mod.ItemType("Circuit") && item.type != mod.ItemType("TheMelter"))
			{
				int numberProjectiles = 1;  //This defines how many projectiles to shot
            for (int index = 0; index < numberProjectiles; ++index)
            {
                Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(201) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
                vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-200, 201);
                vector2_1.Y -= (float)(100 * index);
                float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                if ((double)num13 < 0.0) num13 *= -1f;
                if ((double)num13 < 20.0) num13 = 20f;
                float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
                float num15 = item.shootSpeed / num14;
                float num16 = num12 * num15;
                float num17 = num13 * num15;
                float SpeedX = num16 + (float)Main.rand.Next(-40, 41) * 0.02f;  //this defines the projectile X position speed and randomnes
                float SpeedY = num17 + (float)Main.rand.Next(-40, 41) * 0.02f;  //this defines the projectile Y position speed and randomnes
                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, type, damage, knockBack, Main.myPlayer, 0.0f, 2);
            }
			}
			if(megHat == true && item.magic == true && item.type != mod.ItemType("TheMelter"))
			{
				float numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                 
				  Projectile.NewProjectile(position.X, position.Y, speedX * 0.5f, speedY * 0.5f, type, damage, knockBack, player.whoAmI);
				}
              return true;
			}
			if(Eclipse == true && item.type != mod.ItemType("MightyDart") && item.type != mod.ItemType("Circuit") && item.type != mod.ItemType("TheMelter"))
			{
				float numberProjectiles = 1;
				for (int i = 0; i < numberProjectiles; i++)
				{
                 
				  Projectile.NewProjectile(cursorPos.X, cursorPos.Y, -speedX, -speedY, mod.ProjectileType("EclipseShot"), damage, knockBack, player.whoAmI);
				}
              return true;
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
			if(ceres == true)
			{
				item.useTurn = true;
				item.autoReuse = true;
			}
			if(Andromeda == true)
			{
				item.knockBack = 2;
			}
			if(vulcanBoost == true && item.useAnimation > 40 && item.type == mod.ItemType("Vulcan"))
			{
				return vulcanBoost ? 20f : 1f;
			}
			else if(rapidity == true && item.useAnimation > 6 && item.type != mod.ItemType("FastBrew"))
			{
				return rapidity ? 1.8f : 1f;
			}
			else if(devilHat == true && item.useAnimation > 8 && item.melee == true)
			{
				return 1.25f;
			}
			else if(megHat == true && item.useAnimation > 4 && item.magic == true)
			{
				return 1.25f;
			}
			else if(musketHat == true && item.useAnimation > 6 && item.ranged == true)
			{
				return 1.25f;
			}
			else if(rapidity2 == true && item.useAnimation > 7)
			{
				return rapidity2 ? 0.8f + (player.statLifeMax2 - player.statLife)/350f : 1f;
			}
			else if(ceres == true && item.useAnimation > 4)
			{
				return 1.2f;
			}
			else
			{
				return 1f;
			}
		}
		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if(Oblivion)
			{
				Player player  = Main.player[target.target];	
				if(target.type == mod.NPCType("Libra"))
				{
					if(player.name == "V" || player.name == "Vortex")
					{
					Main.NewText("Trying out Vortex this time huh? He did create this fantastic mod after all!", 255, 255, 255);
					}
					else
					{
					Main.NewText("Just because I have infinite defense doesn't mean you can kill me!", 255, 255, 255);
					}
					
						Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, (mod.ItemType("ShadeMaterial")), 1);
						target.timeLeft = 1;
						target.timeLeft--;
						target.life = 0;
						
				}
				float negativeDefenseMultiplier = 0.65f;
				target.defense++;
				if(Main.expertMode)
				{
				damage += (int)(negativeDefenseMultiplier * target.defense);
				}
				else
				{
				damage += (int)(negativeDefenseMultiplier * target.defense);
				}
				
			}
			if(proj.type == ProjectileID.EnchantedBoomerang && EnchantedBoomerangs > 1)
			{
				target.immune[proj.owner] = 0;
				proj.Kill();
				damage += (int)(target.defense * 0.5f);
			}
		}
	}
	
}




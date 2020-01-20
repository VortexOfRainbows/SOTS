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
using SOTS.Void;

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
		
		public Vector2 starCen;
        private const int saveVersion = 0;
		
		public int mourningStarFire = 0;
		
		public bool deoxysPet = false;
		
		public bool DapperChu = false;
		
		public bool TurtleTem = false;
		
		//public bool PlanetariumBiome = false;
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
		public int CritBonusDamage = 0; //crit coin + amplfiier
		public bool CritFire = false; //hellfire icosahedron
		public bool CritFrost = false; //borealis icosahedron
		public bool CritCurseFire = false; //cursed icosahedron
		
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
			
			 playerMouseWorld = Main.MouseWorld;
			
		if(onhit > 0)
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
			if(PyramidBiome)
				player.AddBuff(mod.BuffType("PharaohsCurse"), 16, false);
        } 
		public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
        {
			 //Fish Set 1
			
			if (Main.rand.Next(24) == 1 && player.ZoneSkyHeight)   {
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
			if (liquidType == 1 && Main.rand.Next(50) == 0){
			caughtType = mod.ItemType("GeodeCrate"); }
			if (liquidType == 1 && Main.rand.Next(17) == 0 && player.FindBuffIndex(BuffID.Crate) > -1) {
			caughtType = mod.ItemType("GeodeCrate"); }
			
			
            if (Main.rand.Next(800) == 0 && player.ZoneBeach && liquidType == 0){
			caughtType = mod.ItemType("PinkJellyfishStaff"); }
            else if (Main.rand.Next(50) == 0 && player.ZoneBeach && liquidType == 0 && bait.type == 2438){ //Checks for pink jellyfish bait
			caughtType = mod.ItemType("PinkJellyfishStaff"); }
            if (Main.rand.Next(800) == 0 && player.ZoneRockLayerHeight && liquidType == 0){
			caughtType = mod.ItemType("BlueJellyfishStaff"); }
            else if (Main.rand.Next(50) == 0 && player.ZoneRockLayerHeight && liquidType == 0 && bait.type == 2436){ //Checks blue for jellyfish bait
			caughtType = mod.ItemType("BlueJellyfishStaff"); }
            else if (Main.rand.Next(50) == 0 && player.ZoneRockLayerHeight && liquidType == 0 && bait.type == 2437){ //Checks blue for jellyfish bait
			caughtType = mod.ItemType("BlueJellyfishStaff"); }
			
		}
		public override void UpdateBiomes()
        {
            //PlanetariumBiome = (SOTSWorld.planetarium > 0);
            //GeodeBiome = (SOTSWorld.geodeBiome > 300);
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
				PyramidBiome = (SOTSWorld.pyramidBiome > 0);
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
			
			if(BloodTapping == 1)
			{
				if(Main.rand.Next(10) == 0 && BloodTapping * damage > 20)
				{
					player.statLife += (int)(BloodTapping * damage/20);
					player.HealEffect((int)(BloodTapping * damage/20));
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
				  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(40));
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
				  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(40));
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
					player.statLife += CritLifesteal;
					player.HealEffect(CritLifesteal);
				}
				if(CritVoidsteal > 0)
				{
					VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
					voidPlayer.voidMeter += CritVoidsteal;
				}
				int randBuff = Main.rand.Next(3);
				if(randBuff == 2 && CritCurseFire)
				{
					target.AddBuff(BuffID.CursedInferno, 300, false);
				}
				if(randBuff == 1 && CritFrost)
				{
					target.AddBuff(BuffID.Frostburn, 300, false);
				}
				else if(randBuff == 0 && CritFire)
				{
					target.AddBuff(BuffID.OnFire, 300, false);
				}
				damage += CritBonusDamage;
			}
		}
		public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit) 
		{
			if(crit)
			{
				if(CritLifesteal > 0)
				{
					player.statLife += CritLifesteal;
					player.HealEffect(CritLifesteal);
				}
				if(CritVoidsteal > 0)
				{
					VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
					voidPlayer.voidMeter += CritVoidsteal;
				}
				int randBuff = Main.rand.Next(2);
				if((randBuff == 1 && CritFrost) || (CritFire && CritFrost && Main.rand.Next(5) == 0))
				{
					target.AddBuff(BuffID.Frostburn, 300, false);
				}
				else if(randBuff == 0 && CritFire)
				{
					target.AddBuff(BuffID.OnFire, 300, false);
				}
				damage += CritBonusDamage;
			}
		}
	}
}




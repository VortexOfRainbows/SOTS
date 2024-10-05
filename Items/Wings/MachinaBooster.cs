using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Items.Planetarium.Furniture;
using SOTS.Void;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static SOTS.SOTS;
using Terraria.Localization;
using SOTS.NPCs.Boss.Lux;
using System;
using SOTS.Helpers;

namespace SOTS.Items.Wings
{
	[AutoloadEquip(EquipType.Wings)]
	public class MachinaBooster : ModItem
	{ 
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(150, 8.2f, 1.4f);
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (string key in SOTS.MachinaBoosterHotKey.GetAssignedKeys()) //gets the key configured to this hotkey
			{
				foreach (TooltipLine line in tooltips) //goes through each tooltip line
				{
					if (line.Mod == "Terraria" && line.Name == "Tooltip0") //checks the name of the tootip line
					{
						line.Text = Language.GetTextValue("Mods.SOTS.MachinaBoosterText", key);
						return;
					}
				}
			}
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.Mod == "Terraria" && line.Name == "Tooltip0")
				{
					string Textkey = Language.GetTextValue("Mods.SOTS.Common.Unbound");
					line.Text = Language.GetTextValue("Mods.SOTS.MachinaBoosterText2", Textkey);
				}
			}
			base.ModifyTooltips(tooltips);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Wings/MachinaBooster").Value;
			Main.spriteBatch.Draw(texture, new Vector2(position.X, position.Y), null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Wings/MachinaBoosterEffect").Value;
			Color color = new Color(110, 110, 110, 0);
			for (int k = 0; k < 4; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				Main.spriteBatch.Draw(texture2, new Vector2(position.X + x, position.Y + y), null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			texture = Mod.Assets.Request<Texture2D>("Items/Wings/MachinaBoosterBorder").Value;
			Main.spriteBatch.Draw(texture, new Vector2(position.X, position.Y), null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Wings/MachinaBooster").Value;
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, lightColor * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Wings/MachinaBoosterEffect").Value;
			Color color = new Color(110, 110, 110, 0);
			for (int k = 0; k < 4; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			texture = Mod.Assets.Request<Texture2D>("Items/Wings/MachinaBoosterBorder").Value;
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, lightColor * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void SetDefaults()
		{
			Item.width = 52;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Red;
			Item.expert = true;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<TwilightGyroscope>(), 1).AddIngredient(ModContent.ItemType<DissolvingAether>(), 1).AddIngredient(ModContent.ItemType<StarlightAlloy>(), 20).AddIngredient(ItemID.SoulofFlight, 20).AddTile(TileID.MythrilAnvil).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<TwilightGyroscope>(), 1).AddIngredient(ModContent.ItemType<DissolvingAether>(), 1).AddIngredient(ModContent.ItemType<StarlightAlloy>(), 20).AddIngredient(ItemID.SoulofFlight, 20).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MachinaBoosterPlayer MachinaBoosterPlayer = player.GetModPlayer<MachinaBoosterPlayer>();
			MachinaBoosterPlayer.canCreativeFlight = true;
			player.wingTimeMax = 150;
			player.noFallDmg = true;
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.bonusVoidGain += 1f;
		}
		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			float num1 = 0.1f;
			float num2 = 0.5f;
			float num3 = 1.5f;
			float num4 = 0.5f;
			float num5 = 0.1f;
			ascentWhenFalling = num2;
			ascentWhenRising = num5;
			maxCanAscendMultiplier = num4;
			maxAscentMultiplier = num3;
			constantAscend = num1;
		}
	}
	public class MachinaBoosterPlayer : ModPlayer
	{
		public void SendPacket()
		{
			var packet = Mod.GetPacket();
			packet.Write((byte)SOTSMessageType.SyncCreativeFlight);
			packet.Write((byte)Player.whoAmI);
			packet.Write(creativeFlight);
            packet.Write(SlowFlight);
            packet.Send();
			netUpdate = false;
		}
		public const float voidDrain = 3f;
		public bool canCreativeFlight = false;
        public bool CreativeFlightTier2 = false;
        public bool creativeFlight = false;
		public float wingSpeed = 7f;
		public void UpdateSlowFlight()
        {
			bool previous = SlowFlight;
            if (Player.whoAmI == Main.myPlayer)
            {
                SlowFlight = CreativeFlightTier2 && SlowFlightHotKey.Current;
                if (SlowFlight != previous)
                {
                    if (Main.netMode != NetmodeID.SinglePlayer)
                        SendPacket();
                }
            }
        }
		public bool SlowFlight = false;
        public float wingSpeedMax
		{
			get
			{
				float num = CreativeFlightTier2 ? 8f : 7f;
				if (SlowFlight)
					num = 4f;
				return num;
            }
			
		}
		public int epicWingType = 0;
		public bool gyro = false;
		public bool netUpdate = false;
		public float FlightModeFloat = 0f;
		public float FlightCounter = 0f;
        public int PlayerWasLastOnASlope = 0; //Used exclusively to makes SOTS wing visuals less buggy when moving down a slope
        public enum EpicWingType : int
		{
			Default = 0,
			Blocky = 1,
		}
		public void DustExplosion()
		{
			for (int i = 0; i < 360; i += 4)
			{
				if(CreativeFlightTier2)
                {
                    Color finalColor1 = Color.Lerp(new Color(100, 100, 100, 0), ColorHelper.Pastel(MathHelper.ToRadians(SOTSWorld.GlobalCounter + i), true), 0.7f);
                    Dust dust = Dust.NewDustDirect(Player.Center + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, finalColor1);
                    dust.noGravity = true;
                    dust.fadeIn = 0.1f;
                    dust.velocity *= 0.5f;
                    dust.scale += 1f;
                    dust.velocity += new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(i));
                    dust.shader = GameShaders.Armor.GetSecondaryShader(Player.cWings, Player);
                }
				else
                {
                    if (epicWingType == 0)
                    {
                        int index = Dust.NewDust(Player.Center + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust>(), 0, 0, 0, Color.White);
                        Dust dust = Main.dust[index];
                        dust.noGravity = true;
                        dust.fadeIn = 0.1f;
                        dust.velocity *= 0.8f;
                        dust.scale += 1.5f;
                        dust.velocity += new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(i));
                        dust.shader = GameShaders.Armor.GetSecondaryShader(Player.cWings, Player);
                    }
                    else
                    {
                        int index = Dust.NewDust(Player.Center + new Vector2(-5, -5), 0, 0, ModContent.DustType<CopyDust>());
                        Dust dust = Main.dust[index];
                        dust.noGravity = true;
                        dust.fadeIn = 0.5f;
                        dust.velocity *= 0.8f;
                        dust.scale = 1.5f;
                        dust.velocity += new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(i));
                        dust.shader = GameShaders.Armor.GetSecondaryShader(Player.cWings, Player);
                    }
                }
			}
			Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuTick, Player.Center);
		}
		public void flightStart()
		{
			if (SOTSPlayer.ModPlayer(Player).CreativeFlightButtonPressed)
			{
				if (!creativeFlight && !CreativeFlightTier2)
                {
                    VoidPlayer voidPlayer = VoidPlayer.ModPlayer(Player);
                    voidPlayer.voidMeter -= 5 * voidPlayer.voidCost;
                    VoidPlayer.VoidEffect(Player, (int)(-5 * voidPlayer.voidCost), false, false);
                }
				creativeFlight = !creativeFlight; 
				if(Main.myPlayer == Player.whoAmI && Main.netMode != NetmodeID.SinglePlayer)
					SendPacket();
				if(CreativeFlightTier2 && (SOTSWorld.downedLux || !SOTS.ServerConfig.NerfInsignia))
					Player.wingTime = 7200;
            }
		}
		bool movingHori = false;
		bool movingVert = false;
		public override void PostUpdate()
		{
			if (creativeFlight)
            {
                if (Player.bodyFrame.Y == Player.bodyFrame.Height * 5 && !TilesBelow())
                    Player.bodyFrame.Y = Player.bodyFrame.Height * 6;
            }
		}
		public bool TilesBelow()
		{
			int i = (int)Player.Center.X / 16;
			int i2 = (int)(Player.Center.X + 10) / 16;
			int i3 = (int)(Player.Center.X - 10) / 16;
			int j = (int)(Player.position.Y + Player.height + 1) / 16;
			int j2 = (int)(Player.position.Y + Player.height + 15) / 16;
			Tile tile = Framing.GetTileSafely(i, j);
			Tile tile2 = Framing.GetTileSafely(i, j2);
			Tile tile3 = Framing.GetTileSafely(i2, j);
			Tile tile4 = Framing.GetTileSafely(i2, j2);
			Tile tile5 = Framing.GetTileSafely(i3, j);
			Tile tile6 = Framing.GetTileSafely(i3, j2);
			return (tile2 == tile && tile.HasTile && !tile.IsActuated && (Main.tileSolid[tile.TileType] || Main.tileTable[tile.TileType])) || (tile3 == tile4 && tile4.HasTile && !tile4.IsActuated && (Main.tileSolid[tile4.TileType] || Main.tileTable[tile4.TileType])) || (tile5 == tile6 && tile6.HasTile && !tile6.IsActuated && (Main.tileSolid[tile6.TileType] || Main.tileTable[tile6.TileType]));
		}
		int dustIter = 0;
		int[] dustID = new int[180];
		int[] dustOwnerID = new int[180];
		Vector2[] dustPos = new Vector2[180];
		private bool AutoCancelFlight = false;
		public void HaloDust()
		{
			if (CreativeFlightTier2)
				return;
			for (int i = 0; i < 3; i++)
			{
				dustIter++;
				Vector2 center = new Vector2(Player.Center.X - 12 * Player.direction, Player.position.Y - 4);
				Vector2 circularLocation = new Vector2(10, 0).RotatedBy(MathHelper.ToRadians(randCounter * 6 + i * 120));
				Vector2 circularLocation2 = new Vector2(circularLocation.X / 2, circularLocation.Y).RotatedBy(MathHelper.ToRadians(45 * Player.direction));
				Vector2 loc = center + circularLocation2;
				int index = Dust.NewDust(loc + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust2>(), 0, 0, 0, new Color(255, 255, 255));
				Dust dust = Main.dust[index];
				dust.noGravity = true;
				dust.velocity = Player.velocity;
				dust.scale = 0.65f;
				dustPos[dustIter % 180] = dust.position - Player.Center;
				dustID[dustIter % 180] = index;
				dustOwnerID[dustIter % 180] = Player.whoAmI;
				dust.shader = GameShaders.Armor.GetSecondaryShader(Player.cWings, Player);
				dust.alpha = (int)(255 - (255 * Player.stealth));
			}
			for (int i = 0; i < (dustIter > 180 ? 180 : dustIter); i++)
			{
				if (dustID[i] != -1 && dustOwnerID[i] != -1)
				{
					Player owner = Main.player[dustOwnerID[i]];
					Dust dust = Main.dust[dustID[i]];
					if (dust.type == ModContent.DustType<CopyDust2>() && dust.active && dustPos[i] != new Vector2(-1, -1))
					{
						dust.position = dustPos[i] + owner.Center;
						if (dustPos[i].X > 0 && owner.direction == 1)
						{
							dust.active = false;
							dustID[i] = -1;
							dustOwnerID[i] = -1;
							dustPos[i] = new Vector2(-1, -1);
						}
						if (dustPos[i].X < 0 && owner.direction == -1)
						{
							dust.active = false;
							dustID[i] = -1;
							dustOwnerID[i] = -1;
							dustPos[i] = new Vector2(-1, -1);
						}
					}
					else
					{
						dustID[i] = -1;
						dustOwnerID[i] = -1;
						dustPos[i] = new Vector2(-1, -1);
					}
				}
			}
		}
		public void flight()
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(Player);
            if (!CreativeFlightTier2)
			{
				voidPlayer.flatVoidRegen -= voidDrain;
            }
			UpdateSlowFlight();
            HaloDust();
			Player.gravity = 0f;
			Player.noFallDmg = true;
			Player.maxFallSpeed *= 4f;
            Player.wingTime = -1;
			bool up = (Player.controlUp || Player.controlJump) && !Player.controlDown;
			bool down = Player.controlDown && !(Player.controlUp || Player.controlJump) && !TilesBelow();
			bool left = Player.controlLeft && Player.dashDelay >= 0 && !Player.controlRight;
			bool right = Player.controlRight && Player.dashDelay >= 0 && !Player.controlLeft;
            movingVert = up || down;
            movingHori = left || right;
            float accelerationSpeed = wingSpeed / 3f;
			float speedMult = 1.5f;
            //if (movingHori && movingVert)
            //{
			//	speedMult /= MathF.Sqrt(2);
            //}
            if (down)
			{
				float toBeAdded = accelerationSpeed;
				if (Player.velocity.Y + toBeAdded < wingSpeed * speedMult)
				{
					Player.velocity.Y += toBeAdded;
				}
				else if (Player.velocity.Y < wingSpeed * speedMult)
				{
					Player.velocity.Y = wingSpeed * speedMult;
				}
			}
			if (up)
			{
				float toBeAdded = -accelerationSpeed;
				if (Player.velocity.Y + toBeAdded > -wingSpeed * speedMult)
				{
					Player.velocity.Y += toBeAdded;
				}
				else if (Player.velocity.Y > -wingSpeed * speedMult)
				{
					Player.velocity.Y = -wingSpeed * speedMult;
				}
			}
			if (left)
			{
				float toBeAdded = -accelerationSpeed;
				if (Player.velocity.X + toBeAdded > -wingSpeed * speedMult)
				{
					Player.velocity.X += toBeAdded;
				}
				else if (Player.velocity.X > -wingSpeed * speedMult)
				{
					Player.velocity.X = -wingSpeed * speedMult;
				}
			}
			if (right)
			{
				float toBeAdded = accelerationSpeed;
				if (Player.velocity.X + toBeAdded < wingSpeed * speedMult)
				{
					Player.velocity.X += toBeAdded;
				}
				else if (Player.velocity.X < wingSpeed * speedMult)
				{
					Player.velocity.X = wingSpeed * speedMult;
				}
			}
			if (SlowFlight)
			{
				Player.velocity.X = Math.Clamp(Player.velocity.X, -wingSpeed * speedMult, wingSpeed * speedMult);
				Player.velocity.Y = Math.Clamp(Player.velocity.Y, -wingSpeed * speedMult, wingSpeed * speedMult);
            }
			if (!movingVert)
			{
				Player.velocity.Y *= 0.8f;
			}
			if (!movingHori && Player.dashDelay >= 0)
			{
				Player.velocity.X *= 0.8f;
			}
			if (TilesBelow())
            {
                if (AutoCancelFlight)
                {
                    if (!Player.controlDown && !Player.controlUp && !Player.controlLeft && !Player.controlRight && !Player.controlJump && Player.velocity.Y > -2)
                        creativeFlight = false;
                }
                if (Player.controlDown)
                    Player.gravity = Player.defaultGravity + 0.2f;
            }
			else if (Player.velocity.Y == 0f)
				Player.velocity.Y = -0.04f;
		}
		public int randCounter = 0;
		float boost = 0f;
		bool runOnce = true;
		public override void ResetEffects()
		{
			UpdateWingTrails();
            if (gyro && !TilesBelow())
			{
				if (Player.controlDown && !creativeFlight)
				{
					if (boost < 300)
					{
						boost++;
						boost *= 1.025f;
					}
					else
					{
						boost = 300;
					}
					if (Player.velocity.Y != 0 && boost > 40)
					{
						Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Electric, 0, -1);
						dust.velocity *= 0.2f;
						dust.noGravity = true;
					}
				}
				else
				{
					boost = 0f;
				}
				Player.gravity += 0.0025f * boost;
				Player.maxFallSpeed += 0.075f * boost;
			}
			else
			{
				boost = 0f;
			}
			gyro = false;
			randCounter++;
			if (Player.gravDir != 1)
				canCreativeFlight = false;
			if (Player.mount.Active || !canCreativeFlight)
			{
				canCreativeFlight = false;
				wingSpeed = wingSpeedMax;
				creativeFlight = false;
				if (!runOnce)
				{
					DustExplosion();
					runOnce = true;
				}
				return;
			}
			if (canCreativeFlight)
				flightStart();
			if (creativeFlight)
			{
				if (runOnce)
				{
					if (Player.velocity.Y == 0)
						Player.velocity.Y -= 12f;
					DustExplosion();
					runOnce = false;
				}
				flight();
			}
			else
			{
				if (!runOnce)
				{
					DustExplosion();
					runOnce = true;
				}
            }
            wingSpeed = wingSpeedMax;
            CreativeFlightTier2 = canCreativeFlight = false;
		}
		public List<Vector2>[] BladeWingTrails = null;
        public static int MaxBladeTrailLength => SOTS.Config.lowFidelityMode ? 10 : 24;
		public bool WingsBeingVisualized = false;
		private void InitWingTrails()
		{
			BladeWingTrails = new List<Vector2>[18];
			for(int i = 0; i < BladeWingTrails.Length; i++)
			{
				BladeWingTrails[i] = new List<Vector2>();
			}
		}
		private void UpdateWingTrails()
        {
            if (BladeWingTrails == null)
				InitWingTrails();
            for (int i = 0; i < BladeWingTrails.Length; i++)
            {
                List<Vector2> list = BladeWingTrails[i];
				for (int j = 1; j < list.Count - 1; j++)
				{
					Vector2 previous = list[j - 1];
					Vector2 next = list[j + 1];
					Vector2 supposedCenter = previous * 0.5f + next * 0.5f;
					Vector2 me = list[j];
					list[j] = me * 0.5f + supposedCenter * 0.5f;
				}
				if(list.Count > MaxBladeTrailLength || !WingsBeingVisualized)
				{
					bool runOnce = true;
					while(runOnce)
                    {
                        if (list.Count > 0)
                            list.RemoveAt(list.Count - 1);
                        runOnce = list.Count > MaxBladeTrailLength;
                    }
				}
            }
			WingsBeingVisualized = false;
        }
	}
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.ModPlayers;
using SOTS.Common.Systems;
using SOTS.Helpers;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Conduit;
using SOTS.Items.Fragments;
using SOTS.Items.Planetarium;
using SOTS.Items.Pyramid;
using SOTS.Items.Secrets;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SOTS
{
	public static class ConduitHelper
	{
		public static void PreDrawBeforePlayers()
		{
			bool hasDrawnToAcediaPortalNature = false, hasDrawnToAcediaPortalEarth = false;
            bool hasDrawnToAvaritiaPortalChaos = false, hasDrawnToAvaritiaPortalOtherworld = false;
            bool hasDrawnToGulaPortalEarth = false, hasDrawnToGulaPortalEvil = false;
            //bool hasDrawnToDreamLamp = false;
            float AcediaPortalMiddleAlpha = 0.0f, AvaritiaPortalMiddleAlpha = 0.0f, GulaPortalMiddleAlpha = 0.0f;
            foreach (ConduitCounterTE tileEntity in TileEntity.ByID.Values.OfType<ConduitCounterTE>())
			{
				if (tileEntity.ConduitTile != null)
				{
					for (int j = 0; j < Main.player.Length; j++)
					{
						Player player = Main.player[j];
						if (player.active)
						{
							float mult = 1f;
                            int powerType = ConduitPowerType(player, tileEntity.ConduitTile, 0);
                            if (powerType > 0 && j == Main.myPlayer)
							{
								float timer = powerType;
								timer = Math.Clamp(timer, 0, ConduitPlayer.ChargeTime);
								float sinusoid = (float)Math.Sin(MathHelper.ToRadians(180f * timer / ConduitPlayer.ChargeTime));
								mult += sinusoid;
							}
							tileEntity.DrawConduitToLocation(tileEntity.Position.X, tileEntity.Position.Y, player.Center, 0.9f * mult);
						}
					}
					if (ImportantTilesWorld.AcediaPortal.HasValue)
					{
						int x = ImportantTilesWorld.AcediaPortal.Value.X;
						int y = ImportantTilesWorld.AcediaPortal.Value.Y;
						Tile tile = Main.tile[x, y];
						bool nature = tileEntity.ConduitTile.DissolvingTileType == ModContent.TileType<DissolvingNatureTile>();
						bool earthen = tileEntity.ConduitTile.DissolvingTileType == ModContent.TileType<DissolvingEarthTile>();
                        if (tile.HasUnactuatedTile && tile.TileType == ModContent.TileType<AcediaGatewayTile>() &&
							(nature || earthen))
						{
							Vector2 acediaPortal = new Vector2(x * 16, y * 16) + new Vector2(8, 8);
							bool succeededDraw = tileEntity.DrawConduitToLocation(tileEntity.Position.X, tileEntity.Position.Y, acediaPortal, 1f, ColorHelper.AcediaColor);
							if (nature && !hasDrawnToAcediaPortalNature && succeededDraw) //This way, it only draws the acedia portal glow once, no matter how many conduits
							{
								float Percent = tileEntity.tileCountDissolving / 20f;
								Percent *= Percent;
								hasDrawnToAcediaPortalNature = true;
                                DrawGatewayGlowmask(x, y, Main.spriteBatch, Percent, -1);
								AcediaPortalMiddleAlpha += Percent * 0.5f;
							}
							if (earthen && !hasDrawnToAcediaPortalEarth && succeededDraw) //This way, it only draws the acedia portal glow once, no matter how many conduits
							{
								float Percent = tileEntity.tileCountDissolving / 20f;
								Percent *= Percent;
								hasDrawnToAcediaPortalEarth = true;
                                DrawGatewayGlowmask(x, y, Main.spriteBatch, Percent, 1);
								AcediaPortalMiddleAlpha += Percent * 0.5f;
							}
						}
					}
					if (ImportantTilesWorld.AvaritiaPortal.HasValue)
                    {
                        int x = ImportantTilesWorld.AvaritiaPortal.Value.X;
                        int y = ImportantTilesWorld.AvaritiaPortal.Value.Y;
                        Tile tile = Main.tile[x, y];
                        bool chaos = tileEntity.ConduitTile.DissolvingTileType == ModContent.TileType<DissolvingBrillianceTile>();
                        bool otherworld = tileEntity.ConduitTile.DissolvingTileType == ModContent.TileType<DissolvingAetherTile>();
                        if (tile.HasUnactuatedTile && tile.TileType == ModContent.TileType<AvaritianGatewayTile>() &&
                            (chaos || otherworld))
                        {
                            Vector2 avaritiaPortal = new Vector2(x * 16, y * 16) + new Vector2(8, 8);
                            bool succeededDraw = tileEntity.DrawConduitToLocation(tileEntity.Position.X, tileEntity.Position.Y, avaritiaPortal, 1f, ColorHelper.OtherworldColor);
                            if (otherworld && !hasDrawnToAvaritiaPortalOtherworld && succeededDraw) //This way, it only draws the acedia portal glow once, no matter how many conduits
                            {
                                float Percent = tileEntity.tileCountDissolving / 20f;
                                Percent *= Percent;
                                hasDrawnToAvaritiaPortalOtherworld = true;
                                DrawGatewayGlowmask(x, y, Main.spriteBatch, Percent, -1);
                                AvaritiaPortalMiddleAlpha += Percent * 0.5f;
                            }
                            if (chaos && !hasDrawnToAvaritiaPortalChaos && succeededDraw) //This way, it only draws the acedia portal glow once, no matter how many conduits
                            {
                                float Percent = tileEntity.tileCountDissolving / 20f;
                                Percent *= Percent;
                                hasDrawnToAvaritiaPortalChaos = true;
                                DrawGatewayGlowmask(x, y, Main.spriteBatch, Percent, 1);
                                AvaritiaPortalMiddleAlpha += Percent * 0.5f;
                            }
                        }
                    }
                    if (ImportantTilesWorld.GulaPortal.HasValue)
                    {
                        int x = ImportantTilesWorld.GulaPortal.Value.X;
                        int y = ImportantTilesWorld.GulaPortal.Value.Y;
                        Tile tile = Main.tile[x, y];
                        bool earth = tileEntity.ConduitTile.DissolvingTileType == ModContent.TileType<DissolvingEarthTile>();
                        bool evil = tileEntity.ConduitTile.DissolvingTileType == ModContent.TileType<DissolvingUmbraTile>();
                        if (tile.HasUnactuatedTile && tile.TileType == ModContent.TileType<GulaGatewayTile>() &&
                            (earth || evil))
                        {
                            Vector2 gulaPortal = new Vector2(x * 16, y * 16) + new Vector2(8, 8);
                            bool succeededDraw = tileEntity.DrawConduitToLocation(tileEntity.Position.X, tileEntity.Position.Y, gulaPortal, 1f, ColorHelper.RedEvilColor);
                            if (earth && !hasDrawnToGulaPortalEarth && succeededDraw) //This way, it only draws the acedia portal glow once, no matter how many conduits
                            {
                                float Percent = tileEntity.tileCountDissolving / 20f;
                                Percent *= Percent;
                                hasDrawnToGulaPortalEarth = true;
                                DrawGatewayGlowmask(x, y, Main.spriteBatch, Percent, -1);
                                GulaPortalMiddleAlpha += Percent * 0.5f;
                            }
                            if (evil && !hasDrawnToGulaPortalEvil && succeededDraw) //This way, it only draws the acedia portal glow once, no matter how many conduits
                            {
                                float Percent = tileEntity.tileCountDissolving / 20f;
                                Percent *= Percent;
                                hasDrawnToGulaPortalEvil = true;
                                DrawGatewayGlowmask(x, y, Main.spriteBatch, Percent, 1);
                                GulaPortalMiddleAlpha += Percent * 0.5f;
                            }
                        }
                    }
                    if (ImportantTilesWorld.dreamLamp.HasValue && tileEntity.ConduitTile.DissolvingTileType == ModContent.TileType<DissolvingNatureTile>())
					{
						int x = ImportantTilesWorld.dreamLamp.Value.X;
						int y = ImportantTilesWorld.dreamLamp.Value.Y;
						Tile tile = Main.tile[x, y];
						if (tile.HasUnactuatedTile && tile.TileType == ModContent.TileType<ForgottenLampTile>())
						{
							Vector2 dreamLamp = new Vector2(x * 16, y * 16) + new Vector2(8, 8);
							tileEntity.DrawConduitToLocation(tileEntity.Position.X, tileEntity.Position.Y, dreamLamp, 1f, ColorHelper.DreamLampColor);
						}
					}
					tileEntity.DrawConduitAura(tileEntity.Position.X, tileEntity.Position.Y);
                }
			}
            DrawGateway(ImportantTilesWorld.AcediaPortal, AcediaPortalMiddleAlpha, hasDrawnToAcediaPortalNature, hasDrawnToAcediaPortalEarth);
            DrawGateway(ImportantTilesWorld.AvaritiaPortal, AvaritiaPortalMiddleAlpha, hasDrawnToAvaritiaPortalOtherworld, hasDrawnToAvaritiaPortalChaos);
            DrawGateway(ImportantTilesWorld.GulaPortal, GulaPortalMiddleAlpha, hasDrawnToGulaPortalEarth, hasDrawnToGulaPortalEvil);
        }
        public static void DrawGateway(Point16? portal, float percent, bool left, bool right)
        {
            if (portal.HasValue && (left || right))
            {
                int x = portal.Value.X;
                int y = portal.Value.Y;
                if (percent > 0.0f)
                    DrawGatewayGlowmask(x, y, Main.spriteBatch, percent, 0);
            }
        }
        public static int ConduitPowerType(Player player, ConduitTile cT, int change = 0)
        {
            ConduitPlayer CP = player.ConduitPlayer();
            if (cT.DissolvingTileType == ModContent.TileType<DissolvingNatureTile>())
                return CP.NaturePower += change;
            if (cT.DissolvingTileType == ModContent.TileType<DissolvingEarthTile>())
                return CP.EarthPower += change;
            if (cT.DissolvingTileType == ModContent.TileType<DissolvingAuroraTile>())
                return CP.PermafrostPower += change;
            if (cT.DissolvingTileType == ModContent.TileType<DissolvingAetherTile>())
                return CP.OtherworldPower += change;
            if (cT.DissolvingTileType == ModContent.TileType<DissolvingDelugeTile>())
                return CP.TidePower += change;
            if (cT.DissolvingTileType == ModContent.TileType<DissolvingUmbraTile>())
                return CP.EvilPower += change;
            if (cT.DissolvingTileType == ModContent.TileType<DissolvingNetherTile>())
                return CP.InfernoPower += change;
            if (cT.DissolvingTileType == ModContent.TileType<DissolvingBrillianceTile>())
                return CP.ChaosPower += change;
            return -1;
        }
		public static void DrawPlayerEffectOutline(Player player)
		{
			ConduitPlayer CP = player.ConduitPlayer();
            DrawSingleCircle(player, ColorHelper.NatureColor, CP.NaturePower);
            DrawSingleCircle(player, ColorHelper.EarthColor, CP.EarthPower);
            DrawSingleCircle(player, ColorHelper.PermafrostColor, CP.PermafrostPower);
            DrawSingleCircle(player, ColorHelper.PurpleOtherworldColor, CP.OtherworldPower);
            DrawSingleCircle(player, ColorHelper.TideColor, CP.TidePower);
            DrawSingleCircle(player, ColorHelper.RedEvilColor, CP.EvilPower);
            DrawSingleCircle(player, ColorHelper.Inferno1, CP.InfernoPower);
            DrawSingleCircle(player, ColorHelper.ChaosPink, CP.ChaosPower);
        }
        public static void DrawSingleCircle(Player player, Color color, int Power)
        {
            if (Power > 0 && Power < ConduitPlayer.ChargeTime)
            {
                float timer = Power;
                if (timer > 0)
                {
                    float percent = timer / ConduitPlayer.ChargeTime;
                    DrawConduitCircleFull(player.Center, percent, color);
                }
            }
        }
		public static void DrawConduitCircleFull(Vector2 position, float percent, Color color)
		{
			color.A = 0;
			SOTSProjectile.DrawStar(position, color, 0.3f * percent, MathHelper.PiOver4, 0f, 1, 52f * (float)Math.Sqrt((1 - percent)), 0, 1f, 600, 0, 1);
		}
        public static void DrawGatewayGlowmask(int x, int y, SpriteBatch spriteBatch, float percent = 1, int side = -1)
        {
            Tile tile = Main.tile[x, y];
            x = x - tile.TileFrameX / 18;
            y = y - tile.TileFrameY / 18;
            string variant = side == -1 ? "Left" : side == 0 ? "Middle" : "Right";
            int maximumGlow = 8;
            maximumGlow = (int)(maximumGlow * percent + 0.5f);
            Texture2D texture = ModContent.Request<Texture2D>("SOTS/Items/Conduit/Portal/AcediaGatewayTileGlow" + variant).Value;
            Texture2D textureMask = ModContent.Request<Texture2D>("SOTS/Items/Conduit/Portal/AcediaGatewayTileGlowMask" + variant).Value;
            Color defaultColor = new Color(120, 100, 130, 0);
            Color alternatingColor = ColorHelper.AcediaColor * 0.65f;
            if (tile.TileType == ModContent.TileType<AvaritianGatewayTile>())
			{
                texture = ModContent.Request<Texture2D>("SOTS/Items/Conduit/Portal/AvaritianGatewayTileGlow" + variant).Value;
                textureMask = ModContent.Request<Texture2D>("SOTS/Items/Conduit/Portal/AvaritianGatewayTileGlowMask" + variant).Value;
                defaultColor = new Color(120, 100, 130, 0);
                alternatingColor = ColorHelper.OtherworldColor * 0.65f;
            }
            if (tile.TileType == ModContent.TileType<GulaGatewayTile>())
            {
                texture = ModContent.Request<Texture2D>("SOTS/Items/Conduit/Portal/GulaGatewayTileGlow" + variant).Value;
                textureMask = ModContent.Request<Texture2D>("SOTS/Items/Conduit/Portal/GulaGatewayTileGlowMask" + variant).Value;
                defaultColor = new Color(130, 100, 110, 0);
                alternatingColor = ColorHelper.GulaColor * 0.65f;
            }
            for (int twice = 0; twice < 2; twice++)
            {
                for (int i = x; i < x + 9; i++)
                {
                    for (int j = y; j < y + 9; j++)
                    {
                        tile = Main.tile[i, j];
                        float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 6;
                        float lerpMult = 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(-uniquenessCounter));
                        Color color = Color.Lerp(defaultColor, alternatingColor, lerpMult);
                        color.A = 0;
                        Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
                        Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y);
                        if (twice == 1)
                        {
                            for (int k = 0; k <= maximumGlow; k++)
                            {
                                float offset = 2f * percent;
                                Vector2 circular = new Vector2(offset, 0).RotatedBy(MathHelper.TwoPi * k / maximumGlow + MathHelper.ToRadians(SOTSWorld.GlobalCounter * side * 0.75f));
                                spriteBatch.Draw(texture, pos + circular + new Vector2(0, 2), frame, color * percent * 0.45f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                            }
                        }
                        else
                        {
                            color = Color.White;
                            spriteBatch.Draw(textureMask, pos + new Vector2(0, 2), frame, color * 0.75f * percent, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        }
                    }
                }
            }
        }
    }
    public class ConduitItem : GlobalItem
    {
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(RunOnce);
            writer.Write(ConduitTransformTimer);
            writer.Write(ConduitTransformType);
            writer.Write(ConduitX);
            writer.Write(ConduitY);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            RunOnce = reader.ReadBoolean();
            ConduitTransformTimer = reader.ReadInt32();
            ConduitTransformType = reader.ReadInt32();
            ConduitX = reader.ReadInt32();
            ConduitY = reader.ReadInt32();
        }
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            bool validItem = entity.type == ModContent.ItemType<DreamLamp>() || entity.type == ModContent.ItemType<CursedApple>(); //Plan to expand this system more thoroughly in the future. For now, we will just check the 2 items that it effects
            return lateInstantiation && validItem;
        }
        public static Color ConduitColor(int i)
        {
            Color color;
            switch (i)
            {
                case 0:
                    color = ColorHelper.NatureColor;
                    break;
                case 1:
                    color = ColorHelper.EarthColor;
                    break;
                case 2:
                    color = ColorHelper.PermafrostColor;
                    break;
                case 3:
                    color = ColorHelper.PurpleOtherworldColor;
                    break;
                case 4:
                    color = ColorHelper.TideColor;
                    break;
                case 5:
                    color = ColorHelper.RedEvilColor;
                    break;
                case 6:
                    color = ColorHelper.Inferno1;
                    break;
                default:
                    color = ColorHelper.ChaosPink;
                    break;
            }
            return color;
        }
        public override bool InstancePerEntity => true;
        private bool IsBeingTransformed => ConduitTransformTimer > 0 && ConduitTransformType != -1;
        private ConduitCounterTE MyConduit
        {
            get
            {
                bool success = TileEntity.ByPosition.TryGetValue(new Point16(ConduitX, ConduitY), out TileEntity te);
                if(success && te is ConduitCounterTE ccte)
                {
                    return ccte;
                }
                return null;
            }
        }
        private float TransfromPercent => ConduitTransformTimer / 150f;
        private float PrevConduitTransformTimer = -30;
        private float ConduitTransformTimer = -30;
        private int ConduitTransformType = -1;
        private int ConduitX = -1;
        private int ConduitY = -1;
        private bool RunOnce = true;
        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (MyConduit != null)
                MyConduit.DrawConduitToLocation(ConduitX, ConduitY, item.Center, 1f, ConduitColor(ConduitTransformType));
            return true;
        }
        public override void PostDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            if (IsBeingTransformed)
            {
                ConduitHelper.DrawConduitCircleFull(item.Center, TransfromPercent, ConduitColor(ConduitTransformType) * MathF.Min(1, ConduitTransformTimer / 90f));
            }
        }
        public override bool OnPickup(Item item, Player player) //happens only on client
        {
            RunOnce = true;
            return true;
        }
        private void ResetVars(Item item)
        {
            ConduitTransformTimer = -30;
            ConduitTransformType = -1;
            ConduitX = -1;
            ConduitY = -1;
        }
        public void CheckForNearbyConduits(Item item)
        {
            bool validForNature = (item.type == ModContent.ItemType<DreamLamp>() && !SOTSWorld.DreamLampSolved) || (item.type == ModContent.ItemType<CursedApple>() && !SOTSWorld.GoldenAppleSolved);
            bool validForEvil = (item.type == ModContent.ItemType<DreamLamp>() && SOTSWorld.DreamLampSolved) || (item.type == ModContent.ItemType<CursedApple>() && SOTSWorld.GoldenAppleSolved);
            foreach (ConduitCounterTE tileEntity in TileEntity.ByID.Values.OfType<ConduitCounterTE>())
            {
                if (tileEntity.ConduitTile != null && tileEntity.tileCountDissolving >= 20)
                {
                    int type = tileEntity.ConduitTile.DissolvingTileType;
                    bool worksForNature = type == ModContent.TileType<DissolvingNatureTile>() && validForNature;
                    bool worksForEvil = type == ModContent.TileType<DissolvingUmbraTile>() && validForEvil;
                    if (worksForNature || worksForEvil)
                    {
                        float distance = Vector2.Distance(tileEntity.Position.ToVector2() * 16 + new Vector2(8, 8), item.Center);
                        if (distance <= 640)
                        {
                            ConduitX = tileEntity.Position.X;
                            ConduitY = tileEntity.Position.Y;
                            ConduitTransformType = worksForEvil ? 5 : 0;
                            break;
                        }
                    }
                }
            }
        }
        public override void Update(Item item, ref float gravity, ref float maxFallSpeed) //Happens on server and client
        {
            if (RunOnce)
            {
                ResetVars(item);
                CheckForNearbyConduits(item);
                RunOnce = false;
            }
            //if (Main.netMode == NetmodeID.Server)
            //    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(ConduitTransformTimer + ", " + ConduitTransformType), Color.Red);
            //else
            //    Main.NewText(ConduitTransformTimer + ", " + ConduitTransformType, Color.Green);
            if (MyConduit == null || MyConduit.tileCountDissolving < 20)
            {
                ResetVars(item);
                if(SOTSWorld.GlobalCounter % 5 == 0)
                    CheckForNearbyConduits(item);
            }
            else
            {
                if (IsBeingTransformed)
                {
                    float perc = 1 - TransfromPercent;
                    gravity *= perc * perc;
                    maxFallSpeed *= perc * perc;
                }
                if (ConduitTransformType >= 0)
                    ConduitTransformTimer++;
                if (ConduitTransformTimer > 150)
                {
                    if (ConduitTransformType == 0)
                    {
                        if (item.type == ModContent.ItemType<DreamLamp>())
                        {
                            SOTSWorld.DreamLampSolved = true;
                            if (Main.netMode != NetmodeID.SinglePlayer)
                                SOTSWorld.SyncGemLocks(Main.LocalPlayer);
                        }
                        if (item.type == ModContent.ItemType<CursedApple>())
                        {
                            SOTSWorld.GoldenAppleSolved = true;
                            if (Main.netMode != NetmodeID.SinglePlayer)
                                SOTSWorld.SyncGemLocks(Main.LocalPlayer);
                        }
                    }
                    if (ConduitTransformType == 5)
                    {
                        if (item.type == ModContent.ItemType<DreamLamp>())
                        {
                            SOTSWorld.DreamLampSolved = false;
                            if (Main.netMode != NetmodeID.SinglePlayer)
                                SOTSWorld.SyncGemLocks(Main.LocalPlayer);
                        }
                        if (item.type == ModContent.ItemType<CursedApple>())
                        {
                            SOTSWorld.GoldenAppleSolved = false;
                            if (Main.netMode != NetmodeID.SinglePlayer)
                                SOTSWorld.SyncGemLocks(Main.LocalPlayer);
                        }
                    }
                    EndingDust(item);
                    ResetVars(item);
                }
            }
        }
        public override bool CanPickup(Item item, Player player) => !IsBeingTransformed;
        private void EndingDust(Item item)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(new EntitySource_Misc("SOTS:ConduitTransform"), item.Center, Vector2.Zero, ModContent.ProjectileType<ConduitParticleProjectile>(), 0, 0, Main.myPlayer, ConduitTransformType, ConduitX, ConduitY);
            }
        }
    }
    public class ConduitParticleProjectile : ModProjectile
    {
        public override string Texture => "SOTS/Assets/WhitePixel";
        public override void SetDefaults()
        {
            Projectile.friendly = Projectile.hostile = false;
            Projectile.hide = true;
            Projectile.timeLeft = 2;
            Projectile.width = 16;
            Projectile.height = 16;
        }
        public override bool PreAI()
        {
            Projectile.Kill();
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Color c = ConduitItem.ConduitColor((int)Projectile.ai[0]);
                SOTSUtils.PlaySound(SoundID.Item30, Projectile.Center, 1f, -0.1f);
                for (int a = 0; a < 72; a++)
                {
                    Vector2 outward = new Vector2(0, 9f).RotatedBy(MathHelper.TwoPi * a / 24f);
                    Dust dust = Dust.NewDustDirect(Projectile.Center + outward.SafeNormalize(Vector2.Zero) * 16 - new Vector2(4, 4), 0, 0, ModContent.DustType<Dusts.AlphaDrainDust>(), 0, 0, 0, c, 1.1f);
                    dust.scale *= 1.5f;
                    dust.velocity *= 0.6f;
                    dust.velocity += outward / dust.scale;
                    dust.fadeIn = 0.1f;
                    dust.noGravity = true;
                }
                Vector2 conduitPosition = new Vector2(Projectile.ai[1] * 16 + 8, Projectile.ai[2] * 16 + 8);
                Vector2 betweenConduit = conduitPosition - Projectile.Center;
                float distanceBetweenDust = 7f;
                float length = betweenConduit.Length() / distanceBetweenDust;
                for(int d = 0; d < length; d++)
                {
                    Vector2 spawnPosition = Projectile.Center + betweenConduit.SafeNormalize(Vector2.Zero) * distanceBetweenDust * d;
                    Dust dust = Dust.NewDustDirect(spawnPosition - new Vector2(4, 4), 0, 0, ModContent.DustType<Dusts.AlphaDrainDust>(), 0, 0, 0, c, 1.1f);
                    dust.scale *= 1.4f;
                    dust.velocity *= 0.5f;
                    dust.fadeIn = 0.1f;
                    dust.noGravity = true;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}

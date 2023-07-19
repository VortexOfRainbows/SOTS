using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Items.Void;
using SOTS.NPCs.Boss;
using SOTS.Items;
using SOTS.Buffs;
using SOTS.NPCs.Constructs;
using SOTS.Items.Celestial;
using SOTS.NPCs.Boss.Polaris;
using SOTS.NPCs.Inferno;
using SOTS.NPCs.Boss.Advisor;
using SOTS.Items.Pyramid;
using SOTS.Items.Otherworld;
using SOTS.Items.Pyramid.PyramidWalls;
using SOTS.NPCs.TreasureSlimes;
using SOTS.Items.Fragments;
using SOTS.Dusts;
using SOTS.Items.Crushers;
using SOTS.Items.Nature;
using SOTS.Items.Tools;
using SOTS.Items.Slime;
using SOTS.Items.Evil;
using SOTS.Items.Tide;
using SOTS.Items.Permafrost;
using SOTS.Items.Otherworld.Blocks;
using SOTS.Items.Inferno;
using SOTS.Items.Chaos;
using SOTS.Items.AbandonedVillage;
using SOTS.NPCs.Phase;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using System.Linq;
using SOTS.NPCs;
using SOTS.Items.Otherworld.FromChests;
using Terraria.GameContent.Bestiary;
using SOTS.Biomes;
using SOTS.Items.Temple;
using Terraria.Localization;
using SOTS.Items.Furniture.Functional;
using SOTS.NPCs.Boss.Glowmoth;
using SOTS.NPCs.Boss.Lux;
using SOTS.Items.Earth.Glowmoth;
using Terraria;
using System.IO;
using Terraria.ModLoader.IO;
using Terraria.ID;
using SOTS.NPCs.Town;
using Microsoft.Xna.Framework.Graphics;
using static SOTS.SOTS;

namespace SOTS.Common
{
    public static class GlobalEntity
    {
        public static void SendServerChanges(Mod Mod, Item item, bool recentlyTeleported)
        {
            var packet = Mod.GetPacket();
            packet.Write((byte)SOTSMessageType.SyncHasTeleported);
            packet.Write(item.whoAmI);
            packet.Write(true);
            packet.Write(recentlyTeleported);
            packet.Send();
        }
        public static void SendServerChanges(Mod Mod, Entity npc, bool recentlyTeleported)
        {
            var packet = Mod.GetPacket();
            packet.Write((byte)SOTSMessageType.SyncHasTeleported);
            packet.Write(npc.whoAmI);
            packet.Write(false);
            packet.Write(recentlyTeleported);
            packet.Send();
        }
        public static void DrawTimeFreeze(Entity ent, SpriteBatch spriteBatch, float percent, float sizeScale = 1f)
        {
            float alphaMult = percent;
            int type = ent.whoAmI % 3 + 1;
            Texture2D ring1 = ModContent.Request<Texture2D>("SOTS/Common/GlobalNPCs/FreezeSpiral" + type).Value;
            Vector2 ringOrigin = new Vector2(ring1.Width / 2, ring1.Height / 2);
            Vector2 drawPos = new Vector2(ent.Center.X, ent.Center.Y) - Main.screenPosition;
            Color color = new Color(70, 0, 105, 0);
            float secondsHandMult = Main.GameUpdateCount / 90f;
            float drawDimensions = ent.Size.Length();
            float scale = 0.02f + 1.04f * (drawDimensions / 800f) * sizeScale;
            float rotation1 = secondsHandMult * MathHelper.TwoPi;
            int direction = ent.direction;
            if (direction == 0)
                direction = 1;
            for (int i = 0; i < 2; i++)
            {
                spriteBatch.Draw(ring1, drawPos, null, color * alphaMult, rotation1 * direction, ringOrigin, scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                color = new Color(40, 0, 100, 0);
                rotation1 = secondsHandMult * MathHelper.TwoPi * 0.5f;
                scale *= 0.97f;
            }
        }
    }

    public class GlobalEntityNPC : GlobalNPC
    {
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (RecentlyTeleported)
            {
                float multiplier = TeleportCounter / 60f;
                if (multiplier > 1)
                    multiplier = 1;
                if(TeleportCounter > 1140)
                {
                    multiplier = (1200 - TeleportCounter) / 60f;
                }
                GlobalEntity.DrawTimeFreeze(npc, spriteBatch, multiplier);
            }
            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }
        public override bool InstancePerEntity => true;
        public bool RecentlyTeleported = false;
        public int TeleportCounter = 0;
        public override void PostAI(NPC npc)
        {
            if (RecentlyTeleported)
            {
                TeleportCounter++;
                if (TeleportCounter == 3)
                {
                    PortalDrawingHelper.SpawnDust(npc);
                }
                if (TeleportCounter >= 1200)
                {
                    TeleportCounter = 0;
                    RecentlyTeleported = false;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        GlobalEntity.SendServerChanges(Mod, npc, false);
                        npc.netUpdate = true;
                    }
                }
            }
            else
                TeleportCounter = 0;
        }
        public override bool CheckActive(NPC npc)
        {
            return RecentlyTeleported ? false : base.CheckActive(npc);
        }
    }
    public class GlobalEntityItem : GlobalItem
    {
        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if(RecentlyTeleported)
            {
                float multiplier = TeleportCounter / 60f;
                if(multiplier > 1)
                    multiplier = 1;
                GlobalEntity.DrawTimeFreeze(item, spriteBatch, multiplier);
            }
            return base.PreDrawInWorld(item, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }
        public override bool InstancePerEntity => true;
        public bool RecentlyTeleported = false;
        public int TeleportCounter = 0;
        public override void PostUpdate(Item item)
        {
            if (RecentlyTeleported)
            {
                TeleportCounter++;
                if (TeleportCounter == 3)
                {
                    PortalDrawingHelper.SpawnDust(item);
                }
            }
            else
            {
                TeleportCounter = 0;
            }
        }
        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if(RecentlyTeleported)
            {
                if (item.velocity.Y > 10)
                    item.velocity.Y = 10;
                item.velocity.Y *= 0.96f;
                gravity = 0.0f;
                maxFallSpeed = 10.0f;
            }
        }
        public override bool CanPickup(Item item, Player player)
        {
            return TeleportCounter == 0 || TeleportCounter > 120;
        }
        public override bool OnPickup(Item item, Player player)
        {
            RecentlyTeleported = false;
            return base.OnPickup(item, player);
        }
        public void NetUpdate(Item item)
        {
            NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item.whoAmI);
            //GlobalEntity.SendServerChanges(Mod, item, RecentlyTeleported);
        }
    }
}
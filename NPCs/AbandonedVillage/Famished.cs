using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using SOTS.Projectiles.AbandonedVillage;
using SOTS.WorldgenHelpers;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using static SOTS.SOTS;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.AbandonedVillage
{
	public class Famished : ModNPC
	{
        public static Color GlowColor
        {
            get
            {
                Color c = WorldGen.crimson ? new Color(255, 185, 81, 0) : new Color(169, 202, 44, 0);
                return c;
            }
        }
        public class FamishBlock
        {
            public int i => (int)owner.Center.X / 16 + x - MaxRadius;
            public int j => (int)owner.Center.Y / 16 + y - MaxRadius;
            public FamishBlock(NPC owner, int x, int y, int FrameX, int FrameY)
            {
                FrameNumber = Main.rand.Next(3);
                this.x = x;
                this.y = y;
                this.FrameX = FrameX;
                this.FrameY = FrameY;
                this.owner = owner;
                HasDetail = Main.rand.NextBool(2);
                HasPathToHost = true;
            }
            public NPC owner;
            public int FrameNumber;
            public int x; //Local positions
            public int y;
            public int FrameX;
            public int FrameY;
            public bool HasPathToHost;
            public bool HasDetail;
            public Rectangle Rect => new Rectangle(FrameX, FrameY, 16, 16);
        }
        public class FamishVine
        {
            public NPC owner;
            public Vector2 StartPosition;
            public Vector2 EndPosition;
            public Vector2 Normal;
            public float Counter { get; private set; }
            public float IdleCounter = 0;
            public bool Kill;
            public float HeightMult;
            public bool IgnoreTiles;
            public FamishVine(NPC owner, Vector2 startPosition, Vector2 endPosition, Vector2 normal, bool ignoreTiles = false)
            {
                StartPosition = startPosition;
                EndPosition = endPosition;
                Normal = normal;
                this.owner = owner;
                HeightMult = ignoreTiles ? 2.5f : Main.rand.NextFloat(0.75f, 1.3f);
                Kill = false;
                IgnoreTiles = ignoreTiles;
            }
            public void Update()
            {
                if (IgnoreTiles)
                {
                    StartPosition = owner.Center;
                    HeightMult = Math.Min(3, StartPosition.Distance(EndPosition) / 80f);
                }
                if (!HasBlock(StartPosition) && !IgnoreTiles)
                    Kill = true;
                else if (!HasBlock(EndPosition) && !IgnoreTiles)
                {
                    if (Counter > 40)
                        Counter = 40;
                    Counter -= 1.5f;
                }
                else if (Counter < 40)
                    Counter++;
                IdleCounter++;
                if (Counter < 0)
                {
                    Kill = true;
                }
            }
            public bool HasBlock(Vector2 pos)
            {
                if (owner.active && owner.type == NPCType<Famished>() && owner.ModNPC is Famished fm)
                {
                    return fm.HasTile(pos);
                }
                return false;
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(targetPosition);
            writer.WriteVector2(groundedPosition);
            writer.Write(CurrentBlocks);
            writer.Write(TotalBlocks);
            writer.Write(QueueHurtFromOtherSource);
            writer.Write(ShootCounter);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            targetPosition = reader.ReadVector2();
            groundedPosition = reader.ReadVector2();
            CurrentBlocks = reader.ReadInt32();
            TotalBlocks = reader.ReadInt32();
            QueueHurtFromOtherSource = reader.ReadSingle();
            ShootCounter = reader.ReadSingle();
        }
        public void ReceiveBlockData(BinaryReader reader)
        {
            int fromClient = reader.ReadInt32();
            int x = reader.ReadInt32();
            int y = reader.ReadInt32();
            int change = reader.ReadInt32();
            
            if (change == 0)
                KillBlock(x, y, true, false); //Dont send packets since we just received the packets we need
            if (change == 1)
                KillBlock(x, y, false, false);
            if (change == 2)
                AddBlock(x, y, false);

            if (Main.netMode == NetmodeID.Server) //If the server receives the packet, it should send it to everyone except the client the server received it from!
            {
                SendBlockData(x, y, change, Main.myPlayer, fromClient);
            }
        }
        public void SendBlockData(int x, int y, int change, int fromClient, int ignoreClient = -1)
        {
            var packet = Mod.GetPacket();
            packet.Write((byte)SOTSMessageType.SyncFamineBlock);
            packet.Write(NPC.whoAmI);
            packet.Write(fromClient);
            packet.Write(x);
            packet.Write(y);
            packet.Write(change);
            packet.Send(-1, ignoreClient);
        }
        public static List<int> TileBreakListeners = new List<int>();
        public static bool CheckForListeners(int x, int y, bool killListeners)
        {
            for (int i = TileBreakListeners.Count - 1; i >= 0; i--)
            {
                if (UpdateTileListener(TileBreakListeners[i], x, y, killListeners))
                {
                    return true;
                }
            }
            return false;
        }
        private static bool UpdateTileListener(int ID, int i, int j, bool killListeners)
        {
            if(ID < 0)
            {
                TileBreakListeners.Remove(ID);
            }
            else
            {
                NPC npc = Main.npc[ID];
                if(npc.active && npc.type == NPCType<Famished>() && npc.ModNPC is Famished fm)
                {
                    int x = i + MaxRadius - (int)npc.Center.X / 16;
                    int y = j + MaxRadius - (int)npc.Center.Y / 16;
                    if(Math.Abs(x - MaxRadius) <= 1 && Math.Abs(y - MaxRadius) <= 1) //Any blocks in the center (where the heart is), are unbreakable.
                    {
                        killListeners = false;
                    }
                    if(!killListeners)
                    {
                        if (x <= 0 || y <= 0 || x >= MaxRadius * 2 || y >= MaxRadius * 2)
                            return false;
                        if (fm.Block[x, y] != null)
                            return true;
                    }
                    else
                        return fm.KillBlock(x, y, true, true);
                }
                else
                {
                    TileBreakListeners.Remove(ID);
                }
            }
            return false;
        }
        private bool HasTile(Vector2 pos)
        {
            return HasTile((int)pos.X / 16, (int)pos.Y / 16);
        }
        private bool HasTile(int i, int j)
        {
            int x = i + MaxRadius - (int)NPC.Center.X / 16;
            int y = j + MaxRadius - (int)NPC.Center.Y / 16;
            if (x <= 0 || y <= 0 || x >= MaxRadius * 2 || y >= MaxRadius * 2)
                return false;
            if (Block[x, y] != null)
                return true;
            return false;
        }
        private Queue<FamishBlock> floodFillQueue = new Queue<FamishBlock>();
        public const int MaxRadius = 14;
        public int MaxSize => MaxRadius * MaxRadius * 4;
        public static int LifePerBlock => Main.masterMode ? 15 : Main.expertMode ? 10 : 5;
        public void UpdateConnections()
        {
            foreach (FamishBlock fb in Block)
            {
                if(fb != null)
                {
                    fb.HasPathToHost = false;
                }
            }
            floodFillQueue = new Queue<FamishBlock>();
            if(Block[MaxRadius, MaxRadius] != null)
                FloodFill(Block[MaxRadius, MaxRadius]);
            int c = 0;
            while (floodFillQueue.TryDequeue(out FamishBlock block))
            {
                if (c > 1000)
                {
                    Main.NewText("SOTS ERROR: SOMEHOW OVERFLOWED FLOODFILL QUEUE", Color.Red);
                    break;
                }
                //Main.NewText(block);
                FloodFill(block);
            }
        }
        public void AddBlockToQueue(FamishBlock fb)
        {
            if (fb != null && !fb.HasPathToHost)
                floodFillQueue.Enqueue(fb);
        }
        public void FloodFill(FamishBlock fb)
        {
            fb.HasPathToHost = true;
            int x = fb.x;
            int y = fb.y;
            if (x < 1 || x >= MaxRadius * 2 || y < 1 || y >= MaxRadius * 2)
            {
                return;
            }
            AddBlockToQueue(Block[x - 1, y]);
            AddBlockToQueue(Block[x + 1, y]);
            AddBlockToQueue(Block[x, y - 1]);
            AddBlockToQueue(Block[x, y + 1]);
        }   
		public void SetFrame(int x, int y, int w, int h)
		{
            if(Block[x, y].FrameX != w || Block[x, y].FrameY != h)
            {
                Block[x, y].FrameX = w;
                Block[x, y].FrameY = h;
            }
        }
        public void FrameTile(FamishBlock toFrame)
        {
            if (toFrame == null)
                return;
            int x = toFrame.x;
            int y = toFrame.y;
            if (Block[x, y] == null || x < 1 || x >= MaxRadius * 2 || y < 1 || y >= MaxRadius * 2)
            {
                return;
            }
            FamishBlock tileLeft = Block[x - 1, y];
            FamishBlock tileRight = Block[x + 1, y];
            FamishBlock tileUp = Block[x, y - 1];
            FamishBlock tileDown = Block[x, y + 1];
            FamishBlock tileTopLeft = Block[x - 1, y - 1];
            FamishBlock tileTopRight = Block[x + 1, y - 1];
            FamishBlock tileBottomLeft = Block[x - 1, y + 1];
            FamishBlock tileBottomRight = Block[x + 1, y + 1];
            //if (Block[x, y].Slope != SlopeType.Solid || Block[x, y].IsHalfBlock)
            //{
            //	forceNoneDown = Block[x, y].BottomSlope;
            //	forceNoneLeft = Block[x, y].LeftSlope;
            //	forceNoneUp = Block[x, y].TopSlope;
            //	forceNoneRight = Block[x, y].RightSlope;
            //	if (Block[x, y].IsHalfBlock)
            //		forceNoneUp = true;
            //	forceSameLeft = tileLeft.HasTile ? true : forceSameLeft;
            //	forceSameRight = tileRight.HasTile ? true : forceSameRight;
            //	forceSameUp = tileUp.HasTile ? true : forceSameUp;
            //	forceSameDown = tileDown.HasTile ? true : forceSameDown;
            //}
            //if ((tileDown.IsHalfBlock || tileDown.TopSlope) && tileDown.HasTile)
            //	forceNoneDown = true;
            //if (tileUp.BottomSlope && tileUp.HasTile)
            //	forceNoneUp = true;
            //if (tileLeft.RightSlope && tileLeft.HasTile)
            //	forceNoneLeft = true;
            //if (tileRight.LeftSlope && tileRight.HasTile)
            //	forceNoneRight = true;
            Similarity leftSim = tileLeft != null ? Similarity.Same : Similarity.None;
            Similarity rightSim = tileRight != null ? Similarity.Same : Similarity.None;
            Similarity upSim = tileUp != null ? Similarity.Same : Similarity.None;
            Similarity downSim = tileDown != null ? Similarity.Same : Similarity.None;
            Similarity topLeftSim = tileTopLeft != null ? Similarity.Same : Similarity.None;
            Similarity topRightSim = tileTopRight != null ? Similarity.Same : Similarity.None;
            Similarity bottomLeftSim = tileBottomLeft != null ? Similarity.Same : Similarity.None;
            Similarity bottomRightSim = tileBottomRight != null ? Similarity.Same : Similarity.None;
            int randomFrame = Block[x, y].FrameNumber;
            switch (leftSim)
            {
                case Similarity.None:
                    switch (upSim)
                    {
                        case Similarity.Same:
                            switch (downSim)
                            {
                                case Similarity.Same:
                                    switch (rightSim)
                                    {
                                        case Similarity.Same:
                                            SetFrame(x, y, 0, 18 * randomFrame);
                                            break;
                                        default:
                                            SetFrame(x, y, 90, 18 * randomFrame);
                                            break;
                                    }
                                    break;
                                default:
                                    if (rightSim == Similarity.Same)
                                    {
                                        SetFrame(x, y, 36 * randomFrame, 72);
                                    }
                                    else
                                    {
                                        SetFrame(x, y, 108 + 18 * randomFrame, 54);
                                    }
                                    break;
                            }
                            break;
                        default:
                            switch (downSim)
                            {
                                case Similarity.Same:
                                    if (rightSim == Similarity.Same)
                                    {
                                        SetFrame(x, y, 36 * randomFrame, 54);
                                        break;
                                    }
                                    _ = 1;
                                    SetFrame(x, y, 108 + 18 * randomFrame, 0);
                                    break;
                                default:
                                    switch (rightSim)
                                    {
                                        case Similarity.Same:
                                            SetFrame(x, y, 162, 18 * randomFrame);
                                            break;
                                        default:
                                            SetFrame(x, y, 162 + 18 * randomFrame, 54);
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    return;
            }
            switch (upSim)
            {
                case Similarity.Same:
                    switch (downSim)
                    {
                        case Similarity.Same:
                            switch (rightSim)
                            {
                                case Similarity.Same:
                                    switch (topLeftSim)
                                    {
                                        case Similarity.Same:
                                            if (topRightSim == Similarity.Same)
                                            {
                                                if (bottomLeftSim == Similarity.Same)
                                                {
                                                    SetFrame(x, y, 18 + 18 * randomFrame, 18);
                                                }
                                                else if (bottomRightSim == Similarity.Same)
                                                {
                                                    SetFrame(x, y, 18 + 18 * randomFrame, 18);
                                                }
                                                else
                                                {
                                                    SetFrame(x, y, 108 + 18 * randomFrame, 36);
                                                }
                                                return;
                                            }
                                            if (bottomLeftSim != 0)
                                            {
                                                break;
                                            }
                                            if (bottomRightSim == Similarity.Same)
                                            {
                                                SetFrame(x, y, 18 + 18 * randomFrame, 18);
                                            }
                                            else
                                            {
                                                SetFrame(x, y, 198, 18 * randomFrame);
                                            }
                                            return;
                                        case Similarity.None:
                                            if (topRightSim == Similarity.Same)
                                            {
                                                if (bottomRightSim == Similarity.Same)
                                                {
                                                    SetFrame(x, y, 18 + 18 * randomFrame, 18);
                                                }
                                                else
                                                {
                                                    SetFrame(x, y, 18 + 18 * randomFrame, 18);
                                                }
                                            }
                                            else
                                            {
                                                SetFrame(x, y, 18 + 18 * randomFrame, 18);
                                            }
                                            return;
                                    }
                                    SetFrame(x, y, 18 + 18 * randomFrame, 18);
                                    break;
                                default:
                                    SetFrame(x, y, 72, 18 * randomFrame);
                                    break;
                            }
                            break;
                        default:
                            switch (rightSim)
                            {
                                case Similarity.Same:
                                    SetFrame(x, y, 18 + 18 * randomFrame, 36);
                                    break;
                                default:
                                    SetFrame(x, y, 18 + 36 * randomFrame, 72);
                                    break;
                            }
                            break;
                    }
                    return;
            }
            switch (downSim)
            {
                case Similarity.Same:
                    switch (rightSim)
                    {
                        case Similarity.Same:
                            SetFrame(x, y, 18 + 18 * randomFrame, 0);
                            break;
                        default:
                            SetFrame(x, y, 18 + 36 * randomFrame, 54);
                            break;
                    }
                    break;
                default:
                    switch (rightSim)
                    {
                        case Similarity.Same:
                            SetFrame(x, y, 108 + 18 * randomFrame, 72);
                            break;
                        default:
                            SetFrame(x, y, 216, 18 * randomFrame);
                            break;
                    }
                    break;
            }
        }
		public Point pointPos => (NPC.Center * 1f / 16f).ToPoint();
        public List<Point> Points = new List<Point>();
        public List<Point> validPoints = new List<Point>();
        public FamishBlock[,] Block = new FamishBlock[MaxRadius * 2 + 1, MaxRadius * 2 + 1];
        public List<FamishVine> Vines = new List<FamishVine>();
		public int TotalBlocks = 0;
        public int CurrentBlocks = 0;
        public bool TileValid(int i, int j)
        {
            return Main.tile[i, j].HasTile && Main.tileSolid[Main.tile[i, j].TileType] && !Main.tileSolidTop[Main.tile[i, j].TileType];
        }
        public bool GrowBlock()
        {
            if (validPoints.Count <= 0)
                return false;
            int rand = Math.Max(Main.rand.Next(validPoints.Count), Main.rand.Next(validPoints.Count)); //This way, it will choose the latest placed blocks more often, leading to wackier shapes
            Point p = validPoints[rand];
            int x = p.X;
            int y = p.Y;
            if (!GrowBlockDirect(x, y))
            {
                validPoints.Remove(p);
                return GrowBlock();
            }
            return false;
        }
        private bool GrowBlockDirect(int x, int y)
        {
            int i = pointPos.X + x - MaxRadius;
            int j = pointPos.Y + y - MaxRadius;
            bool spaceAvailableRight = Block[x + 1, y] == null && TileValid(i + 1, j);
            bool spaceAvailableLeft = Block[x - 1, y] == null && TileValid(i - 1, j);
            bool spaceAvailableUp = Block[x, y - 1] == null && TileValid(i, j - 1);
            bool spaceAvailableDown = Block[x, y + 1] == null && TileValid(i, j + 1);
            if ((!spaceAvailableRight && !spaceAvailableDown && !spaceAvailableLeft && !spaceAvailableUp) || Block[x, y] == null)
            {
                return false;
            }
            else
            {
                List<int> validDir = new List<int>();
                if (spaceAvailableRight)
                    validDir.Add(0);
                if (spaceAvailableLeft)
                    validDir.Add(1);
                if (spaceAvailableUp)
                    validDir.Add(2);
                if (spaceAvailableDown)
                    validDir.Add(3);
                int choice = Main.rand.NextFromCollection(validDir);
                if (choice == 0)
                    return AddBlock(x + 1, y, true);
                if (choice == 1)
                    return AddBlock(x - 1, y, true);
                if (choice == 2)
                    return AddBlock(x, y - 1, true);
                if (choice == 3)
                    return AddBlock(x, y + 1, true);
            }
            return false;
        }
        public bool KillBlock(bool sendPacket)
        {
            if (Points.Count <= 0)
                return false;
            Point toKill = Points.Last();
            return KillBlock(toKill.X, toKill.Y, false, sendPacket);
        }
        public bool KillBlock(int x, int y, bool text, bool sendPacket)
        {
            if (x <= 0 || y <= 0 || x >= MaxRadius * 2 || y >= MaxRadius * 2)
                return false;
            if(Block[x, y] != null)
            {
                bool hadPath = Block[x, y].HasPathToHost; //Store this variable so it can be used to check after the reference has been destroyed.
                GenerateDust(Block[x, y].i, Block[x, y].j, -1, 10, true); //First generate the dust
                CurrentBlocks--; //We have killed a block, so decrease block count
                Block[x, y] = null; //Destroy the reference, so tile framing can happen appropriately.
                FrameTileSquare(x, y); //Frame the tiles

                if(NPC.life > 0)
                    NPC.HitEffect(0, LifePerBlock, false); //Activate the on hit code when a block is broken.
                if (text) //Spawn text for being hurt, so there is better clarity in how much damage the famish takes
                    QueueDamageOrHealing -= LifePerBlock;

                if (hadPath) //If the segment was previously connected to the host, update the connections other segments have now that this one is disconnected
                    UpdateConnections();

                Points.Remove(new Point(x, y));
                validPoints.Remove(new Point(x, y));
                if (sendPacket && Main.netMode != NetmodeID.SinglePlayer)
                {
                    SendBlockData(x, y, text ? 0 : 1, Main.myPlayer, -1);
                }
                return true;
            }
            return false;
        }
        public bool AddBlock(int x, int y, bool sendPacket)
		{
            //int i = pointPos.X - 10;
            //int j = pointPos.Y - 10;
			FamishBlock block = new FamishBlock(NPC, x, y, DefaultFrameX, DefaultFrameY);
			if(x >= 1 && x <= MaxRadius * 2 - 1 && y >= 1 && y <= MaxRadius * 2 - 1)
			{
                if (Block[x, y] == null)
                {
                    Block[x, y] = block;
                    GenerateDust(block.i, block.j, 1, 10, true);
                    validPoints.Add(new Point(x, y));
                    Points.Add(new Point(x, y));
                    FrameTileSquare(x, y);
                    TotalBlocks++;
                    CurrentBlocks++;
                    QueueDamageOrHealing += LifePerBlock;
                    if(sendPacket && Main.netMode != NetmodeID.SinglePlayer)
                    {
                        SendBlockData(x, y, 2, Main.myPlayer, -1);
                    }
                    if(MathF.Abs(x - MaxRadius) < 5 && MathF.Abs(y - MaxRadius) < 5 && (x != MaxRadius || y != MaxRadius))
                    {
                        if (!Main.rand.NextBool(5))
                        {
                            Vector2 blockPos = new Vector2(block.i * 16 + 8, block.j * 16 + 8);
                            TryGrowingVine(NPC.Center + Main.rand.NextVector2Circular(12, 12), blockPos, Main.rand.NextBool(2));
                        }
                    }
                    else
                    {
                        Vector2 blockPos = new Vector2(block.i * 16 + 8, block.j * 16 + 8);
                        Vector2 toCenter = NPC.Center - blockPos;
                        if(!Main.rand.NextBool(4))
                        {
                            if (!TryGrowingVine(blockPos + toCenter * Main.rand.NextFloat(0.25f, 0.75f), blockPos, Main.rand.NextBool(2)))
                            {
                                if (!Main.rand.NextBool(4))
                                {
                                    float dist = toCenter.Length();
                                    TryGrowingVine(blockPos + toCenter * Main.rand.NextFloat() + Main.rand.NextVector2Circular(dist, dist) * 0.5f, blockPos, Main.rand.NextBool(2));
                                }
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        public void GenerateDust(int i, int j, int dir = 1, int num = 10, bool sound = true)
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            float speedMult = num < 10 ? 1 : MathHelper.Clamp(MathF.Sqrt(num / 10f), 1, 4);
            Vector2 center = new Vector2(i * 16 + 8, j * 16 + 8);
            if(sound)
            {
                if (dir == 1)
                {
                    SOTSUtils.PlaySound(SoundID.NPCHit1, center, 1.5f, -0.6f);
                }
                if (dir == -1)
                {
                    SOTSUtils.PlaySound(SoundID.NPCDeath1, center, 1.2f, 0.3f);
                }
            }
            for(int a = 0; a < num; a++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(i * 16 - 2, j * 16 - 2), 16, 16, WorldGen.crimson ? DustType<FamishedDustCrimson>() : DustType<FamishedDustCorruption>());
                Vector2 toCenter = center - dust.position;
                dust.scale = Main.rand.NextFloat(1.35f, 1.45f);
                if(dir == 1)
                {
                    dust.position -= toCenter.SNormalize() * 12;
                }
                dust.velocity *= 0.1f;
                dust.velocity += toCenter.SNormalize() * (toCenter.Length() + 12) * dir * 0.075f * speedMult;
                dust.noGravity = true;
            }
        }
        public void FrameTileSquare(int x, int y)
        {
            FrameTile(Block[x, y]);
            FrameTile(Block[x - 1, y]);
            FrameTile(Block[x + 1, y]);
            FrameTile(Block[x, y - 1]);
            FrameTile(Block[x, y + 1]);
            FrameTile(Block[x - 1, y - 1]);
            FrameTile(Block[x + 1, y + 1]);
            FrameTile(Block[x + 1, y - 1]);
            FrameTile(Block[x + 1, y - 1]);
        }
        public bool TryGrowingVine(Vector2 start, Vector2 end, bool upPriority = false)
        {
            if (Main.netMode == NetmodeID.Server || Config.lowFidelityMode)
                return Config.lowFidelityMode && Main.netMode != NetmodeID.Server;
            Vector2 normal = Vector2.Zero;
            bool canGrow = HasTile(start) && HasTile(end);
            if(canGrow)
            {
                int i1 = (int)start.X / 16;
                int j1 = (int)start.Y / 16;
                int i2 = (int)end.X / 16;
                int j2 = (int)end.Y / 16;
                int up = 0;
                int down = 0;
                if (!HasTile(i1, j1 - 1) && !SOTSWorldgenHelper.TrueTileSolid(i1, j1 - 1))
                    up++;
                if (!HasTile(i1, j1 + 1) && !SOTSWorldgenHelper.TrueTileSolid(i1, j1 + 1))
                    down++;
                if (!HasTile(i1 + 1, j1) && !SOTSWorldgenHelper.TrueTileSolid(i1 + 1, j1))
                    normal += Vector2.UnitX;
                if (!HasTile(i1 - 1, j1) && !SOTSWorldgenHelper.TrueTileSolid(i1 - 1, j1))
                    normal -= Vector2.UnitX;

                if (!HasTile(i2, j2 - 1) && !SOTSWorldgenHelper.TrueTileSolid(i2, j2 - 1))
                    up++;
                if (!HasTile(i2, j2 + 1) && !SOTSWorldgenHelper.TrueTileSolid(i2, j2 + 1))
                    down++;
                if (!HasTile(i2 + 1, j2) && !SOTSWorldgenHelper.TrueTileSolid(i2 + 1, j2))
                    normal += Vector2.UnitX;
                if (!HasTile(i2 - 1, j2) && !SOTSWorldgenHelper.TrueTileSolid(i2 - 1, j2))
                    normal -= Vector2.UnitX;

                if (up == down)
                {
                    if (upPriority)
                        down = 0;
                    else
                        up = 0;
                }
                normal += Vector2.UnitY * (down - up);
            }
            if(normal.LengthSquared() > 2f)
            {
                normal = normal.SNormalize();
                SOTSUtils.PlaySound(SoundID.NPCDeath9, start, 1.9f, -0.1f, 0.05f);
                Vines.Add(new FamishVine(NPC, start, end, normal, false));
                return true;
            }
            return false;
        }
        public override void ModifyHoverBoundingBox(ref Rectangle boundingBox)
        {
            boundingBox = NPC.Hitbox;
        }
        public override string Texture => WorldGen.crimson ? "SOTS/NPCs/AbandonedVillage/FamishedCrimsonNoEye" : "SOTS/NPCs/AbandonedVillage/FamishedCorruptionNoEye";
        public static string TextureEye => WorldGen.crimson ? "SOTS/NPCs/AbandonedVillage/FamishedCrimsonEye" : "SOTS/NPCs/AbandonedVillage/FamishedCorruptionEye";
        public static string SeedTexture => WorldGen.crimson ? "SOTS/NPCs/AbandonedVillage/TheFamishedCrimsonHeart" : "SOTS/NPCs/AbandonedVillage/TheFamishedCorruptionHeart";
        public static string BlockTexture => WorldGen.crimson ? "SOTS/NPCs/AbandonedVillage/TheFamishedCrimsonVersion" : "SOTS/NPCs/AbandonedVillage/TheFamishedCorruptionVersion";
        public static string VineTexture => WorldGen.crimson ? "SOTS/NPCs/AbandonedVillage/FamishVineCrimson" : "SOTS/NPCs/AbandonedVillage/FamishVineCorruption";
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.TrailCacheLength[NPC.type] = FamishedCarrier.MaxLength;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "SOTS/BossCL/FamishedBestiary",
                Position = new Vector2(1, 2),
                Scale = 0.3f,
                PortraitScale = 0.6275f, // Portrait refers to the full picture when clicking on the icon in the bestiary
                PortraitPositionYOverride = 1f,
                PortraitPositionXOverride = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
		public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 15;  
            NPC.damage = 40; 
            NPC.defense = 0;  
            NPC.knockBackResist = 0.5f;
            NPC.width = NPC.height = 46;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.npcSlots = 1.5f;
			NPC.noGravity = true;
            NPC.dontTakeDamage = true;
            NPC.noTileCollide = true;
			NPC.alpha = 0;
			NPC.HitSound = SoundID.NPCHit19;
			NPC.DeathSound = SoundID.NPCDeath23;
            NPC.localAI[0] = 50; //Essentially starts with 15 + 250 = 265, 30 + 500 = 530, 45 + 750 = 795 life, but then keeps growing...
            Banner = NPC.type;
            BannerItem = ItemType<FamishedBanner>();
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.damage = 3 * NPC.damage / 4; //40, 60, 90
        }
		public const int DefaultFrameX = 162;
		public const int DefaultFrameY = 144;
        private void DrawBezierCurves(FamishVine vine, SpriteBatch spriteBatch, Vector2 screenPos, bool doDust, bool justDustLastPoint)
        {
            if (vine.Counter < 0)
                return;
            float circular = MathF.Sin(MathHelper.ToRadians(vine.IdleCounter));
            float percent = MathF.Min(1f, vine.Counter / 40f);
            Vector2 end = vine.EndPosition;
            float distX = MathF.Abs(vine.StartPosition.X - end.X);
            float distY = MathF.Abs(vine.StartPosition.Y - end.Y);
            float minLength = 40 * vine.HeightMult;
            float maxLength = 80 * vine.HeightMult;
            float length = MathHelper.Clamp(minLength + distX * 2f + distY * 2f, minLength, maxLength - distX * 0.5f - distY * 0.5f) + (1 + .2f * circular);
            Vector2 p0 = vine.StartPosition;
            Vector2 p1 = vine.StartPosition * 0.5f + end * 0.5f + vine.Normal * length;
            Vector2 p2 = end + vine.Normal * length * 0.2f * -circular;
            Vector2 p3 = end;
            int segments = 16;
            int max = (int)(segments * percent);
            for (int i = justDustLastPoint ? max - 1 : 0; i < max; i++)
            {
                float t = i / (float)segments;
                Vector2 pos = CalculateBezierPoint(t, p0, p1, p2, p3); //t is the iterative variable that tells where along the curve the texture will be drawn
                if (!doDust)
                {
                    float sin = MathF.Sin((SOTSWorld.GlobalCounter * -4 + i * 60) * MathF.PI / 180f);
                    t = (i + 1) / (float)segments;
                    Vector2 nextPos = CalculateBezierPoint(t, p0, p1, p2, p3); //t varies from 0 to 1 as a decimal number
                    Vector2 toNextPosition = nextPos - pos;
                    Color drawColor = Lighting.GetColor((int)pos.X / 16, (int)pos.Y / 16, Color.White); //fetch the lighting engine's color at the location of the texture
                    float scaleX = MathF.Max(8f / 18f, (toNextPosition.Length() + 4f) / 18f);
                    float rotation = toNextPosition.ToRotation();
                    Vector2 squashAndStretch = new Vector2(MathF.Max(1f + 0.2f * ChaseQueuedHealingDelayed / MaxChaseHealing, .95f), 1f + 0.2f * ChaseQueuedHealing / MaxChaseHealing);
                    Texture2D texture = Request<Texture2D>(VineTexture).Value; //This fetches the texture which will be used for visualizing the bezier curve
                    Vector2 drawOrigin = new Vector2(0, texture.Height / 2);
                    spriteBatch.Draw(texture, pos - screenPos, null, drawColor, rotation, drawOrigin, new Vector2(scaleX, 0.8f + sin * 0.2f) * squashAndStretch, SpriteEffects.None, 0f); //Terraria's main drawing function
                    //Color c2 = (WorldGen.crimson ? ColorHelper.AVIchorLight : ColorHelper.AVCursedLight).ToColor() * (0.4f + 0.4f * sin);
                    //c2.A = 0;
                    //spriteBatch.Draw(texture, pos - screenPos, null, c2, rotation, drawOrigin, new Vector2(scaleX, (0.8f + sin * 0.2f)) * squashAndStretch, SpriteEffects.None, 0f); //Terraria's main drawing function
                }
                else
                {
                    Dust dust = Dust.NewDustDirect(pos - new Vector2(5), 0, 0, WorldGen.crimson ? DustType<FamishedDustCrimson>() : DustType<FamishedDustCorruption>());
                    dust.scale = justDustLastPoint ? 1.2f : Main.rand.NextFloat(1.4f, 1.6f);
                    dust.velocity *= 0.6f;
                    dust.noGravity = true;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (Block == null)
                return false;
            if(!foundSeedableLocation)
            {
                float percent = Math.Max(0, 1 - NPC.ai[3] / armingTime) * Math.Min(1, NPC.ai[2] / armingTime);
                Texture2D pixel = SOTSUtils.WhitePixel;
                Vector2 origin = new Vector2(0, 1);
                Vector2 previous = NPC.Center;
                Color c = GlowColor;
                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    if (NPC.oldPos[i] == Vector2.Zero)
                        break;
                    Vector2 center = NPC.oldPos[i] + NPC.Size / 2;
                    float perc = 1 - i / (float)NPC.oldPos.Length;
                    Vector2 toPrev = previous - center;
                    spriteBatch.Draw(pixel, center - Main.screenPosition, null, c * perc * percent, toPrev.ToRotation(), origin, new Vector2(toPrev.Length() / 2f, 2.5f * perc), SpriteEffects.None, 0f);
                    previous = center;
                }
            }
            foreach (FamishVine vine in Vines)
            {
                DrawBezierCurves(vine, spriteBatch, screenPos, false, false);
            }
            Texture2D pixelGradient = Request<Texture2D>("SOTS/Assets/LongGradient").Value;
            if (!foundSeedableLocation && targetPosition != Vector2.Zero)
            {
                float percent = MathF.Sin(Math.Min(1, NPC.ai[3] / armingTime) * MathHelper.Pi);
                Vector2 toTarget = targetPosition - NPC.Center;
                spriteBatch.Draw(pixelGradient, NPC.Center - screenPos, null, GlowColor * percent * 0.75f, toTarget.ToRotation(), new Vector2(0, 1), new Vector2(toTarget.Length() / pixelGradient.Width, 1), SpriteEffects.None, 0f);
            }
            Texture2D texture = Request<Texture2D>(BlockTexture).Value;
            Texture2D textureGlow = Request<Texture2D>(BlockTexture + "Glow").Value;
            Rectangle frame = new Rectangle(DefaultFrameX, DefaultFrameY, 16, 16);
			Vector2 drawOrigin = new Vector2(8, 8);
			Vector2 drawPos = NPC.Center - screenPos;
            float scale;
            foreach (FamishBlock fB in Block)
            {
				if(fB != null)
                {
                    float x = fB.x - MaxRadius;
                    float y = fB.y - MaxRadius;
                    float sin = MathF.Sin((SOTSWorld.GlobalCounter * -2 + MathF.Sqrt(x * x + y * y) * 30) * MathF.PI / 180f);
                    scale = 1.08f + 0.075f * sin;
                    Vector3[] slices = new Vector3[9];
                    Lighting.GetColor9Slice(fB.i, fB.j, ref slices);
                    Vector2 defPosition = new Vector2(fB.i * 16, fB.j * 16);
                    Rectangle glowOffset = fB.HasDetail ? new Rectangle(fB.FrameX, fB.FrameY + 90, 16, 16) : fB.Rect;
                    Vector3 vector = Lighting.GetColor(fB.i, fB.j).ToVector3();
                    Vector3 tileLight;
                    Vector2 position;
                    Color color = new Color();
                    Rectangle value = new Rectangle();
                    for (int i = 0; i < 9; i++)
                    {
                        value.X = 0;
                        value.Y = 0;
                        value.Width = 4;
                        value.Height = 4;
                        switch (i)
                        {
                            case 1:
                                value.Width = 8;
                                value.X = 4;
                                break;
                            case 2:
                                value.X = 12;
                                break;
                            case 3:
                                value.Height = 8;
                                value.Y = 4;
                                break;
                            case 4:
                                value.Width = 8;
                                value.Height = 8;
                                value.X = 4;
                                value.Y = 4;
                                break;
                            case 5:
                                value.X = 12;
                                value.Y = 4;
                                value.Height = 8;
                                break;
                            case 6:
                                value.Y = 12;
                                break;
                            case 7:
                                value.Width = 8;
                                value.Height = 4;
                                value.X = 4;
                                value.Y = 12;
                                break;
                            case 8:
                                value.X = 12;
                                value.Y = 12;
                                break;
                        }
                        position.X = defPosition.X + value.X * scale;
                        position.Y = defPosition.Y + value.Y * scale;
                        value.X += glowOffset.X;
                        value.Y += glowOffset.Y;
                        tileLight.X = (slices[i].X + vector.X) * 0.5f;
                        tileLight.Y = (slices[i].Y + vector.Y) * 0.5f;
                        tileLight.Z = (slices[i].Z + vector.Z) * 0.5f;
                        int num = (int)(tileLight.X * 255f);
                        int num2 = (int)(tileLight.Y * 255f);
                        int num3 = (int)(tileLight.Z * 255f);
                        if (num > 255)
                            num = 255;

                        if (num2 > 255)
                            num2 = 255;

                        if (num3 > 255)
                            num3 = 255;

                        num3 <<= 16;
                        num2 <<= 8;
                        color.PackedValue = (uint)(num | num2 | num3) | 0xFF000000u;
                        Main.spriteBatch.Draw(texture, position - screenPos + drawOrigin, value, color, 0f, drawOrigin, scale, SpriteEffects.None, 0f);
                        //Color c2 = (WorldGen.crimson ? ColorHelper.AVIchorLight : ColorHelper.AVCursedLight).ToColor() * (0.2f + 0.2f * sin);
                        //c2.A = 0;
                        //Main.spriteBatch.Draw(texture, position - screenPos + drawOrigin, value, c2, 0f, drawOrigin, scale, SpriteEffects.None, 0f);
                    }
                }
            }
            foreach (FamishBlock fB in Block)
            {
                if (fB != null)
                {
                    float x = fB.x - MaxRadius;
                    float y = fB.y - MaxRadius;
                    scale = 1.08f + 0.075f * MathF.Sin((SOTSWorld.GlobalCounter * -2 + MathF.Sqrt(x * x + y * y) * 30) * MathF.PI / 180f);
                    Vector3[] slices = new Vector3[9];
                    Lighting.GetColor9Slice(fB.i, fB.j, ref slices);
                    Vector2 defPosition = new Vector2(fB.i * 16, fB.j * 16);
                    Rectangle glowOffset = fB.HasDetail ? new Rectangle(fB.FrameX, fB.FrameY + 90, 16, 16) : fB.Rect;
                    Vector2 toPlayer = Main.LocalPlayer.Center - defPosition - drawOrigin;
                    spriteBatch.Draw(textureGlow, defPosition - screenPos + drawOrigin + toPlayer.SNormalize() * 3, glowOffset, Color.White, NPC.rotation, drawOrigin, NPC.scale * scale, SpriteEffects.None, 0f);
                }
            }
            bool isSeed = !foundSeedableLocation;
            Texture2D textureHeart = Request<Texture2D>(isSeed ? SeedTexture : Texture).Value;
            Texture2D textureHeartGlow = Request<Texture2D>(TextureEye).Value;
            drawOrigin = textureHeart.Size() / 2;
            if (isSeed)
            {
                float percent = Math.Min(1, MathF.Sqrt(NPC.ai[2] / armingTime));
                float percent2 = Math.Max(0, 1 - NPC.ai[3] / armingTime);
                scale = 0.6f + 0.3f * percent;
                for(int i = 0; i < 6; i++)
                {
                    Vector2 circular = new Vector2(10 * (1 - percent) + 2, 0).RotatedBy(i / 6f * MathHelper.TwoPi + MathHelper.ToRadians(SOTSWorld.GlobalCounter * 1.5f));
                    spriteBatch.Draw(textureHeart, drawPos + circular, null, GlowColor * percent * percent2, NPC.rotation, drawOrigin, NPC.scale * scale, SpriteEffects.None, 0f);
                }
            }
            else
            {
                float initPercent = NPC.ai[1] / TimeToReachMaxSize;
                scale = 0.5f + 0.3f * initPercent + 0.025f * MathF.Sin(SOTSWorld.GlobalCounter * -2 * MathF.PI / 180f);
            }
            Vector2 squashAndStretch = new Vector2(scale + 0.2f * ChaseQueuedHealingDelayed / MaxChaseHealing, scale + 0.2f * ChaseQueuedHealing / MaxChaseHealing);
            spriteBatch.Draw(textureHeart, drawPos, null, drawColor, NPC.rotation, drawOrigin, NPC.scale * squashAndStretch, SpriteEffects.None, 0f);
            if(!isSeed)
            {
                float eyeRecoil = ChaseQueuedHealing < 0 ? ChaseQueuedHealing : 0;
                Vector2 toPlayer = new Vector2(1, 0).RotatedBy(ToPlayerAngle);
                toPlayer = toPlayer * (2 + MathF.Min(5, eyeRecoil * 4f / MaxChaseHealing - 5 * ShootRecoil)) * squashAndStretch;
                if (ChargeLaserPercent > 0)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 circular = new Vector2(8 * (1 - ChargeLaserPercent), 0).RotatedBy(i / 6f * MathHelper.TwoPi + MathHelper.ToRadians(SOTSWorld.GlobalCounter * 0.4f));
                        spriteBatch.Draw(textureHeartGlow, drawPos + circular + toPlayer, null, GlowColor * ChargeLaserPercent * 0.5f, NPC.rotation, drawOrigin, NPC.scale * squashAndStretch * new Vector2(1 + 0.5f * ChargeLaserPercent, 1 - 0.25f * ChargeLaserPercent), SpriteEffects.None, 0f);
                    }
                    float toTarget = (Main.player[NPC.target].Center - NPC.Center).Length() + 64;
                    spriteBatch.Draw(pixelGradient, drawPos + toPlayer, null, GlowColor * 0.75f * ChargeLaserPercent, ToPlayerAngle, new Vector2(0, 1), new Vector2(toTarget / pixelGradient.Width, 3 * ChargeLaserPercent), SpriteEffects.None, 0f);
                }
                spriteBatch.Draw(textureHeartGlow, drawPos + toPlayer, null, Color.White, NPC.rotation, drawOrigin, NPC.scale * squashAndStretch * new Vector2(1 + 0.5f * ChargeLaserPercent, 1 - 0.25f * ChargeLaserPercent), SpriteEffects.None, 0f);
            }
            return false;
		}
		private bool RunOnce = true;
        private bool foundSeedableLocation = false;
        private int LastRecordedBlockCount = 0;
        private float ChaseQueuedHealing = 0;
        private float ChaseQueuedHealingDelayed = 0;
        private float MaxChaseHealing = 10;
        private float TimeToReachMaxSize = 240;
        private float armingTime = 70;
        private Vector2 targetPosition;
        public int QueueDamageOrHealing = 0;
        public float QueueHurtFromOtherSource = 0;
        public int QueueDamageHealingTimer = 0;
        public Vector2 groundedPosition;
        public float ShootCounter = -300;
        private float ToPlayerAngle = -1000;
        private float ShootRecoil = 0;
        private float chargeLaserTime = 120f;
        public float ChargeLaserPercent => MathF.Max(0, ShootCounter / chargeLaserTime);
        private bool SeedAI()
        {
            NPC.rotation += NPC.velocity.X * 0.01f;
            if (NPC.ai[2] == 0)
            {
                SOTSUtils.PlaySound(SoundID.Item111, NPC.Center, 1.4f, 0.5f);
            }
            NPC.ai[2]++;
            if (NPC.ai[2] < armingTime)
            {
                if ((int)NPC.ai[2] == 15)
                    SOTSUtils.PlaySound(SoundID.Item15, NPC.Center, 1.5f, -0.3f);
                if (NPC.velocity.LengthSquared() > 2)
                    NPC.velocity *= 0.984f;
                float percent = NPC.ai[2] / armingTime;
                NPC.velocity.Y += 0.1f * MathF.Cos(percent * MathHelper.TwoPi);
                NPC.velocity *= 1f - percent * 0.1f;
                Vector2 oldVelo = NPC.velocity;
                NPC.velocity = Collision.TileCollision(NPC.Center, NPC.velocity, 0, 0, true, true, 1);
                if (oldVelo.X != NPC.velocity.X) //Bounce off of walls while priming
                    NPC.velocity.X = -oldVelo.X;
                if (oldVelo.Y != NPC.velocity.Y)
                    NPC.velocity.Y = -oldVelo.Y;
                float maxSearchDist = 800;
                Vector2 checkPosition = NPC.Center + Main.rand.NextVector2Circular(maxSearchDist, maxSearchDist) * percent;
                int i = (int)checkPosition.X / 16;
                int j = (int)checkPosition.Y / 16;
                if (targetPosition == Vector2.Zero && TileValid(i, j) && NPC.ai[2] > 15)
                {
                    bool foundTile = false;
                    Vector2 endPosition = NPC.Center;
                    while (!foundTile)
                    {
                        Vector2 toCheckPos = (checkPosition - NPC.Center);
                        float dist = toCheckPos.Length();
                        i = (int)endPosition.X / 16;
                        j = (int)endPosition.Y / 16;
                        foundTile = TileValid(i, j);
                        if (foundTile || endPosition.Distance(checkPosition) < 6)
                            checkPosition = endPosition;
                        endPosition += toCheckPos.SNormalize() * Math.Min(dist, 8);
                    }
                    if (foundTile)
                    {
                        targetPosition = checkPosition;
                        if (Main.netMode == NetmodeID.Server)
                            NPC.netUpdate = true;
                    }
                }
            }
            else
            {
                if(!Main.rand.NextBool(4) && NPC.velocity.LengthSquared() > 0.5f)
                    PixelDust.Spawn(NPC.Center - NPC.velocity * 4f, 0, 0, Main.rand.NextVector2Circular(1, 1) + NPC.velocity * 0.5f, GlowColor * 0.5f, 5).scale = Main.rand.NextFloat(0.5f, 1.25f);
                NPC.ai[3]++;
                if (NPC.ai[3] % 10 == 0 && Vines.Count < 3 && Main.netMode != NetmodeID.Server && targetPosition != Vector2.Zero)
                {
                    SOTSUtils.PlaySound(SoundID.NPCDeath9, NPC.Center, 1.9f, -0.1f, 0.05f);
                    Vines.Add(new FamishVine(NPC, NPC.Center, targetPosition, Main.rand.NextVector2Circular(1, 1) * 1.5f, true));
                }
                Vector2 toTarget = new Vector2(0, 0.1f);
                if (targetPosition != Vector2.Zero)
                {
                    toTarget = (targetPosition - NPC.Center).SNormalize() * 0.1f * MathF.Sqrt(NPC.ai[3] / armingTime);
                }
                NPC.velocity *= 0.965f;
                NPC.velocity += toTarget;
                int i = pointPos.X;
                int j = pointPos.Y;
                if (NPC.ai[3] > armingTime && TileValid(i, j))
                {
                    foundSeedableLocation = true;
                    groundedPosition = pointPos.ToVector2() * 16 + new Vector2(8, 8);
                    NPC.velocity *= 0f;
                    return true;
                }
            }
            return false;
        }
        public override bool PreAI()
        {
            NPC.dontTakeDamage = !foundSeedableLocation;
            if(!TileBreakListeners.Contains(NPC.whoAmI)) //Make sure that this NPC listens to any tile break interactions.
            {
                TileBreakListeners.Add(NPC.whoAmI);
            }
            if (!foundSeedableLocation)
            {
                bool foundSpot = SeedAI();
                if (!foundSpot)
                    return true;
            }
            NPC.knockBackResist = 0;
            NPC.rotation = SOTSUtils.AngularLerp(NPC.rotation, 0, 0.1f);
            int prevQueDamHeal = QueueDamageOrHealing;
            if (QueueHurtFromOtherSource > 45)
                QueueHurtFromOtherSource = 45;
            ChaseQueuedHealing = MathHelper.Clamp(MathHelper.Lerp(ChaseQueuedHealing, QueueDamageOrHealing - QueueHurtFromOtherSource, 0.12f), -MaxChaseHealing, MaxChaseHealing); //Chase the value of the damage/healing in order to create a recoil effect for growing or being hit
            ChaseQueuedHealingDelayed = MathHelper.Clamp(MathHelper.Lerp(ChaseQueuedHealingDelayed, ChaseQueuedHealing - QueueHurtFromOtherSource, 0.12f), -MaxChaseHealing, MaxChaseHealing); //Chase the value of the damage/healing in order to create a recoil effect for growing or being hit
            QueueHurtFromOtherSource *= 0.9f;
            if (NPC.localAI[1] == 0)
                NPC.localAI[1] = NPC.life;
            if (NPC.ai[1] < TimeToReachMaxSize)
                NPC.ai[1]++;
            else
                NPC.ai[1] = TimeToReachMaxSize;
            if (Block == null)
                Block = new FamishBlock[MaxRadius * 2 + 1, MaxRadius * 2 + 1];
            bool anyDisconnected = false;
            foreach (FamishBlock fB in Block)
            {
                if (fB != null)
                {
                    bool dieFromBeingDisconnected = !fB.HasPathToHost;
                    if (dieFromBeingDisconnected)
                    {
                        anyDisconnected = true;
                        float x = fB.x - MaxRadius;
                        float y = fB.y - MaxRadius;
                        int chance = Math.Max(20 - (int)MathF.Sqrt(x * x + y * y), 3);
                        dieFromBeingDisconnected = Main.rand.NextBool(chance);
                    }
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (!TileValid(fB.i, fB.j) || dieFromBeingDisconnected)
                        {
                            KillBlock(fB.x, fB.y, true, true);
                        }
                    }
                }
            }
            if (RunOnce && Main.netMode != NetmodeID.MultiplayerClient)
            {
				AddBlock(MaxRadius, MaxRadius, true);
                NPC.netUpdate = true;
                RunOnce = false;
            }
            else if(!anyDisconnected && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int difference = (int)NPC.localAI[0] - TotalBlocks;
                int rate = Math.Clamp((int)((1 - MathF.Pow(difference / 50f, .25f)) * NPC.localAI[0]), 2, 300);
                if (TotalBlocks < NPC.localAI[0] && NPC.ai[0] >= rate)
                {
                    NPC.netUpdate = true;
                    NPC.ai[0] -= rate;
                    bool success = false;
                    int attempts = 4;
                    while(!success && attempts > 0)
                    {
                        success = GrowBlockDirect(MaxRadius + Main.rand.Next(-1, 2), MaxRadius + Main.rand.Next(-1, 2)); //Try growing a block from the center first
                        attempts--;
                    }
                    if (!success)
                        success = GrowBlock(); //If you couldn't grow a block from the center, try elsewhere
                    if (!success && validPoints.Count <= 0) //If we've run out of points to grow blocks from, try adding some new points to grow blocks from
                    {
                        NPC.localAI[0]--;
                        if(Points.Count > 0)
                        {
                            int pick1 = Main.rand.Next(Points.Count);
                            int pick2 = Main.rand.Next(Points.Count);
                            int pick3 = Main.rand.Next(Points.Count);
                            int pick4 = Main.rand.Next(Points.Count);
                            int pick5 = Main.rand.Next(Points.Count);
                            validPoints.Add(Points[pick1]);
                            if (pick2 != pick1)
                                validPoints.Add(Points[pick2]);
                            if (pick3 != pick2 && pick3 != pick1)
                                validPoints.Add(Points[pick3]);
                            if (pick4 != pick3 && pick4 != pick2 && pick4 != pick1)
                                validPoints.Add(Points[pick4]);
                            if (pick5 != pick4 && pick5 != pick3 && pick5 != pick2 && pick5 != pick1)
                                validPoints.Add(Points[pick5]);
                        }
                        else
                        {
                            AddBlock(MaxRadius, MaxRadius, true);
                        }
                    }
                }
                if (TotalBlocks >= NPC.localAI[0] && NPC.localAI[0] < MaxSize)
                    NPC.localAI[0]++;
            }
			NPC.Center = groundedPosition;
            NPC.velocity *= 0.95f;
            NPC.ai[0]++;

            int life = (int)NPC.localAI[1] + CurrentBlocks * LifePerBlock;
            if(NPC.lifeMax < life)
                NPC.lifeMax = life;
            if(NPC.life > life)
            {
                NPC.life = life;
            }
            NPC.life = Math.Min(NPC.life, NPC.lifeMax);
            NPC.TargetClosest(false);
            if(LastRecordedBlockCount != TotalBlocks)
            {
                NPC.life += LifePerBlock;
                LastRecordedBlockCount = TotalBlocks;
            }
            int howManyBlocksIShouldHaveBasedOnMyLife = (int)MathF.Ceiling((NPC.life - (int)NPC.localAI[1]) / (float)LifePerBlock);
            if (TotalBlocks > 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (howManyBlocksIShouldHaveBasedOnMyLife < CurrentBlocks && CurrentBlocks > 0)
                {
                    KillBlock(true);
                }
            }
            if (QueueDamageOrHealing == 0 || prevQueDamHeal != QueueDamageOrHealing)
                QueueDamageHealingTimer = 0;
            else
                QueueDamageHealingTimer++;
            if (QueueDamageHealingTimer > 3)
            {
                if (QueueDamageOrHealing < 0)
                {
                    if (Main.netMode != NetmodeID.Server)
                        CombatText.NewText(NPC.Hitbox, CombatText.DamagedHostile, -QueueDamageOrHealing, false, false);
                    QueueDamageOrHealing = 0;
                }
                else if (QueueDamageOrHealing > 0)
                {
                    if (Main.netMode != NetmodeID.Server)
                        CombatText.NewText(NPC.Hitbox, CombatText.HealLife, QueueDamageOrHealing, false, true);
                    QueueDamageOrHealing = 0;
                }
                QueueDamageHealingTimer = 0;
            }
            return true;
		}
        public override void AI()
        {
            for (int i = 0; i < Vines.Count; i++)
            {
                FamishVine vine = Vines[i];
                vine.Update();
                bool killOldHooks = vine.IgnoreTiles && foundSeedableLocation;
                if(vine.Kill || killOldHooks)
                {
                    if(!killOldHooks)
                        DrawBezierCurves(vine, null, Vector2.Zero, true, false);
                    Vines.Remove(vine);
                    i--;
                }
                else if(vine.Counter < 40 && vine.Counter > 3 && !Main.rand.NextBool(4))
                {
                    DrawBezierCurves(vine, null, Vector2.Zero, true, true);
                }
            }
            if(foundSeedableLocation)
            {
                Player player = Main.player[NPC.target];
                Vector2 toPlayer = (player.Center - NPC.Center).SNormalize() * 32;
                bool canSeePlayer = player.Center.Distance(NPC.Center) < 800 && (ChargeLaserPercent > 0.6f || Collision.CanHitLine(NPC.Center + toPlayer, 0, 0, player.position, player.width, player.height) 
                    || Collision.CanHitLine(NPC.Center + toPlayer - new Vector2(0, 16), 0, 0, player.position, player.width, player.height) 
                    || Collision.CanHitLine(NPC.Center + toPlayer + new Vector2(0, 16), 0, 0, player.position, player.width, player.height)
                    || Collision.CanHitLine(NPC.Center + toPlayer - new Vector2(16, 0), 0, 0, player.position, player.width, player.height)
                    || Collision.CanHitLine(NPC.Center + toPlayer + new Vector2(16, 0), 0, 0, player.position, player.width, player.height));
                if (ToPlayerAngle < -100)
                    ToPlayerAngle = toPlayer.ToRotation();
                else
                {
                    float aimPercent = MathHelper.Clamp(1 - ChargeLaserPercent, 0, 1);
                    ToPlayerAngle = SOTSUtils.AngularLerp(ToPlayerAngle, toPlayer.ToRotation(), 0.07f * aimPercent);
                }
                toPlayer = new Vector2(1, 0).RotatedBy(ToPlayerAngle);
                if (canSeePlayer && QueueHurtFromOtherSource < 0.05f && ChaseQueuedHealingDelayed > -0.05f)
                {
                    ShootCounter++;
                    if (ShootCounter > chargeLaserTime)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 predictPlayerLocation = toPlayer * (player.Center - NPC.Center).Length();
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + toPlayer * 16, toPlayer, ProjectileType<FamishedLaser>(), NPC.GetBaseDamage() / 2, 0, Main.myPlayer, NPC.Center.X + predictPlayerLocation.X, NPC.Center.Y + predictPlayerLocation.Y);
                        }
                        ShootRecoil = 1f;
                        ShootCounter -= chargeLaserTime;
                    }
                }
                else if (ShootCounter > 0)
                {
                    ShootCounter--;
                    if (ShootCounter < 0)
                        ShootCounter = 0;
                }
                float percent = ShootCounter == 0 ? 1 : ChargeLaserPercent;
                ShootRecoil = MathHelper.Lerp(ShootRecoil, 0, 0.07f * percent);
                ShootRecoil -= 0.02f * percent;
                if (ShootRecoil < 0)
                    ShootRecoil = 0;
            }
        }
        public override void HitEffect(NPC.HitInfo hit)
		{
            if(NPC.life <= 0)
            {
                while (KillBlock(false)) ;
                for (int i = 0; i < Vines.Count; i++)
                {
                    FamishVine vine = Vines[i];
                    DrawBezierCurves(vine, null, Vector2.Zero, true, false);
                }
                GenerateDust(pointPos.X, pointPos.Y, -2, 35, false);
                if (Main.netMode != NetmodeID.Server && foundSeedableLocation)
                {
                    if(WorldGen.crimson)
                    {
                        Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(hit.HitDirection, -1), ModGores.GoreType("Gores/Famished/FamishedGore9"), .8f);
                        Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(14, 0), new Vector2(hit.HitDirection, -1), ModGores.GoreType("Gores/Famished/FamishedGore10"), .8f);
                        Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 18), new Vector2(hit.HitDirection, -1), ModGores.GoreType("Gores/Famished/FamishedGore11"), .8f);
                    }
                    else
                    {
                        for (int i = 1; i <= 8; i++)
                        {
                            Vector2 circular = new Vector2(-28, 0).RotatedBy(MathHelper.ToRadians(i * 45 - 90));
                            Gore.NewGore(NPC.GetSource_Death(), NPC.Center + circular - new Vector2(9, 9), circular * 0.15f, ModGores.GoreType("Gores/Famished/FamishedGore" + i), .8f);
                        }
                    }
                    for(int i = 0; i < 24; i++)
                    {
                        Vector2 circular = new Vector2(3, 0).RotatedBy(i / 24f * MathHelper.TwoPi);
                        PixelDust.Spawn(NPC.Center, 0, 0, Main.rand.NextVector2Circular(1, 1) + circular, GlowColor, 4).scale = Main.rand.NextFloat(1, 1.5f);
                    }
                }
            }
            else if(hit.HitDirection != 0)
            {
                ShootCounter *= 0.9f;
                QueueHurtFromOtherSource += hit.Damage * 0.2f;
                ChaseQueuedHealing -= hit.Damage * 0.2f;
                ChaseQueuedHealingDelayed -= hit.Damage * 0.1f;
                ChaseQueuedHealing = MathHelper.Clamp(ChaseQueuedHealing, -MaxChaseHealing, MaxChaseHealing);
                ChaseQueuedHealing = MathHelper.Clamp(ChaseQueuedHealingDelayed, -MaxChaseHealing, MaxChaseHealing);
                GenerateDust(pointPos.X, pointPos.Y, -1, (int)(hit.Damage / 8f + 0.99f), false);
                NPC.netUpdate = true;
            }
            if (Main.netMode == NetmodeID.Server)
				return;
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SetMaxDamage(target.life - 1);
        }
        public override bool PreKill()
        {
			return true;
		}
        public override bool ModifyCollisionData(Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox)
        {
            npcHitbox = victimHitbox;
            return false;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (Block == null)
                return false;
            foreach (FamishBlock fB in Block)
            {
                if (fB != null)
                {
                    Rectangle blockHitbox = new Rectangle(fB.i * 16 - 1, fB.j * 16 - 1, 18, 18);
                    if (target.Hitbox.Intersects(blockHitbox))
                        return true;
                }
            }
            return false;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule worldCrimson = new LeadingConditionRule(new Conditions.IsCrimson());
            LeadingConditionRule worldCorrupt = new LeadingConditionRule(new Conditions.IsCorruption());
            npcLoot.Add(ItemDropRule.Common(ItemType<FragmentOfEvil>(), 1, 1, 1));
            worldCrimson.OnSuccess(ItemDropRule.Common(ItemType<FamishedBlockCrimson>(), 1, 20, 40));
            worldCorrupt.OnSuccess(ItemDropRule.Common(ItemType<FamishedBlockCorruption>(), 1, 20, 40));
            npcLoot.Add(worldCrimson);
            npcLoot.Add(worldCorrupt);
        }
    }
}
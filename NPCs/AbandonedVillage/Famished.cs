using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using SOTS.Dusts;
using SOTS.NPCs.Town;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static SOTS.SOTS;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.AbandonedVillage
{
	public class Famished : ModNPC
	{
        public const int MaxRadius = 14;
        public int MaxSize => MaxRadius * MaxRadius * 4;
        public int LifePerBlock => Main.masterMode ? 15 : Main.expertMode ? 10 : 5;
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
		public Point pointPos => (NPC.Center * 1f / 16f).ToPoint();
        public List<Point> Points = new List<Point>();
        public List<Point> validPoints = new List<Point>();
        public FamishBlock[,] Block = new FamishBlock[MaxRadius * 2 + 1, MaxRadius * 2 + 1];
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
            int i = pointPos.X + x - MaxRadius;
            int j = pointPos.Y + y - MaxRadius;
            bool spaceAvailableRight = Block[x + 1, y] == null && TileValid(i + 1, j);
            bool spaceAvailableLeft = Block[x - 1, y] == null && TileValid(i - 1, j);
            bool spaceAvailableUp = Block[x, y - 1] == null && TileValid(i, j - 1);
            bool spaceAvailableDown = Block[x, y + 1] == null && TileValid(i, j + 1);
            if (!spaceAvailableRight && !spaceAvailableDown && !spaceAvailableLeft && !spaceAvailableUp)
            {
                validPoints.Remove(p);
                return GrowBlock();
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
                if(choice == 0)
                    return AddBlock(x + 1, y);
                if (choice == 1)
                    return AddBlock(x - 1, y);
                if (choice == 2)
                    return AddBlock(x, y - 1);
                if (choice == 3)
                    return AddBlock(x, y + 1);
            }
            return false;
        }
        public bool KillBlock()
        {
            if (Points.Count <= 0)
                return false;
            Point toKill = Points.Last();
            KillBlock(toKill.X, toKill.Y, false);
            return true;
        }
        public bool KillBlock(int x, int y, bool text = false)
        {
            if(Block[x, y] != null)
            {
                GenerateDust(Block[x, y].i, Block[x, y].j, -1, 10);
                Block[x, y] = null;
                FrameTileSquare(x, y);
                Points.Remove(new Point(x, y));
                validPoints.Remove(new Point(x, y));
                CurrentBlocks--;

                NPC.HitEffect(0, LifePerBlock, false);
                if (text && Main.netMode != NetmodeID.Server)
                    CombatText.NewText(NPC.Hitbox, CombatText.DamagedHostile, LifePerBlock, false, false);
            }
            return true;
        }
        public bool AddBlock(int x, int y)
		{
            //int i = pointPos.X - 10;
            //int j = pointPos.Y - 10;
			FamishBlock block = new FamishBlock(NPC, x, y, DefaultFrameX, DefaultFrameY);
			if(x >= 1 && x <= MaxRadius * 2 - 1 && y >= 1 && y <= MaxRadius * 2 - 1)
			{
                if (Block[x, y] == null)
                {
                    Block[x, y] = block;
                    GenerateDust(block.i, block.j, 1, 10);
                    validPoints.Add(new Point(x, y));
                    Points.Add(new Point(x, y));
                    FrameTileSquare(x, y);
                    TotalBlocks++;
                    CurrentBlocks++;
                    if (Main.netMode != NetmodeID.Server)
                        CombatText.NewText(NPC.Hitbox, CombatText.HealLife, LifePerBlock, false, true);
                    return true;
                }
            }
            return false;
        }
        public void GenerateDust(int i, int j, int dir = 1, int num = 10)
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            Vector2 center = new Vector2(i * 16 + 8, j * 16 + 8);
            if (dir == 1)
            {
                SOTSUtils.PlaySound(SoundID.NPCHit1, center, 1.5f, -0.6f);
            }
            if(dir == -1)
            {
                SOTSUtils.PlaySound(SoundID.NPCDeath1, center, 1.2f, 0.3f);
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
                dust.velocity += toCenter.SNormalize() * (toCenter.Length() + 12) * dir * 0.075f;
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
        public override void ModifyHoverBoundingBox(ref Rectangle boundingBox)
        {
            boundingBox = NPC.Hitbox;
        }
        public override string Texture => WorldGen.crimson ? "SOTS/NPCs/AbandonedVillage/TheFamishedCrimsonVersion" : "SOTS/NPCs/AbandonedVillage/TheFamishedCorruptionVersion";
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Hide = false
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}
		public override void SetDefaults()
		{
			NPC.aiStyle = -1;
            NPC.lifeMax = 15;  
            NPC.damage = 40; 
            NPC.defense = 0;  
            NPC.knockBackResist = 0.0f;
            NPC.width = 16;
            NPC.height = 16;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.npcSlots = 2.0f;
			NPC.noGravity = true;
            NPC.noTileCollide = true;
			NPC.alpha = 0;
			NPC.HitSound = SoundID.NPCHit19;
			NPC.DeathSound = SoundID.NPCDeath1;
            NPC.localAI[0] = 50; //Essentially starts with 15 + 250 = 265, 30 + 500 = 530, 45 + 750 = 795 life, but then keeps growing...
		}
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.damage = 3 * NPC.damage / 4; //40, 60, 90
        }
		public const int DefaultFrameX = 162;
		public const int DefaultFrameY = 144;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (Block == null)
                return false;
            Texture2D texture = (Texture2D)Request<Texture2D>(Texture);
            Texture2D textureGlow = (Texture2D)Request<Texture2D>(this.Texture + "Glow");
            Rectangle frame = new Rectangle(DefaultFrameX, DefaultFrameY, 16, 16);
			Vector2 drawOrigin = new Vector2(8, 8);
			Vector2 drawPos = NPC.Center - screenPos;
            foreach (FamishBlock fB in Block)
            {
				if(fB != null)
                {
                    float x = fB.x - MaxRadius;
                    float y = fB.y - MaxRadius;
                    float scale = 1.08f + 0.075f * MathF.Sin((SOTSWorld.GlobalCounter * -2 + MathF.Sqrt(x * x + y * y) * 30) * MathF.PI / 180f);
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
                    }
                }
            }
            foreach (FamishBlock fB in Block)
            {
                if (fB != null)
                {
                    float x = fB.x - MaxRadius;
                    float y = fB.y - MaxRadius;
                    float scale = 1.08f + 0.075f * MathF.Sin((SOTSWorld.GlobalCounter * -2 + MathF.Sqrt(x * x + y * y) * 30) * MathF.PI / 180f);
                    Vector3[] slices = new Vector3[9];
                    Lighting.GetColor9Slice(fB.i, fB.j, ref slices);
                    Vector2 defPosition = new Vector2(fB.i * 16, fB.j * 16);
                    Rectangle glowOffset = fB.HasDetail ? new Rectangle(fB.FrameX, fB.FrameY + 90, 16, 16) : fB.Rect;
                    Vector2 toPlayer = Main.LocalPlayer.Center - defPosition - drawOrigin;
                    spriteBatch.Draw(textureGlow, defPosition - screenPos + drawOrigin + toPlayer.SNormalize() * 3, glowOffset, Color.White, NPC.rotation, drawOrigin, NPC.scale * scale, SpriteEffects.None, 0f);
                }
            }
            spriteBatch.Draw(texture, drawPos, frame, drawColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(textureGlow, drawPos, frame, Color.White, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
            return false;
		}
		private bool RunOnce = true;
        private bool foundSeedableLocation = false;
        private int LastRecordedBlockCount = 0;
        public override bool PreAI()
		{
            if (NPC.localAI[1] == 0)
                NPC.localAI[1] = NPC.life;
            if (Block == null)
                Block = new FamishBlock[MaxRadius * 2 + 1, MaxRadius * 2 + 1];
            if (!foundSeedableLocation)
            {
                NPC.velocity.Y += 0.1f;
                int i = pointPos.X;
                int j = pointPos.Y;
                if (TileValid(i, j))
                {
                    foundSeedableLocation = true;
                    NPC.velocity *= 0f;
                }
                else
                    return true;
            }
			if(RunOnce && Main.netMode != NetmodeID.MultiplayerClient)
            {
				AddBlock(MaxRadius, MaxRadius);
                NPC.netUpdate = true;
                RunOnce = false;
            }
            else
            {
                int difference = (int)NPC.localAI[0] - TotalBlocks;
                int rate = Math.Clamp((int)((1 - MathF.Pow(difference / 50f, .25f)) * NPC.localAI[0]), 2, 300);
                if (TotalBlocks < NPC.localAI[0] && NPC.ai[0] >= rate)
                {
                    NPC.ai[0] -= rate;
                    bool success = GrowBlock();
                    if(!success && validPoints.Count <= 0)
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
                            AddBlock(MaxRadius, MaxRadius);
                        }
                    }
                }
                if (TotalBlocks >= NPC.localAI[0] && NPC.localAI[0] < MaxSize)
                    NPC.localAI[0]++;
            }
			NPC.Center = pointPos.ToVector2() * 16 + new Vector2(8, 8);
            NPC.velocity *= 0.95f;
            NPC.ai[0]++;
            foreach (FamishBlock fB in Block)
            {
                if(fB != null)
                {
                    if (!TileValid(fB.i, fB.j))
                    {
                        KillBlock(fB.x, fB.y, true);
                    }
                }
            }

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
            if (TotalBlocks > 0)
            {
                if (howManyBlocksIShouldHaveBasedOnMyLife < CurrentBlocks && CurrentBlocks > 0)
                {
                    KillBlock();
                }
            }
            return true;
		}
		public override void AI()
		{

		}
		public override void HitEffect(NPC.HitInfo hit)
		{
            if(NPC.life <= 0)
            {
                while(CurrentBlocks > 0)
                {
                    KillBlock();
                }
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
    }
}
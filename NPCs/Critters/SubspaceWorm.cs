using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Slime;

namespace SOTS.NPCs.Critters
{
	public class SubspaceWorm : ModNPC
    {
        public virtual Color GoreColor => new Color(78, 77, 123);
        public virtual int CatchItem => ModContent.ItemType<SubspaceWormItem>();
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
            Main.npcCatchable[Type] = true;

            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.TownCritter[Type] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.NormalGoldCritterBestiaryPriority.Add(Type);
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Worm);
            NPC.width = 28;
            NPC.height = 14;
            NPC.aiStyle = NPCAIStyleID.CritterWorm;
            NPC.catchItem = CatchItem;
            NPC.lavaImmune = true;
            NPC.scale = 0.9f;
            AnimationType = NPCID.Worm;
            AIType = NPCID.Worm;
        }
        public override bool PreAI()
        {
            if (NPC.ai[3] == 0)
            {
                NPC.TargetClosest(true);
                NPC.ai[3] = -1;
                NPC.netUpdate = true;
                return false;
            }
            NPC.spriteDirection = -NPC.direction;


            NPC.TargetClosest(false);
            bool PortalAway = NPC.ai[2] >= 60;
            Vector2 toPlayer = Main.player[NPC.target].Center - NPC.Center;
            if (PortalAway || toPlayer.Length() < 160)
            {
                NPC.ai[2]++;
                if (NPC.ai[2] >= 60)
                {
                    NPC.aiStyle = -1;
                    AIType = 0;
                    if(NPC.ai[2] == 60)
                    {
                        NPC.netUpdate = true;
                        NPC.velocity += new Vector2(-MathF.Sign(toPlayer.X) * 9f, -13.5f);
                    }
                    NPC.noGravity = true;
                    NPC.velocity *= 0.94f;
                    NPC.rotation += MathF.PI * 0.1f;
                    if (NPC.velocity.Length() < 0.05f && NPC.ai[3] == -1)
                    {
                        NPC.netUpdate = true;
                        NPC.ai[3] = 1;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 2), Vector2.Zero, ModContent.ProjectileType<TreasureStarPortal>(), 0, 0, Main.myPlayer, 0, 11);
                        }
                    }
                    if (NPC.ai[3] >= 1)
                    {
                        NPC.localAI[3]++;
                        if (NPC.localAI[3] >= 70)
                        {
                            NPC.netUpdate = true;
                            NPC.active = false;
                            return false;
                        }
                    }
                }
            }
            else if (NPC.ai[2] > 0)
                NPC.ai[2]--;
            return true;
        }
        public override void FindFrame(int frameHeight)
        {
            //frameHeight = 14;
            //if(MathF.Abs(NPC.velocity.X) > 0)
                //NPC.frameCounter++;
            //if(NPC.frameCounter > 20)
            //{
                //NPC.frame.Y += frameHeight;
                //if (NPC.frame.Y >= 2 * frameHeight)
                    //NPC.frame.Y = 0;
                //NPC.frameCounter = 0;
            //}
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.TintableDust, 2 * hit.HitDirection, -2f, 0, GoreColor);
                    if (Main.rand.NextBool(2))
                    {
                        dust.noGravity = true;
                        dust.scale = 1.2f * NPC.scale;
                    }
                    else
                    {
                        dust.scale = 0.8f * NPC.scale;
                    }
                }
            }
        }
    }
    public class SubspaceWormItem : ModItem
    {
        public virtual int BaitPower => 40;
        public virtual int NPC => ModContent.NPCType<SubspaceWorm>();
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(20);
        }
        public override void SetDefaults()
        {
            Item.Size = new Vector2(20, 26);
            Item.CloneDefaults(ItemID.Frog);
            Item.makeNPC = NPC;
            Item.value = Item.sellPrice(0, 0, BaitPower, 0); // Make this critter worth slightly more than the frog
            Item.rare = ItemRarityID.Blue;
            Item.bait = BaitPower;
            Item.scale = 0.9f;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ItemID.ObsidianSkinPotion, 3)
                .AddIngredient(ItemID.BottledWater, 3)
                .AddIngredient<FragmentOfInferno>()
                .AddIngredient(Type)
                .AddIngredient(ItemID.Fireblossom, 1)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}
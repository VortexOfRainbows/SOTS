using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using SOTS.Items.Fragments;

namespace SOTS.NPCs.Critters
{
	public class TardigradeBrown : ModNPC
    {
        public virtual int GoreMin => 1;
        public virtual int GoreMax => 3;
        public virtual Color GoreColor => new Color(173, 114, 73);
        public virtual int CatchItem => ModContent.ItemType<BrownTardigrade>();
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 7;
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
            NPC.width = 30;
            NPC.height = 18;
            NPC.aiStyle = NPCAIStyleID.CritterWorm;
            NPC.catchItem = CatchItem;
            NPC.lavaImmune = true;
            NPC.scale = 0.9f;
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
            if (NPC.velocity.Y == 0)
            {
                NPC.velocity.X *= 1.02f;
                NPC.rotation = Utils.AngleLerp(NPC.rotation, 0, 0.1f);
            }
            else
            {
                NPC.ai[2]++;
                NPC.rotation += NPC.velocity.Y * 0.02f * NPC.direction;
                NPC.rotation += MathF.Abs(NPC.velocity.X) * 0.025f * NPC.direction;
                NPC.velocity.Y *= 0.5f;
                float gravityAdd = 0.08f;
                if (Main.player[NPC.target].Center.Y > NPC.velocity.Y)
                {
                    gravityAdd = 0.15f;
                }
                NPC.velocity.Y += 0.7f * MathF.Sin(NPC.ai[2] * MathF.PI / 50f) + gravityAdd;
                NPC.velocity.X += NPC.direction * 0.0175f;
                if (MathF.Abs(NPC.velocity.X) > 3)
                    NPC.velocity.X *= 0.9f;
            }
            return true;
        }
        public override void FindFrame(int frameHeight)
        {
            frameHeight = 22;
            if(MathF.Abs(NPC.velocity.X) > 0)
                NPC.frameCounter++;
            if(NPC.frameCounter > 6)
            {
                NPC.frame.Y += 22;
                if (NPC.frame.Y >= Main.npcFrameCount[Type] * frameHeight)
                    NPC.frame.Y = 0;
                NPC.frameCounter = 0;
            }
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
                for(int i = GoreMin; i <= GoreMax; ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(12 * ((i - 1) % 3), 0), NPC.velocity, ModGores.GoreType($"Gores/Tardigrade/TardigradeGore{i}"), NPC.scale);
            }
        }
    }
    public class TardigradeBlue : TardigradeBrown
    {
        public override int GoreMin => 4;
        public override int GoreMax => 6;
        public override Color GoreColor => new Color(137, 131, 195);
        public override int CatchItem => ModContent.ItemType<BlueTardigrade>();
    }
    public class TardigradeGreen : TardigradeBrown
    {
        public override int GoreMin => 7;
        public override int GoreMax => 9;
        public override Color GoreColor => new Color(119, 161, 99);
        public override int CatchItem => ModContent.ItemType<GreenTardigrade>();
    }
    public class TardigradePink : TardigradeBrown
    {
        public override int GoreMin => 10;
        public override int GoreMax => 12;
        public override Color GoreColor => new Color(227, 102, 140);
        public override int CatchItem => ModContent.ItemType<PinkTardigrade>();
    }
    public class BrownTardigrade : ModItem
    {
        public virtual int BaitPower => 20;
        public virtual int NPC => ModContent.NPCType<TardigradeBrown>();
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(20);
        }
        public override void SetDefaults()
        {
            Item.Size = new Vector2(30, 18);
            Item.CloneDefaults(ItemID.Frog);
            Item.makeNPC = NPC;
            Item.value = Item.sellPrice(0, 0, BaitPower, 0); // Make this critter worth slightly more than the frog
            Item.rare = ItemRarityID.Blue;
            Item.bait = BaitPower;
            Item.scale = 0.9f;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ItemID.GravitationPotion)
                .AddIngredient(ItemID.BottledWater, 1)
                .AddIngredient<FragmentOfOtherworld>()
                .AddIngredient(Type)
                .AddIngredient(ItemID.Moonglow, 1)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
    public class BlueTardigrade : BrownTardigrade
    {
        public override int BaitPower => 30;
        public override int NPC => ModContent.NPCType<TardigradeBlue>();
        public override void AddRecipes()
        {
            Recipe.Create(ItemID.GravitationPotion, 2)
                .AddIngredient(ItemID.BottledWater, 2)
                .AddIngredient<FragmentOfOtherworld>()
                .AddIngredient(Type)
                .AddIngredient(ItemID.Moonglow, 1)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
    public class GreenTardigrade : BrownTardigrade
    {
        public override int BaitPower => 40;
        public override int NPC => ModContent.NPCType<TardigradeGreen>();
        public override void AddRecipes()
        {
            Recipe.Create(ItemID.GravitationPotion, 3)
                .AddIngredient(ItemID.BottledWater, 3)
                .AddIngredient<FragmentOfOtherworld>()
                .AddIngredient(Type)
                .AddIngredient(ItemID.Moonglow, 1)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
    public class PinkTardigrade : BrownTardigrade
    {
        public override int BaitPower => 50;
        public override int NPC => ModContent.NPCType<TardigradePink>();
        public override void AddRecipes()
        {
            Recipe.Create(ItemID.GravitationPotion, 4)
                .AddIngredient(ItemID.BottledWater, 4)
                .AddIngredient<FragmentOfOtherworld>()
                .AddIngredient(Type)
                .AddIngredient(ItemID.Moonglow, 1)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}
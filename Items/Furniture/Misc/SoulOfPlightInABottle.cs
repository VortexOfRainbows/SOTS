using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Items.Permafrost;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Furniture.Misc
{
    public class SoulOfPlightInABottle : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(14, 32);
            Item.rare = ItemRarityID.White;
            Item.createTile = ModContent.TileType<SoulOfPlightInABottleTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Bottle, 1).AddIngredient<SoulOfPlight>(1).Register();
        }
    }
    public class SoulOfPlightInABottleTile : Lantern<SoulOfPlightInABottle>
    {
        protected override Vector3 LightClr => ColorHelpers.PolarisColor(0.5f + 0.5f * (float)Math.Sin(SOTSWorld.GlobalCounter * MathHelper.TwoPi / 720f)).ToVector3();
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter > 5)
            {
                frameCounter = 0;
                frame++;
            }
            frame %= 8;
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            offsetY = -2;
        }
        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            int uniqueAnimationFrame = Main.tileFrame[Type] + i;
            if (i % 2 == 0)
                uniqueAnimationFrame += 3;
            if (i % 3 == 0)
                uniqueAnimationFrame += 3;
            if (i % 4 == 0)
                uniqueAnimationFrame += 3;
            uniqueAnimationFrame %= 8;
            frameXOffset = uniqueAnimationFrame * animationFrameWidth;
        }
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            // Flips the sprite if x coord is odd. Makes the tile more interesting
            if (i % 2 == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
        }
        private readonly int animationFrameWidth = 18;
    }
}

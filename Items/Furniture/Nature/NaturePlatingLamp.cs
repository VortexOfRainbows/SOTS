using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Furniture.Nature
{
    public class NaturePlatingLamp : ModItem
    {
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.StoneBlock);
            item.Size = new Vector2(14, 32);
            item.rare = ItemRarityID.Blue;
            item.createTile = ModContent.TileType<NaturePlatingLampTile>();
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<NaturePlating>(), 3);
            recipe.AddIngredient(ItemID.Torch, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
    public class NaturePlatingLampTile : Lamp<NaturePlatingLamp>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override Vector3 LightClr => SOTSTile.NaturePlatingLight * 3f;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int xFrameOffset = Main.tile[i, j].frameX;
            int yFrameOffset = Main.tile[i, j].frameY;
            Texture2D glowmask = ModContent.GetTexture(this.GetPath("Glow"));
            Vector2 drawOffest = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawPosition = new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y) + drawOffest;
            Color drawColour = new Color(100, 100, 100, 0);
            var effects = SpriteEffects.None;
            SetSpriteEffects(i, j, ref effects);
            for (int k = 0; k < 5; k++)
            {
                spriteBatch.Draw(glowmask, drawPosition + Main.rand.NextVector2Circular(1, 1) * (k * 0.25f), new Rectangle(xFrameOffset, yFrameOffset, 18, 18), drawColour, 0.0f, Vector2.Zero, 1f, effects, 0.0f);
            }
        }
    }
}
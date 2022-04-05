using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ObjectData;
using SOTS.Items.Fragments;
using Terraria.ID;

namespace SOTS.Items.Furniture.Earthen
{
    public class EarthenPlatingWorkbench : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Also functions as an anvil");
        }
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.StoneBlock);
            item.Size = new Vector2(32, 18);
            item.rare = ItemRarityID.Blue;
            item.createTile = ModContent.TileType<EarthenPlatingWorkbenchTile>();
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<EarthenPlating>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
    public class EarthenPlatingWorkbenchTile : Workbench<EarthenPlatingWorkbench>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override void SetDefaults(TileObjectData t)
        {
            Main.tileLighted[Type] = true;
            base.SetDefaults(t);
            adjTiles = new int[] { TileID.WorkBenches, TileID.Anvils };
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = ModContent.GetTexture(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ObjectData;
using SOTS.Items.Fragments;
using Terraria.ID;

namespace SOTS.Items.Furniture.Nature
{
    public class NaturePlatingWorkBench : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Also functions as an anvil");
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(32, 18);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<NaturePlatingWorkBenchTile>();
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<NaturePlating>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
    public class NaturePlatingWorkBenchTile : Workbench<NaturePlatingWorkBench>
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
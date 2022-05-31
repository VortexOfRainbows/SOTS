using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ObjectData;
using SOTS.Items.Fragments;
using Terraria.ID;

namespace SOTS.Items.Furniture.Earthen
{
    public class EarthenPlatingWorkBench : ModItem
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
            Item.createTile = ModContent.TileType<EarthenPlatingWorkBenchTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EarthenPlating>(), 10).AddTile(TileID.Anvils).Register();
        }
    }
    public class EarthenPlatingWorkBenchTile : Workbench<EarthenPlatingWorkBench>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override void SetStaticDefaults(TileObjectData t)
        {
            Main.tileLighted[Type] = true;
            base.SetStaticDefaults(t);
            AdjTiles = new int[] { TileID.WorkBenches, TileID.Anvils };
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
}
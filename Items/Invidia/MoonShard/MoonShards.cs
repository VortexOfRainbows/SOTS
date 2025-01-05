using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Planetarium.Furniture;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Invidia.MoonShard
{
	public abstract class MoonShard  : ModItem
	{
		private Texture2D Hook => ModContent.Request<Texture2D>("SOTS/Items/Invidia/MoonShard/MoonShardOutline").Value;
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            scale *= 0.85f;
			Vector2 drawOrigin = new Vector2(Hook.Width * 0.5f, Hook.Height * 0.5f);
            for(int i = 0; i < 6; i++)
            {
                Vector2 circular = new Vector2(3 * scale, 0).RotatedBy(MathF.PI * i / 2f + MathHelper.ToRadians(SOTSWorld.GlobalCounter));
			    Main.spriteBatch.Draw(Hook, position + circular, null, new Color(60, 70, 65, 0), 0f, drawOrigin, scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            scale *= 0.85f;
			Vector2 drawOrigin = new Vector2(Hook.Width * 0.5f, Hook.Height * 0.5f);
            for (int i = 0; i < 6; i++)
            {
                Vector2 circular = new Vector2(3 * scale, 0).RotatedBy(MathF.PI * i / 2f + MathHelper.ToRadians(SOTSWorld.GlobalCounter));
			    Main.spriteBatch.Draw(Hook, Item.Center + circular - Main.screenPosition, null, new Color(60, 70, 65, 0), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            scale *= 0.85f;
			Vector2 drawOrigin = new Vector2(Hook.Width * 0.5f, Hook.Height * 0.5f);
            Main.spriteBatch.Draw(Hook, position, null, Color.White * 1f, 0f, drawOrigin, scale, SpriteEffects.None, 0f);
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			Vector2 drawOrigin = new Vector2(Hook.Width * 0.5f, Hook.Height * 0.5f);
			Main.spriteBatch.Draw(Hook, Item.Center - Main.screenPosition, null, Color.White * 1f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemNoGravity[Type] = true;
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 26;
			Item.maxStack = 1;
			Item.rare = ModContent.RarityType<StrangeGreenRarity>();
			Item.value = Item.sellPrice(0, 0, 0, 0);
			Item.shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
		}
        public virtual Color InnerColor => Color.Black;

        public virtual Color OuterColor => new Color(86, 226, 100);
    }
	public class MoonShard1 : MoonShard
	{
        public override Color InnerColor => Color.Black;
        public override Color OuterColor => base.OuterColor;
    }
    public class MoonShard2 : MoonShard
    {
        public override Color InnerColor => new Color(31, 39, 57);
        public override Color OuterColor => base.OuterColor * 1.1f;
    }
    public class MoonShard3 : MoonShard
    {
        public override Color InnerColor => new Color(46, 63, 77);
        public override Color OuterColor => base.OuterColor * 1.25f;
    }
    public class MoonShard4 : MoonShard
    {
        public override Color InnerColor => new Color(67, 83, 96);
        public override Color OuterColor => base.OuterColor * 1.45f;
    }
    public class MoonShard5 : MoonShard
    {
        public override Color InnerColor => new Color(79, 98, 113);
        public override Color OuterColor => base.OuterColor * 1.6f;
    }
    public class MoonShard6 : MoonShard
    {
        public override Color InnerColor => new Color(67, 83, 96);

        public override Color OuterColor => base.OuterColor * 1.45f;
    }
    public class MoonShard7 : MoonShard
    {
        public override Color InnerColor => new Color(46, 63, 77);
        public override Color OuterColor => base.OuterColor * 1.25f;
    }
    public class MoonShard8 : MoonShard
    {
        public override Color InnerColor => new Color(31, 39, 57);
        public override Color OuterColor => base.OuterColor * 1.1f;
    }
    public class LunarClock : ModItem
    {
        //private Texture2D Hook => ModContent.Request<Texture2D>("SOTS/Items/Invidia/MoonShard/MoonShardOutline").Value;
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            scale *= 0.9f;
            Draw(position, drawColor, scale, false, 0f);
            Draw(position, drawColor, scale, true, 0f);
            for(int i = 0; i < 4; i++)
            {
                Main.spriteBatch.Draw(TextureAssets.Item[Type].Value, position + new Vector2(2f * scale, 0).RotatedBy(MathHelper.PiOver2 * i), null, Color.Black, 0f, origin, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(TextureAssets.Item[Type].Value, position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
        public void Draw(Vector2 position, Color color, float scale, bool front = false, float rotation = 0f)
        {
            Vector2 origin = new Vector2(13, 13);
            for(int passNum = 0; passNum < 2; passNum++)
            {
                for (int i = 1; i <= 8; i++)
                {
                    Texture2D texture = ModContent.Request<Texture2D>("SOTS/Items/Invidia/MoonShard/MoonShard" + i).Value;
                    float rad = MathHelper.WrapAngle(i * MathHelper.PiOver4 + MathHelper.ToRadians(-SOTSWorld.GlobalCounter * 0.25f));
                    bool drawNow = (front && rad > 0) || (!front && rad <= 0);
                    if (drawNow)
                    {
                        if(passNum == 1)
                        {
                            float sin = 0.7f + 0.05f * MathF.Sin(rad + MathHelper.PiOver4);
                            Vector2 circular = new Vector2(22 * scale).RotatedBy(rad);
                            circular.Y *= 0.8f;
                            circular = circular.RotatedBy(-MathHelper.PiOver4 + rotation);
                            float r = circular.X * 0.004f;
                            Main.spriteBatch.Draw(texture, position + circular, null, color, -MathHelper.PiOver4 / 4f + r, origin, scale * sin, SpriteEffects.None, 0f);
                        }
                    }
                    if (i <= 4 && !front && passNum == 0)
                    {
                        DrawChains(position, scale, i, rotation);
                    }
                }
            }
        }
        private void DrawChains(Vector2 pos, float size2 = 1f, float offset = 0f, float rotation = 0f)
        {
            float completionPercent = 1f;
            Color color = new Color(11, 142, 50, 0) * completionPercent;
            Texture2D texture = SOTSUtils.WhitePixel;
            Vector2 drawOrigin = new Vector2(0, texture.Height * 0.5f);
            int startEnd = 0;
            int increment = 5;
            for (int j = -1; j <= 1; j += 2)
            {
                for (int i = startEnd; i < startEnd + 360; i += increment)
                {
                    float size = size2 * 0.8f - 0.1f * MathF.Sin(MathF.PI / 180f * i) + 0.1f * completionPercent;
                    Vector2 CP = circlePos(i + 90, j, size2, offset, rotation);
                    Vector2 nextCP = circlePos(i + 90 + increment, j, size2, offset, rotation);
                    Vector2 toNext = nextCP - CP;
                    Main.spriteBatch.Draw(texture, pos + CP, null, color * (1 - 0.5f * MathF.Sin(MathF.PI / 180f * i)), toNext.ToRotation(), drawOrigin, new Vector2(toNext.Length() / 2, size * 0.9f), SpriteEffects.None, 0f);
                }
            }
        }
        private Vector2 circlePos(int i, int j, float size = 1, float offset = 0f, float rotation = 0f)
        {
            float speedM = .125f;
            float rad = MathHelper.WrapAngle(offset * MathHelper.PiOver4 + MathHelper.ToRadians(-SOTSWorld.GlobalCounter * speedM));
            float sin = MathF.Sin(i * MathF.PI / 30f + MathHelper.ToRadians(SOTSWorld.GlobalCounter * 1.5f));
            float jOffset = j * sin;
            float radians = MathHelper.ToRadians(i);
            float timer = MathHelper.ToRadians(-SOTSWorld.GlobalCounter * speedM) + rad;
            float sinusoid = MathF.Sin(timer);
            Vector2 circular = new Vector2(26 + jOffset * 1.5f, 0).RotatedBy(radians) * size;
            circular.X *= 0.8f;
            circular.X *= sinusoid;
            circular += new Vector2(jOffset * 2.5f, 0).RotatedBy(radians) * size;
            circular = circular.RotatedBy(timer - MathHelper.PiOver4 + rotation);
            return circular;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Draw(Item.Center - Main.screenPosition, Color.White, scale, false, rotation);
            Draw(Item.Center - Main.screenPosition, Color.White, scale, true, rotation);
            scale *= 0.9f;
            for (int i = 0; i < 4; i++)
            {
                Main.spriteBatch.Draw(TextureAssets.Item[Type].Value, Item.Center - Main.screenPosition + new Vector2(2f * scale, 0).RotatedBy(MathHelper.PiOver2 * i), null, Color.Black, rotation, TextureAssets.Item[Type].Size() / 2, scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

        }
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Type] = true;
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 46;
            Item.maxStack = 1;
            Item.rare = ModContent.RarityType<StrangeGreenRarity>();
            Item.value = Item.sellPrice(1, 0, 0, 0);
            Item.shopCustomPrice = Item.buyPrice(8, 0, 0, 0);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AddRecipes()
        {
			CreateRecipe(1).AddIngredient<MoonShard1>().AddIngredient<MoonShard2>().AddIngredient<MoonShard3>().AddIngredient<MoonShard4>()
                .AddIngredient<MoonShard5>().AddIngredient<MoonShard6>().AddIngredient<MoonShard7>().AddIngredient<MoonShard8>().AddTile<HardlightFabricatorTile>().Register();
        }
    }
}
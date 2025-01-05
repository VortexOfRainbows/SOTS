using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Helpers;
using System;
using Terraria;
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
}
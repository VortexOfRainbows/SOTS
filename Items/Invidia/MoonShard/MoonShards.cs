using Humanizer;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using SOTS.Items.Planetarium.Furniture;
using System;
using Terraria;
using Terraria.DataStructures;
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
                            Vector2 circular = new Vector2(21 * scale).RotatedBy(rad);
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
            Vector2 circular = new Vector2(25 + jOffset * 1.5f, 0).RotatedBy(radians) * size;
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
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<MoonClock>();
            Item.shootSpeed = 10;
            Item.noMelee = Item.noUseGraphic = Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
        }
        public override bool? UseItem(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
            velocity *= 0.01f;
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
    public class MoonClock : ModProjectile
    {
        public float HourHand => MathHelper.WrapAngle(Utils.GetDayTimeAs24FloatStartingFromMidnight() / 12f * MathHelper.TwoPi);
        public float MinuteHand => MathHelper.WrapAngle(Utils.GetDayTimeAs24FloatStartingFromMidnight() % 1f * MathHelper.TwoPi);
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D hand = ModContent.Request<Texture2D>("SOTS/Items/Invidia/MoonShard/LunarClockHand").Value;
            Texture2D core = ModContent.Request<Texture2D>("SOTS/Items/Invidia/MoonShard/MoonClock").Value;
            Texture2D nums = ModContent.Request<Texture2D>("SOTS/Items/Invidia/MoonShard/Numerals").Value;
            lightColor = Color.White;
            Vector2 handOrigin = new Vector2(hand.Width / 2, hand.Height);
            Main.EntitySpriteDraw(hand, Projectile.Center - Main.screenPosition, null, Color.White, MinuteHand, handOrigin, 1f, SpriteEffects.None, 0f);
            int size = hand.Height - 14;
            Main.EntitySpriteDraw(hand, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, hand.Width, size), Color.White, HourHand, new Vector2(hand.Width/ 2, size), 1f, SpriteEffects.None, 0f);
            return true;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 30;
        }
        public override bool PreAI()
        {
            if (Projectile.ai[0] == 0)
            {
                Vector2 startingOffset = new Vector2(0, 1).RotatedBy(MinuteHand);
                Projectile.Center += startingOffset;
            }
            Projectile.velocity = Vector2.Zero;
            Player player = Main.player[Projectile.owner];
            if(Main.myPlayer == Projectile.owner)
            {
                if (Projectile.ai[0] != Main.MouseWorld.X || Projectile.ai[1] != Main.MouseWorld.Y)
                {
                    Projectile.ai[0] = Main.MouseWorld.X;
                    Projectile.ai[1] = Main.MouseWorld.Y;
                    Projectile.netUpdate = true;
                }
                if(player.channel)
                {
                    Projectile.timeLeft = 30;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            Vector2 mousePos = new Vector2(Projectile.ai[0], Projectile.ai[1]) - Projectile.Center;
            float angle = MathHelper.WrapAngle(mousePos.ToRotation() + MathHelper.PiOver2);
            float nextAngle = MathHelper.WrapAngle(MinuteHand - SOTSUtils.AngularLerp(MinuteHand, angle, 0.95f));
            double rateMod = -nextAngle * 60.0;
            if (Projectile.ai[2] > 6)
            {
                rateMod = Math.Abs(rateMod);
            }
            if (Projectile.ai[2] < -6)
            {
                rateMod = -Math.Abs(rateMod);
            }
            SOTSWorld.TimeRateModify = rateMod;
            Projectile.ai[2] += (float)rateMod * 0.1f;
            Projectile.ai[2] *= 0.75f;
            Main.NewText(Projectile.ai[2]);
            SOTSWorld.TimeRateModify -= 1;
            UpdateHoldOut();
            return base.PreAI();
        }
        private void UpdateHoldOut()
        {
            Vector2 mousePos = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            Player player = Main.player[Projectile.owner];
            if (mousePos != Vector2.Zero)
            {
                Vector2 toMouse = mousePos - player.Center;
                int direction = 1;
                if (toMouse.X < 0)
                    direction = -1;
                Projectile.alpha = 0;
                player.ChangeDir(direction);
                player.itemTime = 4;
                player.itemAnimation = 4;
                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, 0f);
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle(player.gravDir * toMouse.ToRotation() + MathHelper.ToRadians(-90)));
            }
        }
    }
}
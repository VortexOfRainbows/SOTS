using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Items.Planetarium.Furniture;
using SOTS.Projectiles.Camera;
using SOTS.Void;
using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
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
    public class LunarClock : VoidItem
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
                            circular.Y *= 0.85f;
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
            circular.X *= 0.85f;
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
        public override void SafeSetDefaults()
        {
            Item.width = 46;
            Item.height = 46;
            Item.maxStack = 1;
            Item.rare = ModContent.RarityType<StrangeGreenRarity>();
            Item.value = Item.sellPrice(1, 0, 0, 0);
            Item.shopCustomPrice = Item.buyPrice(8, 0, 0, 0);
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<MoonClock>();
            Item.shootSpeed = 10;
            Item.noMelee = Item.noUseGraphic = Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item4;
        }
        public override bool BeforeUseItem(Player player)
        {
            for(int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile proj = Main.projectile[i];
                if(proj.active && proj.type == Item.shoot)
                {
                    return false;
                }
            }
            return true;
        }
        public override int GetVoid(Player player)
        {
            return 20;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity.SNormalize() * 14f, ModContent.ProjectileType<MoonClockHoldOut>(), 0, 0, player.whoAmI);
            return true;
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
        private float prevAccumulatedTime;
        private float accumulatedTime;
        private bool canGoBackward = false;
        public float HourHand => MathHelper.WrapAngle(Utils.GetDayTimeAs24FloatStartingFromMidnight() / 12f * MathHelper.TwoPi);
        public float MinuteHand => MathHelper.WrapAngle(Utils.GetDayTimeAs24FloatStartingFromMidnight() % 1f * MathHelper.TwoPi);
        private float Percent => MathF.Sin(MathHelper.PiOver2 * MathF.Sqrt(MathF.Min(1, Projectile.localAI[0] / 60f)));
        public override bool PreDraw(ref Color lightColor)
        {
			Texture2D extra = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value;
			Texture2D extra2 = Mod.Assets.Request<Texture2D>("Assets/Glow").Value;
            Texture2D hand = ModContent.Request<Texture2D>("SOTS/Items/Invidia/MoonShard/LunarClockHand").Value;
            //Texture2D core = ModContent.Request<Texture2D>("SOTS/Items/Invidia/MoonShard/MoonClock").Value;
            Texture2D nums = ModContent.Request<Texture2D>("SOTS/Items/Invidia/MoonShard/Numerals").Value;
            float normal = Percent;
            float inverse = 1 - normal;
            float offsetRotation1 = MathF.PI * 1.5f * inverse;
            float offsetRotation2 = offsetRotation1;
            float offsetRotation3 = MathF.PI * 4.5f * inverse;
            Vector2 numOrigin = new Vector2(nums.Width / 2, nums.Height / 24);
            for(int i = 0; i < 4; i++)
                Main.EntitySpriteDraw(extra2, Projectile.Center - Main.screenPosition + new Vector2(1, 0).RotatedBy(i * MathHelper.PiOver2), null, Color.Black * normal * normal * 0.74f, 0f, extra2.Size() / 2, 3.2f * normal, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(extra, Projectile.Center - Main.screenPosition, null, new Color(11, 142, 50, 0) * normal * normal * 0.32f, 0f, extra.Size() / 2, 1.9f * normal, SpriteEffects.None, 0f);
            for(int i = 0; i < 2; i++)
                DrawChains(Projectile.Center - Main.screenPosition, -0.125f, .1f, 34f * normal, i * MathHelper.Pi / 2f, 20f);
            DrawChains(Projectile.Center - Main.screenPosition, 0f, .08f, 52f * normal, 0f, 10f);
            DrawChains(Projectile.Center - Main.screenPosition, 0f, -.04f, 88f * normal, 0f, 8f);
            for(int i = 0; i < 12; i++)
            {
                Rectangle frame = new Rectangle(0, nums.Height / 12 * i, nums.Width, nums.Height / 12);
                float r = (i + 1) * MathF.PI / 6f + offsetRotation2;
                float rad = MathHelper.WrapAngle(offsetRotation1);
                float sinusoid = MathF.Cos(rad);
                Vector2 circular = new Vector2(0, -70 * normal).RotatedBy(r - MathHelper.PiOver4);
                circular.X *= sinusoid;
                circular = circular.RotatedBy(rad + MathHelper.PiOver4);
                float widthMult = 1f;
                if (i == 5 || i == 8 || i == 11)
                    widthMult = 0.95f;
                if (i == 6)
                    widthMult = 0.925f;
                if (i == 7)
                    widthMult = 0.85f;
                Main.EntitySpriteDraw(nums, circular + Projectile.Center - Main.screenPosition, frame, new Color(166, 166, 166, 66) * normal, circular.ToRotation() + MathHelper.PiOver2, numOrigin, new Vector2((sinusoid * 0.3f + 0.4f) * widthMult, sinusoid * 0.3f + 0.4f) * normal, SpriteEffects.None, 0f);
            }
            lightColor = Color.White * normal;
            Vector2 handOrigin = new Vector2(hand.Width / 2, hand.Height - 10);
            Main.EntitySpriteDraw(hand, Projectile.Center - Main.screenPosition, null, Color.White * normal, MinuteHand - offsetRotation3, handOrigin, normal * 0.9f, SpriteEffects.None, 0f);
            int size = hand.Height - 10;
            Main.EntitySpriteDraw(hand, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, hand.Width, size), Color.White * normal, HourHand - offsetRotation1, new Vector2(hand.Width/ 2, size - 6) * 0.9f, normal, SpriteEffects.None, 0f);
            return true;
        }
        private void DrawChains(Vector2 pos, float speedModifier = 1f, float speedModifier2 = 1f, float size2 = 1f, float offset = 0f, float circleRate = 20f)
        {
            float completionPercent = Percent;
            Color color = new Color(11, 142, 50, 0) * completionPercent;
            Texture2D texture = SOTSUtils.WhitePixel;
            Vector2 drawOrigin = new Vector2(0, texture.Height * 0.5f);
            int increment = 3;
            offset += MathF.PI * 1.5f * (1 - completionPercent);
            for (int j = -1; j <= 1; j += 2)
            {
                for (int i = 0; i < 360; i += increment)
                {
                    Vector2 CP = circlePos(i + 90, j, speedModifier, speedModifier2, size2, offset, circleRate);
                    Vector2 nextCP = circlePos(i + 90 + increment, j, speedModifier, speedModifier2, size2, offset, circleRate);
                    Vector2 toNext = nextCP - CP;
                    Main.spriteBatch.Draw(texture, pos + CP, null, color, toNext.ToRotation(), drawOrigin, new Vector2(toNext.Length() / 2, completionPercent), SpriteEffects.None, 0f);
                }
            }
        }
        private Vector2 circlePos(int i, int j, float speedModifier = 1f, float speedModifier2 = 1f, float size = 1, float offset = 0f, float chainRate = 20f)
        {
            float rotater = SOTSWorld.GlobalCounter + accumulatedTime * MathF.Abs(speedModifier2);
            float rad = MathHelper.WrapAngle(offset + MathHelper.ToRadians(-rotater * speedModifier));
            float jOffset = 0;
            if(chainRate != 0)
            {
                float sin = MathF.Sin(i * MathF.PI / chainRate + MathHelper.ToRadians(rotater * MathF.Sign(speedModifier2) * 1.5f));
                jOffset = j * sin;
            }
            float timer = MathHelper.ToRadians(-rotater * speedModifier) + rad;
            float sinusoid = MathF.Cos(timer);
            float radians = MathHelper.ToRadians(i);
            Vector2 circular = new Vector2(size + jOffset * 2f, 0).RotatedBy(radians);
            circular.X *= sinusoid;
            circular += new Vector2(jOffset * 4f, 0).RotatedBy(radians);
            circular = circular.RotatedBy(timer + MathHelper.PiOver4);
            return circular;
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 8;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.width = Projectile.height = 32;
            Projectile.timeLeft = 30;
            Projectile.localAI[0] = 0;
        }
        public override bool PreAI()
        {
            if (Projectile.ai[0] == 0)
            {
                Vector2 startingOffset = new Vector2(0, 1).RotatedBy(MinuteHand);
                Projectile.Center += startingOffset;
                SOTSUtils.PlaySound(SoundID.Item78, Projectile.Center, 2, -0.4f);
                Projectile.velocity = Vector2.Zero;
            }
            Player player = Main.player[Projectile.owner];
            if(Main.myPlayer == Projectile.owner)
            {
                if (Projectile.ai[0] != Main.MouseWorld.X || Projectile.ai[1] != Main.MouseWorld.Y)
                {
                    Projectile.ai[0] = Main.MouseWorld.X;
                    Projectile.ai[1] = Main.MouseWorld.Y;
                    Projectile.netUpdate = true;
                }
                if (Projectile.localAI[0] < 60 && Projectile.ai[2] >= 0)
                    player.channel = true;
                if(player.channel)
                {
                    Projectile.timeLeft = 60;
                }
                else
                {
                    Projectile.ai[2] = -1;
                    Projectile.netUpdate = true;
                }
            }
            else
                Projectile.timeLeft = 60;
            Vector2 mousePos = new Vector2(Projectile.ai[0], Projectile.ai[1]) - Projectile.Center;
            float angle = MathHelper.WrapAngle(mousePos.ToRotation() + MathHelper.PiOver2);
            float nextAngle = MathHelper.WrapAngle(MinuteHand - angle);
            double rateMod = -nextAngle * 60.0;
            if (Projectile.ai[2] > 6)
            {
                rateMod = Math.Abs(rateMod);
            }
            if (!canGoBackward && rateMod < 0)
                rateMod = 0;
            //if (Projectile.ai[2] < -6 && canGoBackward)
            //{
            //    rateMod = -Math.Abs(rateMod);
            //}
            if (Projectile.ai[2] <= -1)
            {
                if (Projectile.localAI[0] == 50)
                {
                    SOTSUtils.PlaySound(SoundID.Item130, Projectile.Center, 2, -1f);
                }
                Projectile.localAI[0]--;
                if (Projectile.localAI[0] <= 0)
                    Projectile.Kill();
            }
            else
            {
                Projectile.ai[2] += (float)rateMod * 0.05f;
                Projectile.ai[2] *= 0.7f;
                if (Projectile.localAI[0] < 60)
                    Projectile.localAI[0] += 1f;
                else
                {
                    SOTSWorld.TimeRateModify = rateMod;
                    accumulatedTime += (float)SOTSWorld.TimeRateModify;
                    prevAccumulatedTime += (float)SOTSWorld.TimeRateModify;
                    SOTSWorld.TimeRateModify -= 1;
                }
            }
            UpdateHoldOut();
            DoVisuals();
            Projectile.rotation = MathF.PI * 1.5f * (1 - Percent);
            Projectile.scale = Percent * 0.9f;
            if(prevAccumulatedTime > 390)
            {
                SOTSUtils.PlaySound(SoundID.Tink, Projectile.Center, 1.05f, -0.45f);
                prevAccumulatedTime = 0;
            }
            Projectile.frame = (Main.moonPhase + 4) % 8;
            return base.PreAI();
        }
        private float prevPercent = 0f;
        private void DoVisuals()
        {
            Color color = new Color(11, 142, 50, 0);
            float amt;
            if (Projectile.localAI[0] <= 2)
            {
                amt = 35;
                for (int i = 0; i < amt; i++)
                {
                    Vector2 norm = new Vector2(1, 0).RotatedBy(MathHelper.TwoPi * i / amt + Main.rand.NextFloat(MathHelper.TwoPi / amt));
                    Dust d = PixelDust.Spawn(Projectile.Center, 0, 0, norm * Main.rand.NextFloat(1, 7) + Main.rand.NextVector2Circular(1f, 1f), color * Main.rand.NextFloat(1, 2), 8);
                    d.scale = Main.rand.NextFloat(1.5f, 2.25f);
                }
            }
            float percent = Percent;
            if(prevPercent == 0)
                prevPercent = Percent;
            float diff = (percent - prevPercent) * 50f;
            float inverse = 1 - percent;
            float dist = 96 * percent;
            amt = 3 + 7 * inverse;
            color *= percent * 0.8f + 0.2f * inverse;
            for(int i = 0; i < amt; i++)
            {
                Vector2 norm = new Vector2(1, 0).RotatedBy(MathHelper.TwoPi * i / amt + Main.rand.NextFloat(MathHelper.TwoPi / amt));
                Vector2 circular = norm * dist;
                Dust d = PixelDust.Spawn(circular + Projectile.Center, 0, 0, norm * (6 * inverse + diff * Main.rand.NextFloat(1, 2)) + Main.rand.NextVector2Circular(.75f, .75f), color, 5);
                d.scale = Main.rand.NextFloat(0.75f, 1.25f) + 0.75f * inverse;
            }
            prevPercent = Percent;
        }
        private void UpdateHoldOut()
        {
            Vector2 mousePos = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            Player player = Main.player[Projectile.owner];
            if (mousePos != Vector2.Zero)
            {
                Vector2 toProj = Projectile.Center - player.Center;
                Vector2 handPos = toProj.SNormalize() + new Vector2(0, -0.5f).RotatedBy(MinuteHand);
                Vector2 handPos2 = toProj.SNormalize() + new Vector2(0, -0.5f).RotatedBy(HourHand);
                int direction = 1;
                if (toProj.X < 0)
                    direction = -1;
                Projectile.alpha = 0;
                player.ChangeDir(direction);
                if(player.itemTime < 4)
                {
                    player.itemTime = 4;
                    player.itemAnimation = 4;
                }
                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle(player.gravDir * handPos2.ToRotation() - MathHelper.PiOver2));
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle(player.gravDir * handPos.ToRotation() - MathHelper.PiOver2));
            }
        }
        public override void OnKill(int timeLeft)
        {
            SOTSUtils.PlaySound(SoundID.Item110, Projectile.Center, 1.6f, 0.8f);
        }
    }
    public class MoonClockHoldOut : ModProjectile
    {
        public Vector2 MousePosition
        {
            get
            {
                return new Vector2(Projectile.ai[0], Projectile.ai[1]);
            }
            set
            {
                Projectile.ai[0] = value.X;
                Projectile.ai[1] = value.Y;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            if (player.itemTime == 0 || player.itemAnimation == 0)
            {
                return false;
            }
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            for(int i = 0; i < 6; i++)
            {
                Vector2 circular = new Vector2(6, 0).RotatedBy(i * MathF.PI / 3f + MathHelper.ToRadians(SOTSWorld.GlobalCounter));
                Main.spriteBatch.Draw(texture, drawPos + circular, null, new Color(50, 50, 50, 0), Projectile.rotation + MathF.PI, drawOrigin, Projectile.scale * 0.9f, Projectile.velocity.X > 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, drawPos, null, Color.White, Projectile.rotation + MathF.PI, drawOrigin, Projectile.scale * 0.9f, Projectile.velocity.X > 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 40;
            Projectile.hide = true;
            Projectile.alpha = 255;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 centerOnPlayer = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter, true);
            int ID = -1;
            for (int i = 0; i < 1000; i++)
            {
                Projectile camera = Main.projectile[i];
                if (camera.active && camera.owner == Projectile.owner && camera.type == ModContent.ProjectileType<MoonClock>())
                {
                    ID = i;
                    break;
                }
            }
            if (Main.myPlayer == Projectile.owner)
            {
                MousePosition = Main.MouseWorld;
                Projectile.netUpdate = true;
            }
            if (ID != -1 || MousePosition != Vector2.Zero)
            {
                Vector2 center = MousePosition;
                if (ID != -1)
                {
                    Projectile camera = Main.projectile[ID];
                    center = camera.Center;
                }
                Projectile.velocity = Projectile.velocity.Length() * (center - player.Center).SNormalize();
                Projectile.rotation = Projectile.velocity.ToRotation();
                if (Projectile.hide == false)
                {
                    Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
                    Projectile.alpha = 0;
                }
                if (player.itemTime != 0 && player.itemAnimation != 0)
                {
                    player.ChangeDir(Projectile.direction);
                    player.heldProj = Projectile.whoAmI;
                    Projectile.timeLeft = 6;
                }
                else
                    Projectile.Kill();
                Projectile.hide = false;
                Projectile.spriteDirection = Projectile.direction;
                Projectile.Center = centerOnPlayer;
            }
            else
            {
                Projectile.Kill();
            }
            return false;
        }
    }
}
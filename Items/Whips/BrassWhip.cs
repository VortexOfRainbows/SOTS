using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs.WhipBuffs;
using SOTS.Dusts;
using SOTS.Items.Earth.Glowmoth;
using SOTS.Items.Slime;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Whips
{
	public class BrassWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			// This method quickly sets the whip's properties.
			// Mouse over to see its parameters.
			Item.DefaultToWhip(ModContent.ProjectileType<BrassWhipProjectile>(), 24, 4f, 12, 45);
			Item.Size = new Vector2(40, 32);
			Item.shootSpeed = 4;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai1: 0.25f);
			return false;
        }
        public override bool MeleePrefix()
		{
			return true;
		}
        public override void AddRecipes()
        {
            CreateRecipe(1).AddRecipeGroup("SOTS:SilverBar", 12).AddIngredient<GlowNylon>(30).AddTile(TileID.Anvils).Register();
        }
    }
	public class BrassWhipProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.IsAWhip[Type] = true;
		}
		public override void SetDefaults()
		{
			Projectile.DefaultToWhip();
			Projectile.WhipSettings.Segments = 17;
			Projectile.WhipSettings.RangeMultiplier = 1.0f + Projectile.ai[1];
		}
        public override bool PreAI()
		{
			Projectile.WhipSettings.RangeMultiplier = 1.0f + Projectile.ai[1];
			List<Vector2> list = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, list);
			if(Timer > 27)
				for (int i = 0; i < list.Count; i++)
				{
					bool tip = i == list.Count - 1;
                    if (Main.rand.NextBool(40) || (tip && !Main.rand.NextBool(5)))
					{
						Vector2 pos = list[i];
						Dust dust = Dust.NewDustDirect(pos - new Vector2(6, 6), 4, 4, ModContent.DustType<SootDust>());
						dust.velocity *= tip ? 0.25f : 0.5f;
						dust.noGravity = true;
						dust.scale *= tip ? 0.75f : 0.5f;
						dust.scale += 0.75f;
						dust.fadeIn = 0.2f;
						dust.color = new Color(150, 170, 190, 0);
					}
				}
			return base.PreAI();
        }
        private float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<GlowWhipDebuff>(), 120);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
		}

		// This method draws a line between all points of the whip, in case there's empty space between the sprites.
		private void DrawLine(List<Vector2> list)
		{
			Texture2D texture = TextureAssets.FishingLine.Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = new Vector2(frame.Width / 2, 2);

			Vector2 pos = list[0];
			for (int i = 0; i < list.Count - 2; i++)
			{
				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates(), new Color(53, 60, 100));
                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

				pos += diff;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			List<Vector2> list = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, list);

			DrawLine(list);

			//Main.DrawWhip_WhipBland(Projectile, list);
			// The code below is for custom drawing.
			// If you don't want that, you can remove it all and instead call one of vanilla's DrawWhip methods, like above.
			// However, you must adhere to how they draw if you do.

			SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Main.instance.LoadProjectile(Type);
			Texture2D texture = TextureAssets.Projectile[Type].Value;

			Vector2 pos = list[0];

			for (int i = 0; i < list.Count - 1; i++)
			{
				Rectangle frame = new Rectangle(0, 0, 22, 18);
				Vector2 origin = new Vector2(11, 9);
				float scale = 1.1f;

				if (i == list.Count - 2)
				{
					scale = 1f;
					frame.Y = 36;
                    origin = new Vector2(11, 4);
                    Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
					float t = Timer / timeToFlyOut;
					scale *= MathHelper.Lerp(1f, 1.25f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
				}
				else if (i > 0)
				{
                    origin = new Vector2(11, 4);
                    scale = 1f;
					frame.Y = 20;
					frame.Height = 14;
				}

				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2; // This projectile's sprite faces down, so PiOver2 is used to correct rotation.
				Color color = Lighting.GetColor(element.ToTileCoordinates());

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

				pos += diff;
			}
			return false;
		}
	}
}
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using SOTS.Items.OreItems;
using SOTS.Projectiles.Otherworld;
using SOTS.Projectiles.Temple;
using System;

namespace SOTS.Items.Temple
{
	public class Revolution : VoidItem
	{
		public static int RevolutionType()
        {
			if(Main.dayTime)
			{
				if (Main.eclipse)
					return 0;
				return 1;
            }
			else
            {
				return 2;
            }
        }
		public override void SetStaticDefaults()
		{
			ItemID.Sets.UsesCursedByPlanteraTooltip[Type] = true;
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 48;
            Item.DamageType = DamageClass.Magic;
            Item.width = 74;
            Item.height = 76;
            Item.useTime = 15;
			Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
			Item.knockBack = 3.75f;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.Item109;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<RevolutionBolt>();
            Item.shootSpeed = 8f;
			Item.noUseGraphic = true;
			Item.reuseDelay = 10;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Temple/Revolution").Value;
			}
			Item.staff[Item.type] = true;
		}
		int counter = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float sinusoid = (float)Math.Sin(MathHelper.ToRadians(counter * 45));
			type = ModContent.ProjectileType<RevolutionBolt>();
			if(RevolutionType() == 1)
			{
				type = ModContent.ProjectileType<RevolutionBoltDay>();
			}
			else if (RevolutionType() == 2)
			{
				type = ModContent.ProjectileType<RevolutionBoltNight>();
			}
			if (RevolutionType() == 1)
			{
				for(int j = -2; j <= 2; j++)
				{
					for (int i = -1; i <= 1; i++)
					{
						Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians((7f - j * 2f + 5f * sinusoid) * i + Main.rand.NextFloat(-4, 4)) * Main.rand.NextFloat(0.8f, 1.0f)) * (1 + 0.1f * j) * Main.rand.NextFloat(0.9f, 1.1f);
						Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
					}
				}
			}
			if (RevolutionType() == 2)
			{
				for (int i = -1; i <= 1; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians((8f + 6f * sinusoid) * i)) * Main.rand.NextFloat(0.7f, 1.3f);
					Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
				}
			}
			if (RevolutionType() == 0)
			{
				for (int j = -2; j <= 1; j++)
				{
					for (int i = -1; i <= 1; i++)
					{
						Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians((6f + 2.4f * sinusoid) * i + Main.rand.NextFloat(-3, 3)) * Main.rand.NextFloat(0.9f, 1.0f)) * (1 + 0.1f * j);
						Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
					}
				}
			}
			counter++; 
			return false;
        }
		public static string TextureName
        {
			get => "Items/Temple/Revolution" + (RevolutionType() == 1 ? "Day" : RevolutionType() == 2 ? "Night" : "");
		}
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>(TextureName).Value;
			Main.spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>(TextureName).Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
        }
        public override int GetVoid(Player player)
		{
			if (RevolutionType() == 2)
			{
				return 0;
			}
			if (RevolutionType() == 0)
            {
				return 20;
            }
			return 30;
		}
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			Vector2 perturbedSpeed = velocity;
			velocity = perturbedSpeed;
			position += velocity * 9;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.LunarTabletFragment, 20).AddIngredient(ItemID.LihzahrdPowerCell, 1).AddTile(TileID.MythrilAnvil).Register();
		}
		public override bool BeforeUseItem(Player player)
		{
			return NPC.downedPlantBoss;
		}
	}
}

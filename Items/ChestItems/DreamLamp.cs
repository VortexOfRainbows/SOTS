using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;
using System;
using SOTS.Projectiles.Inferno;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Camera;

namespace SOTS.Items.ChestItems
{
	public class DreamLamp : VoidItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dreaming Lamp");
			Tooltip.SetDefault("Chain together up to 10 enemies, slowing them\nWhen a chained enemy is killed, all chained enemies will take additional damage\nChain together items to pull them towards you\n'What do you wish for?'");
			ItemID.Sets.ItemNoGravity[Type] = true;
			this.SetResearchCost(1);
		}
		public Texture2D texture => Mod.Assets.Request<Texture2D>("Items/ChestItems/DreamLamp").Value;
		public Texture2D inventoryBoxTexture => Terraria.GameContent.TextureAssets.InventoryBack.Value;
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			scale *= Item.scale;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 sinusoid = new Vector2(0, 6 * scale * (float)Math.Cos(1.7f * MathHelper.ToRadians(VoidPlayer.soulColorCounter))) + new Vector2(0, -3 * scale);
			rotation = 15 * (float)Math.Sin(1f * MathHelper.ToRadians(VoidPlayer.soulColorCounter));
			spriteBatch.Draw(texture, Item.position + origin + sinusoid - Main.screenPosition, null, lightColor, rotation * MathHelper.Pi / 180f, origin, scale, SpriteEffects.None, 0f);
			return false;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
			origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 inventoryBoxTextureCenter = new Vector2(inventoryBoxTexture.Width / 2 - 5, inventoryBoxTexture.Height / 2 - 1) * scale; //this puts the center in the box's usual position
			Vector2 sinusoid = new Vector2(0, 3 * scale * (float)Math.Cos(0.85f * MathHelper.ToRadians(VoidPlayer.soulColorCounter))) + new Vector2(0, -scale);
			float rotation = 14 * (float)Math.Sin(0.5f * MathHelper.ToRadians(VoidPlayer.soulColorCounter));
			spriteBatch.Draw(texture, position + inventoryBoxTextureCenter + sinusoid, frame, drawColor, rotation * MathHelper.Pi / 180f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void SafeSetDefaults()
		{
			Item.damage = 20;
			Item.DamageType = DamageClass.Magic;
			Item.width = 42;
			Item.height = 50;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.LightRed;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = false;            
			Item.shoot = ModContent.ProjectileType<Projectiles.Camera.DreamLamp>(); //temporary because the proj is not worked on yet
			Item.shootSpeed = 5f;
			Item.knockBack = 5;
			Item.channel = true;
			Item.UseSound = SoundID.Item8; 
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.scale = 0.8f;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 normal = velocity.SafeNormalize(Vector2.Zero);
			Projectile.NewProjectile(source, position, normal * 12, type, damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<DreamingFrame>(), damage, knockback, player.whoAmI);
			return false;
        }
		public override int GetVoid(Player player)
		{
			return 6;
		}
	}
}
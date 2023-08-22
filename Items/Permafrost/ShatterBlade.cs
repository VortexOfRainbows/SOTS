using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using SOTS.Projectiles.Permafrost;
using Terraria.DataStructures;

namespace SOTS.Items.Permafrost
{
	public class ShatterBlade : ModItem
	{
		int counterResetter = 0;
		int counter = 0;
		int broken = 0;
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Permafrost/ShatterBlade").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Permafrost/ShatterFix").Value;
			if(broken == 0)
				spriteBatch.Draw(texture, position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0f);
			if(broken == 1)
			{
				spriteBatch.Draw(texture2, position, new Rectangle(0, 46 * counter + 1, 42, 45), drawColor, 0, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Permafrost/ShatterBlade").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Permafrost/ShatterFix").Value;
			if (broken == 0)
				spriteBatch.Draw(texture, Item.Center - Main.screenPosition, new Rectangle(0, 0, 42, 42), lightColor, 0, new Vector2(17,17), scale, SpriteEffects.None, 0f);
			if (broken == 1)
			{
				spriteBatch.Draw(texture2, Item.Center - Main.screenPosition, new Rectangle(0, 46 * counter + 1, 42, 45), lightColor, 0, new Vector2(17, 17), scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void SetDefaults()
		{
			Item.damage = 15;
			Item.DamageType = DamageClass.Melee;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3f;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = null;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<ShatterFix>();
			Item.shootSpeed = 1;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (broken == 1 || modPlayer.brokenFrigidSword >= 1)
			{
				Item.useStyle = ItemUseStyleID.HoldUp;
				Item.noMelee = true;
				Item.noUseGraphic = true;
			}
			else
			{
				Item.useStyle = ItemUseStyleID.Swing;
				Item.noMelee = false;
				Item.noUseGraphic = false;
			}
		}
		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{ 
			counterResetter++;
			if (counterResetter > 180 && counter > 0)
			{
				counter--;
				counterResetter = 0;
			}
			if (broken == 1)
			{
				Item.useStyle = ItemUseStyleID.HoldUp;
				Item.noMelee = true;
				Item.noUseGraphic = true;
			}
			else
			{
				Item.useStyle = ItemUseStyleID.Swing;
				Item.noMelee = false;
				Item.noUseGraphic = false;
			}
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (broken == 1)
			{
				position += new Vector2(10 * player.direction, -10);
				counterResetter = 0;
				counter++;
				if (counter >= 10)
				{
					broken = 0;
				}
				Projectile.NewProjectile(source, position.X, position.Y, 0, 0, type, damage, knockback, player.whoAmI, counter);
				if (counter >= 10)
				{
					counter = 0;
				}
				return false;
			}
			SOTSUtils.PlaySound(SoundID.Item1, position);
			return false;
		}
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (hit.Crit && player.whoAmI == Main.myPlayer)
			{
				for (int i = 0; i < 4; i++)
				{
					Vector2 circularSpeed = new Vector2(0, 12).RotatedBy(MathHelper.ToRadians(i * 90));
					int calc = hit.Damage + modPlayer.bonusShardDamage;
					if (calc <= 0) calc = 1;
					Projectile.NewProjectile(player.GetSource_OnHit(target), player.Center.X, player.Center.Y, circularSpeed.X, circularSpeed.Y, ModContent.ProjectileType<ShatterShard>(), calc, 3f, player.whoAmI);
				}
				broken = 1;
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<FrigidBar>(), 8).AddTile(TileID.Anvils).Register();
		}
	}
}
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BestFriend.Content.Projectiles;

public class RubberBullet : ModProjectile {
  public override string Texture => BestFriend.ASSET_PATH + "Textures/Projectiles/RubberBullet";

  public override void SetStaticDefaults() {
    // ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50; // The length of old position to be recorded
    // ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
  }

  public override void SetDefaults() {
    Projectile.width = 4; // The width of projectile hitbox
    Projectile.height = 4; // The height of projectile hitbox
    Projectile.aiStyle = 1; // The ai style of the projectile, please reference the source code of Terraria
    Projectile.friendly = true; // Can the projectile deal damage to enemies?
    Projectile.hostile = false; // Can the projectile deal damage to the player?
    Projectile.DamageType = DamageClass.Ranged; // Is the projectile shoot by a ranged weapon?
    Projectile.penetrate = 2; // How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
    Projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
    Projectile.light = 0.5f;
    Projectile.alpha = 255; // The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
    Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
    Projectile.tileCollide = true; // Can the projectile collide with tiles?
    Projectile.extraUpdates = 1; // Set to above 0 if you want the projectile to update multiple time in a frame
    AIType = ProjectileID.Bullet; // Act exactly like default Bullet
  }

  public override bool OnTileCollide(Vector2 old_velocity) {
    // If collide with tile, reduce the penetrate.
    // So the projectile can reflect at most 5 times
    Projectile.penetrate--;
    if (Projectile.penetrate <= 0) {
      Projectile.Kill();
    }

    else {
      Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
      SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

      // If the projectile hits the left or right side of the tile, reverse the X velocity
      if (Math.Abs(Projectile.velocity.X - old_velocity.X) > float.Epsilon) {
        Projectile.velocity.X = -old_velocity.X;
      }

      // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
      if (Math.Abs(Projectile.velocity.Y - old_velocity.Y) > float.Epsilon) {
        Projectile.velocity.Y = -old_velocity.Y;
      }
    }

    return false;
  }

  public override void OnKill(int time_left) {
    // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
    Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
    SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
  }
}
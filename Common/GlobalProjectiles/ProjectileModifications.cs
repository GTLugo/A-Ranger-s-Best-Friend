using System;
using System.Collections.Generic;
using BestFriend.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BestFriend.Common.GlobalProjectiles;

public class ProjectileModifications : GlobalProjectile {
  private static readonly int projectile_lifetime_ai = Projectile.maxAI - 2;
  private static readonly int flashed_ai = Projectile.maxAI - 1;
  public override bool InstancePerEntity => true;

  public static List<int> BlacklistedTypes { get; } = new() {
    ProjectileID.BlackBolt,
    ProjectileID.Xenopopper
  };

  private static bool IsPlayerBullet(Projectile projectile, Player player) {
    return IsPlayerProjectile(projectile) && HoldingGun(player) && !BlacklistedTypes.Contains(projectile.type);
  }

  private static bool HoldingGun(Player player) {
    return AmmoID.Sets.IsBullet[player.HeldItem.useAmmo];
  }

  private static bool IsPlayerProjectile(Projectile projectile) {
    return projectile.DamageType == DamageClass.Ranged && projectile.friendly && !projectile.npcProj &&
           !projectile.trap;
  }

  public override void AI(Projectile projectile) {
    var owner = Main.player[projectile.owner];

    if (IsPlayerBullet(projectile, owner)) {
      var abilities = owner.GetModPlayer<BestFriendAbilitiesPlayer>();

      // HIT-SCAN BULLET ACCELERATION SECTION
      if (abilities.hit_scan) {
        projectile.extraUpdates = 100;
        projectile.alpha = 255;
        projectile.light = 0;

        var projectile_direction = Vector2.Normalize(projectile.velocity);
        var barrel_direction = Vector2.UnitX.RotatedBy(owner.itemRotation) * owner.direction;
        var barrel_position = owner.Center + barrel_direction * (owner.itemWidth * owner.HeldItem.scale);

        HitscanBeam(projectile, projectile_direction, barrel_position);
        MuzzleFlash(projectile, projectile_direction, barrel_position);
      }

      projectile.localAI[projectile_lifetime_ai]++;
      projectile.netUpdate = true;
    }

    base.AI(projectile);
  }

  private static void HitscanBeam(Projectile projectile, Vector2 projectile_direction, Vector2 barrel_position) {
    var scale = projectile.localAI[projectile_lifetime_ai] / 10f;
    var alpha = 255 - (int)(projectile.localAI[projectile_lifetime_ai] * 10);
    projectile.netUpdate = true;

    var pos = projectile.position;
    if (projectile.localAI[flashed_ai] < 0.9f) {
      pos += barrel_position;
    }

    const int iterations = 3;
    for (var i = 0; i < iterations; ++i) {
      var adj_position = pos - projectile.velocity * (i * (1f / iterations));
      var trail = Dust.NewDustDirect(
        adj_position,
        2,
        2,
        DustID.MushroomTorch,
        0f,
        0f,
        alpha > 80 ? alpha : 80,
        Color.White,
        scale < 1.5f ? scale : 1.5f
      );
      trail.velocity *= 0.2f;
      // trail.noLight = true;
      trail.noGravity = true;

      if (!(Main.rand.NextFloat() <= 0.1f)) {
        continue;
      }

      var sparkle = Dust.NewDustDirect(
        adj_position,
        2,
        2,
        DustID.HallowSpray,
        0f,
        0f,
        alpha > 80 ? alpha : 80,
        Color.White,
        scale < 1f ? scale : 1f
      );
      sparkle.fadeIn = scale < 1f ? scale : 1f;
      sparkle.velocity = projectile_direction.RotateRandom(MathHelper.Pi / 6) * 3.5f;
      // trail.noLight = true;
      sparkle.noGravity = true;
    }
  }

  private static void MuzzleFlash(Projectile projectile, Vector2 projectile_direction, Vector2 barrel_position) {
    if (projectile.localAI[flashed_ai] >= 0.9f) return;
    projectile.localAI[flashed_ai] += 1f;
    projectile.netUpdate = true;

    // MUZZLE FLASH FLAME
    SpawnFlame(barrel_position, projectile_direction);
  }

  private static void SpawnFlame(Vector2 position, Vector2 velocity) {

    for (var i = 0; i < 4; ++i) {
      var fire = Dust.NewDustDirect(
        position,
        2,
        2,
        DustID.Smoke,
        0f,
        0f,
        0,
        Color.White,
        1.5f
      );
      fire.velocity = velocity.RotateRandom(MathHelper.Pi / 6);
      fire.noGravity = true;
    }
  }
}
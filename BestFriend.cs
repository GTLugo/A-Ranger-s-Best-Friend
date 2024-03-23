using System.IO;
using BestFriend.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BestFriend;

public class BestFriend : Mod {
  internal static BestFriend instance;

  public const string MOD_SHORT_NAME = $"{nameof(BestFriend)}";
  public const string ASSET_PATH = MOD_SHORT_NAME + "/Assets/";


  public override void Load() {
    instance = ModContent.GetInstance<BestFriend>();
  }

  public override void HandlePacket(BinaryReader reader, int whoAmI) {
    var msgType = (BestFriendModMessageType)reader.ReadByte();

    switch (msgType) {
      case BestFriendModMessageType.SyncPlayer:
        byte player_handle = reader.ReadByte();
        var mod_player = Main.player[player_handle].GetModPlayer<BestFriendAbilitiesPlayer>();

        bool has_serum = reader.ReadBoolean();
        mod_player.has_serum = has_serum;

        bool hit_scan = reader.ReadBoolean();
        mod_player.hit_scan = hit_scan;
        // SyncPlayer will be called automatically, so there is no need to forward this data to other clients.
        break;
    }

    //ModNetHandler.HandlePacket(reader, whoAmI);
  }

  internal enum BestFriendModMessageType : byte {
    SyncPlayer
  }

  // internal class ModNetHandler {
  //   When a lot of handlers are added, it might be wise to automate
  //   creation of them
  //   public const byte bulletTrailType = 2;
  //   internal static BulletTrailPacketHandler bulletTrail = new BulletTrailPacketHandler(bulletTrailType);
  //
  //   public static void HandlePacket(BinaryReader r, int fromWho) {
  //     switch (r.ReadByte()) {
  //       case bulletTrailType:
  //         bulletTrail.HandlePacket(r, fromWho);
  //         break;
  //     }
  //   }
  // }

  // internal abstract class PacketHandler {
  //   protected PacketHandler(byte handlerType) {
  //     HandlerType = handlerType;
  //   }
  //
  //   internal byte HandlerType { get; set; }
  //
  //   public abstract void HandlePacket(BinaryReader reader, int fromWho);
  //
  //   protected ModPacket GetPacket(byte packetType, int fromWho) {
  //     var p = BestFriend.instance.GetPacket();
  //     p.Write(HandlerType);
  //     p.Write(packetType);
  //     if (Main.netMode == NetmodeID.Server) p.Write((byte)fromWho);
  //     return p;
  //   }
  // }

  // internal class BulletTrailPacketHandler : PacketHandler {
  //   public const byte SyncProjectile = 2;
  //
  //   public BulletTrailPacketHandler(byte handlerType) : base(handlerType) { }
  //
  //   public override void HandlePacket(BinaryReader reader, int fromWho) {
  //     //throw new NotImplementedException();
  //     switch (reader.ReadByte()) {
  //       case SyncProjectile:
  //         ReceiveProjectile(reader, fromWho);
  //         break;
  //     }
  //   }
  //
  //   public void SendProjectile(int toWho, int fromWho, int projectile, int trail, int iter) {
  //     var packet = GetPacket(SyncProjectile, fromWho);
  //     packet.Write(projectile);
  //     packet.Write(trail);
  //     packet.Write(iter);
  //     packet.Send(toWho, fromWho);
  //   }
  //
  //   public void ReceiveProjectile(BinaryReader reader, int fromWho) {
  //     int projectile = reader.ReadInt32();
  //     int trail = reader.ReadInt32();
  //     int iter = reader.ReadInt32();
  //
  //     if (Main.netMode == NetmodeID.Server) {
  //       SendProjectile(-1, fromWho, projectile, trail, iter);
  //     } else {
  //       var targetProj = Main.projectile[projectile];
  //       var dust = Main.dust[trail];
  //
  //       var projectilePosition = targetProj.position;
  //       projectilePosition -= targetProj.velocity * (iter * 0.25f);
  //
  //       Main.dust[trail].position = projectilePosition;
  //       //Main.dust[trail].scale = (float)Main.rand.Next(70, 110) * 0.013f;
  //       Main.dust[trail].velocity *= 0.2f;
  //       Main.dust[trail].noGravity = true;
  //       targetProj.netUpdate = true;
  //     }
  //   }
  // }
}
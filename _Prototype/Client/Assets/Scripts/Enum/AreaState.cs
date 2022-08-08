using System;

[Flags]
public enum Area
{
    None = 0,
    Cave = 1,
    Forest = 2,
    Beach = 4,
    Field = 8,
    Altar = 16,
    EngineRoom = 32,
    ChargeRoom = 64,
    BatteryRoom = 128,
    BottleRoom = 256,
    BlueLobby = 512,
    RedLobby = 1024,
    ShipInside = 2048,
    Ship = 4096,

    OutSide = Forest | Field | Altar | BlueLobby | RedLobby | Ship | Beach,
    InSide = Cave | EngineRoom | ChargeRoom | BatteryRoom | BottleRoom | ShipInside,
}
